using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

namespace PressePapierTests.Infrastructure.Wpf
{
    public abstract class WpfApplicationBase : ICodedUIApplication
    {
        public virtual void Start()
        {
            try
            {
                Trace.WriteLine(string.Format("Starting Application {0}", ApplicationExeLocation));

                Application = Microsoft.VisualStudio.TestTools.UITesting.ApplicationUnderTest.Launch(ApplicationExeLocation);
                Application.TechnologyName = "UIA";

                MainWindow = GetMainWindow();
                MainWindow.Find();
                MainWindow.WaitForControlReady();
            }
            catch (Exception)
            {
                Close();
                throw;
            }
        }

        protected abstract WpfWindow GetMainWindow();

        protected abstract string ApplicationExeLocation { get; }

        public WpfWindow MainWindow { get; private set; }

        public Microsoft.VisualStudio.TestTools.UITesting.ApplicationUnderTest Application { get; private set; }

        public virtual void Close()
        {
            Trace.WriteLine("Closing Application");
            if (Application.Process.HasExited)
            {
                Trace.WriteLine("Application Process has already exited (crashed?)");
                Application.Process.Dispose();
                return;
            }
            Application.Process.CloseMainWindow();
            Application.Process.WaitForExit(5000);
            if (!Application.Process.HasExited)
            {
                Trace.WriteLine("Application Failed to exit (modal window open?), killing process");
                Application.Process.Kill();
            }
            Application.Process.Dispose();
            Application = null;
        }
    }
}