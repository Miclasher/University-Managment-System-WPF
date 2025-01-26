using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using University.Domain.Commands;

namespace University.Domain.ViewModels
{
    public abstract class BaseCrudViewModel : BaseViewModel
    {
        private bool _isSaved = true;
        public bool IsSaved
        {
            get => _isSaved;
            set
            {
                if (value != _isSaved)
                {
                    _isSaved = value;
                    OnPropertyChanged(nameof(IsSaved));
                }
            }
        }
    }
}
