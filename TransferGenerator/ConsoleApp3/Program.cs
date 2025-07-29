/*
You are given data in json format indicating start date, frequency(Daily, Weekly, Monthly), amount and count. Write a function that return a list of transfer schedules
*/
using System.Text.Json;

namespace ConsoleApp3;

public class Program
{
    public static void Main(string[] args)
    {
        Result<Config> result = Result<Config>.Success(new Config(new DateOnly(2024, 1, 1), TransferFrequency.Daily, 700, 3));

        Tests.RunAllTests();
    }
}

// Core business service responsible for generating transfer schedules
public class TransferService
{
    private readonly IConfigReader _configReader;
    private readonly IDateStrategyFactory _dateStrategyFactory;

    public TransferService(IConfigReader configReader, IDateStrategyFactory dateStrategyFactory)
    {
        _configReader = configReader;
        _dateStrategyFactory = dateStrategyFactory;
    }

    public Result<List<TransferSchedule>> GenerateSchedules(string jsonString)
    {
        
        var configResult = _configReader.ReadConfig(jsonString);

        if (configResult.IsFailure)
            return configResult.Error!;
        var config = configResult.Value!;

        var dateStrategyResult = _dateStrategyFactory.GetStrategy(config.Frequency);
        if (dateStrategyResult.IsFailure)
            return dateStrategyResult.Error!;

        var dateStrategy = dateStrategyResult.Value!;

        var dates = dateStrategy.GetDates(config.StartDate, config.TransferCount);
        return dates
            .Select(date => new TransferSchedule(date, config.Amount))
            .ToList();
    }
}
// Abstraction for Date Strategy factory to enable flexible strategy selection based on frequency
public interface IDateStrategyFactory
{
    Result<IDateStrategy> GetStrategy(TransferFrequency frequency);
}

public class DateStrategyFactory : IDateStrategyFactory
{
    private readonly Dictionary<TransferFrequency, IDateStrategy> _strategiesCache = new();
    public DateStrategyFactory()
    {
        InitializeStrategies();
    }

    private void InitializeStrategies()
    {
        _strategiesCache[TransferFrequency.Daily] = new DailyStrategy();
        _strategiesCache[TransferFrequency.Weekly] = new WeeklyStrategy();
        _strategiesCache[TransferFrequency.TwiceWeekly] = new TwiceWeeklyStrategy();
        _strategiesCache[TransferFrequency.TwiceWeeklyConstrained] = new TwiceWeeklyConstrainedStrategy();
        _strategiesCache[TransferFrequency.ThriceWeeklyConstrained] = new ThriceWeeklyConstrainedStrategy();
        _strategiesCache[TransferFrequency.Monthly] = new MonthlyStrategy();
        _strategiesCache[TransferFrequency.TwiceMonthly] = new TwiceMonthlyStrategy();
    }

    public Result<IDateStrategy> GetStrategy(TransferFrequency frequency)
    {
        if (_strategiesCache.TryGetValue(frequency, out var strategy))
            return Result<IDateStrategy>.Success(strategy);

        return $"Unsupproted frequency: {frequency}";
    }
}

// Abstration for Date strategy
public interface IDateStrategy
{
    IEnumerable<DateOnly> GetDates(DateOnly startDate, int count);
}

// Generates dates with daily frequency
public class DailyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return startDate.AddDays(i);
        }
    }
}

// Generates dates with weekly frequency
public class WeeklyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return startDate.AddDays(i * 7);
        }
    }
}

public class TwiceWeeklyStrategy : IDateStrategy
{
    private readonly DayOfWeek _firstDay;
    private readonly DayOfWeek _secondDay;

    public TwiceWeeklyStrategy(DayOfWeek firstDay = DayOfWeek.Monday, DayOfWeek secondDay = DayOfWeek.Wednesday)
    {
        _firstDay = firstDay;
        _secondDay = secondDay;
    }

    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        var currentDate = startDate;
        int transferCount = 0;

        while (transferCount < count)
        {
            if (currentDate.DayOfWeek == _firstDay || currentDate.DayOfWeek == _secondDay)
            {
                yield return currentDate;
                transferCount++;
            }
            currentDate = currentDate.AddDays(1);
        }
    }
}

