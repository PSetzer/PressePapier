using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

// ReSharper disable CheckNamespace
public static class WpfControlExtensions
// ReSharper restore CheckNamespace
{
    public static T Get<T>(this WpfControl container, string automationId) where T : WpfControl, new()
    {
        container.WaitForControlReady();
        var foo = new T
                        {
                            Container = container,
                            SearchProperties =
                                {
                                    {WpfControl.PropertyNames.AutomationId, automationId}
                                },
                            TechnologyName = "UIA"
                        };

        foo.SearchConfigurations.Add(SearchConfiguration.AlwaysSearch);
        foo.Find();
        return foo;
    }

    public static T Get<T>(this WpfControl container, Func<PropertyExpressionCollection> searchProperties) where T : WpfControl, new()
    {
        container.WaitForControlReady();
        var foo = new T
                        {
                            Container = container,
                            TechnologyName = "UIA"
                        };
        foo.SearchProperties.AddRange(searchProperties());

        foo.SearchConfigurations.Add(SearchConfiguration.AlwaysSearch);
        foo.Find();
        return foo;
    }

    public static IEnumerable<T> GetMultiple<T>(this WpfControl container, string automationId) where T : WpfControl, new()
    {
        container.WaitForControlReady();
        var foo = new T
                        {
                            Container = container,
                            SearchProperties =
                                {
                                    {WpfControl.PropertyNames.AutomationId, automationId}
                                },
                            TechnologyName = "UIA"
                        };

        foo.SearchConfigurations.Add(SearchConfiguration.AlwaysSearch);
        return foo.FindMatchingControls().Cast<T>();
    }
}