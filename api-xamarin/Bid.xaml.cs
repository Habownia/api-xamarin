using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace api_xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bid : ContentPage
    {
        public Bid()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public async void Navigate_Main(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}