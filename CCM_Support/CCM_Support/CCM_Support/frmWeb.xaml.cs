using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CCM_Support
{
    public partial class frmWeb : ContentPage
    {
        public frmWeb(string source)
        {
            InitializeComponent();
            //WebViewer.Source = source;
            Device.OpenUri(new Uri(source));
        }

        public frmWeb(HtmlWebViewSource source)
        {
            InitializeComponent();
            WebViewer.Source = source;
        }
    }
}
