using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANGKOK.TRAFFIC.CAMS.Model
{
    public class CameraModel : INotifyPropertyChanged
    {
        private string _cam_id;
        public string cam_id
        {
            get
            {
                return _cam_id;
            }
            set
            {
                if (value != _cam_id)
                {
                    _cam_id = value;
                    NotifyPropertyChanged("cam_id");

                }
            }

        }
        private string _cam_group;
        public string cam_group
        {
            get
            {
                return _cam_group;
            }
            set
            {
                if (value != _cam_group)
                {
                    _cam_group = value;
                    NotifyPropertyChanged("cam_group");

                }
            }

        }
        private string _ip_camera;
        public string ip_camera
        {
            get
            {
                return _ip_camera;
            }
            set
            {
                if (value != _ip_camera)
                {
                    _ip_camera = value;
                    NotifyPropertyChanged("ip_camera");

                }
            }

        }
        private string _locationa;
        public string location
        {
            get
            {
                return _locationa;
            }
            set
            {
                if (value != _locationa)
                {
                    _locationa = value;
                    NotifyPropertyChanged("location");

                }
            }

        }
        private string _discription_public;
        public string discription_public
        {
            get
            {
                return _discription_public;
            }
            set
            {
                if (value != _discription_public)
                {
                    _discription_public = value;
                    NotifyPropertyChanged("discription_public");

                }
            }

        }
        private string _location_en;
        public string location_en
        {
            get
            {
                return _location_en;
            }
            set
            {
                if (value != _location_en)
                {
                    _location_en = value;
                    NotifyPropertyChanged("location_en");

                }
            }

        }
        private string _discription_public_en;
        public string discription_public_en
        {
            get
            {
                return _discription_public_en;
            }
            set
            {
                if (value != _discription_public_en)
                {
                    _discription_public_en = value;
                    NotifyPropertyChanged("discription_public_en");

                }
            }

        }
        private string _latitude;
        public string latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                if (value != _latitude)
                {
                    _latitude = value;
                    NotifyPropertyChanged("latitude");

                }
            }

        }
        private string _longitude;
        public string longitude
        {

            get
            {
                return _longitude;
            }
            set
            {
                if (value != _longitude)
                {
                    _longitude = value;
                    NotifyPropertyChanged("longitude");

                }
            }
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