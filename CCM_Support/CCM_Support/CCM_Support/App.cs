using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CCM_Support
{
    public class App : Application
    {
        private const string _username = "Username";
        private const string _password = "Password";
        //private const string _token = "Token";

        public App()
        {
            // The root page of your application
            MainPage = new NavigationPage(new frmLogin());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            //Save App data
            Application.Current.SavePropertiesAsync();
            //unsubscribe to Messages
            MessagingCenter.Unsubscribe<frmMain>(this, "DisplayMsg");
            MessagingCenter.Unsubscribe<TestPage>(this, "DisplayMsg");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public string Username
        {
            get
            {
                if (Properties.ContainsKey(_username)) return Properties[_username].ToString();
                return "";
            }
            set
            {
                Properties[_username] = value;
            }
        }

        public string Password
        {
            get
            {
                if (Properties.ContainsKey(_password)) return Properties[_password].ToString();
                return "";
            }
            set
            {
                Properties[_password] = value;
            }
        }
        //public string Token
        //{
        //    get
        //    {
        //        if (Properties.ContainsKey(_token)) return Properties[_token].ToString();
        //        return "";
        //    }
        //    set
        //    {
        //        Properties[_token] = value;
        //    }
        //}


    }
}
