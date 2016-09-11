namespace ResourceKeysBox
{
    using System;
    using System.IO;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)this.DataContext;

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog { Filter = "resource dictionaries (*.xaml)|*.xaml" };
            if (fileDialog.ShowDialog(this) == true)
            {
                try
                {
                    this.ViewModel.Sources.Add(KeyAndColorResources.Parse(new FileInfo(fileDialog.FileName)));
                }
                catch (Exception ex)
                {
                    this.ViewModel.Exception = ex;
                }
            }
        }
    }
}
