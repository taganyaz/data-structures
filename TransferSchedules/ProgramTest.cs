// Add this at the end of your Program.cs file
namespace Tests_App;
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

// Uncomment to run tests
// await Tests.RunAllTests();
// Additional tests for comprehensive coverage
#region Additional Tests

// Simple test framework for better organization
public class TestResult
{
    public string TestName { get; set; }
    public bool Passed { get; set; }
    public string? ErrorMessage { get; set; }
    
    public TestResult(string testName, bool passed, string? errorMessage = null)
    {
        TestName = testName;
        Passed = passed;
        ErrorMessage = errorMessage;
    }
}

public static class TestFramework
{
    private static List<TestResult> _results = new();
    
    public static void RunTest(string testName, Action testAction)
    {
        try
        {
            testAction();
            _results.Add(new TestResult(testName, true));
            Console.WriteLine($"✓ {testName}");
        }
        catch (Exception ex)
        {
            _results.Add(new TestResult(testName, false, ex.Message));
            Console.WriteLine($"✗ {testName}: {ex.Message}");
        }
    }
    
    public static async Task RunTestAsync(string testName, Func<Task> testAction)
    {
        try
        {
            await testAction();
            _results.Add(new TestResult(testName, true));
            Console.WriteLine($"✓ {testName}");
        }
        catch (Exception ex)
        {
            _results.Add(new TestResult(testName, false, ex.Message));
            Console.WriteLine($"✗ {testName}: {ex.Message}");
        }
    }
    
    public static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new Exception($"Assertion failed: {message}");
    }
    
    public static void AssertEqual<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception($"Expected {expected} but got {actual}");
    }
    
    public static void PrintSummary()
    {
        var passed = _results.Count(r => r.Passed);
        var failed = _results.Count(r => !r.Passed);
        
        Console.WriteLine($"\n=== Test Summary ===");
        Console.WriteLine($"Total: {_results.Count} | Passed: {passed} | Failed: {failed}");
        
        if (failed > 0)
        {
            Console.WriteLine("\nFailed Tests:");
            foreach (var failure in _results.Where(r => !r.Passed))
            {
                Console.WriteLine($"  - {failure.TestName}: {failure.ErrorMessage}");
            }
        }
    }
    
    public static void Reset()
    {
        _results.Clear();
    }
}

// Comprehensive test suite
public static class ComprehensiveTests
{
    public static async Task RunAll()
    {
        Console.WriteLine("\n=== Running Comprehensive Tests ===\n");
        TestFramework.Reset();
        
        // Date Strategy Tests
        await TestDateStrategies();
        
        // Factory Tests
        await TestDateStrategyFactories();
        
        // Config Reader Tests
        await TestJsonConfigReader();
        
        // Integration Tests
        await TestIntegrationScenarios();
        
        TestFramework.PrintSummary();
    }
    
