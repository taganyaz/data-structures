/*
You are given data in json format indicating start date, frequency(Daily, Weekly, Monthly), amount and count. Write a function that return a list of transfer schedules
*/
using System.Text.Json;
namespace ConsoleApp2;
public class Program
{
    public static void Main(string[] args)
    {
        var configReader = new JsonConfigReader();
        var dateStrategyFactory = new DateStrategyFactory(false);

        var service = new TransferService(configReader, dateStrategyFactory);
        var schedulesResult = service.GenerateSchedules("config.json");
        if (schedulesResult.IsSuccess)
        {

            var schedules = schedulesResult.Value!;

            foreach (var schedule in schedules)
            {
                Console.WriteLine($"Transfer Date: {schedule.TransferDate}, Amount: {schedule.Amount}");
            }

        }
        else
        {
            Console.WriteLine($"Error occurred: {schedulesResult.Error!}");
        }

        Tests.RunAllTests();
    }
}

// Cor business service used to orchestrate schedule generation
public class TransferService
{
    private readonly IConfigReader _configReader;
    private readonly IDateStrategyFactory _dateStrategyFactory;

    public TransferService(IConfigReader configReader, IDateStrategyFactory dateStrategyFactory)
    {
        _configReader = configReader;
        _dateStrategyFactory = dateStrategyFactory;
    }

    public Result<List<TransferSchedule>> GenerateSchedules(string filePath)
    {
        var configResult = _configReader.ReadConfig(filePath);
        if (configResult.IsFailure)
            return configResult.Error!;

        var config = configResult.Value!;

        var dateStrategyResult = _dateStrategyFactory.GetStrategy(config.Frequency);
        if (dateStrategyResult.IsFailure)
            return dateStrategyResult.Error!;

        var dateStrategy = dateStrategyResult.Value!;

        var dates = dateStrategy.GenerateDates(config.StartDate, config.TransferCount);

        return dates
            .Select(d => new TransferSchedule(d, config.TransferAmount))
            .ToList();
    }
}
// DateStrategy Factory interface - ensures flexibility in DateStrategy selection based on PaymentFrequency
public interface IDateStrategyFactory
{
    Result<IDateStrategy> GetStrategy(PaymentFrequency frequency);
}
// ConfigReader interface - allow supporting different file format and sources
public interface IConfigReader
{
    Result<Config> ReadConfig(string filePath);
}

// DateStrategy interface - allow flexibility in dates generation logic for different frequencies
public interface IDateStrategy
{
    IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int transferCount);
}

// Domail model - allow separation of internal and external data represenation
public record Config(DateOnly StartDate, PaymentFrequency Frequency, decimal TransferAmount, int TransferCount);

public enum PaymentFrequency
{
    Daily = 1,
    Weekly,
    Monthly
}

public record TransferSchedule(DateOnly TransferDate, decimal Amount);

// DTO for json configuration
public class JsonConfig
{
    public required string Date { get; set; }
    public required string Frequency { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
}

public class JsonConfigReader : IConfigReader
{
    public Result<Config> ReadConfig(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Configurarion file was not found: {filePath}");
        }
        try
        {
            var json = File.ReadAllText(filePath);
            return ValidateAndParse(json);
        }
        catch (Exception ex)
        {
            return $"Unable to read configuration: {ex.Message}";
        }
    }

    private Result<Config> ValidateAndParse(string json)
    {
        JsonConfig? jsonConfig;
        try
        {
            jsonConfig = JsonSerializer.Deserialize<JsonConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            return $" Unable to deserialize configuration. {ex.Message}";
        }

        if (jsonConfig == null)
        {
            return "Configuration file is empty";
        }

        if (string.IsNullOrWhiteSpace(jsonConfig.Date))
            return "Date is required";

        if (string.IsNullOrWhiteSpace(jsonConfig.Frequency))
            return "Frequency is required";

        if (jsonConfig.Amount <= 0)
            return "Transfer amount must be greater than zero.";

        if (jsonConfig.Count <= 0)
            return "Transfer Count must be greater than zero";

        if (!DateOnly.TryParse(jsonConfig.Date, out var startDate))
            return $"Invalid date format: {jsonConfig.Date}";

        if (!Enum.TryParse<PaymentFrequency>(jsonConfig.Frequency, true, out var frequency))
            return $"Invalid frequency: {jsonConfig.Frequency}";

        return new Config(
            startDate,
            frequency,
            jsonConfig.Amount,
            jsonConfig.Count
            );
    }
}

public class DateStrategyFactory : IDateStrategyFactory
{
    private readonly bool _skipWeekends;
    private readonly Dictionary<PaymentFrequency, IDateStrategy> _strategiesCache = new();

    public DateStrategyFactory(bool skipWeekends = false)
    {
        _skipWeekends = skipWeekends;
        InitializeStrategy();
    }

