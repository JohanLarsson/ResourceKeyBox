namespace ResourceKeysBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;

    public class KeyAndColorResources : ReadOnlyObservableCollection<KeyAndColorResource>
    {
        public KeyAndColorResources(string name, IReadOnlyList<KeyAndColorResource> source)
            : base(new ObservableCollection<KeyAndColorResource>(source))
        {
            this.Name = name;
        }

        public string Name { get; }

        private static readonly System.Windows.Media.ColorConverter ColorConverter = new System.Windows.Media.ColorConverter();

        public static KeyAndColorResources Parse(FileInfo file)
        {
            var element = XDocument.Parse(File.ReadAllText(file.FullName));
            var brushElements = element.Root
                                       .Elements()
                                       .Where(x => x.Name.LocalName == "SolidColorBrush")
                                       .ToArray();
            var colorKeys = brushElements.Select(e => e.AttributeValue("Key"))
                //.OrderBy(x => x)
                .Select(p => new ComponentResourceKey(typeof(KeyAndColorResource), p))
                .ToArray();

            var colorAndBrushKeys = colorKeys.Select(key => new Keys(key, null))
                                             .ToArray();

            var colorResources = colorKeys.ToLookup(k => GetColor(k, brushElements), k => colorAndBrushKeys.Single(x => x.ColorKey == k))
                    .Select(x => new ColorResource(x.Key, x.ToArray()))
                    .ToArray();

            var keyColorAndResources = colorAndBrushKeys.Select(x => new KeyAndColorResource(x, colorResources.Single(c => c.Keys.Any(k => k == x))))
                                                         .ToArray();
            return new KeyAndColorResources(Path.GetFileNameWithoutExtension(file.FullName), keyColorAndResources);
        }

        public static KeyAndColorResources SystemColors()
        {
           return new KeyAndColorResources("SystemColors", GetColorKeysAndValues(typeof(SystemColors)));
        }

        private static IReadOnlyList<KeyAndColorResource> GetColorKeysAndValues(Type type)
        {
            var colorKeys = type.GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(x => typeof(ResourceKey).IsAssignableFrom(x.PropertyType))
                .Where(x => x.Name.Contains("Color"))
                //.OrderBy(x => x.Name)
                .Select(p => (ResourceKey)p.GetValue(null))
                .ToArray();
            var colorAndBrushKeys = colorKeys.Select(key => new Keys(key, (ResourceKey)type.GetProperty(key.ToString().Replace("Color", "Brush") + "Key")?.GetValue(null)))
                                             .ToArray();

            var colorResources = colorKeys.ToLookup(k => (Color)Application.Current.FindResource(k), k => colorAndBrushKeys.Single(x => x.ColorKey == k))
                    .Select(x => new ColorResource(x.Key, x.ToArray()))
                    .ToArray();

            return colorAndBrushKeys.Select(x => new KeyAndColorResource(x, colorResources.Single(c => c.Keys.Any(k => k == x))))
                                    .ToArray();
        }

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
    }
}