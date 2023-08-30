using Newtonsoft.Json;

namespace Pack__n__Go
{
    public partial class Optionals : ContentPage
    {
        string stagione;
        string nomeVacanza;
        int durata;
        string categoria;
        string categoriaMaiuscola;
        string nomeFileJSON;

        // Creo il dictionary per gli x:Name delle checkbox
        Dictionary<string, CheckBox> myCheckboxList;

        // Lista di checkbox true
        List<string> checkboxTrue;

        public Optionals(string stagione, string nomeVacanza, int durata, string categoria)
        {
            InitializeComponent();

            this.stagione = stagione;
            this.nomeVacanza = nomeVacanza;
            this.durata = durata;
            this.categoria = categoria;

            string[] temp = beautifyCategoria(this.categoria);
            categoriaMaiuscola = temp[0];
            nomeFileJSON = temp[1];

            myCheckboxList = new Dictionary<string, CheckBox>();

            SizeChanged += OnPageSizeChanged;

            GeneraXaml(this.stagione, this.categoria);
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            SetGeneraWidth();
            SetBackWidth();
        }

        private void SetGeneraWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthEntry = widthSchermo * 0.2;
            buttonGeneraValigia.WidthRequest = widthEntry;
        }
        
        private void SetBackWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthEntry = widthSchermo * 0.2;
            backButton.WidthRequest = widthEntry;
        }

        private async Task GeneraXaml(string stagione, string categoria)
        {
            // Controllo se è connesso a internet
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Genero la label con la categoria scelta
                Label categoriaScelta = new Label
                {
                    Text = categoriaMaiuscola,
                    FontSize = 30,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                StackLayout stackLayoutCategoria = this.FindByName<StackLayout>("dynamicCategoria");
                stackLayoutCategoria.Children.Add(categoriaScelta);

                // Leggo da Github il file optionals.json
                string path = "https://raw.githubusercontent.com/Mattiadv03/PackNGo/master/Resources/JSON/optionals.json";

                // Creo un'istanza di HttpClient
                HttpClient client = new HttpClient();

                // Scarico il file JSON da Internet
                HttpResponseMessage response = await client.GetAsync(path);

                // Controllo se la richiesta è andata a buon fine
                if (response.IsSuccessStatusCode)
                {
                    // Ottiengo il corpo della risposta
                    string data = await response.Content.ReadAsStringAsync();

                    // Deserializzo il JSON
                    Opzionali opzionali = JsonConvert.DeserializeObject<Opzionali>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprieta in opzionali.GetType().GetProperties())
                    {
                        // Crea una checkbox
                        CheckBox checkBox = new CheckBox
                        {
                            IsChecked = false,
                            Color = Color.FromHex("#BB86FC"),
                            VerticalOptions = LayoutOptions.Center
                        };

                        // Aggiungo al dictionary
                        myCheckboxList.Add(proprieta.Name, checkBox);

                        // Crea una label
                        Label label = new Label
                        {
                            Text = proprieta.Name,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            GestureRecognizers =
                                    {
                                        new TapGestureRecognizer
                                        {
                                            Command = new Command(() =>
                                            {
                                                checkBox.IsChecked = !checkBox.IsChecked;
                                            })
                                        }
                                    }
                        };

                        // StackLayout padre per ogni gruppo checkbox + label
                        StackLayout stackLayoutPadre = new StackLayout
                        {
                            Padding = new Thickness(0),
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Center,
                            Orientation = StackOrientation.Horizontal
                        };

                        stackLayoutPadre.Children.Add(checkBox);
                        stackLayoutPadre.Children.Add(label);

                        // Aggiungi la checkbox e la label al layout
                        StackLayout stackLayout = this.FindByName<StackLayout>("layoutCheckBox");
                        stackLayout.Children.Add(stackLayoutPadre);
                    }
                }
                else
                {
                    await DisplayAlert("Errore", "File non trovato, controlla il repository su Github", "OK");
                }
            }
            else
            {
                await DisplayAlert("Errore", "Connettiti a internet per utilizzare l'applicazione", "OK");
            }
        }

        private string[] beautifyCategoria(string categoria)
        {
            if (categoria == "mare")
            {
                categoriaMaiuscola = "Mare";
                nomeFileJSON = "mare-lago.json";
            }
            else if (categoria == "montagna")
            {
                categoriaMaiuscola = "Montagna";
                nomeFileJSON = "montagna.json";
            }
            else if (categoria == "lago")
            {
                categoriaMaiuscola = "Lago";
                nomeFileJSON = "mare-lago.json";
            }
            else if (categoria == "visitaCitta")
            {
                categoriaMaiuscola = "Visita di città";
                nomeFileJSON = "visitaCitta.json";
            }
            else
            {
                categoriaMaiuscola = "errore";
                nomeFileJSON = "errore";
            }

            return new string[2]
            {
                categoriaMaiuscola,
                nomeFileJSON
            };
        }

        private void GeneraValigia(object sender, EventArgs e)
        {
            checkboxTrue = new List<string>();

            // Prendo le chiavi delle checkbox true
            foreach (var keys in myCheckboxList.Keys)
            {
                // Prendo la checkbox di riferimento
                CheckBox cb = myCheckboxList[keys];

                // Controllo se è checked
                if (cb.IsChecked)
                {
                    checkboxTrue.Add(keys);
                }
            }

            // Mando alla pagina di generazione della valigia
            Navigation.PushAsync(new Valigia(stagione, nomeVacanza, durata, categoria, categoriaMaiuscola, nomeFileJSON, checkboxTrue));
        }

        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}