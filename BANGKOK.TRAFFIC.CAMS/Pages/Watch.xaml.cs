using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BANGKOK.TRAFFIC.CAMS.Core;
using BANGKOK.TRAFFIC.CAMS.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Shapes;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BANGKOK.TRAFFIC.CAMS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Watch : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
        DataManager shareDataManager;
        PassedData param;
        CameraModel entry;
        private DispatcherTimer dispatcherTimer;
        public Watch()
        {
            this.InitializeComponent();
            shareDataManager = DataManager.shareInstance;
            statusBar.BackgroundColor = Color.FromArgb(255, 44, 69, 102);
            statusBar.BackgroundOpacity = 1;

            dispatcherTimer = new DispatcherTimer();
            if (localSettings.Values["reloadtime"] != null)
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, int.Parse(localSettings.Values["reloadtime"].ToString()));
            }
            else
            {
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            }

            if (localSettings.Values["resolution"] != null)
            {
                localSettings.Values["resolution"] = localSettings.Values["resolution"];
            }
            else
            {
                localSettings.Values["resolution"] = 360;
            }

            if (localSettings.Values["lang"] != null)
            {
                localSettings.Values["lang"] = localSettings.Values["lang"];
            }
            else
            {
                localSettings.Values["lang"] = "th";
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter.GetType() == typeof(PassedData))
                {
                    param = (PassedData)e.Parameter;
                    entry = (CameraModel)param.watch;
                    var ipcamera = @"http://traffic.promrat.com/camera/watch/" + entry.cam_id + "/" + localSettings.Values["resolution"].ToString() + "/" + Utility.GetTimeStamp();
                    loadImage(ipcamera);
                    if (localSettings.Values["lang"].ToString() == "th")
                    {
                        locationTitle.Text = "สถานที่ :";
                        descriptionTitle.Text = "รายละเอียด :";
                        location.Text = entry.location;
                        description.Text = entry.discription_public;
                    }
                    else
                    {
                        locationTitle.Text = "Location :";
                        descriptionTitle.Text = "Description :";
                        location.Text = entry.location_en;
                        description.Text = entry.discription_public_en;
                    }
                }
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Start();
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        List<BitmapImage> imageList = new List<BitmapImage>();
        private void dispatcherTimer_Tick(object sender, object e)
        {
            try
            {
                var ipcamera = @"http://traffic.promrat.com/camera/watch/" + entry.cam_id + "/" + localSettings.Values["resolution"].ToString() + "/" + Utility.GetTimeStamp();
                loadImage(ipcamera);
            }
            catch (Exception ex) { }
        }
        private async void loadImage(string url)
        {
            try
            {
                var ims = await Utility.getLiveImage(url);
                var bitmap = new BitmapImage();
                bitmap.SetSource(ims);
                liveCam.Source = bitmap;
            }
            catch (Exception ex) { }
        }
        private async void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Geopoint location = new Geopoint(new BasicGeoposition()
                {
                    Latitude = Double.Parse(entry.latitude),
                    Longitude = Double.Parse(entry.longitude)
                });
                Image iconStart = new Image();
                iconStart.Source = new BitmapImage(new Uri("ms-appx:///Assets/icons/pin.png"));
                MyMap.Children.Add(iconStart);
                MapControl.SetLocation(iconStart, location);
                MapControl.SetNormalizedAnchorPoint(iconStart, new Point(0.5, 0.5));
                await MyMap.TrySetViewAsync(location, 15, 0, 0, MapAnimationKind.Bow);
                MyMap.TrafficFlowVisible = true;
            }
            catch (Exception ex) { }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                dispatcherTimer.Tick -= dispatcherTimer_Tick;
                dispatcherTimer.Stop();
                HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            }
            catch (Exception ex) { }
        }
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            try
            {
                e.Handled = true;
                var frame = Window.Current.Content as Frame;
                if (frame != null && frame.CanGoBack)
                {
                    frame.GoBack();
                }
            }
            catch (Exception ex) { }
        }
    }
}
