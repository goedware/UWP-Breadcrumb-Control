using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using GoedWare.Controls.Breadcrumb.Services;

namespace GoedWare.Controls.Breadcrumb
{
    /// <summary>
    /// Control that will create a breadcrumb trail in your application.
    /// </summary>
    public class BreadcrumbControl: ContentControl
    {
        /// <summary>
        /// Event that occurs when the home item is selected
        /// </summary>
        public event EventHandler HomeSelected;
        /// <summary>
        /// Event that occurs when an item in the breadcrumb is selected
        /// </summary>
        public event EventHandler<BreadcrumbEventArgs> ItemSelected;

        /// <summary>
        /// Gets or sets the breadcrumbs home/start icon.
        /// </summary>
        /// <value>The icon to show as home button</value>
        public IconElement HomeIcon
        {
            get { return (IconElement)GetValue(HomeIconProperty); }
            set { SetValue(HomeIconProperty, value); }
        }

        /// <summary>
        /// Identifier for the<see cref="HomeIcon" /> dependency property.
        /// </summary>
        public readonly DependencyProperty HomeIconProperty =
            DependencyProperty.Register(nameof(HomeIcon), typeof(IconElement), typeof(BreadcrumbControl),
                new PropertyMetadata(new SymbolIcon(Symbol.Home)));