    private void InitializeStrategy()
    {
        _strategiesCache[PaymentFrequency.Daily] = new DailyStrategy();
        _strategiesCache[PaymentFrequency.Weekly] = new WeeklyStrategy();
        _strategiesCache[PaymentFrequency.Monthly] = new MonthlyStrategy();
    }

    public Result<IDateStrategy> GetStrategy(PaymentFrequency frequency)
    {
        if (_strategiesCache.TryGetValue(frequency, out var strategy))
            return Result<IDateStrategy>.Success(strategy);

        return $"Unsupported frequency: {frequency}";
    }
}

public class DailyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return startDate.AddDays(i);
        }
    }
}

public class WeeklyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return startDate.AddDays(i * 7);
        }
    }
}

public class MonthlyStrategy : IDateStrategy
{
    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return startDate.AddMonths(i);
        }
    }
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

public static class Tests
{
    public static void RunAllTests()
    {
        TransferService_Success_ReturnsSchedules();
        TransferService_ConfigReaderFailure_ReturnsError();
        TransferService_DateStrategyFactoryFailure_ReturnsError();

        TestDateStrategyFactory_ValidFrequency_ReturnsStrategy();
        TestDateStrategyFactory_InvalidFrequency_ReturnError();

        TestDailyStrategy_GenerateDates_ReturnsCorrectCount();
        TestWeeklyStrategy_GenerateDates_ReturnsCorrectCount();
        TestMonthlyStrategy_GenerateDates_ReturnsCorrectCount();

    }

    // TransferService tests
    public static void TransferService_Success_ReturnsSchedules()
    {
        // Arrange
        var config = new Config(
        new DateOnly(2025, 1, 1),
        PaymentFrequency.Daily,
        1000m,
        3);

        var dates = new List<DateOnly> {
            new DateOnly(2025,1, 1),
            new DateOnly(2025,1, 2),
            new DateOnly(2025,1, 3),
        };

        var dateStrategy = new MockDateStrategy(dates);
        var configReader = new MockConfigReader(config);
        var dateStrategyFactory = new MockDateStrategyFactory(Result<IDateStrategy>.Success(dateStrategy));

        var transferService = new TransferService(configReader, dateStrategyFactory);

        // Act
        var result = transferService.GenerateSchedules("test.json");

        // Assert
        if (result.IsFailure)
            throw new Exception($"Test failed. Expected success but got error: {result.Error!}");

        if (result.Value!.Count != 3)
            throw new Exception($"Test failed. Expected 3 schedules but got {result.Value!.Count}");

        if (result.Value![0].Amount != config.TransferAmount)
            throw new Exception($"Test Failed. Expected amount to be {config.TransferAmount} but got {result.Value![0].Amount}");

        Console.WriteLine("Success: TransferService_Success_ReturnsSchedules");
    }

    public static void TransferService_ConfigReaderFailure_ReturnsError()
    {
        // Arrange
        string configErrorMessage = "Unable to read configuration";
        var mockConfigReader = new MockConfigReader(Result<Config>.Failure(configErrorMessage));
        var mockDateStrategyFactory = new MockDateStrategyFactory(Result<IDateStrategy>.Success(new MockDateStrategy(Array.Empty<DateOnly>())));

        var transferService = new TransferService(mockConfigReader, mockDateStrategyFactory);

        // Act
        var result = transferService.GenerateSchedules("test.json");

        // Assert
        if (result.IsSuccess)
            throw new Exception("Test failed. Expected failure but got success");

        if (result.Error != configErrorMessage)
            throw new Exception($"Test failed. Expected {configErrorMessage} but got {result.Error!}");

        Console.WriteLine("Success: TransferService_ConfigReaderError_ReturnsFailure");
    }

    public static void TransferService_DateStrategyFactoryFailure_ReturnsError()
    {
        // Arrange
        var config = new Config(
        new DateOnly(2025, 1, 1),
        PaymentFrequency.Daily,
        1000m,
        3);

        var strategyFactoryErrorMessage = "Unsupported transfer frequency";

        var mockConfigReader = new MockConfigReader(Result<Config>.Success(config));
        var mockDateStrategyFactory = new MockDateStrategyFactory(Result<IDateStrategy>.Failure(strategyFactoryErrorMessage));

        var transferService = new TransferService(mockConfigReader, mockDateStrategyFactory);

        // Act
        var result = transferService.GenerateSchedules("test.json");

        // Assert
        if (result.IsSuccess)
            throw new Exception("Test failed. Expected failure but got success");

        if (result.Error != strategyFactoryErrorMessage)
            throw new Exception($"Test failed. Expected {strategyFactoryErrorMessage} but got {result.Error!}");

        Console.WriteLine("Success: TransferService_DateStrategyFactoryFailure_ReturnsError");
    }

