namespace ResourceKeysBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private Exception exception;

        public ViewModel()
        {
            this.Sources.Add(KeyAndColorResources.SystemColors());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<KeyAndColorResources> Sources { get;  } = new ObservableCollection<KeyAndColorResources>();

        public Exception Exception
        {
            get { return this.exception; }
            set
            {
                if (Equals(value, this.exception)) return;
                this.exception = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}