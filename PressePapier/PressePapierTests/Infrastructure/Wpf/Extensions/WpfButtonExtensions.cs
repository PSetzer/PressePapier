using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

// ReSharper disable CheckNamespace
public static class WpfButtonExtensions
// ReSharper restore CheckNamespace
{
    public static void Click(this WpfButton button)
    {
        Mouse.Click(button);
    }
}