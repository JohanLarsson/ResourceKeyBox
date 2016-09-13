namespace ResourceKeyBox.Tests
{
    using System;
    using System.Text;

    using NUnit.Framework;

    using ResourceKeysBox;

    public class SystemColorsTests
    {
        [Test]
        public void DumpBrushDictionary()
        {
            var stringBuilder = new StringBuilder();

            foreach (var resource in KeyAndColorResources.SystemColors())
            {
                stringBuilder.AppendLine($"<SolidColorBrush x:Key=\"{{x:Static SystemColors.{resource.Keys.BrushKey}Key}}\" Color=\"{{DynamicResource {{x:Static SystemColors.{resource.Keys.ColorKey}Key}}}}\" />");
            }

            var xaml = stringBuilder.ToString();
            Console.Write(xaml);
        }
    }
}
