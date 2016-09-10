namespace ResourceKeysBox
{
    public class KeyAndColorResource
    {
        public KeyAndColorResource(Keys keys, ColorResource colorResource)
        {
            this.Keys = keys;
            this.ColorResource = colorResource;
        }

        public Keys Keys { get; }

        public ColorResource ColorResource { get; }
    }
}