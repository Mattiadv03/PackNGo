using Newtonsoft.Json;
using Microsoft.Maui.Controls;

namespace Pack__n__Go
{
    public partial class Mare : ContentPage
    {
        string stagione;

        public Mare(string stagione)
        {
            InitializeComponent();

            this.stagione = stagione;

            GeneraCheckBoxAsync(this.stagione);
        }

        private async Task GeneraCheckBoxAsync(string stagione)
        {
            // Controllo se è connesso
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Il percorso del file JSON da Internet
                string path = "https://raw.githubusercontent.com/Mattiadv03/PackNGo/master/Resources/JSON/items.json";

                // Crea un'istanza di HttpClient
                HttpClient client = new HttpClient();

                // Scarica il file JSON da Internet
                HttpResponseMessage response = await client.GetAsync(path);

                // Controlla se la richiesta è andata a buon fine
                if (response.IsSuccessStatusCode)
                {
                    // Ottieni il corpo della risposta
                    string data = await response.Content.ReadAsStringAsync();

                    // Deserializza il JSON
                    Root items = JsonConvert.DeserializeObject<Root>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprietà in items.GetType().GetProperties())
                    {
                        // Se le stagioni non corrispondono salto l'iterazione, così ottengo solo i vestiti per la stagione corretta
                        if (stagione == "estate" && proprietà.Name == "VestitiInverno")
                        {
                            continue;
                        } 
                        else if (stagione == "inverno" && proprietà.Name == "VestitiEstate") 
                        {
                            continue;
                        }
                        else
                        {
                            // Crea una checkbox
                            CheckBox checkBox = new CheckBox
                            {
                                IsChecked = false
                            };

                            // Crea una label
                            Label label = new Label
                            {
                                Text = proprietà.Name
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
                }
            }
        }

        private void backClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}