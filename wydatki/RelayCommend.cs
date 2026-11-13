using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace wydatki
{
    public class RelayCommend : ICommand
    {
        private readonly Action _wykonaj;
        public RelayCommend(Action wykonaj) => _wykonaj = wykonaj;
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _wykonaj();
    }
}
