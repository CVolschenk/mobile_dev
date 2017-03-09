
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CCM_Support
{
    public partial class frmLogin : ContentPage
    {
        private string _url = "http://gateway5.sybrin.systems/GWRobotServices/api/session";
        private HttpClient _client = new HttpClient(); 

        public frmLogin()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetTitleIcon(this, "Sybrin_Horizontal.png");

            BindingContext = Application.Current;
        }

        async void Login_Clicked(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Login_Activity.IsRunning = true;
                btnLogin.IsEnabled = false;
            });

            try
            {
                var loginRequest = new LoginRequest { SecurityModel = "ADSI", UserID = txtUsername.Text, Password = txtPassword.Text };
                var content = JsonConvert.SerializeObject(loginRequest);
                var content2 = new StringContent(content, Encoding.UTF8, "application/json");
                _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                var response = _client.PostAsync(_url, content2).Result;

                if (response.IsSuccessStatusCode)
                {
                    //HockeyApp.MetricsManager.TrackEvent("Login Event");
                    await Application.Current.SavePropertiesAsync();
                    await Navigation.PushAsync(new frmMain());
                }

                else
                {
                    await DisplayAlert("Login Failure", response.StatusCode + " | " + response.ToString(), "OK");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(string.Format("Exception : {0}", this), ex.Message, "OK");
            }


            Device.BeginInvokeOnMainThread(() =>
            {
                btnLogin.IsEnabled = true;
                Login_Activity.IsRunning = false;
                Login_Activity.IsVisible = false;
            });

        }

    }
}