        /// <summary>
        /// Gets or sets the breadcrumb datasource
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ItemsSource" /> dependency property.
        /// </summary>
        public readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BreadcrumbControl), 
                new PropertyMetadata(null, OnItemsSourcePropertyChanged));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as BreadcrumbControl;
            control?.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var value = oldValue as INotifyCollectionChanged;
            if (value != null) value.CollectionChanged -= ItemsOnCollectionChanged;
            this.Items = new ObservableCollection<object>(newValue as IEnumerable<object>);
            if (newValue is INotifyCollectionChanged)
                (newValue as INotifyCollectionChanged).CollectionChanged += ItemsOnCollectionChanged;
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    if (e.NewItems != null)
                    {
                        var index = e.NewStartingIndex;
                        foreach (var item in e.NewItems)
                        {
                            this.Items.Insert(index, item);
                            index++;
                        }
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Move:
                {
                    var item = this.Items[e.OldStartingIndex];
                    this.Items.RemoveAt(e.OldStartingIndex);
                    this.Items.Insert(e.NewStartingIndex, item);
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                            this.Items.Remove(item);
                        }
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Items.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.OnApplyTemplate();
        }

        /// <summary>
        /// Gets or sets the collection of items.
        /// </summary>
        /// <value>The collection of about items.</value>
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="Items" /> dependency property.
        /// </summary>
        public readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(BreadcrumbControl), new PropertyMetadata(new ObservableCollection<object>()));

        /// <summary>
        /// Gets or sets the DataTemplate used to display the home item.
        /// </summary>
        /// <value>A DataTemplate that specifies the visualization of the data objects. The default is null.</value>
        public DataTemplate HomeTemplate
        {
            get { return (DataTemplate)GetValue(HomeTemplateProperty); }
            set { SetValue(HomeTemplateProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="HomeTemplate" /> dependency property.
        /// </summary>
        public readonly DependencyProperty HomeTemplateProperty =
            DependencyProperty.Register(nameof(HomeTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), 
                new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the DataTemplate used to display each item.
        /// </summary>
        /// <value>A DataTemplate that specifies the visualization of the data objects. The default is null.</value>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ItemTemplate" /> dependency property.
        /// </summary>
        public readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the DataTemplate used to display the seperator item.
        /// </summary>
        /// <value>A DataTemplate that specifies the visualization of the data objects. The default is null.</value>
        public DataTemplate SeperatorTemplate
        {
            get { return (DataTemplate)GetValue(SeperatorTemplateProperty); }
            set { SetValue(SeperatorTemplateProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="SeperatorTemplate" /> dependency property.
        /// </summary>
        public readonly DependencyProperty SeperatorTemplateProperty =
            DependencyProperty.Register(nameof(SeperatorTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the DataTemplate used to display the item in overflow mode.
        /// </summary>
        /// <value>A DataTemplate that specifies the visualization of the data objects. The default is null.</value>
        public DataTemplate OverFlowTemplate
        {
            get { return (DataTemplate)GetValue(OverFlowTemplateProperty); }
            set { SetValue(OverFlowTemplateProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="OverFlowTemplate" /> dependency property.
        /// </summary>
        public readonly DependencyProperty OverFlowTemplateProperty =
            DependencyProperty.Register(nameof(OverFlowTemplate), typeof(DataTemplate), typeof(BreadcrumbControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a path to a value on the source object to serve as the visual representation of the object.
        /// </summary>
        /// <value>The path to a value on the source object. This can be any path, or an XPath such as "@Name".</value>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="DisplayMemberPath" /> dependency property.
        /// </summary>
        public readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the string used to display the seperator item.
        /// </summary>
        /// <value>The seperator string to display.</value>
        public string Seperator
        {
            get { return (string)GetValue(SeperatorProperty); }
            set { SetValue(SeperatorProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="Seperator" /> dependency property.
        /// </summary>
        public readonly DependencyProperty SeperatorProperty =
            DependencyProperty.Register(nameof(Seperator), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata("/"));


        /// <summary>
        /// Gets or sets the text for the home item.
        /// </summary>
        /// <value>The home string to display.</value>
        public string HomeText
        {
            get { return (string)GetValue(HomeTextProperty); }
            set { SetValue(HomeTextProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="HomeText" /> dependency property.
        /// </summary>
        public readonly DependencyProperty HomeTextProperty =
            DependencyProperty.Register(nameof(HomeText), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the text for items in overflow mode.
        /// </summary>
        /// <value>The home string to display.</value>
        public string OverFlow
        {
            get { return (string)GetValue(OverFlowProperty); }
            set { SetValue(OverFlowProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="OverFlow" /> dependency property.
        /// </summary>
        public readonly DependencyProperty OverFlowProperty =
            DependencyProperty.Register(nameof(OverFlow), typeof(string), typeof(BreadcrumbControl), new PropertyMetadata("..."));


        /// <summary>
        /// Gets or sets the container style of the breadcrumb items.
        /// </summary>
        /// <value>The style to apply.</value>
        public Style ItemStyle
        {
            get { return (Style)GetValue(ItemStyleProperty); }
            set { SetValue(ItemStyleProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ItemStyle" /> dependency property.
        /// </summary>
        public readonly DependencyProperty ItemStyleProperty =
            DependencyProperty.Register(nameof(ItemStyle), typeof(Style), typeof(BreadcrumbControl), new PropertyMetadata(null));


        /// <summary>
        /// Gets or sets the container style of the home item.
        /// </summary>
        /// <value>The style to apply.</value>
        public Style HomeItemStyle
        {
            get { return (Style)GetValue(HomeItemStyleProperty); }
            set { SetValue(HomeItemStyleProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="HomeItemStyle" /> dependency property.
        /// </summary>
        public readonly DependencyProperty HomeItemStyleProperty =
            DependencyProperty.Register(nameof(HomeItemStyle), typeof(Style), typeof(BreadcrumbControl), new PropertyMetadata(null));


        /// <summary>
        /// Occurs when a breadcrumb is clicked.
        /// </summary>
        public ICommand ItemCommand
        {
            get { return (ICommand)GetValue(ItemCommandProperty); }
            set { SetValue(ItemCommandProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="ItemCommand" /> dependency property.
        /// </summary>
        public readonly DependencyProperty ItemCommandProperty =
          DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(BreadcrumbControl), new PropertyMetadata(null));

        /// <summary>
        /// Occurs when the home item is clicked.
        /// </summary>
        public ICommand HomeCommand
        {
            get { return (ICommand)GetValue(HomeCommandProperty); }
            set { SetValue(HomeCommandProperty, value); }
        }

        /// <summary>
        /// Identifier for the <see cref="HomeCommand" /> dependency property.
        /// </summary>
        public readonly DependencyProperty HomeCommandProperty =
            DependencyProperty.Register("HomeCommand", typeof(ICommand), typeof(BreadcrumbControl), new PropertyMetadata(null));

        private StackPanel StackPanel { get; set; }

        public BreadcrumbControl()
        {
            this.DefaultStyleKey = typeof(BreadcrumbControl);

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;
        }

        protected override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            this.StackPanel = (StackPanel)this.GetTemplateChild("PART_Stack");

            if (this.StackPanel == null) return;

            this.StackPanel.Children.Clear();

            SetHomeItem(this.StackPanel.Children);

            if (!this.Items.Any()) return;

            foreach (var item in this.Items)
            {
                SetSeperatorItem(this.StackPanel.Children, this.SeperatorTemplate, this.Seperator);
                SetItem(this.StackPanel.Children, item, this.Items, this.ItemTemplate, this.DisplayMemberPath);
            }
            // Update layout to be able to measure the actual width
            this.UpdateLayout();

            var items = this.StackPanel.Children.OfType<BreadcrumbItem>().ToList();
            var seperators = this.StackPanel.Children.OfType<BreadcrumbSeperator>().ToList();
            int i = 0;
            while (this.StackPanel.ActualWidth > this.ActualWidth)
            {
                if (i < items.Count-1)
                {
                    var item = items?[i];

                    if (item == null) break;

                    SetOverflowItem(item, this.OverFlowTemplate, this.OverFlow);
                    i++;
                }
                else
                {
                    var item = items.FirstOrDefault(b => b.Visibility == Visibility.Visible);
                    var seperator = seperators.FirstOrDefault(b => b.Visibility == Visibility.Visible);
                    seperator.Visibility = Visibility.Collapsed;
                    item.Visibility = Visibility.Collapsed;
                }
                
                this.UpdateLayout();
            }
        }

        private static void SetSeperatorItem(UIElementCollection collection, DataTemplate seperatorTemplate, string seperator)
        {
            if (seperatorTemplate != null)
            {
                collection.Add(new BreadcrumbSeperator()
                {
                    ContentTemplate = seperatorTemplate,
                    VerticalAlignment = VerticalAlignment.Center
                });
            }
            else
            {
                collection.Add(new BreadcrumbSeperator()
                {
                    Content = seperator,
                    VerticalAlignment = VerticalAlignment.Center
                });
            }
        }

        private void SetOverflowItem(BreadcrumbItem item, DataTemplate overflowTemplate, string overflow)
        {
            item.Content = null;
            item.ContentTemplate = null;

            if (overflowTemplate != null)
                item.ContentTemplate = overflowTemplate;
            else
                item.Content = overflow;
        }

        private void SetItem(UIElementCollection collection, object item, IList<object> items, 
            DataTemplate itemTemplate, string displayMemberPath)
        {
            var content = CreateButton();

            if (itemTemplate != null)
            {
                content.ContentTemplate = itemTemplate;
                content.DataContext = item;
            }
            else
            {
                content.SetBinding(ContentProperty, new Binding()
                {
                    Source = item,
                    Path = new PropertyPath(displayMemberPath)
                });
            }

            content.Click += (sender, args) => OnItemSelected(new BreadcrumbEventArgs(item, items.IndexOf(item)));
            collection.Add(content);
        }

        private BreadcrumbItem CreateButton()
        {
            return new BreadcrumbItem()
            {
                Style = this.ItemStyle ?? ResourceService.GetDictionaryValue<Style>("BreadcrumbButtonStyle"),
            };
        }

        private void SetHomeItem(UIElementCollection collection)
        {
            var home = new BreadcrumbHome()
            {
                Style = this.HomeItemStyle ?? ResourceService.GetDictionaryValue<Style>("BreadcrumbButtonStyle")
            };
            if (string.IsNullOrEmpty(this.HomeText))
            {
                if (this.HomeTemplate == null)
                {
                    home.ContentTemplate = ResourceService.GetDictionaryValue<DataTemplate>("BreadCrumbHome");
                    home.DataContext = this.HomeIcon;
                }
                else
                {
                    home.ContentTemplate = this.HomeTemplate;
                }
            }
            else
                home.Content = this.HomeText;

            home.Click += (sender, args) => OnHomeSelected();
            collection.Add(home);
        }

        protected virtual void OnHomeSelected()
        {
            HomeSelected?.Invoke(this, EventArgs.Empty);
            if (this.HomeCommand == null) return;
            if (this.HomeCommand.CanExecute(null)) this.HomeCommand.Execute(EventArgs.Empty);
        }

        protected virtual void OnItemSelected(BreadcrumbEventArgs e)
        {
            ItemSelected?.Invoke(this, e);
            if (this.ItemCommand == null) return;
            if (this.ItemCommand.CanExecute(null)) this.ItemCommand.Execute(e);
        }
    }
}
