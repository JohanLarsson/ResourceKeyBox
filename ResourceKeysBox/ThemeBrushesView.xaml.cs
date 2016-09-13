namespace ResourceKeysBox
{
    using System.Text;
    using System.Windows.Controls;

    public partial class ThemeBrushesView : UserControl
    {
        public ThemeBrushesView()
        {
            this.InitializeComponent();
            var stringBuilder = new StringBuilder();

            foreach (var resource in KeyAndColorResources.SystemColors())
            {
                stringBuilder.AppendLine($"<SolidColorBrush x:Key=\"{{x:Static SystemColors.{resource.Keys.BrushKey}Key}}\" Color=\"{{DynamicResource {{x:Static SystemColors.{resource.Keys.ColorKey}Key}}}}\" />");
            }

            this.XamlBox.Text = stringBuilder.ToString();
        }
    }
}
