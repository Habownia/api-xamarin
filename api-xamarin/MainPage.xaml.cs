using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;


namespace api_xamarin
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiHelper apiHelper;
        private bool dataLoaded = false;

        public MainPage()
        {
            InitializeComponent();
            apiHelper = new ApiHelper();
        }

        public Image GetFlag(string currCode)
        {
            CurrencyCountryConverter converter = new CurrencyCountryConverter();
            string countryCode = converter.ConvertCurrencyToCountry(currCode).ToLower();
            Image image = new Image();
            image.Source = ImageSource.FromUri(new Uri($"https://flagcdn.com/96x72/{countryCode}.png"));
            return image;
        }

        public async void ShowData()
        {
            Label ErrorLabel = new Label();
            var exchangeRate = await apiHelper.GetApiDataAsync();

            if (exchangeRate != null)
            {
                apiDataContainer.Children.Clear();
                ErrorLabel.Text = "Ładowanie";
                apiDataContainer.Children.Add(ErrorLabel);

                // Wyświetl dane z API na etykiecie
                apiDataContainer.Children.Clear();
                Label info = new Label
                {
                    Text = $"No: {exchangeRate.no}\n" +
                    $"Effective Date: {exchangeRate.effectiveDate}\n" +
                    "Rates:\n",
                };
                apiDataContainer.Children.Add(info);


                foreach (var rate in exchangeRate.rates)
                {
                    StackLayout rateContainer = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                    };
                    Label dataLabel = new Label
                    {
                        Text = $"{rate.currency} ({rate.code}): {rate.mid}"
                    };
                    rateContainer.Children.Add(GetFlag(rate.code));
                    rateContainer.Children.Add(dataLabel);
                    apiDataContainer.Children.Add(rateContainer);
                }
                dataLoaded = true;
            }
            else
            {
                apiDataContainer.Children.Clear();
                ErrorLabel.Text = "Błąd podczas pobierania danych z API.";
                apiDataContainer.Children.Add(ErrorLabel);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowData(); // pokazuje dane za raz po załadowaniu
        }

        private void Reload(object sender, EventArgs e)
        {
            ShowData();//Odświeża dane ale to wsm nic nie zmienia bo dane się nie zmieniają co sekunde
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            apiDataContainer.Children.Clear();
            Label ErrorLabel = new Label();
            ErrorLabel.Text = "Błąd podczas pobierania danych z API.";
            apiDataContainer.Children.Add(ErrorLabel);
        }
    }
}