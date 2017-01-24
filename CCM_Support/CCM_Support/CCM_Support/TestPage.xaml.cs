using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CCM_Support
{
    public partial class TestPage : ContentPage
    {

        private HttpClient _client = new HttpClient();
        private string _url = "http://gateway5.sybrin.systems/CCM_Support_RWS/CCM_Support_RESTService.svc/";

        public TestPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<CrossPushNotificationListener, string>(this, "DisplayMsg", DisplayMsg);
            base.OnAppearing(); 
        }

        void UnRegister()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Test_Activity.IsRunning = true;
                btnReg.IsEnabled = false;
                btnUnreg.IsEnabled = false;
                btnSend.IsEnabled = false;
            });

            CrossPushNotification.Current.Unregister();
        }

        void Register()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Test_Activity.IsRunning = true;
                btnReg.IsEnabled = false;
                btnUnreg.IsEnabled = false;
                btnSend.IsEnabled = false;
            });
            CrossPushNotification.Current.Register();
        }

        void Send_Clicked()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Test_Activity.IsRunning = true;
                btnReg.IsEnabled = false;
                btnUnreg.IsEnabled = false;
                btnSend.IsEnabled = false;
            });

            var tokenObject = txtMessage.Text;
            var content1 = JsonConvert.SerializeObject(tokenObject);
            var content2 = new StringContent(content1, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(_url + "SendNote/", content2).Result;

            if (response.IsSuccessStatusCode)
            {
                DisplayAlert("Notification", "Message Sent", "OK");
                Device.BeginInvokeOnMainThread(() =>
                {
                    Test_Activity.IsRunning = false;
                    btnReg.IsEnabled = true;
                    btnUnreg.IsEnabled = true;
                    btnSend.IsEnabled = true;
                });
            }
        }

        void DisplayMsg(CrossPushNotificationListener cs, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Notification", message, "OK");
                Test_Activity.IsRunning = false;
                btnReg.IsEnabled = true;
                btnUnreg.IsEnabled = true;
                btnSend.IsEnabled = true;
            });
        }
    }
}
