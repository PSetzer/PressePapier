using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

// ReSharper disable CheckNamespace
public static class WpfWindowExtensions
// ReSharper restore CheckNamespace
{
    public static WpfWindow GetWindow(this WpfWindow parent, string windowTitle)
    {
        var modalWindow = new WpfWindow
                                {
                                    SearchProperties =
                                        {
                                            {UITestControl.PropertyNames.Name, windowTitle}
                                        }
                                };
        modalWindow.Find();
        return modalWindow;
    }

    public static WpfWindow AccessKeyPress(this WpfWindow currentScreen, string accessKey)
    {
        Keyboard.PressModifierKeys(ModifierKeys.Alt);
        Keyboard.SendKeys(accessKey);
        Keyboard.ReleaseModifierKeys(ModifierKeys.Alt);

        return currentScreen;
    }
}