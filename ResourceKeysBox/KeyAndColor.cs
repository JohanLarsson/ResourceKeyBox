namespace ResourceKeysBox
{
    using System.Windows;
    using System.Windows.Media;

    public class KeyAndColor
    {
        public KeyAndColor(ResourceKey key, Color color)
        {
            this.ResourceKey = key.ToString();
            this.Color = color.ToString();
        }

        public string ResourceKey { get; }

        public string Color { get; }
    }
}