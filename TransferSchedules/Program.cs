// See https://aka.ms/new-console-template for more information
/*
Problem:
You are given json config file with the following format:
{
    date: '2025-07-01',
    amount: 5000,
    frequency: 'Daily',
    count: 10
}
Write a function that return transfer schedules with date and amount

Approach:
- Read file into a config object
- Calculate tranfer dates
- For each transfer date, add a new transfer schedule
*/
using System.Text.Json;

// Dependency injection setup - in production, this would be handled by DI container
IConfigReader configReader = new JsonConfigReader();
IDateStrategyFactory strategyFactory = new DateStrategyFactoryWithCache(true);

TransferService transferService = new TransferService(configReader, strategyFactory);

var result = await transferService.GetTransferSchedules("config.json");
if (result.IsSuccess)
{

    //var schedules = await transferService.GetTransferSchedules("config.json");

    foreach (var schedule in result.Value!)
    {
        Console.WriteLine($"Date: {schedule.TransferDate}, Day Of Week: {schedule.TransferDate.DayOfWeek}, Amount: {schedule.Amount}");
    }
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}

await Tests.RunAllTests();

/// <summary>
/// Core business service responsible for generating transfer schedules based on configuration.
/// Follows SRP by delgating configuration reading and date calculation to injected dependencies
/// </summary>
public class TransferService
{
    private readonly IConfigReader _configReader;
    private readonly IDateStrategyFactory _dateStrategyFactory;

    public TransferService(IConfigReader configReader, IDateStrategyFactory dateStrategyFactory)
    {
        _configReader = configReader;
        _dateStrategyFactory = dateStrategyFactory;
    }

    /// <summary>
    /// Generates transfer schedules based on configuration files
    /// </summary>
    /// <param name="filePath">Path to configuration file</param>
    /// <returns>List of transfer schedules ordered by date</returns>
    /// <remarks>
    /// This method orchestrates the schedule generation process:
    /// 1. Reads and validates configuration
    /// 2. Selects appropriate date generation strategy
    /// 3. Mpas generated dates to transfer schedules
    /// Thread-safe as it doesn't modify shared state
    /// </remaks>
    public async Task<Result<List<TransferSchedule>>> GetTransferSchedules(string filePath)
    {

        var configResult = await _configReader.ReadConfigAsync(filePath);

        if (configResult.IsFailure)
            return configResult.Error!;

        var config = configResult.Value!;

        var dateStrategyResult = _dateStrategyFactory.GetDateStrategy(config.PaymentFrequency);
        if (dateStrategyResult.IsFailure)
            return dateStrategyResult.Error!;

        var dateStrategy = dateStrategyResult.Value!;  
        var dates = dateStrategy.GenerateDates(config.StartDate, config.TransferCount);


        // Using LINQ for functional transformation - immutable and testable
        return dates
        .Select(d => new TransferSchedule(d, config.Amount))
        .ToList();
    }

}

/// <summary>
/// Abstration for configuration reading to support multiple formats (JSON. XML, etc)
/// and sources (file, API, DB) in the future.
/// </summary>

public interface IConfigReader
{
    Task<Result<Config>> ReadConfigAsync(string filePath);
}

