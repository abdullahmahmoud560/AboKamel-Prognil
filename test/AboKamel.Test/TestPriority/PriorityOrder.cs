using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Services.Test.TestPriority;

public class PriorityOrder : ITestCaseOrderer
{
    /// <summary>
    /// Orders the test cases by their priority, then by method name.
    /// </summary>
    /// <typeparam name="TTestCase">The test case type.</typeparam>
    /// <param name="testCases">The test cases to be ordered.</param>
    /// <returns>An ordered sequence of test cases.</returns>
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        string assemblyName = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
        var sortedMethods = new SortedDictionary<int, List<TTestCase>>();

        foreach (TTestCase testCase in testCases)
        {
            int priority = testCase.TestMethod.Method
                .GetCustomAttributes(assemblyName)
                .FirstOrDefault()
                ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        foreach (TTestCase testCase in
            sortedMethods.Keys.SelectMany(
                priority => sortedMethods[priority].OrderBy(
                    testCase => testCase.TestMethod.Method.Name)))
        {
            yield return testCase;
        }
    }

    /// <summary>
    /// Gets the value from the dictionary if the key exists, otherwise creates a new value and adds it to the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="dictionary">The dictionary to operate on.</param>
    /// <param name="key">The key to search for.</param>
    /// <returns>The existing or newly created value.</returns>
    private static TValue GetOrCreate<TKey, TValue>(
        IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : struct
        where TValue : new() =>
        dictionary.TryGetValue(key, out TValue? result)
            ? result
            : (dictionary[key] = new TValue());
}
