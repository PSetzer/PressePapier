using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

namespace PressePapierTests.Infrastructure.Wpf
{
    public class WpfScreen
    {
        public WpfApplicationBase Application { get; set; }
        public WpfWindow Window { get; set; }

        public WpfScreen(WpfApplicationBase application, WpfWindow window)
        {
            Application = application;
            Window = window;
        }
    }
}