/// <summary>
/// Json-Specific implementation of configuration reader.
/// Handles deserialization, validation and type conversion
/// </summary>
public class JsonConfigReader : IConfigReader
{
    public async Task<Result<Config>> ReadConfigAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return $"Configuration file was not found: {filePath}";
            }

            var json = await File.ReadAllTextAsync(filePath);

            // Using Result for validation failures - these are expected
            return ValidateAndParseConfig(json);
        }
        catch (Exception ex)
        {
            // Only catch truly exceptional cases (eg, file system errors)
            return $"Failed to read configuration: {ex.Message}";
        }
    }


    /// <summary>
    /// Validates business rules for configuration
    /// Separated from parsing logic for clarity and testability
    /// </summary>
    /// <param name="jsonConfig"></param>
    /// <exception cref="ArgumentException">Throw Argument exception for invalid jsonConfig values</exception>
    private void ValidateConfig(JsonConfig jsonConfig)
    {
        if (string.IsNullOrWhiteSpace(jsonConfig.Date))
        {
            throw new ArgumentException("Date cannot be null or empty", nameof(jsonConfig.Date));
        }

        if (string.IsNullOrWhiteSpace(jsonConfig.Frequency))
        {
            throw new ArgumentException("Frequency cannot be null or empty", nameof(jsonConfig.Frequency));
        }

        // Busines rule: Tranfers must have positive count and amount
        if (jsonConfig.Count <= 0)
        {
            throw new ArgumentException("Transfer count must be greater than zero.", nameof(jsonConfig.Count));
        }

        if (jsonConfig.Amount <= 0)
        {
            throw new ArgumentException("Transfer amount must be greater than zero.", nameof(jsonConfig.Amount));
        }
    }

    private Result<Config> ValidateAndParseConfig(string json)
    {
        try
        {
            var jsonConfig = JsonSerializer.Deserialize<JsonConfig>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            if (jsonConfig == null)
            {
                return "Unable to deserialize configuration";
            }

            if (string.IsNullOrWhiteSpace(jsonConfig.Date))
            {
                return "Date cannot be empty";
            }

            if (jsonConfig.Count <= 0)
            {
                return "Transfer count must be greater than zero";
            }

            if (jsonConfig.Amount <= 0)
            {
                return "Transfer amount must be greater then zero";
            }

            // Parse and validate date format early to faily fast
            if (!DateOnly.TryParse(jsonConfig.Date, out var startDate))
            {
                return $"Invalid date format: {jsonConfig.Date}";
            }

            // Parse frequency with case-insensitive comparison for flexibility
            if (!Enum.TryParse<PaymentFrequency>(jsonConfig.Frequency, true, out var frequency))
            {
                return $"Invalid frequency: {jsonConfig.Frequency}";
            }

            return new Config(
                startDate,
                jsonConfig.Amount,
                frequency,
                jsonConfig.Count
                );
        }
        catch (JsonException ex)
        {
            return $"Invalid JSON format: {ex.Message}";
        }
    }
}

/// <summary>
/// Strategy pattern interface for date generation algorithms.
/// Allows extensibility for new frequency types without modifying existing code (OCP)
/// </summary>
public interface IDateStrategy
{
    /// <summary>
    /// Generates sequence of dates based on specific frequency logic.
    /// </summary>
    /// <remarks>
    /// Returns IEnumerable for lazy evaluation - importannt for large transfer counts
    /// Each strategy handles edge cases like month-end differently
    /// </remakrs>
    IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount);
}

public abstract class BusinessDayDateStrategy : IDateStrategy
{
    public abstract IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount);

    protected DateOnly AdjustForWeekend(DateOnly date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Saturday => date.AddDays(2),
            DayOfWeek.Sunday => date.AddDays(1),
            _ => date
        };
    }
}

public interface IDateStrategyFactory
{
    Result<IDateStrategy> GetDateStrategy(PaymentFrequency paymentFrequency);
}

/// <summary>
/// Factory for creating strategies based on payment frequency
/// Centralizes strategy selection logic
/// </summary>
public class DateStrategyFactory : IDateStrategyFactory
{
    private bool _skipWeekends = false;
    public DateStrategyFactory(bool skipWeekends = false)
    {
        _skipWeekends = skipWeekends;
    }
    /// <summary>
    /// Returns appropriate strategy for given payment frequency
    /// </summary>
    /// <param name="paymentFrequency"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>
    /// Using switch expression for concise, exhaustive pattern matching.
    /// </remarks>
    public Result<IDateStrategy> GetDateStrategy(PaymentFrequency paymentFrequency)
    {
        return paymentFrequency switch
        {
            PaymentFrequency.Daily => _skipWeekends ? new DailyWeekendAwareStrategy() : new DailyStrategy(),
            PaymentFrequency.Weekly => _skipWeekends ? new WeeklyWeekendAwareStrategy() : new WeeklyStrategy(),
            PaymentFrequency.Monthly => _skipWeekends ? new MonthlyWeekendAwareStrategy() : new MonthlyStrategy(),
            _ => $"Unsupported payment frequency: {paymentFrequency}"
        };
    }
}

