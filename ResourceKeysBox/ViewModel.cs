namespace ResourceKeysBox
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private Exception exception;
        private string filterText;
        private Predicate<object> filter;

        public ViewModel()
        {
            this.Sources.Add(KeyAndColorResources.SystemColors());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<KeyAndColorResources> Sources { get; } = new ObservableCollection<KeyAndColorResources>();

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

        public string FilterText
        {
            get { return this.filterText; }
            set
            {
                if (value == this.filterText) return;
                this.filterText = value;
                this.OnPropertyChanged();
                this.Filter = string.IsNullOrEmpty(this.filterText) ? (Predicate<object>)null : this.IsMatch;
            }
        }

        public Predicate<object> Filter
        {
            get { return this.filter; }
            private set
            {
                this.filter = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsMatch(object item)
        {
            var isMatch = IsMatch((KeyAndColorResource)item, this.filterText);
            //if (isMatch)
            //{
            //    Debugger.Break();
            //}
            return isMatch;
        }

        private static bool IsMatch(KeyAndColorResource item, string filterText)
        {
            if (string.IsNullOrEmpty(filterText))
            {
                return true;
            }

            var keyAndColorResource = (KeyAndColorResource)item;
            var name = keyAndColorResource.Keys.Name;
            if (filterText.Length == 1)
            {
                return name.StartsWith(filterText, StringComparison.OrdinalIgnoreCase);
            }

            return name.IndexOf(filterText, 0, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}