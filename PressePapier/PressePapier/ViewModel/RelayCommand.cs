using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PressePapier.ViewModel
{
    class RelayCommand : ICommand
    {
        private readonly Action _action;
        private readonly Action<string> _actionS;

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public RelayCommand(Action<string> action)
        {
            _actionS = action;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            string s = parameter as string;
            if (s != null) _actionS(s);
            else _action();
        }
    }
}
