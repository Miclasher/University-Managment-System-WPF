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

        public abstract void SaveChanges();
    }
}
