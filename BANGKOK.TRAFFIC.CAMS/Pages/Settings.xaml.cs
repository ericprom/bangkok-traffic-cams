using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BANGKOK.TRAFFIC.CAMS.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public Settings()
        {
            this.InitializeComponent();
            foreach (ComboBoxItem d in ComboBoxResolution.Items)
            {
                if (localSettings.Values["resolution"] != null)
                {
                    if (d.Tag.ToString() == localSettings.Values["resolution"].ToString())
                    {
                        ComboBoxResolution.SelectedItem = d;
                    }
                }
            }
            foreach (ComboBoxItem d in ComboBoxLoop.Items)
            {
                if (localSettings.Values["reloadtime"] != null)
                {
                    if (d.Tag.ToString() == localSettings.Values["reloadtime"].ToString())
                    {
                        ComboBoxLoop.SelectedItem = d;
                    }
                }
            }
            foreach (ComboBoxItem d in ComboBoxLanguage.Items)
            {
                if (localSettings.Values["lang"] != null)
                {
                    if (d.Tag.ToString() == localSettings.Values["lang"].ToString())
                    {
                        ComboBoxLanguage.SelectedItem = d;
                    }

                    if (localSettings.Values["lang"].ToString() == "th")
                    {
                        loadTitle.Text = "รีโหลด :";
                        vidTitle.Text = "คุณภาพ :";
                        langTitle.Text = "ภาษา :";
                    }
                    else
                    {
                        loadTitle.Text = "Reload :";
                        vidTitle.Text = "Quality";
                        langTitle.Text = "Language :";
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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
        private void ComboBoxLoop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var reloadtime = ((ComboBoxItem)ComboBoxLoop.SelectedItem).Tag.ToString();
                if (reloadtime != null)
                    localSettings.Values["reloadtime"] = reloadtime;
            }
            catch (Exception ex) { }
        }
        private void ComboBoxResolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var tracktime = ((ComboBoxItem)ComboBoxResolution.SelectedItem).Tag.ToString();
                if (tracktime != null)
                    localSettings.Values["resolution"] = tracktime;
            }
            catch (Exception ex) { }
        }
        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var tracktime = ((ComboBoxItem)ComboBoxLanguage.SelectedItem).Tag.ToString();
                if (tracktime != null)
                    localSettings.Values["lang"] = tracktime;

                if (localSettings.Values["lang"].ToString() == "th")
                {
                    loadTitle.Text = "รีเฟรช :";
                    vidTitle.Text = "คุณภาพ :";
                    langTitle.Text = "ภาษา :";
                }
                else
                {
                    loadTitle.Text = "Reload :";
                    vidTitle.Text = "Qualitye";
                    langTitle.Text = "Language :";
                }
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

