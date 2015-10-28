using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

// ReSharper disable CheckNamespace
public static class WpfEditExtensions
// ReSharper restore CheckNamespace
{
    public static void Enter(this WpfEdit control, string text)
    {
        Keyboard.SendKeys(control, text);
    }
}