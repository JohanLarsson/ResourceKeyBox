namespace ResourceKeysBox
{
    using System.Windows;

    public class Keys
    {
        public Keys(ResourceKey colorKey, ResourceKey brushKey)
        {
            this.BrushKey = brushKey;
            this.ColorKey = colorKey;
            this.Name = this.ColorKey.ToString().Replace("Color", "");
        }

        public ResourceKey BrushKey { get; set; }

        public ResourceKey ColorKey { get; }

        public string Name { get;  }

        public override string ToString() => this.ColorKey.ToString();
    }
}