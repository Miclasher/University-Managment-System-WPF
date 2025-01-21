using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Domain.ViewModels
{
    abstract public class BaseViewModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
