namespace ResourceKeysBox
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class KeyAndColorResources : ReadOnlyObservableCollection<KeyAndColorResource>
    {
        public KeyAndColorResources(IReadOnlyList<KeyAndColorResource> source)
            : base(new ObservableCollection<KeyAndColorResource>(source))
        {
        }
    }
}