namespace ResourceKeysBox
{
    using System.Windows;

    public class Keys
    {
        public Keys(ResourceKey colorKey, ResourceKey brushKey)
        {
            this.BrushKey = brushKey;
            this.ColorKey = colorKey;
            this.Name = GetName(colorKey) ?? GetName(brushKey).Replace("Color", "").Replace("Brush", "");
        }

        public ResourceKey BrushKey { get; }

        public ResourceKey ColorKey { get; }

        public string Name { get; }

        public override string ToString() => this.Name;

        private static string GetName(ResourceKey resourceKey)
        {
            if (resourceKey == null)
            {
                return null;
            }

            var componentResourceKey = resourceKey as ComponentResourceKey;
            if (componentResourceKey != null)
            {
                return (string)componentResourceKey.ResourceId;
            }

            return resourceKey.ToString();
        }
    }
}