public class DateStrategyFactoryWithCache : IDateStrategyFactory
{
    private bool _skipWeekends = false;
    private readonly Dictionary<(PaymentFrequency, bool), IDateStrategy> _strategiesCahe = new();
    public DateStrategyFactoryWithCache(bool skipWeekends = false)
    {
        _skipWeekends = skipWeekends;
        InitializeStrategies();
    }

    private void InitializeStrategies()
    {
        _strategiesCahe[(PaymentFrequency.Daily, false)] = new DailyStrategy();
        _strategiesCahe[(PaymentFrequency.Daily, true)] = new DailyWeekendAwareStrategy();
        _strategiesCahe[(PaymentFrequency.Weekly, false)] = new WeeklyStrategy();
        _strategiesCahe[(PaymentFrequency.Weekly, true)] = new WeeklyWeekendAwareStrategy();
        _strategiesCahe[(PaymentFrequency.Monthly, false)] = new MonthlyStrategy();
        _strategiesCahe[(PaymentFrequency.Monthly, false)] = new MonthlyWeekendAwareStrategy();
    }
    public Result<IDateStrategy> GetDateStrategy(PaymentFrequency paymentFrequency)
    {
        var key = (paymentFrequency, _skipWeekends);
        if (_strategiesCahe.TryGetValue(key, out var strategy))
        {
            return Result<IDateStrategy>.Success(strategy);
        }

        return $"Unsupported frequency: {paymentFrequency}";
    }
}

/// <summary>
/// Generates dates with daily frequency
/// Simple increment logic suitable for high-frequency transfers.
/// </summary>
public class DailyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        // Using yield return for memory efficiency with large sequences
        for (int i = 0; i < transferCount; i++)
        {
            yield return startDate.AddDays(i);
        }
    }
}
/// <summary>
/// Generates dates with daily frequency, excluding weekends
/// </summary>
public class DailyWeekendAwareStrategy : BusinessDayDateStrategy
{
    public override IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        // Using yield return for memory efficiency with large sequences

        var currentDate = startDate;
        var count = 0;
        while (count < transferCount)
        {
            // Skip weekend
            var adjustedDate = AdjustForWeekend(currentDate);
            yield return adjustedDate;
            count++;

            currentDate = currentDate.AddDays(1);
        }
    }
}

/// <summary>
/// Generates dates with weekly frequency (7 days intervals)
/// </summary>
public class WeeklyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        for (int i = 0; i < transferCount; i++)
        {
            yield return startDate.AddDays(i * 7);
        }
    }
}

/// <summary>
/// Generates dates with weekly frequency, skipping weekends
/// </summary>
public class WeeklyWeekendAwareStrategy : BusinessDayDateStrategy
{
    public override IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        for (int i = 0; i < transferCount; i++)
        {
            var date = startDate.AddDays(i * 7);
            // Skip weekends
            yield return AdjustForWeekend(date);
        }
    }
}

/// <summary>
/// Generates dates with monthly frequency
/// </summary>
/// <remarks>
/// Important: AddMonths handles month-end edge cases (eg. Jan 31 -> Feb 28/29).
/// Business may require explicit handling of these cases depending on requirements.
/// </remarks>
public class MonthlyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        for (int i = 0; i < transferCount; i++)
        {
            yield return startDate.AddMonths(i);
        }
    }
}

/// <summary>
/// Generates dates with monthly frequency, excluding weekends
/// </summary>
public class MonthlyWeekendAwareStrategy : BusinessDayDateStrategy
{
    public override IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        for (int i = 0; i < transferCount; i++)
        {
            var date = startDate.AddMonths(i);
            yield return AdjustForWeekend(date);
        }
    }
}

