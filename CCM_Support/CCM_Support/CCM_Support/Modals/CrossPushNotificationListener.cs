using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushNotification.Plugin.Abstractions;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;

namespace CCM_Support
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        private string _url = "http://gateway5.sybrin.systems/CCM_Support_RWS/CCM_Support_RESTService.svc/";
        private HttpClient _client = new HttpClient();

        //Enable/Disable Showing the notification
        bool IPushNotificationListener.ShouldShowNotification()
        {
            return true;
        }

        //Here you will receive all push notification messages
        //Messages arrives as a dictionary, the device type is also sent in order to check specific keys correctly depending on the platform.
        public void OnMessage(JObject values, PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived");
        }

        //Gets the registration token after push registration
        public void OnRegistered(string token, PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", token));

            //Send Token to Server
            var tokenObject = new Token { TokenID = token, Type = "U", User = Application.Current.Properties["Username"].ToString() };
            var content1 = JsonConvert.SerializeObject(tokenObject);
            var content2 = new StringContent(content1, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_url + "RegisterToken/", content2).Result;

            if (!response.IsSuccessStatusCode)
            {
                MessagingCenter.Send(this, "DisplayMsg", "Token sent to Server unsuccessfully");
                Application.Current.Properties["Token"] = "";
            }
            else
            {
                Application.Current.Properties["Token"] = token;
                MessagingCenter.Send(this, "DisplayMsg", "Device has been registered successfully");
            }
            Application.Current.SavePropertiesAsync();
        }

        //Fires when device is unregistered
        public void OnUnregistered(PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");

            //Remove Token from Server
            var tokenObject = new Token { TokenID = Application.Current.Properties["Token"].ToString(), Type = "U" };
            var content1 = JsonConvert.SerializeObject(tokenObject);
            var content2 = new StringContent(content1, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_url + "UnRegisterToken/", content2).Result;

            if (!response.IsSuccessStatusCode)
            {
                MessagingCenter.Send(this, "DisplayMsg", "Token removed from Server unsuccessfully");
            }
            //else //remove token regardless, otherwise the user wont be registered again
            //{
            Application.Current.Properties["Token"] = "";
            MessagingCenter.Send(this, "DisplayMsg", "Device has been unregistered successfully");
            Application.Current.SavePropertiesAsync();
            //}
        }

        //Fires when error
        public void OnError(string message, PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
            MessagingCenter.Send(this, "DisplayMsg", string.Format("Push notification error - {0}", message));
        }
    }

    /// <summary>
    /// Device type.
    /// </summary>
    public enum DeviceType
    {
        Android,
        iOS,
        WindowsPhone,
        Windows
    }
}
