namespace ResourceKeysBox
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using ResourceKeysBox.Annotations;

    public class SystemColorsViewModel : INotifyPropertyChanged
    {
        public SystemColorsViewModel()
        {
            this.KeyAndColorResources = this.GetColorKeysAndValues();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyList<KeyAndColorResource> KeyAndColorResources { get;  }

        private IReadOnlyList<KeyAndColorResource> GetColorKeysAndValues()
        {
            var colorKeys = typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => typeof(ResourceKey).IsAssignableFrom(x.PropertyType))
                .Where(x => x.Name.Contains("Color"))
                .OrderBy(x => x.Name)
                .Select(p => (ResourceKey)p.GetValue(null))
                .ToArray();
            var colorAndBrushKeys =
                colorKeys.Select(key => new Keys(key, (ResourceKey)typeof(SystemColors).GetProperty(key.ToString().Replace("Color", "Brush") + "Key").GetValue(null)))
                         .ToArray();

            var colorResources = colorKeys.ToLookup(k => (Color) Application.Current.FindResource(k), k => colorAndBrushKeys.Single(x => x.ColorKey == k))
                    .Select(x => new ColorResource(x.Key, x.ToArray()))
                    .ToArray();

            return colorAndBrushKeys.Select(x => new KeyAndColorResource(x, colorResources.Single(c => c.Keys.Any(k => k == x)))).ToList();
        }

        private static string DumpMarkdownTable(IReadOnlyList<KeyAndColorResource> source)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"|Key|Color|").AppendLine("| ------------- | ------------- |");
            foreach (var pair in source)
            {
                stringBuilder.AppendLine($"|{pair.Keys.ColorKey}|{pair.ColorResource}|");
            }
            var markdown = stringBuilder.ToString();
            return markdown;
        }

        private static string DumpResources(IReadOnlyList<KeyAndColorResource> source)
        {
            var stringBuilder = new StringBuilder();
            foreach (var keyAndColor in source)
            {
                stringBuilder.AppendLine($"<Color x:Key=\"{{x:Static SystemColors.{keyAndColor.Keys.ColorKey}}}\">{keyAndColor.ColorResource.Color}</Color>");
                stringBuilder.AppendLine($"<SolidColorBrush x:Key=\"{{x:Static SystemColors.{keyAndColor.Keys.BrushKey}}}\" Color=\"{keyAndColor.ColorResource.Color}\" />");
            }

            var xaml = stringBuilder.ToString();
            return xaml;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