public class TwiceWeeklyConstrainedStrategy : IDateStrategy
{
    private readonly DayOfWeek _firstDay;
    private readonly DayOfWeek _secondDay;

    public TwiceWeeklyConstrainedStrategy(DayOfWeek firstDay = DayOfWeek.Monday, DayOfWeek secondDay = DayOfWeek.Wednesday)
    {
        _firstDay = firstDay;
        _secondDay = secondDay;
    }

    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        var currentDate = startDate;
        var endDate = startDate.AddDays(count * 7);
        int transferCount = 0;

        // Find first transfer day
        while (currentDate.DayOfWeek != _firstDay && currentDate.DayOfWeek != _secondDay)
        {
            currentDate = currentDate.AddDays(1);
        }

        while (currentDate < endDate)
        {
            yield return currentDate;

            // Find next transfer date
            if (currentDate.DayOfWeek == _firstDay)
            {
                // Go to second day
                var daysToAdd = ((int)_secondDay - (int)_firstDay + 7) % 7;
                currentDate = currentDate.AddDays(daysToAdd);
            }
            else
            {
                // Find first day
                var daysToAdd = ((int)_firstDay - (int)_secondDay + 7) % 7;
                currentDate = currentDate.AddDays(daysToAdd);
            }
        }
    }
}

public class ThriceWeeklyConstrainedStrategy : IDateStrategy
{
    private readonly DayOfWeek _firstDay;
    private readonly DayOfWeek _secondDay;
    private readonly DayOfWeek _thirdDay;

    public ThriceWeeklyConstrainedStrategy(DayOfWeek firstDay = DayOfWeek.Monday, DayOfWeek secondDay = DayOfWeek.Wednesday, DayOfWeek thirdDay = DayOfWeek.Friday)
    {
        _firstDay = firstDay;
        _secondDay = secondDay;
        _thirdDay = thirdDay;

    }

    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        var currentDate = startDate;
        var endDate = startDate.AddDays(7 * count);

        var days = new[] { _firstDay, _secondDay, _thirdDay }.OrderBy(d => (int)d).ToArray();

        //First transfer date
        while (!days.Any(d => d == currentDate.DayOfWeek))
        {
            currentDate = currentDate.AddDays(1);
        }

        while (currentDate < endDate)
        {
            yield return currentDate;

            // Find next date
            int dayIndex = Array.FindIndex(days, d => d == currentDate.DayOfWeek);
            if (dayIndex < days.Length - 1)
            {
                var daysToAdd = (int)days[dayIndex + 1] - (int)days[dayIndex];
                currentDate = currentDate.AddDays(daysToAdd);
            }
            else
            {
                var daysToAdd = ((int)days[0] - (int)days[dayIndex] + 7) % 7;
                if (daysToAdd == 0)
                {
                    daysToAdd = 7; // Move to next week
                }
                currentDate = currentDate.AddDays(daysToAdd);
            }
        }
    }
}
// Generates dates with monthly frequency
public class MonthlyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return startDate.AddMonths(i);
        }
    }
}

// Generates dates with monthly frequency
public class TwiceMonthlyStrategy : IDateStrategy
{
    private readonly int _firstDay;
    private readonly int _secondDay;

    public TwiceMonthlyStrategy(int firstDay = 1, int secondDay = 2)
    {
        _firstDay = firstDay;
        _secondDay = secondDay;
    }

    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        var currentDate = startDate;
        int transferCount = 0;


        while (transferCount < count)
        {
            var firstDate = new DateOnly(currentDate.Year, currentDate.Month, Math.Min(_firstDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)));
            var secondDate = new DateOnly(currentDate.Year, currentDate.Month, Math.Min(_secondDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)));

            if (firstDate >= startDate && transferCount < count)
            {
                yield return firstDate;
                transferCount++;
            }

            if (secondDate >= startDate && transferCount < count)
            {
                yield return secondDate;
                transferCount++;
            }
            currentDate = currentDate.AddMonths(1);
        }
    }
}


// Abstration for configuration reading
public interface IConfigReader
{
    Result<Config> ReadConfig(string json);
}

// Json-specific implementation of config reader