    // DateStrategyFactory tests
    public static void TestDateStrategyFactory_ValidFrequency_ReturnsStrategy()
    {
        // Arrange
        var frequency = PaymentFrequency.Daily;
        var dateStrategyFactory = new DateStrategyFactory();

        // Act
        var result = dateStrategyFactory.GetStrategy(frequency);

        // Assert
        if (result.IsFailure)
            throw new Exception($"Test failed. Expected success but got error: {result.Error!}");

        if (result.Value is not DailyStrategy)
            throw new Exception($"Test failed. Expected DailyStrategy but got {result.Value!.GetType()}");

        Console.WriteLine("Success: DateStrategyFactory_Valid_Frequency_ReturnDateStrategy");

    }

    public static void TestDateStrategyFactory_InvalidFrequency_ReturnError()
    {
        // Arrange 
        var frequency = default(PaymentFrequency);
        var dateStrategyFactory = new DateStrategyFactory();
        var errorMessage = $"Unsupported frequency: {frequency}";

        // Act
        var result = dateStrategyFactory.GetStrategy(frequency);

        // Assert
        if (result.IsSuccess)
            throw new Exception("Test failed. Expected failure but got success");

        if (result.Error != errorMessage)
            throw new Exception($"Test failed. Expected {errorMessage} but got {result.Error!}");

        Console.WriteLine("Success: DateStrategyFactory_Invalid_Frequency_ReturnFailure");
    }

    // DateStrategy tests

    public static void TestDailyStrategy_GenerateDates_ReturnsCorrectCount()
    {
        // Arrange
        int count = 3;
        DateOnly startDate = new DateOnly(2025, 1, 1);
        DateOnly lastDate = startDate.AddDays(2);
        var dateStrategy = new DailyStrategy();

        // Act
        var dates = dateStrategy.GenerateDates(startDate, count).ToList();

        // Assert
        if (dates.Count() != count)
            throw new Exception($"Test failed. Expected dates count to be {count} but got {dates.Count()}");

        if (dates[0] != startDate)
            throw new Exception($"Test failed. Expected first date to be {startDate} but got {dates[0]}");

        if (dates[2] != lastDate)
            throw new Exception($"Test failed. Expected last date to be {lastDate} but got {dates[2]}");

        Console.WriteLine("Success: TestDailyStrategy_GenerateDates_ReturnsCorrectCount");
    }

    public static void TestWeeklyStrategy_GenerateDates_ReturnsCorrectCount()
    {
        // Arrange
        int count = 3;
        DateOnly startDate = new DateOnly(2025, 1, 1);
        DateOnly lastDate = startDate.AddDays(2 * 7);
        var dateStrategy = new WeeklyStrategy();

        // Act
        var dates = dateStrategy.GenerateDates(startDate, count).ToList();

        // Assert
        if (dates.Count() != count)
            throw new Exception($"Test failed. Expected dates count to be {count} but got {dates.Count()}");

        if (dates[0] != startDate)
            throw new Exception($"Test failed. Expected first date to be {startDate} but got {dates[0]}");

        if (dates[2] != lastDate)
            throw new Exception($"Test failed. Expected last date to be {lastDate} but got {dates[2]}");

        Console.WriteLine("Success: TestWeeklyStrategy_GenerateDates_ReturnsCorrectCount");
    }

    public static void TestMonthlyStrategy_GenerateDates_ReturnsCorrectCount()
    {
        // Arrange
        int count = 3;
        DateOnly startDate = new DateOnly(2025, 1, 1);
        DateOnly lastDate = startDate.AddMonths(2);
        var dateStrategy = new MonthlyStrategy();

        // Act
        var dates = dateStrategy.GenerateDates(startDate, count).ToList();

        // Assert
        if (dates.Count() != count)
            throw new Exception($"Test failed. Expected dates count to be {count} but got {dates.Count()}");

        if (dates[0] != startDate)
            throw new Exception($"Test failed. Expected first date to be {startDate} but got {dates[0]}");

        if (dates[2] != lastDate)
            throw new Exception($"Test failed. Expected last date to be {lastDate} but got {dates[2]}");

        Console.WriteLine("Success: TestDailyStrategy_GenerateDates_ReturnsCorrectCount");
    }
}

public class MockConfigReader : IConfigReader
{
    private readonly Result<Config> _config;
    public MockConfigReader(Result<Config> config)
    {
        _config = config;
    }

    public Result<Config> ReadConfig(string filePath)
    {
        return _config;
    }
}

public class MockDateStrategyFactory : IDateStrategyFactory
{
    private readonly Result<IDateStrategy> _dateStrategy;
    public MockDateStrategyFactory(Result<IDateStrategy> dateStrategy)
    {
        _dateStrategy = dateStrategy;
    }

    public Result<IDateStrategy> GetStrategy(PaymentFrequency frequency)
    {
        return _dateStrategy;
    }
}

public class MockDateStrategy : IDateStrategy
{
    private readonly IEnumerable<DateOnly> _dates;
    public MockDateStrategy(IEnumerable<DateOnly> dates)
    {
        _dates = dates;
    }

    public IEnumerable<DateOnly> GenerateDates(DateOnly startDate, int count)
    {
        return _dates;
    }
}

#endregion