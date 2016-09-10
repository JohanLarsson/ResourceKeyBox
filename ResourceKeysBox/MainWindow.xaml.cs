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
            this.InitializeComponent();
            this.DataGrid.ItemsSource = this.GetColorKeysAndValues();
            //DumpResources(this.GetColorKeysAndValues());
        }

        private IReadOnlyList<KeyAndColor> GetColorKeysAndValues()
        {
            return typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => typeof(ResourceKey).IsAssignableFrom(x.PropertyType))
                .Where(x => x.Name.Contains("Color"))
                .OrderBy(x => x.Name)
                .Select(p => new KeyAndColor(p.Name, (Color)this.FindResource(p.GetValue(null))))
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

        private static string DumpResources(IReadOnlyList<KeyAndColor> source)
        {
            var stringBuilder = new StringBuilder();
            foreach (var keyAndColor in source)
            {
                stringBuilder.AppendLine($"<Color x:Key=\"{{x:Static SystemColors.{keyAndColor.ResourceKey}}}\">{keyAndColor.Color}</Color>");
                stringBuilder.AppendLine($"<SolidColorBrush x:Key=\"{{x:Static SystemColors.{keyAndColor.ResourceKey.Replace("Color", "Brush")}}}\" Color=\"{{DynamicResource {{x:Static SystemColors.{keyAndColor.ResourceKey}}}}}\" />");
            }

            var xaml = stringBuilder.ToString();
            return xaml;
        }
    }
}
