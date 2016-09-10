namespace ResourceKeysBox
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataGrid.ItemsSource = this.GetColorKeysAndValues();
        }

        private IReadOnlyList<KeyAndColor> GetColorKeysAndValues()
        {
            return typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => typeof(ResourceKey).IsAssignableFrom(x.PropertyType))
                .Where(x => x.Name.Contains("Color"))
                .OrderBy(x => x.Name)
                .Select(x => (ResourceKey) x.GetValue(null))
                .Select(k => new KeyAndColor(k, (Color) this.FindResource(k)))
                .ToArray();
        }

        private static string DumpMarkdownTable(IReadOnlyList<KeyAndColor> source)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"|Key|Color|").AppendLine("| ------------- | ------------- |");
            foreach (var pair in source)
            {
                stringBuilder.AppendLine($"|{pair.ResourceKey}|{pair.Color}|");
            }
            var markdown = stringBuilder.ToString();
            return markdown;
        }
    }
}
