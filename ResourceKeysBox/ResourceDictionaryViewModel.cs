namespace ResourceKeysBox
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    public class ResourceDictionaryViewModel
    {
        private static readonly System.Windows.Media.ColorConverter ColorConverter = new System.Windows.Media.ColorConverter();

        public ResourceDictionaryViewModel(FileInfo fileName)
        {
            var element = XDocument.Parse(File.ReadAllText(fileName.FullName));
            var brushElements = element.Root
                                       .Elements()
                                       .Where(x => x.Name.LocalName == "SolidColorBrush")
                                       .ToArray();
            var colorKeys = brushElements.Select(e => e.AttributeValue("Key"))
                .OrderBy(x => x)
                .Select(p => new ComponentResourceKey(this.GetType(), p))
                .ToArray();

            var colorAndBrushKeys = colorKeys.Select(key => new Keys(key, null))
                                             .ToArray();

            var colorResources = colorKeys.ToLookup(k => GetColor(k, brushElements), k => colorAndBrushKeys.Single(x => x.ColorKey == k))
                    .Select(x => new ColorResource(x.Key, x.ToArray()))
                    .ToArray();

            var keyColorAndResources = colorAndBrushKeys.Select(x => new KeyAndColorResource(x, colorResources.Single(c => c.Keys.Any(k => k == x))))
                                                         .ToArray();
            this.KeyAndColorResources = new KeyAndColorResources(keyColorAndResources);
        }

        public KeyAndColorResources KeyAndColorResources { get; }

        private static Color GetColor(ComponentResourceKey key, IEnumerable<XElement> brushElements)
        {
            var match = brushElements.Single(e => e.AttributeValue("Key") == key.ResourceId);
            var attributeValue = match.AttributeValue("Color");
            if (attributeValue.Contains("DynamicResource") || attributeValue.Contains("StaticResource"))
            {
                return Colors.HotPink;
            }

            return (Color)ColorConverter.ConvertFrom(attributeValue);
        }
    }
}