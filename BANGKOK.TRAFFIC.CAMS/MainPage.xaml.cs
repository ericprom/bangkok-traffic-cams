using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BANGKOK.TRAFFIC.CAMS.Model;
using BANGKOK.TRAFFIC.CAMS.Pages;
using BANGKOK.TRAFFIC.CAMS.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace BANGKOK.TRAFFIC.CAMS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager;
        CommandBar mainBar;
        AppBarButton searchBtn, settingBtn;
        bool hasCams = false;
        public MainPage()
        {
            this.InitializeComponent();
            PrepareAppBars();
            BottomAppBar = mainBar;
            shareDataManager = DataManager.shareInstance;
            statusBar.BackgroundColor = Color.FromArgb(255, 44, 69, 102);
            statusBar.BackgroundOpacity = 1;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (!hasCams)
                {
                    statusBar.ProgressIndicator.Text = "Loading bangkok traffic cams...";
                    await statusBar.ProgressIndicator.ShowAsync();
                    shareDataManager.CameraDataSource = new CameraDataSource("all");
                    shareDataManager.CameraDataSource.LoadData(0);
                    shareDataManager.CameraDataSource.PropertyChanged += CameraDataSource_PropertyChanged;
                    HardwareButtons.BackPressed += HardwareButtons_BackPressed;
                }
            }
            catch (Exception ex) { }
        }
        private async void CameraDataSource_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "IsLoading")
                {
                    if (shareDataManager.CameraDataSource.IsLoading == false)
                    {
                        if (shareDataManager.CameraDataSource.hasItem)
                        {
                            var result = from t in shareDataManager.CameraDataSource.Items
                                         group t by t.cam_group into g
                                         select new { Key = g.Key, Items = g };
                            groupCamera.Source = result;
                            hasCams = true;
                        }
                        else
                        {
                            hasCams = false;
                        }
                        await statusBar.ProgressIndicator.HideAsync();
                    }
                }
            }
            catch (Exception ex) { }
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listview = (ListView)sender;
                if (listview.SelectedItem == null)
                    return;
                CameraModel entry = (CameraModel)listview.SelectedItem;
                Frame.Navigate(typeof(Watch), new PassedData
                {
                    watch = entry
                });
                listview.SelectedItem = null;
            }
            catch (Exception ex) { }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        #region prepare appbar
        private void PrepareAppBars()
        {
            try
            {
                mainBar = new CommandBar();
                mainBar.Background = new SolidColorBrush(Color.FromArgb(255, 44, 69, 102));
                mainBar.IsOpen = true;

                //searchBtn = new AppBarButton() { Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/icons/search.png") } };
                //searchBtn.Label = "Search";
                //searchBtn.Tag = "Search";
                //searchBtn.Click += appbarBtn_Clicked;
                //mainBar.PrimaryCommands.Add(searchBtn);

                //settingBtn = new AppBarButton();
                settingBtn = new AppBarButton() { Icon = new BitmapIcon() { UriSource = new Uri("ms-appx:///Assets/icons/settings.png") } };
                
                settingBtn.Label = "Settings";
                settingBtn.Tag = "Setting";
                settingBtn.Click += appbarBtn_Clicked;
                mainBar.PrimaryCommands.Add(settingBtn);
                //mainBar.SecondaryCommands.Add(settingBtn);
            }
            catch (Exception ex) { }
        }

        private void appbarBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var Button = sender as Button;
                if (Button == null) return;
                switch ((string)Button.Tag)
                {
                    case "Setting":
                        Frame.Navigate(typeof(Settings));
                        break;
                    case "Search":
                        //Frame.Navigate(typeof(Map));
                        break;
                }
            }
            catch (Exception ex) { }
        }

        #endregion
        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            try
            {
                e.Handled = true;
                var frame = Window.Current.Content as Frame;
                if (frame != null && frame.CanGoBack)
                {
                    frame.GoBack();
                }
                else
                {
                    var msg = new MessageDialog("คุณต้องการออกจากแอพใช่หรือไม่?", "BUS TRACKER");
                    var okBtn = new UICommand("OK");
                    var cancelBtn = new UICommand("Cancel");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "OK")
                    {
                        Application.Current.Exit();
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}
