namespace ResourceKeysBox
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Media;
    using JetBrains.Annotations;

    public class ColorResource : INotifyPropertyChanged
    {
        private Color color;
        private Hsv hsv;

        public ColorResource(Color color, IReadOnlyList<Keys> keys)
        {
            this.OriginalColor = color;
            this.color = color;
            this.Keys = keys;
            this.OriginalHsv = Hsv.ColorToHSV(color);
            this.hsv = Hsv.ColorToHSV(color);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Color OriginalColor { get; }

        public Hsv OriginalHsv { get; }

        public Color Color
        {
            get => this.color;
            set
            {
                if (this.color == value)
                {
                    return;
                }

                this.color = value;
                this.OnPropertyChanged();
                this.Hsv = Hsv.ColorToHSV(this.color);
                foreach (var key in this.Keys)
                {
                    if (key.ColorKey != null)
                    {
                        Application.Current.Resources[key.ColorKey] = value;
                    }

                    if (key.BrushKey != null)
                    {
                        Application.Current.Resources[key.BrushKey] = new SolidColorBrush(value);
                    }
                }
            }
        }

        public Hsv Hsv
        {
            get => this.hsv;
            private set
            {
                if (value.Equals(this.hsv)) return;
                this.hsv = value;
                this.OnPropertyChanged();
            }
        }

        public IReadOnlyList<Keys> Keys { get; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
