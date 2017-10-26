using BANGKOK.TRAFFIC.CAMS.Core;
using BANGKOK.TRAFFIC.CAMS.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANGKOK.TRAFFIC.CAMS.ViewModel
{
    public class CameraDataSource : INotifyPropertyChanged
    {
        public ObservableCollection<CameraModel> Items { get; private set; }
        public CameraDataSource()
        {
            this.Items = new ObservableCollection<CameraModel>();

        }
        public string url { get; set; }
        public CameraDataSource(string path)
        {

            this.url = "http://traffic.promrat.com/camera/location/" + path;
            this.Items = new ObservableCollection<CameraModel>();

        }


        private bool _hasItem = false;
        public bool hasItem
        {
            get
            {
                return _hasItem;
            }
            set
            {
                _hasItem = value;
                NotifyPropertyChanged("hasItem");

            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");

            }
        }

        public async void LoadData(int skip)
        {
            try
            {
                if (skip == 0)
                {
                    this.Items.Clear();
                }

                this.IsLoading = true;

                var content = await Utility.PostData(this.url, "");
                if (!string.IsNullOrEmpty(content))
                {
                    downloadCompleted(content);
                }
            }
            catch (Exception) { }
        }

        private void downloadCompleted(string jsonString)
        {
            try
            {
                JObject json = JObject.Parse(jsonString);
                List<JToken> DataItems = json["feed"]["entry"].ToList();
                foreach (var dataItem in DataItems)
                {
                    try
                    {
                        CameraModel model = new CameraModel()
                        {
                            cam_id = (string)dataItem["cam_id"],
                            ip_camera = (string)dataItem["ip_camera"],
                            location = (string)dataItem["location"],
                            discription_public = (string)dataItem["discription_public"],
                            location_en = (string)dataItem["location_en"],
                            discription_public_en = (string)dataItem["discription_public_en"],
                            latitude = (string)dataItem["latitude"],
                            longitude = (string)dataItem["longitude"],
                            cam_group = "BMA TRAFFIC"
                        };
                        this.Items.Add(model);
                    }
                    catch (Exception) { }
                }
                this.hasItem = DataItems.Count() != 0;
            }
            catch (Exception) { }
            this.IsLoading = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}