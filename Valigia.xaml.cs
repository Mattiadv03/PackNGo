using Newtonsoft.Json;
using System.Reflection;

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
                    Text = "Valigia " + categoriaMaiuscola,
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
                // Ottengo il corpo della risposta
                string data = await response.Content.ReadAsStringAsync();

                // Deserializzo il JSON
                if(nomeFile == "default.json")
                {
                    Default datiDefault = JsonConvert.DeserializeObject<Default>(data);

                    foreach (var categoryProperty in datiDefault.GetType().GetProperties())
                    {
                        var categoryValue = categoryProperty.GetValue(datiDefault);
                        data = categoryValue.ToString();

                        Console.WriteLine(categoryValue.GetType());

                        if (categoryValue is string[] itemList)
                        {
                            Console.WriteLine($"{categoryProperty.Name}:");

                            foreach (var item in itemList)
                            {
                                Console.WriteLine($"- {item}");
                            }
                        }
                        else if (categoryValue is Dictionary<string, object> itemDictionary)
                        {
                            Console.WriteLine($"{categoryProperty.Name}:");

                            foreach (var kvp in itemDictionary)
                            {
                                Console.WriteLine($"- {kvp.Key}: {kvp.Value}");
                            }
                        }
                        else if (categoryValue is int intValue)
                        {
                            Console.WriteLine($"{categoryProperty.Name}: {intValue}");
                        }
                    }

                    /*// Itera sull'oggetto JSON
                    foreach (var proprietà in datiDefault.GetType().GetProperties())
                    {
                        // Itera sull'array e stampa il valore di ogni elemento
                        foreach (string elemento in proprietà.GetValue(datiDefault))
                        {
                            Console.WriteLine("Elemento: {0}", elemento);
                        }
                    }*/

                    // Creo le checkbox per ogni categoria
                    foreach (var proprieta in datiDefault.GetType().GetProperties())
                    {

                        /*foreach (string elemento in proprieta)
                        {
                            Console.WriteLine("Elemento: {0}", elemento);
                        }*/

                        // Crea una checkbox
                        CheckBox checkBox = new CheckBox
                        {
                            IsChecked = false,
                            Color = Color.FromHex("#BB86FC")
                        };

                        // Crea una label
                        Label label = new Label
                        {
                            //Text = proprieta
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
                        StackLayout stackLayout = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayout.Children.Add(stackLayoutPadre);
                    }
                }  
                else if (nomeFile == "optionals.json")
                {
                    Opzionali opzionali = JsonConvert.DeserializeObject<Opzionali>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprietà in opzionali.GetType().GetProperties())
                    {
                        // Crea una checkbox
                        CheckBox checkBox = new CheckBox
                        {
                            IsChecked = false,
                            Color = Color.FromHex("#BB86FC")
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
                        StackLayout stackLayout = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayout.Children.Add(stackLayoutPadre);
                    }
                }
                else if (nomeFile == "mare-lago.json")
                {
                    Mare_lago mare_lago = JsonConvert.DeserializeObject<Mare_lago>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprietà in mare_lago.GetType().GetProperties())
                    {
                        // Filtro in base alla stagione
                        if(stagione == "estate" && proprietà.Name == "Vestitiinverno")
                        {
                            continue;
                        }
                        else if (stagione == "inverno" && proprietà.Name == "Vestitiestate")
                        {
                            continue;
                        }
                        else 
                        {
                            // Crea una checkbox
                            CheckBox checkBox = new CheckBox
                            {
                                IsChecked = false,
                                Color = Color.FromHex("#BB86FC")
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
                            StackLayout stackLayout = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                            stackLayout.Children.Add(stackLayoutPadre);
                        }
                    }
                }
                else if (nomeFile == "montagna.json")
                {
                    Montagna montagna = JsonConvert.DeserializeObject<Montagna>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprietà in montagna.GetType().GetProperties())
                    {
                        // Crea una checkbox
                        CheckBox checkBox = new CheckBox
                        {
                            IsChecked = false,
                            Color = Color.FromHex("#BB86FC")
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
                        StackLayout stackLayout = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayout.Children.Add(stackLayoutPadre);
                    }
                }
                else if (nomeFile == "visitaCitta.json")
                {
                    VisitaCitta visitaCitta = JsonConvert.DeserializeObject<VisitaCitta>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprietà in visitaCitta.GetType().GetProperties())
                    {
                        // Crea una checkbox
                        CheckBox checkBox = new CheckBox
                        {
                            IsChecked = false,
                            Color = Color.FromHex("#BB86FC")
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
                        StackLayout stackLayout = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayout.Children.Add(stackLayoutPadre);
                    }
                }
                else
                {
                    await DisplayAlert("Errore", "Nome del file da caricare errato, controllare codice", "OK");
                }
            }
            else
            {
                await DisplayAlert("Errore", "Connettiti a internet per utilizzare l'applicazione", "OK");
            }
        }


        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}