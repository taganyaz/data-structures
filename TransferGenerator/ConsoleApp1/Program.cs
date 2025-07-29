using System.Text.Json;
using System.Collections.Generic;

namespace ConsoleApp1;

/*
You are given data in json format indicating start date, frequency(Daily, Weekly, Monthly), amount and count. Write a function that return a list of transfer schedules
*/

public class Program
{
    public static void Main(string[] args)
    {
        var configReader = new JsonConfigReader();
        var dateStrategyFactory = new DateStrategyFactory(false);

        var service = new TransferService(configReader, dateStrategyFactory);

        try
        {
            var schedules = service.GenerateSchedules("config.json");

            foreach (var schedule in schedules)
            {
                Console.WriteLine($"Transfer Date: {schedule.TransferDate}, Amount: {schedule.Amount}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");

        }
    }
}

// Business service used to orchestrate schedule generation
public class TransferService
{
    private readonly IConfigReader _configReader;
    private readonly IDateStrategyFactory _dateStrategyFactory;

    public TransferService(IConfigReader configReader, IDateStrategyFactory dateStrategyFactory)
    {
        _configReader = configReader;
        _dateStrategyFactory = dateStrategyFactory;
    }

    public IEnumerable<TransferSchedule> GenerateSchedules(string filePath)
    {
        var config = _configReader.ReadConfig(filePath);
        var dateStrategy = _dateStrategyFactory.GetStrategy(config.Frequency);

        var dates = dateStrategy.GenerateDates(config.StartDate, config.TransferCount);

        return dates
            .Select(d => new TransferSchedule(d, config.TransferAmount));
    }
}
// DateStrategy Factory interface - ensures flexibility in DateStrategy selection based on PaymentFrequency
public interface IDateStrategyFactory
{
    IDateStrategy GetStrategy(PaymentFrequency frequency);
}
// ConfigReader interface - allow supporting different file format and sources
public interface IConfigReader
{
    Config ReadConfig(string filePath);
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
    public Config ReadConfig(string filePath)
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
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to read configuration", ex);
        }
    }

    private Config ValidateAndParse(string json)
    {
        JsonConfig? jsonConfig = null;
        try
        {
            jsonConfig = JsonSerializer.Deserialize<JsonConfig>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($" Unable to deserialize configuration. {ex.Message}");

        }

        if (jsonConfig == null)
        {
            throw new InvalidOperationException("Configuration file is empty");
        }

        if (string.IsNullOrWhiteSpace(jsonConfig.Date))
            throw new ArgumentException("Date is required", nameof(jsonConfig.Date));

        if (string.IsNullOrWhiteSpace(jsonConfig.Frequency))
            throw new ArgumentException("Frequency is required", nameof(jsonConfig.Frequency));

        if (jsonConfig.Amount <= 0)
            throw new ArgumentException("Transfer amount must be greater than zero.", nameof(jsonConfig.Amount));

        if (jsonConfig.Count <= 0)
            throw new ArgumentException("Transfer Count must be greater than zero", nameof(jsonConfig.Count));

        if (!DateOnly.TryParse(jsonConfig.Date, out var startDate))
            throw new ArgumentException($"Invalid date format: {jsonConfig.Date}", nameof(jsonConfig.Date));

        if (!Enum.TryParse<PaymentFrequency>(jsonConfig.Frequency, true, out var frequency))
            throw new ArgumentException($"Invalid frequency: {jsonConfig.Frequency}", nameof(jsonConfig.Frequency));

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

    public IDateStrategy GetStrategy(PaymentFrequency frequency)
    {
        if (_strategiesCache.TryGetValue(frequency, out var strategy))
            return strategy;

        throw new InvalidOperationException($"Unsupported frequency: {frequency}");
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
