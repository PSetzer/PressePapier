using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PressePapier
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            base.OnStartup(e);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(GetExMessage(e.ExceptionObject), "PressePapier - Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(GetExMessage(e.Exception), "PressePapier - Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            e.Handled = true;
        }

        private string GetExMessage(Object e)
        {
            Exception ex = e as Exception;
            string exMessage = ex != null ? (ex.InnerException != null ? ex.Message + ex.InnerException.Message : ex.Message) : "";
            return exMessage;
        }
    }
}
