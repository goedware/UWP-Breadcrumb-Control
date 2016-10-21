using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GoedWare.Controls.Breadcrumb;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoedWare.Samples.Breadcrumb
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<DataItem> _dataSource;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _dataSource = new ObservableCollection<DataItem>()
            {
                new DataItem() {Name = "Folder 1"},
                new DataItem() {Name = "Folder 2"},
                new DataItem() {Name = "Folder 3"}
            };
            this.BreadcrumbControl.ItemsSource = _dataSource;
            this.BreadcrumbControl.ItemSelected += OnItemSelected;
            this.BreadcrumbControl.HomeSelected += OnHomeSelected;
        }

        private async void OnHomeSelected(object sender, EventArgs eventArgs)
        {
            var dlg = new MessageDialog("Home item selected");
            await dlg.ShowAsync();
        }

        private async void OnItemSelected(object sender, BreadcrumbEventArgs e)
        {
            var dlg = new MessageDialog("ItemIndex:" + e.ItemIndex, ((DataItem)e.Item).Name);
            await dlg.ShowAsync();
        }


        private void OnClick(object sender, RoutedEventArgs e)
        {
            _dataSource.Add(new DataItem() { Name = "Folder " + (_dataSource.Count + 1) });
        }
    }

    public class DataItem
    {
        public string Name { get; set; }
    }
}