/// <summary>
/// DTO for JSON desrialization. Maps directly to expected JSON structure.
/// </summary>
public class JsonConfig
{
    public required string Date { get; set; }
    public decimal Amount { get; set; }
    public required string Frequency { get; set; }
    public int Count { get; set; }
}
/// <summary>
/// Immutable value object representing a single transfer in the schedule
/// Using record for conscise sysntax and value equality semantics
/// </summary>
public record TransferSchedule(DateOnly TransferDate, decimal Amount);

/// <summary>
/// Domail model for validated configuration
/// Immutable to ensure configuration consistency through the application lifetime
/// </summary>
public record Config(DateOnly StartDate, decimal Amount, PaymentFrequency PaymentFrequency, int TransferCount);

/// <summary>
/// Strongly typed enumeration for payment frequencies
/// Explicit values support future DB storage and API versioning
/// </summary>
public enum PaymentFrequency
{
    Daily = 1,
    Weekly,
    Monthly
}

public readonly struct Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value)
    {
        Value = value;
        Error = null;
        IsSuccess = true;
    }

    private Result(string error)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(string error) => Failure(error);
}

#region Tests

// Test implementation to verify BusinessDayDateStrategy base class
public class TestBusinessDayStrategy : BusinessDayDateStrategy
{
    public override IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        // Simple implementation for testing AdjustForWeekend
        for (int i = 0; i < transferCount; i++)
        {
            yield return AdjustForWeekend(startDate.AddDays(i));
        }
    }
    
    // Expose protected method for testing
    public DateOnly TestAdjustForWeekend(DateOnly date) => AdjustForWeekend(date);
}

// Mock implementations for testing TransferService
public class MockConfigReader : IConfigReader
{
    private readonly Result<Config> _result;
    
    public MockConfigReader(Result<Config> result)
    {
        _result = result;
    }
    
    public Task<Result<Config>> ReadConfigAsync(string filePath)
    {
        return Task.FromResult(_result);
    }
}

public class MockDateStrategyFactory : IDateStrategyFactory
{
    private readonly Result<IDateStrategy> _result;
    
    public MockDateStrategyFactory(Result<IDateStrategy> result)
    {
        _result = result;
    }
    
    public Result<IDateStrategy> GetDateStrategy(PaymentFrequency paymentFrequency)
    {
        return _result;
    }
}

public class MockDateStrategy : IDateStrategy
{
    private readonly IEnumerable<DateOnly> _dates;
    
    public MockDateStrategy(IEnumerable<DateOnly> dates)
    {
        _dates = dates;
    }
    
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount)
    {
        return _dates;
    }
}

// Test runner
public static class Tests
{
    public static async Task RunAllTests()
    {
        Console.WriteLine("\n=== Running Tests ===\n");
        
        // BusinessDayDateStrategy Tests
        TestAdjustForWeekend_Saturday_ReturnsMonday();
        TestAdjustForWeekend_Sunday_ReturnsMonday();
        TestAdjustForWeekend_Weekday_ReturnsSameDay();
        
        // TransferService Tests
        await TestTransferService_Success_ReturnsSchedules();
        await TestTransferService_ConfigReaderFailure_ReturnsError();
        await TestTransferService_StrategyFactoryFailure_ReturnsError();
        
        Console.WriteLine("\n=== All Tests Passed ===\n");
    }
    
    // BusinessDayDateStrategy Tests
    private static void TestAdjustForWeekend_Saturday_ReturnsMonday()
    {
        var strategy = new TestBusinessDayStrategy();
        var saturday = new DateOnly(2025, 1, 4); // Saturday
        var result = strategy.TestAdjustForWeekend(saturday);
        
        if (result != new DateOnly(2025, 1, 6)) // Monday
            throw new Exception($"Test failed: Expected Monday but got {result}");
            
        Console.WriteLine("✓ TestAdjustForWeekend_Saturday_ReturnsMonday");
    }
    
