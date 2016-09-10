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
        private Color color;
        private double currentLuminance;

        public ColorResource(Color color, IReadOnlyList<Keys> keys)
        {
            this.OriginalColor = color;
            this.color = color;
            this.Keys = keys;
            this.Luminance = CalculateLuminance(color);
            this.currentLuminance = CalculateLuminance(color);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Color OriginalColor { get; }

        public Color Color
        {
            get { return this.color; }
            set
            {
                if (this.color == value)
                {
                    return;
                }

                this.color = value;
                this.OnPropertyChanged();
                this.CurrentLuminance = CalculateLuminance(this.color);
                foreach (var key in this.Keys)
                {
                    Application.Current.Resources[key.ColorKey] = value;
                    Application.Current.Resources[key.BrushKey] = new SolidColorBrush(value);
                }
            }
        }

        public double Luminance { get; }

        public double CurrentLuminance
        {
            get { return this.currentLuminance; }
            private set
            {
                if (value.Equals(this.currentLuminance)) return;
                this.currentLuminance = value;
                this.OnPropertyChanged();
            }
        }

        public IReadOnlyList<Keys> Keys { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static double CalculateLuminance(Color color) => 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B;

    }
}
