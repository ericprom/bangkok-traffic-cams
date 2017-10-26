using BANGKOK.TRAFFIC.CAMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BANGKOK.TRAFFIC.CAMS.Model
{
   public class DataManager
    {
        static volatile DataManager _classInstance = null;
        static readonly object _threadSafetyLock = new object();
        public static DataManager shareInstance
        {
            get
            {
                if (_classInstance == null)
                {
                    lock (_threadSafetyLock)
                    {
                        if (_classInstance == null)
                            _classInstance = new DataManager();
                    }
                }
                return _classInstance;
            }
        }

        private CameraDataSource _CameraDataSource;
        public CameraDataSource CameraDataSource
        {
            get { return _CameraDataSource; }
            set
            {
                if (_CameraDataSource != value)
                {
                    _CameraDataSource = value;
                }
            }
        }
    }
}