public class JsonConfigReader : IConfigReader
{
    public Result<Config> ReadConfig(string json)
    {
        try
        {
            var jsonConfig = JsonSerializer.Deserialize<JsonConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return ValidateAndParse(jsonConfig);
        }
        catch (JsonException ex)
        {
            return $"Unable to deserialize json. {ex.Message}";
        }
    }

    private Result<Config> ValidateAndParse(JsonConfig jsonConfig)
    {
        if (jsonConfig == null)
            return "Configuration is empty";

        if (string.IsNullOrWhiteSpace(jsonConfig.Date))
            return "Date is required";

        if (string.IsNullOrWhiteSpace(jsonConfig.Frequency))
            return "Frequency is required";

        if (jsonConfig.Amount <= 0)
            return "Transfer amount must be greater than zero";

        if (jsonConfig.Count <= 0)
            return "Transfer count must be greater than zero";

        if (!DateOnly.TryParse(jsonConfig.Date, out var startDate))
            return $"Invalid date format: {jsonConfig.Date}";

        if (!Enum.TryParse<TransferFrequency>(jsonConfig.Frequency, true, out var frequency))
            return $"Invalid frequency: {jsonConfig.Frequency}";

        return new Config(
            startDate,
            frequency,
            jsonConfig.Amount,
            jsonConfig.Count
            );
    }
}
// Domain model for validated config data
public record Config(DateOnly StartDate, TransferFrequency Frequency, decimal Amount, int TransferCount);

// Enumeration for strongly typed transfer frequency
public enum TransferFrequency
{
    Daily = 1,
    Weekly,
    TwiceWeekly,
    TwiceWeeklyConstrained,
    ThriceWeeklyConstrained,
    Monthly,
    TwiceMonthly,
    TwiceMonthlyConstrained,
    ThriceMonthlyConstrained
}
// DTO for deserializing JSON. Conforms to expected json structure
public class JsonConfig
{
    public required string Date { get; set; }
    public required string Frequency { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
}

// Immutable Value object that represent a single item in transfer schedules
public record TransferSchedule(DateOnly TransferDate, decimal Amount);

// Result pattern class for better exception handling
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

public static class Tests
{
    // Integration tests

    public static void TestTransferService_ValidJson_ReturnsSchedules()
    {
        // Arrange
        IConfigReader configReader = new JsonConfigReader();
        IDateStrategyFactory dateStrategyFactory = new DateStrategyFactory();
        var transferService = new TransferService(configReader, dateStrategyFactory);

        string jsonData = "{\"Date\": \"2025-01-01\", \"Frequency\": \"ThriceWeeklyConstrained\", \"Amount\": 7000, \"Count\": 3}";

        //  Act
        var result = transferService.GenerateSchedules(jsonData);

        // Assert
        if (!result.IsSuccess)
            throw new Exception($"Test failed. Expected success but got error: {result.Error!}");

        if (result.Value!.Count != 3)
            throw new Exception($"Test failed. Expected 3 schedules but got {result.Value!.Count}");

        Console.WriteLine("Success: TestTransferService_ValidJson_ReturnsSchedules");
    }

    public static void TestTransferService_InvalidJson_ReturnsFailure()
    {
        // Arrange
        IConfigReader configReader = new JsonConfigReader();
        IDateStrategyFactory dateStrategyFactory = new DateStrategyFactory();
        var transferService = new TransferService(configReader, dateStrategyFactory);

        string jsonData = "";

        //  Act
        var result = transferService.GenerateSchedules(jsonData);

        // Assert
        if (!result.IsFailure)
            throw new Exception("Test failed. Expected failure but got success");

        Console.WriteLine("Success: TestTransferService_InvalidJson_ReturnsFailure");
    }

    // Transfer Service tests

