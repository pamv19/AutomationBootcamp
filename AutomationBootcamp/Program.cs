List<TestResult> testResults = new();

testResults.Add(
    await ExecuteTestAsync(
        "Valid login",
        shouldPass: true
    )
);

testResults.Add(
    await ExecuteTestAsync(
        "Invalid password",
        shouldPass: true
    )
);

testResults.Add(
    await ExecuteTestAsync(
        "Add product to cart",
        shouldPass: false
    )
);

Console.WriteLine();
Console.WriteLine("Test execution summary");
Console.WriteLine("----------------------");

foreach (TestResult testResult in testResults)
{
    Console.WriteLine(
        $"{testResult.Name}: {testResult.Status}"
    );
}

int passedTests = testResults.Count(
    result => result.Status == TestStatus.Passed
);

int failedTests = testResults.Count(
    result => result.Status == TestStatus.Failed
);

Console.WriteLine();
Console.WriteLine($"Total: {testResults.Count}");
Console.WriteLine($"Passed: {passedTests}");
Console.WriteLine($"Failed: {failedTests}");

static async Task<TestResult> ExecuteTestAsync(
    string testName,
    bool shouldPass
)
{
    Console.WriteLine($"Executing: {testName}");

    await Task.Delay(1000);

    TestStatus status = shouldPass
        ? TestStatus.Passed
        : TestStatus.Failed;

    return new TestResult(testName, status);
}

public enum TestStatus
{
    Passed,
    Failed
}

public class TestResult
{
    public string Name { get; set; }
    public TestStatus Status { get; set; }

    public TestResult(
        string name,
        TestStatus status
    )
    {
        Name = name;
        Status = status;
    }
}