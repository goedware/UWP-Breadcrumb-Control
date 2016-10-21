using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GoedWare.Samples.Breadcrumb
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<TestItem> TestList;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            TestList = new ObservableCollection<TestItem>()
            {
                new TestItem() {Name = "Folder 1"},
                new TestItem() {Name = "Folder 2"},
                new TestItem() {Name = "Folder 3"}
            };
            this.BreadcrumbControl.ItemsSource = TestList;
            this.BreadcrumbControl.HomeSelected += async (sender, args) =>
            {
                var dlg = new MessageDialog("Home item selected");
                await dlg.ShowAsync();
            };
            this.BreadcrumbControl.ItemSelected += async (sender, args) =>
            {
                var dlg = new MessageDialog("Itemindex:" + args.ItemIndex, ((TestItem) args.Item).Name);
                await dlg.ShowAsync();
            };

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TestList.Add(new TestItem() { Name = "Folder 7" });
        }
    }

    public class TestItem
    {
        public string Name { get; set; }
    }
}
