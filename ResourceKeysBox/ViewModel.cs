namespace ResourceKeysBox
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class ViewModel : INotifyPropertyChanged
    {
        private ResourceDictionaryViewModel fromFile;

        public event PropertyChangedEventHandler PropertyChanged;

        public SystemColorsViewModel SystemColors { get; } = new SystemColorsViewModel();

        public ResourceDictionaryViewModel FromFile
        {
            get { return this.fromFile; }
            set
            {
                if (Equals(value, this.fromFile)) return;
                this.fromFile = value;
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