    private static void TestAdjustForWeekend_Sunday_ReturnsMonday()
    {
        var strategy = new TestBusinessDayStrategy();
        var sunday = new DateOnly(2025, 1, 5); // Sunday
        var result = strategy.TestAdjustForWeekend(sunday);
        
        if (result != new DateOnly(2025, 1, 6)) // Monday
            throw new Exception($"Test failed: Expected Monday but got {result}");
            
        Console.WriteLine("✓ TestAdjustForWeekend_Sunday_ReturnsMonday");
    }
    
    private static void TestAdjustForWeekend_Weekday_ReturnsSameDay()
    {
        var strategy = new TestBusinessDayStrategy();
        var tuesday = new DateOnly(2025, 1, 7); // Tuesday
        var result = strategy.TestAdjustForWeekend(tuesday);
        
        if (result != tuesday)
            throw new Exception($"Test failed: Expected same day but got {result}");
            
        Console.WriteLine("✓ TestAdjustForWeekend_Weekday_ReturnsSameDay");
    }
    
    // TransferService Tests
    private static async Task TestTransferService_Success_ReturnsSchedules()
    {
        // Arrange
        var config = new Config(
            new DateOnly(2025, 1, 1),
            1000m,
            PaymentFrequency.Daily,
            3
        );
        
        var dates = new[] { 
            new DateOnly(2025, 1, 1), 
            new DateOnly(2025, 1, 2), 
            new DateOnly(2025, 1, 3) 
        };
        
        var mockConfigReader = new MockConfigReader(Result<Config>.Success(config));
        var mockStrategy = new MockDateStrategy(dates);
        var mockFactory = new MockDateStrategyFactory(Result<IDateStrategy>.Success(mockStrategy));
        
        var service = new TransferService(mockConfigReader, mockFactory);
        
        // Act
        var result = await service.GetTransferSchedules("test.json");
        
        // Assert
        if (!result.IsSuccess)
            throw new Exception($"Test failed: Expected success but got error: {result.Error}");
            
        if (result.Value!.Count != 3)
            throw new Exception($"Test failed: Expected 3 schedules but got {result.Value!.Count}");
            
        if (result.Value![0].Amount != 1000m)
            throw new Exception($"Test failed: Expected amount 1000 but got {result.Value![0].Amount}");
            
        Console.WriteLine("✓ TestTransferService_Success_ReturnsSchedules");
    }
    
    private static async Task TestTransferService_ConfigReaderFailure_ReturnsError()
    {
        // Arrange
        var mockConfigReader = new MockConfigReader(Result<Config>.Failure("Config read error"));
        var mockFactory = new MockDateStrategyFactory(Result<IDateStrategy>.Success(new MockDateStrategy(Array.Empty<DateOnly>())));
        
        var service = new TransferService(mockConfigReader, mockFactory);
        
        // Act
        var result = await service.GetTransferSchedules("test.json");
        
        // Assert
        if (result.IsSuccess)
            throw new Exception("Test failed: Expected failure but got success");
            
        if (result.Error != "Config read error")
            throw new Exception($"Test failed: Expected 'Config read error' but got '{result.Error}'");
            
        Console.WriteLine("✓ TestTransferService_ConfigReaderFailure_ReturnsError");
    }
    
    private static async Task TestTransferService_StrategyFactoryFailure_ReturnsError()
    {
        // Arrange
        var config = new Config(
            new DateOnly(2025, 1, 1),
            1000m,
            PaymentFrequency.Daily,
            3
        );
        
        var mockConfigReader = new MockConfigReader(Result<Config>.Success(config));
        var mockFactory = new MockDateStrategyFactory(Result<IDateStrategy>.Failure("Strategy not found"));
        
        var service = new TransferService(mockConfigReader, mockFactory);
        
        // Act
        var result = await service.GetTransferSchedules("test.json");
        
        // Assert
        if (result.IsSuccess)
            throw new Exception("Test failed: Expected failure but got success");
            
        if (result.Error != "Strategy not found")
            throw new Exception($"Test failed: Expected 'Strategy not found' but got '{result.Error}'");
            
        Console.WriteLine("✓ TestTransferService_StrategyFactoryFailure_ReturnsError");
    }
}

#endregion
