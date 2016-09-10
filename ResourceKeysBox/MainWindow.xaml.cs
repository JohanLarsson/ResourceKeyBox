namespace ResourceKeysBox
{
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataGrid.ItemsSource = typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                                        .Where(x => typeof(ResourceKey).IsAssignableFrom(x.PropertyType))
                                        .Where(x => x.Name.Contains("Color"))
                                        .OrderBy(x => x.Name)
                                        .Select(x => (ResourceKey)x.GetValue(null))
                                        .Select(k => new KeyAndValue(k, (Color) this.FindResource(k)))
                                        .ToArray();
        }
    }
}
