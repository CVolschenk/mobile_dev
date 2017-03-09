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
    public partial class frmSub : ContentPage
    {
        private Config _config;
        private HttpClient _client = new HttpClient();

        public frmSub(Config config)
        {
            if (config == null)
                throw new ArgumentNullException();
            _config = config;
            BindingContext = _config;

            InitializeComponent();

            NavigationPage.SetTitleIcon(this, "Sybrin_Horizontal.png");
        }

        protected override void OnAppearing()
        {
            lstActionType.ItemsSource = _config.ActionType;

            base.OnAppearing();
        }

        async void Item_Selected(object sender, System.EventArgs e)
        {
            var item = lstActionType.SelectedItem as ActionType;

            Device.BeginInvokeOnMainThread(() =>
            {
                Sub_Activity.IsRunning = true;
                lstActionType.IsEnabled = false;
            });
            
            if (item.Type == "RWS")
            {
                var request = string.Format("{0}{1}", item.Url, item.Indno);

                _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json;version=1");
                _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                var content = DecodeBase64(await _client.GetStringAsync(request));

                var htmlSource = new HtmlWebViewSource();
                var htmlString = "<html style=\"color: White; background-color:Black; \"><body>" + content + "<body/><html/>";

                htmlSource.Html = htmlString.Replace("\t","").Replace("\n","").Replace("\r","").Replace("\\","");

                await Navigation.PushAsync(new frmWeb(htmlSource));
            }
            else
            {
                await Navigation.PushAsync(new frmWeb(item.Url));
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                Sub_Activity.IsRunning = false;
                lstActionType.IsEnabled = true;
            });
        }

        public string DecodeBase64(string input)
        {
            string test = input.Replace("\"", "");
            string incoming = test.Replace('_', '/').Replace('-', '+');
            switch (test.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(incoming);
            return Encoding.UTF8.GetString(bytes,0 ,bytes.Length);
        }
    }
}
