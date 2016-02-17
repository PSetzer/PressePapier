using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressePapier.ViewModel
{
    /// <summary>
    /// Cette classe n'est pas utilisée pour le moment dans l'application, elle n'existe que pour exemple
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class RelayCommandGeneric<T> : ICommand
    {
        private readonly Action<T> _action;

        public RelayCommandGeneric(Action<T> action)
        {
            _action = action;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //possible InvalidCastException, mais l'interface ICommand spécifie un paramètre object
            _action((T)parameter);
        }
    }
}
