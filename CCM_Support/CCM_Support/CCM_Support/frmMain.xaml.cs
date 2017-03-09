using Newtonsoft.Json;
using PushNotification.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CCM_Support
{
    public partial class frmMain : ContentPage
    {

        private string _url = "http://gateway5.sybrin.systems/CCM_Support_RWS/CCM_Support_RESTService.svc/GetConfig/";

        private HttpClient _client = new HttpClient();
        private List<string> _clientList = new List<string>();
        private object _token = string.Empty;

        public frmMain()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetTitleIcon(this, "Sybrin_Horizontal.png");
            MessagingCenter.Subscribe<CrossPushNotificationListener, string>(this, "DisplayMsg", DisplayMsg);
        }

        async protected override void OnAppearing()
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Main_Activity.IsRunning = true;
                });

                _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var content = await _client.GetStringAsync(_url);
                var config = JsonConvert.DeserializeObject<List<Config>>(content);

                lstClient.ItemsSource = config;

                _token = Application.Current.Properties.ContainsKey("Token") ? Application.Current.Properties["Token"].ToString() : "";
                if (string.IsNullOrEmpty(_token.ToString()))
                {
                    Register();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Main_Activity.IsRunning = false;
                        Main_Activity.IsVisible = false;
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Format("Exception : {0}", this), ex.Message, "OK");
            }
            finally
            {
                base.OnAppearing();
            }
        }

        async void SelectedItem_Change(object sender, System.EventArgs e)
        {
            var selected = lstClient.SelectedItem as Config;
            await Navigation.PushAsync(new frmSub(selected));
        }

        void Register()
        {
            CrossPushNotification.Current.Register();
        }
        void UnRegister()
        {
            CrossPushNotification.Current.Unregister();
        }

        void DisplayMsg(CrossPushNotificationListener cs, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("Notification", message, "OK");
                Main_Activity.IsRunning = false;
                Main_Activity.IsVisible = false;
            });
        }

        async void Test(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TestPage());
        }
    }
}