    public static void TestTransferService_Success_ReturnsSchedules()
    {
        // Arrange
        var config = new Config(
            new DateOnly(2025, 1, 1),
            TransferFrequency.Daily,
            5600,
            3
            );

        var dates = new List<DateOnly>() {
                new DateOnly(2025, 1, 1),
                new DateOnly(2025, 1, 2),
                new DateOnly(2025, 1, 3)
        };

        var mockDateStrategy = new MockDateStrategy(dates);
        var mockDateStrategyFacory = new MockDateStrategyFactory(Result<IDateStrategy>.Success(mockDateStrategy));
        var mockConfigReader = new MockConfigReader(config);

        var transferService = new TransferService(mockConfigReader, mockDateStrategyFacory);

        // Act
        var result = transferService.GenerateSchedules("test.json");

        // Assert
        if (!result.IsSuccess)
            throw new Exception($"Test failed. Expected success but got error: {result.Error!}");

        if (result.Value!.Count != 3)
            throw new Exception($"Test failed: Expected 3 schedules but got {result.Value!.Count}");

        if (result.Value![0].Amount != config.Amount)
            throw new Exception($"Test failed. Expect amount to be {config.Amount} but got {result.Value![0].Amount}");

        Console.WriteLine("Success: TestTransferService_Success_ReturnsSchedules");
    }

    public static void TestTransferService_DateStrategyFactoryError_ReturnsFailure()
    {
        // Arrange
        var config = new Config(
            new DateOnly(2025, 1, 1),
            TransferFrequency.Daily,
            5600,
            3
            );

        string errorMessage = "Invalid frequency";
        var mockDateStrategyFacory = new MockDateStrategyFactory(Result<IDateStrategy>.Failure(errorMessage));
        var mockConfigReader = new MockConfigReader(config);
        var transferService = new TransferService(mockConfigReader, mockDateStrategyFacory);

        // Act
        var result = transferService.GenerateSchedules("test.json");

        // Assert
        if (!result.IsFailure)
            throw new Exception("Test failed. Expected failure but got success");

        if (result.Error != errorMessage)
            throw new Exception($"Test failed. Expected {errorMessage} but got {result.Error!}");

        Console.WriteLine("Success: TestTransferService_DateStrategyFactoryError_ReturnsFailure");
    }

    public static void TestTransferService_ConfigReaderError_ReturnsFailure()
    {
        // Arrange
        string errorMessage = "Unable to deserialize json";
        var mockConfigReader = new MockConfigReader(Result<Config>.Failure(errorMessage));
        var mockDateStrategyFacory = new MockDateStrategyFactory(Result<IDateStrategy>.Success(new MockDateStrategy(Array.Empty<DateOnly>())));

        var transferService = new TransferService(mockConfigReader, mockDateStrategyFacory);

        // Act
        var result = transferService.GenerateSchedules("test.json");

        // Assert
        if (!result.IsFailure)
            throw new Exception("Test failed. Expected failure but got success");

        if (result.Error != errorMessage)
            throw new Exception($"Test failed. Expected {errorMessage} but got {result.Error!}");

        Console.WriteLine("Success: TestTransferService_ConfigReaderError_ReturnsFailure");
    }

    public static void RunAllTests()
    {
        TestTransferService_ValidJson_ReturnsSchedules();
        TestTransferService_InvalidJson_ReturnsFailure();

        TestTransferService_Success_ReturnsSchedules();
        TestTransferService_DateStrategyFactoryError_ReturnsFailure();
        TestTransferService_ConfigReaderError_ReturnsFailure();
    }
}

public class MockConfigReader : IConfigReader
{
    private readonly Result<Config> _config;

    public MockConfigReader(Result<Config> config)
    {
        _config = config;
    }

    public Result<Config> ReadConfig(string json)
    {
        return _config;
    }
}

public class MockDateStrategy : IDateStrategy
{
    private readonly IEnumerable<DateOnly> _dates;
    public MockDateStrategy(IEnumerable<DateOnly> dates)
    {
        _dates = dates;
    }

    public IEnumerable<DateOnly> GetDates(DateOnly startDate, int count)
    {
        return _dates;
    }
}

public class MockDateStrategyFactory : IDateStrategyFactory
{
    private readonly Result<IDateStrategy> _dateStrategy;
    public MockDateStrategyFactory(Result<IDateStrategy> dateStrategy)
    {
        _dateStrategy = dateStrategy;
    }

    public Result<IDateStrategy> GetStrategy(TransferFrequency frequency)
    {
        return _dateStrategy;
    }
}
#endregion