using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.DataLayer;

namespace University.Domain.ViewModels
{
    public class StudentsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly UniversityContext _context;
        public StudentsViewModel(UniversityContext context)
        {
            _context = context;
        }
    }
}
