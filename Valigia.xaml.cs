using Newtonsoft.Json;

namespace Pack__n__Go
{
    public partial class Valigia : ContentPage
    {
        string stagione;
        string categoria;
        string categoriaMaiuscola;
        string nomeFileJSON;

        // Lista di checkbox true
        List<string> checkboxTrue;

        public Valigia(string stagione, string categoria, string categoriaMaiuscola, string nomeFileJSON, List<string> checkboxTrue)
        {
            InitializeComponent();

            this.stagione = stagione;
            this.categoria = categoria;
            this.categoriaMaiuscola = categoriaMaiuscola;
            this.nomeFileJSON = nomeFileJSON;
            this.checkboxTrue = checkboxTrue;

            GeneraXaml(categoriaMaiuscola, nomeFileJSON);
        }

        private async Task GeneraXaml(string categoriaMaiuscola, string nomeFileJSON)
        {
            // Controllo se è connesso a internet
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                // Genero la label con la categoria scelta
                Label categoriaScelta = new Label
                {
                    Text = "Valigia" + categoriaMaiuscola,
                    FontSize = 30,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                StackLayout stackLayoutCategoria = this.FindByName<StackLayout>("dynamicCategoria");
                stackLayoutCategoria.Children.Add(categoriaScelta);

                // Carico il default
                await JsonToCheckBox("default.json");

                // Carico l'optionals
                await JsonToCheckBox("optionals.json");

                // Carico il file della categoria
                await JsonToCheckBox(nomeFileJSON);
            }
        }

        private async Task JsonToCheckBox(string nomeFile)
        {
            // Leggo da Github il file
            string path = "https://raw.githubusercontent.com/Mattiadv03/PackNGo/master/Resources/JSON/" + nomeFile;

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
                if(nomeFile == "default.json")
                {
                    Default myDeserializedClass = JsonConvert.DeserializeObject<Default>(data);
                }  
                else if (nomeFile == "optionals.json")
                {
                    Opzionali opzionali = JsonConvert.DeserializeObject<Opzionali>(data);
                }
                else if (nomeFile == "mare-lago.json")
                {
                    Mare_lago myDeserializedClass = JsonConvert.DeserializeObject<Mare_lago>(data);
                }
                else if (nomeFile == "montagna.json")
                {

                }
                else if (nomeFile == "visitaCitta.json")
                {

                }
                else
                {
                    DisplayAlert("Errore", "Nome del file da caricare errato, controllare codice", "OK");
                }
            }

        }


        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}