    private static async Task TestDateStrategies()
    {
        Console.WriteLine("\n--- Date Strategy Tests ---");
        
        // Daily Strategy Tests
        TestFramework.RunTest("DailyStrategy_GeneratesCorrectDates", () =>
        {
            var strategy = new DailyStrategy();
            var startDate = new DateOnly(2025, 1, 1);
            var dates = strategy.GenerateDates(startDate, 5).ToList();
            
            TestFramework.AssertEqual(5, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 1), dates[0]);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 5), dates[4]);
        });
        
        TestFramework.RunTest("DailyWeekendAwareStrategy_SkipsWeekends", () =>
        {
            var strategy = new DailyWeekendAwareStrategy();
            var startDate = new DateOnly(2025, 1, 3); // Friday
            var dates = strategy.GenerateDates(startDate, 3).ToList();
            
            TestFramework.AssertEqual(3, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 3), dates[0]); // Friday
            TestFramework.AssertEqual(new DateOnly(2025, 1, 6), dates[1]); // Monday (skipped weekend)
            TestFramework.AssertEqual(new DateOnly(2025, 1, 6), dates[2]); // Monday
        });
        
        // Weekly Strategy Tests
        TestFramework.RunTest("WeeklyStrategy_GeneratesWeeklyDates", () =>
        {
            var strategy = new WeeklyStrategy();
            var startDate = new DateOnly(2025, 1, 1);
            var dates = strategy.GenerateDates(startDate, 3).ToList();
            
            TestFramework.AssertEqual(3, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 1), dates[0]);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 8), dates[1]);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 15), dates[2]);
        });
        
        TestFramework.RunTest("WeeklyWeekendAwareStrategy_AdjustsWeekendDates", () =>
        {
            var strategy = new WeeklyWeekendAwareStrategy();
            var startDate = new DateOnly(2025, 1, 4); // Saturday
            var dates = strategy.GenerateDates(startDate, 2).ToList();
            
            TestFramework.AssertEqual(2, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 6), dates[0]); // Adjusted to Monday
            TestFramework.AssertEqual(new DateOnly(2025, 1, 13), dates[1]); // Next Monday
        });
        
        // Monthly Strategy Tests
        TestFramework.RunTest("MonthlyStrategy_GeneratesMonthlyDates", () =>
        {
            var strategy = new MonthlyStrategy();
            var startDate = new DateOnly(2025, 1, 15);
            var dates = strategy.GenerateDates(startDate, 3).ToList();
            
            TestFramework.AssertEqual(3, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 15), dates[0]);
            TestFramework.AssertEqual(new DateOnly(2025, 2, 15), dates[1]);
            TestFramework.AssertEqual(new DateOnly(2025, 3, 15), dates[2]);
        });
        
        TestFramework.RunTest("MonthlyStrategy_HandlesMonthEndCorrectly", () =>
        {
            var strategy = new MonthlyStrategy();
            var startDate = new DateOnly(2025, 1, 31);
            var dates = strategy.GenerateDates(startDate, 3).ToList();
            
            TestFramework.AssertEqual(3, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 1, 31), dates[0]);
            TestFramework.AssertEqual(new DateOnly(2025, 2, 28), dates[1]); // February has 28 days
            TestFramework.AssertEqual(new DateOnly(2025, 3, 31), dates[2]);
        });
        
        TestFramework.RunTest("MonthlyWeekendAwareStrategy_AdjustsWeekends", () =>
        {
            var strategy = new MonthlyWeekendAwareStrategy();
            var startDate = new DateOnly(2025, 5, 31); // Saturday
            var dates = strategy.GenerateDates(startDate, 2).ToList();
            
            TestFramework.AssertEqual(2, dates.Count);
            TestFramework.AssertEqual(new DateOnly(2025, 6, 2), dates[0]); // Adjusted to Monday
            TestFramework.AssertEqual(new DateOnly(2025, 7, 1), dates[1]); // Tuesday
        });
    }
    
    private static async Task TestDateStrategyFactories()
    {
        Console.WriteLine("\n--- Factory Tests ---");
        
        TestFramework.RunTest("DateStrategyFactory_ReturnsCorrectStrategies", () =>
        {
            var factory = new DateStrategyFactory(false);
            
            var dailyResult = factory.GetDateStrategy(PaymentFrequency.Daily);
            TestFramework.Assert(dailyResult.IsSuccess, "Daily strategy should succeed");
            TestFramework.Assert(dailyResult.Value is DailyStrategy, "Should return DailyStrategy");
            
            var weeklyResult = factory.GetDateStrategy(PaymentFrequency.Weekly);
            TestFramework.Assert(weeklyResult.IsSuccess, "Weekly strategy should succeed");
            TestFramework.Assert(weeklyResult.Value is WeeklyStrategy, "Should return WeeklyStrategy");
            
            var monthlyResult = factory.GetDateStrategy(PaymentFrequency.Monthly);
            TestFramework.Assert(monthlyResult.IsSuccess, "Monthly strategy should succeed");
            TestFramework.Assert(monthlyResult.Value is MonthlyStrategy, "Should return MonthlyStrategy");
        });
        
        TestFramework.RunTest("DateStrategyFactory_WithSkipWeekends_ReturnsWeekendAwareStrategies", () =>
        {
            var factory = new DateStrategyFactory(true);
            
            var dailyResult = factory.GetDateStrategy(PaymentFrequency.Daily);
            TestFramework.Assert(dailyResult.Value is DailyWeekendAwareStrategy, "Should return DailyWeekendAwareStrategy");
            
            var weeklyResult = factory.GetDateStrategy(PaymentFrequency.Weekly);
            TestFramework.Assert(weeklyResult.Value is WeeklyWeekendAwareStrategy, "Should return WeeklyWeekendAwareStrategy");
            
            var monthlyResult = factory.GetDateStrategy(PaymentFrequency.Monthly);
            TestFramework.Assert(monthlyResult.Value is MonthlyWeekendAwareStrategy, "Should return MonthlyWeekendAwareStrategy");
        });
        
        TestFramework.RunTest("DateStrategyFactoryWithCache_ReturnsCachedStrategies", () =>
        {
            var factory = new DateStrategyFactoryWithCache(true);
            
            var result1 = factory.GetDateStrategy(PaymentFrequency.Daily);
            var result2 = factory.GetDateStrategy(PaymentFrequency.Daily);
            
            TestFramework.Assert(result1.IsSuccess && result2.IsSuccess, "Both should succeed");
            TestFramework.Assert(ReferenceEquals(result1.Value, result2.Value), "Should return same cached instance");
        });
    }
    
    private static async Task TestJsonConfigReader()
    {
        Console.WriteLine("\n--- Config Reader Tests ---");
        
        await TestFramework.RunTestAsync("JsonConfigReader_ValidatesEmptyDate", async () =>
        {
            var reader = new JsonConfigReader();
            var json = """{"date": "", "amount": 100, "frequency": "Daily", "count": 5}""";
            var tempFile = Path.GetTempFileName();
            
            try
            {
                await File.WriteAllTextAsync(tempFile, json);
                var result = await reader.ReadConfigAsync(tempFile);
                
                TestFramework.Assert(result.IsFailure, "Should fail with empty date");
                TestFramework.Assert(result.Error!.Contains("Date cannot be empty"), "Should have correct error message");
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
        
        await TestFramework.RunTestAsync("JsonConfigReader_ValidatesNegativeAmount", async () =>
        {
            var reader = new JsonConfigReader();
            var json = """{"date": "2025-01-01", "amount": -100, "frequency": "Daily", "count": 5}""";
            var tempFile = Path.GetTempFileName();
            
            try
            {
                await File.WriteAllTextAsync(tempFile, json);
                var result = await reader.ReadConfigAsync(tempFile);
                
                TestFramework.Assert(result.IsFailure, "Should fail with negative amount");
                TestFramework.Assert(result.Error!.Contains("amount must be greater"), "Should have correct error message");
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
        
        await TestFramework.RunTestAsync("JsonConfigReader_ValidatesZeroCount", async () =>
        {
            var reader = new JsonConfigReader();
            var json = """{"date": "2025-01-01", "amount": 100, "frequency": "Daily", "count": 0}""";
            var tempFile = Path.GetTempFileName();
            
            try
            {
                await File.WriteAllTextAsync(tempFile, json);
                var result = await reader.ReadConfigAsync(tempFile);
                
                TestFramework.Assert(result.IsFailure, "Should fail with zero count");
                TestFramework.Assert(result.Error!.Contains("count must be greater"), "Should have correct error message");
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
        
        await TestFramework.RunTestAsync("JsonConfigReader_ParsesValidConfigCaseInsensitive", async () =>
        {
            var reader = new JsonConfigReader();
            var json = """{"DATE": "2025-01-01", "AMOUNT": 100, "FREQUENCY": "daily", "COUNT": 5}""";
            var tempFile = Path.GetTempFileName();
            
            try
            {
                await File.WriteAllTextAsync(tempFile, json);
                var result = await reader.ReadConfigAsync(tempFile);
                
                TestFramework.Assert(result.IsSuccess, "Should succeed with valid config");
                TestFramework.AssertEqual(new DateOnly(2025, 1, 1), result.Value!.StartDate);
                TestFramework.AssertEqual(100m, result.Value!.Amount);
                TestFramework.AssertEqual(PaymentFrequency.Daily, result.Value!.PaymentFrequency);
                TestFramework.AssertEqual(5, result.Value!.TransferCount);
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
    }
    
    private static async Task TestIntegrationScenarios()
    {
        Console.WriteLine("\n--- Integration Tests ---");
        
        await TestFramework.RunTestAsync("TransferService_EndToEnd_DailyTransfers", async () =>
        {
            var json = """{"date": "2025-01-01", "amount": 1000, "frequency": "Daily", "count": 3}""";
            var tempFile = Path.GetTempFileName();
            
            try
            {
                await File.WriteAllTextAsync(tempFile, json);
                
                var configReader = new JsonConfigReader();
                var strategyFactory = new DateStrategyFactory(false);
                var service = new TransferService(configReader, strategyFactory);
                
                var result = await service.GetTransferSchedules(tempFile);
                
                TestFramework.Assert(result.IsSuccess, "Should succeed");
                TestFramework.AssertEqual(3, result.Value!.Count);
                TestFramework.AssertEqual(new DateOnly(2025, 1, 1), result.Value![0].TransferDate);
                TestFramework.AssertEqual(1000m, result.Value![0].Amount);
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
        
        await TestFramework.RunTestAsync("TransferService_EndToEnd_WeekendAwareMonthly", async () =>
        {
            var json = """{"date": "2025-05-31", "amount": 2500, "frequency": "Monthly", "count": 2}""";
            var tempFile = Path.GetTempFileName();
            
            try
            {
                await File.WriteAllTextAsync(tempFile, json);
                
                var configReader = new JsonConfigReader();
                var strategyFactory = new DateStrategyFactory(true); // Skip weekends
                var service = new TransferService(configReader, strategyFactory);
                
                var result = await service.GetTransferSchedules(tempFile);
                
                TestFramework.Assert(result.IsSuccess, "Should succeed");
                TestFramework.AssertEqual(2, result.Value!.Count);
                TestFramework.AssertEqual(new DateOnly(2025, 6, 2), result.Value![0].TransferDate); // May 31 is Saturday, adjusted to Monday
                TestFramework.AssertEqual(2500m, result.Value![0].Amount);
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
    }
}

// Update the main Tests class to use the comprehensive suite
public static class UpdatedTests
{
    public static async Task RunAllTestsWithFramework()
    {
        // Run original tests
        await Tests.RunAllTests();
        
        // Run comprehensive tests
        await ComprehensiveTests.RunAll();
    }
}

#endregion