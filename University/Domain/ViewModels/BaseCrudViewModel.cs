using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (_isSaved != value)
                {
                    _isSaved = value;
                    OnPropertyChanged(nameof(IsSaved));
                }
            }
        }
    }
}
