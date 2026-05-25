using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Test.TestPriority;

/// <summary>
/// Specifies the priority of a test method for ordered execution.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestPriorityAttribute : Attribute
{
    /// <summary>
    /// Gets the priority value of the test method.
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPriorityAttribute"/> class with a specified priority.
    /// </summary>
    /// <param name="priority">The priority value of the test method.</param>
    public TestPriorityAttribute(int priority) => Priority = priority;
}
