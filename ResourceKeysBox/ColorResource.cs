namespace ResourceKeysBox
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using ResourceKeysBox.Annotations;

    public class ColorResource : INotifyPropertyChanged
    {
        private static Dictionary<string, ColorResource> cache = new Dictionary<string, ColorResource>();
        private Color color;

        private ColorResource(Color color, IReadOnlyList<ResourceKey> keys)
        {
            this.Color = color;
            this.Keys = keys;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Color Color
        {
            get { return this.color; }
            set
            {
                if (value.Equals(this.color)) return;
                this.color = value;
                this.OnPropertyChanged();
                foreach (var key in Keys)
                {
                    if (Application.Current.Resources[key] is Color)
                    {
                        Application.Current.Resources[key] = value;
                    }
                    if (Application.Current.Resources[key] is SolidColorBrush)
                    {
                        Application.Current.Resources[key] = new SolidColorBrush(value);
                    }
                }
            }
        }

        public IReadOnlyList<ResourceKey> Keys { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
