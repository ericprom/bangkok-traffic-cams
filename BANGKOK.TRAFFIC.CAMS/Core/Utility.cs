using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace BANGKOK.TRAFFIC.CAMS.Core
{
    public class Utility
    {
        public static async Task<string> PostData(string url, string postData)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url))
                {
                    Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded")
                };
                var response = await client.SendAsync(request);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<InMemoryRandomAccessStream> getLiveImage(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                var contentBytes = await httpClient.GetByteArrayAsync(url);
                var ims = new InMemoryRandomAccessStream();
                var dataWriter = new DataWriter(ims);
                dataWriter.WriteBytes(contentBytes);
                await dataWriter.StoreAsync();
                ims.Seek(0);
                return ims;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
