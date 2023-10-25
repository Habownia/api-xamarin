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
        // Słownik publiczny, który zawiera nazwę typu flagi i jego wartość do API
        static public Dictionary<string, string> flagDesign = new Dictionary<string, string>()
        {
            {"Pofalowane", "96x72"},
            {"Proste", "w80"}
        };
        // Ustawia jako domyślne pofalowane flagi
        private string flagType = flagDesign["Pofalowane"];




        public MainPage()
        {
            InitializeComponent();
            apiHelper = new ApiHelper();
        }

        public Image GetFlag(string currCode)
        {
            CurrencyCountryConverter converter = new CurrencyCountryConverter();
            string countryCode = converter.ConvertCurrencyToCountry(currCode).ToLower();
            Image image = new Image
            {
                Source = ImageSource.FromUri(new Uri($"https://flagcdn.com/{flagType}/{countryCode}.png"))
            };
            return image;
        }

        public void ShowDataInUI(ExchangeRate r, Label errLbl, StackLayout container) // nie trzeba tego 3 elementu ale w razie przejścia na full C# będzie on przydatny (teraz jest tu trochę bajzel)
        {
            container.Children.Clear();
            errLbl.Text = "Ładowanie";
            container.Children.Add(errLbl);

            // Wyświetl dane z API na etykiecie
            container.Children.Clear();
            Label info = new Label
            {
                Text = $"No: {r.no}\n" +
                $"Effective Date: {r.effectiveDate}\n" +
                "Rates:\n",
            };
            container.Children.Add(info);


            foreach (var rate in r.rates)
            {
                StackLayout rateContainer = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                };

                string currName = rate.currency;
                string rateWithoutPar = currName.IndexOf("(") > 0
                    ? currName.Substring(0, currName.IndexOf("(") - 1)
                    : currName;
                Label name = new Label
                {
                    Text = rateWithoutPar,
                    FontSize = 15,
                    VerticalTextAlignment = TextAlignment.Center,
                };
                Label code = new Label
                {
                    Text = $"({rate.code}): ",
                    VerticalTextAlignment = TextAlignment.Center,

                };
                Label rateValue = new Label
                {
                    Text = $"{rate.mid} zł",
                    VerticalTextAlignment = TextAlignment.Center,

                };

                rateContainer.Children.Add(GetFlag(rate.code));
                rateContainer.Children.Add(name);
                rateContainer.Children.Add(code);
                rateContainer.Children.Add(rateValue);

                container.Children.Add(rateContainer);
            }
            dataLoaded = true;
        }

        public async void ShowData()
        {
            Label ErrorLabel = new Label();
            var exchangeRate = await apiHelper.GetApiDataAsync();

            if (exchangeRate != null) // Pokazuje dane jeśl nie wystąpił błąd
            {
                ShowDataInUI(exchangeRate, ErrorLabel, apiDataContainer);
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

        private async void Reload(object sender, EventArgs e)
        {
            await reloadButton.RelRotateTo(-360, 500); // kręci tym buttonem, żeby było widać, że coś się dzieje
            ShowData();//Odświeża dane ale to wsm nic nie zmienia bo dane się nie zmieniają co sekunde
        }

        // Wywołuje się po zmianie indexu w pickerze
        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            string selectedItem = picker.SelectedItem.ToString(); // Wybrany element jako tekst
            flagDesign.TryGetValue(selectedItem, out flagType); // pobiera dane ze słownika po kluczu i ustawia wartość w zmiennej flagType
        }


        private void Info(object sender, EventArgs e)
        {
            apiDataContainer.Children.Clear();

            Label Title = new Label()
            {
                Text = "exchange-today",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold,
            };

            Label Description = new Label()
            {
                Text = "Jest to projekt na zajęcia programowania aplikacji mobilnych. Apilkacja używa połączenia z API Narodowego Banku Polskiego (NBP) i pokazuje deserializację z obiektu JSON na obiekt w języku C#. \n\nKolejnym punktem tego projektu jest stworzenie aplikacji, która jest przystępna wizualnie"
            };

            Label SettingsTitle = new Label()
            {
                Text = "# Ustawienia",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
            };

            Label SettingsDesc = new Label()
            {
                Text = "Wybierz typ flag: ",
            };

            string[] flagDesignNameSource = flagDesign.Keys.ToArray(); // pobiera tylko klucze słownika i dodaje do pickera
            Picker Flags = new Picker
            {
                ItemsSource = flagDesignNameSource,
                SelectedIndex = flagType == "96x72" ? 0 : 1,
                TextColor = Color.White,
            };
            Flags.SelectedIndexChanged += OnPickerSelectedIndexChanged;

            StackLayout SettingCont = new StackLayout();
            SettingCont.Children.Add(SettingsDesc);
            SettingCont.Children.Add(Flags);

            apiDataContainer.Children.Add(Title);
            apiDataContainer.Children.Add(Description);
            apiDataContainer.Children.Add(SettingCont);
        }

        private void TitleTapped(object sender, EventArgs e) => ShowData(); // po kliknięciu w tytuł przenosi cię na stronę "główną"
    }
}