using Newtonsoft.Json;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Pack__n__Go
{
    public partial class Valigia : ContentPage
    {
        string stagione;
        string nomeVacanza;
        int durata;
        string categoria;
        string categoriaMaiuscola;
        string nomeFileJSON;

        // Lista di checkbox true
        List<string> checkboxTrue;

        // Stacklayout
        StackLayout stackLayoutSezione;
        StackLayout stackLayoutBase;


        public Valigia(string stagione, string nomeVacanza, int durata, string categoria, string categoriaMaiuscola, string nomeFileJSON, List<string> checkboxTrue)
        {
            InitializeComponent();

            this.stagione = stagione;
            this.nomeVacanza = nomeVacanza;
            this.durata = durata;
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
                    Text = "Valigia " + categoriaMaiuscola + " - " + nomeVacanza,
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
            //string path = "https://raw.githubusercontent.com/Mattiadv03/PackNGo/master/Resources/JSON/temp/string.json";

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
                if (nomeFile == "default.json")
                {
                    Default datiDefault = JsonConvert.DeserializeObject<Default>(data);

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = datiDefault.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                        string nomeProprieta = property.Name;
                        if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                        {
                            nomeProprieta = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                        }

                        // Creo il titolo di questa sezione
                        Label labelTitolo = new Label
                        {
                            Text = nomeProprieta,
                            TextColor = Color.FromHex("#BB86FC"),
                            FontSize = 30,
                            HorizontalTextAlignment = TextAlignment.Center
                        };

                        // Creo lo stacklayout che conterrà tutta la sezione
                        stackLayoutSezione = new StackLayout
                        {
                            Padding = new Thickness(0),
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        };

                        // Aggiungo il titolo della sezione
                        stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayoutBase.Children.Add(labelTitolo);

                        stackLayoutSezione.Children.Add(labelTitolo);

                        // Prendo i dati in base alla proprietà
                        object value = property.GetValue(datiDefault);

                        // Skippo se c'è un oggetto senza valori assegnati
                        if (value == null)
                        {
                            continue;
                        }

                        PropertyInfo[] intimoProperties = value.GetType().GetProperties();

                        foreach (PropertyInfo intimoProperty in intimoProperties)
                        {
                            // Popolo la sezione con le checkbox
                            object intimoPropertyValue = intimoProperty.GetValue(value);

                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            nomeProprieta = intimoProperty.Name;
                            if (intimoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeProprieta = intimoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Controllo se è un array o un oggetto
                            if (intimoPropertyValue is string || intimoPropertyValue is int)
                            {
                                    // E' una stringa o un intero
                                // Creo una checkbox
                                CheckBox checkBox = GeneraCheckbox();

                                // Crea una label
                                Label label = new Label
                                {
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

                                if (intimoPropertyValue is string intimoString)
                                {
                                    // Controllo se è un valore o un'espressione
                                    string stringaLetta = intimoString;

                                    if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                    {
                                        // Espressione con /
                                        string[] parts = stringaLetta.Split('/');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) / numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        label.Text = nomeProprieta + " ➞ " + numeroOggetti;
                                    }
                                    else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                    {
                                        // Espressione con *
                                        string[] parts = stringaLetta.Split('*');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) * numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        label.Text = nomeProprieta + " ➞ " + numeroOggetti;
                                    }
                                    else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                    {
                                        // Espressione con +
                                        string[] parts = stringaLetta.Split('+');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) + numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        label.Text = nomeProprieta + " ➞ " + numeroOggetti;
                                    }

                                    else
                                    {
                                        label.Text = nomeProprieta + " ➞ " + intimoString;
                                    }
                                }
                                else if (intimoPropertyValue is int intimoInt)
                                {
                                    label.Text = nomeProprieta + " ➞ " + intimoInt.ToString();
                                }

                                // Creo lo stacklayout che lo contiene
                                StackLayout stackLayoutCheckbox = new StackLayout
                                {
                                    Orientation = StackOrientation.Horizontal
                                };

                                // StackLayout padre per ogni gruppo checkbox + label
                                stackLayoutCheckbox.Children.Add(checkBox);
                                stackLayoutCheckbox.Children.Add(label);

                                // Aggiungi la checkbox e la label al layout
                                stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                                stackLayoutBase.Children.Add(stackLayoutCheckbox);
                            }
                            else if (intimoPropertyValue is IList<string> intimoStringList)
                            {
                                    // E' un array
                                foreach (string intimoListValue in intimoStringList)
                                {
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();

                                    // Crea una label
                                    Label label = new Label 
                                    {
                                        Text = intimoListValue,
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

                                    // Creo lo stacklayout che lo contiene
                                    StackLayout stackLayoutCheckbox = new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal
                                    };

                                    // StackLayout padre per ogni gruppo checkbox + label
                                    stackLayoutCheckbox.Children.Add(checkBox);
                                    stackLayoutCheckbox.Children.Add(label);

                                    // Aggiungi la checkbox e la label al layout
                                    stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                                    stackLayoutBase.Children.Add(stackLayoutCheckbox);
                                }
                            }
                        }
                    }
                }
                else if (nomeFile == "optionals.json")
                {
                    Opzionali opzionali = JsonConvert.DeserializeObject<Opzionali>(data);

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = opzionali.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Controllo se presente in checkboxTrue
                        if (checkboxTrue.Contains(property.Name))
                        {
                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            string nomeProprieta = property.Name;
                            if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeProprieta = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Creo il titolo di questa sezione
                            Label labelTitolo = new Label
                            {
                                Text = nomeProprieta,
                                TextColor = Color.FromHex("#BB86FC"),
                                FontSize = 30,
                                HorizontalTextAlignment = TextAlignment.Center
                            };

                            // Creo lo stacklayout che conterrà tutta la sezione
                            stackLayoutSezione = new StackLayout
                            {
                                Padding = new Thickness(0),
                                HorizontalOptions = LayoutOptions.Center,
                                VerticalOptions = LayoutOptions.Center
                            };

                            // Aggiungo il titolo della sezione
                            stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                            stackLayoutBase.Children.Add(labelTitolo);

                            stackLayoutSezione.Children.Add(labelTitolo);

                            // Prendo i dati in base alla proprietà
                            object value = property.GetValue(opzionali);

                            // Skippo se c'è un oggetto senza valori assegnati
                            if (value == null)
                            {
                                continue;
                            }

                            PropertyInfo[] elementProperties = value.GetType().GetProperties();

                            foreach (PropertyInfo elementProperty in elementProperties)
                            {
                                // Popolo la sezione con le checkbox
                                object elementPropertiesValue = elementProperty.GetValue(value);

                                // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                                nomeProprieta = elementProperty.Name;
                                if (elementProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                                {
                                    nomeProprieta = elementProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                                }

                                // Controllo se è un array o un oggetto
                                if (elementPropertiesValue is string || elementPropertiesValue is int)
                                {
                                    // E' una stringa o un intero
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();

                                    // Crea una label
                                    Label label = new Label
                                    {
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

                                    if (elementPropertiesValue is string intimoString)
                                    {
                                        // Controllo se è un valore o un'espressione
                                        string stringaLetta = intimoString;

                                        if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                        {
                                            // Espressione con /
                                            string[] parts = stringaLetta.Split('/');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) / numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            label.Text = nomeProprieta + " ➞ " + numeroOggetti;
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                        {
                                            // Espressione con *
                                            string[] parts = stringaLetta.Split('*');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) * numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            label.Text = nomeProprieta + " ➞ " + numeroOggetti;
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                        {
                                            // Espressione con +
                                            string[] parts = stringaLetta.Split('+');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) + numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            label.Text = nomeProprieta + " ➞ " + numeroOggetti;
                                        }

                                        else
                                        {
                                            label.Text = nomeProprieta + " ➞ " + intimoString;
                                        }
                                    }
                                    else if (elementPropertiesValue is int intimoInt)
                                    {
                                        label.Text = nomeProprieta + " ➞ " + intimoInt.ToString();
                                    }

                                    // Creo lo stacklayout che lo contiene
                                    StackLayout stackLayoutCheckbox = new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal
                                    };

                                    // StackLayout padre per ogni gruppo checkbox + label
                                    stackLayoutCheckbox.Children.Add(checkBox);
                                    stackLayoutCheckbox.Children.Add(label);

                                    // Aggiungi la checkbox e la label al layout
                                    stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                                    stackLayoutBase.Children.Add(stackLayoutCheckbox);
                                }
                                else if (elementPropertiesValue is IList<string> intimoStringList)
                                {
                                    // E' un array
                                    foreach (string intimoListValue in intimoStringList)
                                    {
                                        // Creo una checkbox
                                        CheckBox checkBox = GeneraCheckbox();

                                        // Crea una label
                                        Label label = new Label
                                        {
                                            Text = intimoListValue,
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

                                        // Creo lo stacklayout che lo contiene
                                        StackLayout stackLayoutCheckbox = new StackLayout
                                        {
                                            Orientation = StackOrientation.Horizontal
                                        };

                                        // StackLayout padre per ogni gruppo checkbox + label
                                        stackLayoutCheckbox.Children.Add(checkBox);
                                        stackLayoutCheckbox.Children.Add(label);

                                        // Aggiungi la checkbox e la label al layout
                                        stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                                        stackLayoutBase.Children.Add(stackLayoutCheckbox);
                                    }
                                }
                            }
                        }
                        
                    }
                }
                else if (nomeFile == "mare-lago.json")
                {
                    Mare_lago mare_lago = JsonConvert.DeserializeObject<Mare_lago>(data);

                    // Creo le checkbox per ogni categoria
                    foreach (var proprieta in mare_lago.GetType().GetProperties())
                    {
                        // Filtro in base alla stagione
                        if (stagione == "estate" && proprieta.Name == "Vestitiinverno")
                        {
                            continue;
                        }
                        else if (stagione == "inverno" && proprieta.Name == "Vestitiestate")
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
                                Text = proprieta.Name
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
                    foreach (var proprieta in montagna.GetType().GetProperties())
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
                            Text = proprieta.Name
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
                    foreach (var proprieta in visitaCitta.GetType().GetProperties())
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
                            Text = proprieta.Name
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

        private CheckBox GeneraCheckbox()
        {
            return new CheckBox
            {
                IsChecked = false,
                Color = Color.FromHex("#BB86FC"),
                VerticalOptions = LayoutOptions.Center,
                //InputTransparent = true // Diventa non più cliccabile
            };
        }

        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}