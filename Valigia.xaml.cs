using Microsoft.Maui.Controls;
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

        // Creo il dictionary per gli x:Name delle checkbox
        Dictionary<string, CheckBox> myCheckboxList;

        // Lista di checkbox true
        List<string> checkboxTrue;
        Dictionary<string, string> dictionaryCheckboxTrue;

        // Stacklayout
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

            myCheckboxList = new Dictionary<string, CheckBox>();
            dictionaryCheckboxTrue = new Dictionary<string, string>();

            SizeChanged += OnPageSizeChanged;

            GeneraXaml(categoriaMaiuscola, nomeFileJSON);
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            SetSaveWidth();
            SetCheckWidth();
            SetBackWidth();
        }

        private void SetSaveWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthButton = widthSchermo * 0.2;
            saveButton.WidthRequest = widthButton;
        }
        
        private void SetCheckWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthButton = widthSchermo * 0.2;
            checkAllButton.WidthRequest = widthButton;
        }
        
        private void SetBackWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthButton = widthSchermo * 0.2;
            backButton.WidthRequest = widthButton;
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

                if (nomeFileJSON == "mare-lago.json" || nomeFileJSON == "montagna.json" || nomeFileJSON == "visitaCitta.json")
                {
                    // Carico il default
                    JsonToCheckBox("default.json");

                    // Carico l'optionals
                    JsonToCheckBox("optionals.json");
                }

                // Carico il file della categoria
                await JsonToCheckBox(nomeFileJSON);

                // Solo l'ultima funzione deve avere l'await, altrimenti non vengono generate le checkbox!!!
            }
            else
            {
                await DisplayAlert("Errore", "Connettiti a internet per utilizzare l'applicazione", "OK");
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
                if (nomeFile == "default.json")
                {
                    Default datiDefault = JsonConvert.DeserializeObject<Default>(data);

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = datiDefault.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                        string nomeSezione = property.Name;
                        if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                        {
                            nomeSezione = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                        }

                        // Creo il titolo di questa sezione
                        Label labelTitolo = new Label
                        {
                            Text = nomeSezione,
                            TextColor = Color.FromHex("#BB86FC"),
                            FontSize = 30,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0, 30, 0, 0) // Applica un margine di 20 top
                        };

                        // Aggiungo il titolo della sezione
                        stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayoutBase.Children.Add(labelTitolo);

                        // Prendo i dati in base alla proprietà
                        object value = property.GetValue(datiDefault);

                        // Skippo se c'è un oggetto senza valori assegnati
                        if (value == null)
                        {
                            continue;
                        }

                        PropertyInfo[] elementoProperties = value.GetType().GetProperties();

                        foreach (PropertyInfo elementoProperty in elementoProperties)
                        {
                            // Popolo la sezione con le checkbox
                            object elementoPropertiesValue = elementoProperty.GetValue(value);

                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            string nomeProprieta = elementoProperty.Name;
                            if (elementoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeProprieta = elementoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Controllo se è un array o un oggetto
                            if (elementoPropertiesValue is string || elementoPropertiesValue is int)
                            {
                                    // E' una stringa o un intero
                                // Creo una checkbox
                                CheckBox checkBox = GeneraCheckbox();

                                myCheckboxList.Add(elementoPropertiesValue + ";" + nomeSezione, checkBox);

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

                                if (elementoPropertiesValue is string elementoString)
                                {
                                    // Controllo se è un valore o un'espressione
                                    string stringaLetta = elementoString;

                                    if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                    {
                                        // Espressione con /
                                        string[] parts = stringaLetta.Split('/');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) / numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }
                                    else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                    {
                                        // Espressione con *
                                        string[] parts = stringaLetta.Split('*');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) * numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }
                                    else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                    {
                                        // Espressione con +
                                        string[] parts = stringaLetta.Split('+');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) + numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }

                                    else
                                    {
                                        string valore = nomeProprieta + " ➞ " + elementoString;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }
                                }
                                else if (elementoPropertiesValue is int elementoInt)
                                {
                                    string valore = nomeProprieta + " ➞ " + elementoInt.ToString();

                                    label.Text = valore;

                                    // Salvo nel dictionary l'elemento
                                    dictionaryCheckboxTrue.Add(valore, nomeSezione);
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
                            else if (elementoPropertiesValue is IList<string> elementoStringList)
                            {
                                    // E' un array
                                foreach (string elementoListValue in elementoStringList)
                                {
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();

                                    myCheckboxList.Add(elementoListValue + ";" + nomeSezione, checkBox);

                                    // Crea una label
                                    Label label = new Label 
                                    {
                                        Text = elementoListValue,
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

                                    // Salvo nel dictionary l'elemento
                                    dictionaryCheckboxTrue.Add(elementoListValue, nomeSezione);

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
                            string nomeSezione = property.Name;
                            if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeSezione = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Creo il titolo di questa sezione
                            Label labelTitolo = new Label
                            {
                                Text = nomeSezione,
                                TextColor = Color.FromHex("#BB86FC"),
                                FontSize = 30,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = new Thickness(0, 30, 0, 0) // Applica un margine di 20 top
                            };

                            // Aggiungo il titolo della sezione
                            stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                            stackLayoutBase.Children.Add(labelTitolo);

                            // Prendo i dati in base alla proprietà
                            object value = property.GetValue(opzionali);

                            // Skippo se c'è un oggetto senza valori assegnati
                            if (value == null)
                            {
                                continue;
                            }

                            PropertyInfo[] elementoProperties = value.GetType().GetProperties();

                            foreach (PropertyInfo elementoProperty in elementoProperties)
                            {
                                // Popolo la sezione con le checkbox
                                object elementoPropertiesValue = elementoProperty.GetValue(value);

                                // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                                string nomeProprieta = elementoProperty.Name;
                                if (elementoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                                {
                                    nomeProprieta = elementoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                                }

                                // Controllo se è un array o un oggetto
                                if (elementoPropertiesValue is string || elementoPropertiesValue is int)
                                {
                                        // E' una stringa o un intero
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();
                                    myCheckboxList.Add(elementoPropertiesValue + ";" + nomeSezione, checkBox);

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

                                    if (elementoPropertiesValue is string elementoString)
                                    {
                                        // Controllo se è un valore o un'espressione
                                        string stringaLetta = elementoString;

                                        if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                        {
                                            // Espressione con /
                                            string[] parts = stringaLetta.Split('/');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) / numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                        {
                                            // Espressione con *
                                            string[] parts = stringaLetta.Split('*');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) * numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                        {
                                            // Espressione con +
                                            string[] parts = stringaLetta.Split('+');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) + numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }

                                        else
                                        {
                                            string valore = nomeProprieta + " ➞ " + elementoString;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                    }
                                    else if (elementoPropertiesValue is int elementoInt)
                                    {
                                        string valore = nomeProprieta + " ➞ " + elementoInt.ToString();

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
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
                                else if (elementoPropertiesValue is IList<string> elementoStringList)
                                {
                                    // E' un array
                                    foreach (string elementoListValue in elementoStringList)
                                    {
                                        // Creo una checkbox
                                        CheckBox checkBox = GeneraCheckbox();

                                        myCheckboxList.Add(elementoListValue + ";" + nomeSezione, checkBox);

                                        // Crea una label
                                        Label label = new Label
                                        {
                                            Text = elementoListValue,
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

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(elementoListValue, nomeSezione);

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

                    // Popolo i vestiti in base alla stagione
                    string nomeOggettoTitolo = "Vestiti" + stagione + "marelago";

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = mare_lago.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Facci in modo da dare una stagione sola
                        if (stagione == "estate" && property.Name == "Vestitiinvernomarelago")
                        {
                            continue;
                        } 
                        else if (stagione == "inverno" && property.Name == "Vestitiestatemarelago")
                        {
                            continue;
                        }
                        else
                        {
                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            string nomeSezione = property.Name;
                            if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeSezione = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;

                                // Sostituisco mare-lago con categoria di vacanza corretta
                                if (nomeSezione.Contains("mare-lago"))
                                {
                                    nomeSezione = nomeSezione.Replace("mare-lago", categoria);
                                }
                            }

                            // Creo il titolo di questa sezione
                            Label labelTitolo = new Label
                            {
                                Text = nomeSezione,
                                TextColor = Color.FromHex("#BB86FC"),
                                FontSize = 30,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = new Thickness(0, 30, 0, 0) // Applica un margine di 20 top
                            };

                            // Aggiungo il titolo della sezione
                            stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                            stackLayoutBase.Children.Add(labelTitolo);

                            // Prendo i dati in base alla proprietà
                            object value = property.GetValue(mare_lago);

                            // Skippo se c'è un oggetto senza valori assegnati
                            if (value == null)
                            {
                                continue;
                            }

                            PropertyInfo[] elementoProperties = value.GetType().GetProperties();

                            foreach (PropertyInfo elementoProperty in elementoProperties)
                            {
                                // Popolo la sezione con le checkbox
                                object elementoPropertiesValue = elementoProperty.GetValue(value);

                                // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                                string nomeProprieta = elementoProperty.Name;
                                if (elementoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                                {
                                    nomeProprieta = elementoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                                }

                                // Controllo se è un array o un oggetto
                                if (elementoPropertiesValue is string || elementoPropertiesValue is int)
                                {
                                    // E' una stringa o un intero
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();

                                    myCheckboxList.Add(elementoPropertiesValue + ";" + nomeSezione, checkBox);

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

                                    if (elementoPropertiesValue is string elementoString)
                                    {
                                        // Controllo se è un valore o un'espressione
                                        string stringaLetta = elementoString;

                                        if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                        {
                                            // Espressione con /
                                            string[] parts = stringaLetta.Split('/');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) / numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                        {
                                            // Espressione con *
                                            string[] parts = stringaLetta.Split('*');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) * numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                        {
                                            // Espressione con +
                                            string[] parts = stringaLetta.Split('+');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) + numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }

                                        else
                                        {
                                            string valore = nomeProprieta + " ➞ " + elementoString;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                    }
                                    else if (elementoPropertiesValue is int elementoInt)
                                    {
                                        string valore = nomeProprieta + " ➞ " + elementoInt.ToString();

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
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
                                else if (elementoPropertiesValue is IList<string> elementoStringList)
                                {
                                    // E' un array
                                    foreach (string elementoListValue in elementoStringList)
                                    {
                                        // Creo una checkbox
                                        CheckBox checkBox = GeneraCheckbox();

                                        myCheckboxList.Add(elementoListValue + ";" + nomeSezione, checkBox);

                                        // Crea una label
                                        Label label = new Label
                                        {
                                            Text = elementoListValue,
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

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(elementoListValue, nomeSezione);

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
                else if (nomeFile == "montagna.json")
                {
                    Montagna montagna = JsonConvert.DeserializeObject<Montagna>(data);

                    // Popolo i vestiti in base alla stagione
                    string nomeOggettoTitolo = "Vestiti" + stagione + "montagna";

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = montagna.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Faccio in modo da dare una stagione sola
                        if (stagione == "estate" && property.Name == "Vestitiinvernomontagna")
                        {
                            continue;
                        }
                        else if (stagione == "inverno" && property.Name == "Vestitiestatemontagna")
                        {
                            continue;
                        }
                        else
                        {
                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            string nomeSezione = property.Name;
                            if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeSezione = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Creo il titolo di questa sezione
                            Label labelTitolo = new Label
                            {
                                Text = nomeSezione,
                                TextColor = Color.FromHex("#BB86FC"),
                                FontSize = 30,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = new Thickness(0, 30, 0, 0) // Applica un margine di 20 top
                            };

                            // Aggiungo il titolo della sezione
                            stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                            stackLayoutBase.Children.Add(labelTitolo);

                            // Prendo i dati in base alla proprietà
                            object value = property.GetValue(montagna);

                            // Skippo se c'è un oggetto senza valori assegnati
                            if (value == null)
                            {
                                continue;
                            }

                            PropertyInfo[] elementoProperties = value.GetType().GetProperties();

                            foreach (PropertyInfo elementoProperty in elementoProperties)
                            {
                                // Popolo la sezione con le checkbox
                                object elementoPropertiesValue = elementoProperty.GetValue(value);

                                // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                                string nomeProprieta = elementoProperty.Name;
                                if (elementoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                                {
                                    nomeProprieta = elementoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                                }

                                // Controllo se è un array o un oggetto
                                if (elementoPropertiesValue is string || elementoPropertiesValue is int)
                                {
                                        // E' una stringa o un intero
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();

                                    myCheckboxList.Add(elementoPropertiesValue + ";" + nomeSezione, checkBox);

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

                                    if (elementoPropertiesValue is string elementoString)
                                    {
                                        // Controllo se è un valore o un'espressione
                                        string stringaLetta = elementoString;

                                        if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                        {
                                            // Espressione con /
                                            string[] parts = stringaLetta.Split('/');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) / numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                        {
                                            // Espressione con *
                                            string[] parts = stringaLetta.Split('*');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) * numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                        {
                                            // Espressione con +
                                            string[] parts = stringaLetta.Split('+');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) + numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }

                                        else
                                        {
                                            string valore = nomeProprieta + " ➞ " + elementoString;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                    }
                                    else if (elementoPropertiesValue is int elementoInt)
                                    {
                                        string valore = nomeProprieta + " ➞ " + elementoInt.ToString();

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
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
                                else if (elementoPropertiesValue is IList<string> elementoStringList)
                                {
                                    // E' un array
                                    foreach (string elementoListValue in elementoStringList)
                                    {
                                        // Creo una checkbox
                                        CheckBox checkBox = GeneraCheckbox();

                                        myCheckboxList.Add(elementoListValue + ";" + nomeSezione, checkBox);

                                        // Crea una label
                                        Label label = new Label
                                        {
                                            Text = elementoListValue,
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

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(elementoListValue, nomeSezione);

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
                else if (nomeFile == "visitaCitta.json")
                {
                    VisitaCitta visitaCitta = JsonConvert.DeserializeObject<VisitaCitta>(data);

                    // Popolo i vestiti in base alla stagione
                    string nomeOggettoTitolo = "Vestiti" + stagione + "montagna";

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = visitaCitta.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Facci in modo da dare una stagione sola
                        if (stagione == "estate" && property.Name == "Vestitiinvernocitta")
                        {
                            continue;
                        }
                        else if (stagione == "inverno" && property.Name == "Vestitiestatecitta")
                        {
                            continue;
                        }
                        else
                        {
                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            string nomeSezione = property.Name;
                            if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeSezione = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Creo il titolo di questa sezione
                            Label labelTitolo = new Label
                            {
                                Text = nomeSezione,
                                TextColor = Color.FromHex("#BB86FC"),
                                FontSize = 30,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = new Thickness(0, 30, 0, 0) // Applica un margine di 20 top
                            };

                            // Aggiungo il titolo della sezione
                            stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                            stackLayoutBase.Children.Add(labelTitolo);

                            // Prendo i dati in base alla proprietà
                            object value = property.GetValue(visitaCitta);

                            // Skippo se c'è un oggetto senza valori assegnati
                            if (value == null)
                            {
                                continue;
                            }

                            PropertyInfo[] elementoProperties = value.GetType().GetProperties();

                            foreach (PropertyInfo elementoProperty in elementoProperties)
                            {
                                // Popolo la sezione con le checkbox
                                object elementoPropertiesValue = elementoProperty.GetValue(value);

                                // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                                string nomeProprieta = elementoProperty.Name;
                                if (elementoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                                {
                                    nomeProprieta = elementoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                                }

                                // Controllo se è un array o un oggetto
                                if (elementoPropertiesValue is string || elementoPropertiesValue is int)
                                {
                                        // E' una stringa o un intero
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();

                                    myCheckboxList.Add(elementoPropertiesValue + ";" + nomeSezione, checkBox);

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

                                    if (elementoPropertiesValue is string elementoString)
                                    {
                                        // Controllo se è un valore o un'espressione
                                        string stringaLetta = elementoString;

                                        if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                        {
                                            // Espressione con /
                                            string[] parts = stringaLetta.Split('/');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) / numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                        {
                                            // Espressione con *
                                            string[] parts = stringaLetta.Split('*');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) * numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                        else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                        {
                                            // Espressione con +
                                            string[] parts = stringaLetta.Split('+');
                                            int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                            // Calcolo numero di output in base alla durata
                                            double calcolo = Convert.ToDouble(durata) + numero;
                                            int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                            string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }

                                        else
                                        {
                                            string valore = nomeProprieta + " ➞ " + elementoString;

                                            label.Text = valore;

                                            // Salvo nel dictionary l'elemento
                                            dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                        }
                                    }
                                    else if (elementoPropertiesValue is int elementoInt)
                                    {
                                        string valore = nomeProprieta + " ➞ " + elementoInt.ToString();

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
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
                                else if (elementoPropertiesValue is IList<string> elementoStringList)
                                {
                                    // E' un array
                                    foreach (string elementoListValue in elementoStringList)
                                    {
                                        // Creo una checkbox
                                        CheckBox checkBox = GeneraCheckbox();

                                        myCheckboxList.Add(elementoListValue + ";" + nomeSezione, checkBox);

                                        // Crea una label
                                        Label label = new Label
                                        {
                                            Text = elementoListValue,
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

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(elementoListValue, nomeSezione);

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
                else if (nomeFile == "escursioneInMontagna.json")
                {
                    Escursione escursioneInMontagna = JsonConvert.DeserializeObject<Escursione>(data);

                    // Salvo le proprietà lette
                    PropertyInfo[] properties = escursioneInMontagna.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                        string nomeSezione = property.Name;
                        if (property.GetCustomAttribute<JsonPropertyAttribute>() != null)
                        {
                            nomeSezione = property.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                        }

                        // Creo il titolo di questa sezione
                        Label labelTitolo = new Label
                        {
                            Text = nomeSezione,
                            TextColor = Color.FromHex("#BB86FC"),
                            FontSize = 30,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0, 30, 0, 0) // Applica un margine di 20 top
                        };

                        // Aggiungo il titolo della sezione
                        stackLayoutBase = this.FindByName<StackLayout>("stackLayoutListaOggetti");
                        stackLayoutBase.Children.Add(labelTitolo);

                        // Prendo i dati in base alla proprietà
                        object value = property.GetValue(escursioneInMontagna);

                        // Skippo se c'è un oggetto senza valori assegnati
                        if (value == null)
                        {
                            continue;
                        }

                        PropertyInfo[] elementoProperties = value.GetType().GetProperties();

                        foreach (PropertyInfo elementoProperty in elementoProperties)
                        {
                            // Popolo la sezione con le checkbox
                            object elementoPropertiesValue = elementoProperty.GetValue(value);

                            // Prendo il nome della json property solo se presente, perchè altrimenti è mal leggibile
                            string nomeProprieta = elementoProperty.Name;
                            if (elementoProperty.GetCustomAttribute<JsonPropertyAttribute>() != null)
                            {
                                nomeProprieta = elementoProperty.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                            }

                            // Controllo se è un array o un oggetto
                            if (elementoPropertiesValue is string || elementoPropertiesValue is int)
                            {
                                // E' una stringa o un intero
                                // Creo una checkbox
                                CheckBox checkBox = GeneraCheckbox();

                                myCheckboxList.Add(elementoPropertiesValue + ";" + nomeSezione, checkBox);

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

                                if (elementoPropertiesValue is string elementoString)
                                {
                                    // Controllo se è un valore o un'espressione
                                    string stringaLetta = elementoString;

                                    if (Regex.IsMatch(stringaLetta, @"\b\w+\s*/\s*\d+"))
                                    {
                                        // Espressione con /
                                        string[] parts = stringaLetta.Split('/');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del /

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) / numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }
                                    else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\*\s*\d+"))
                                    {
                                        // Espressione con *
                                        string[] parts = stringaLetta.Split('*');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del *

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) * numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }
                                    else if (Regex.IsMatch(stringaLetta, @"\b\w+\s*\+\s*\d+"))
                                    {
                                        // Espressione con +
                                        string[] parts = stringaLetta.Split('+');
                                        int numero = Convert.ToInt32(parts[1].Trim()); // Parte a destra del +

                                        // Calcolo numero di output in base alla durata
                                        double calcolo = Convert.ToDouble(durata) + numero;
                                        int numeroOggetti = Convert.ToInt32(Math.Ceiling(calcolo));

                                        string valore = nomeProprieta + " ➞ " + numeroOggetti;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }

                                    else
                                    {
                                        string valore = nomeProprieta + " ➞ " + elementoString;

                                        label.Text = valore;

                                        // Salvo nel dictionary l'elemento
                                        dictionaryCheckboxTrue.Add(valore, nomeSezione);
                                    }
                                }
                                else if (elementoPropertiesValue is int elementoInt)
                                {
                                    string valore = nomeProprieta + " ➞ " + elementoInt.ToString();

                                    label.Text = valore;

                                    // Salvo nel dictionary l'elemento
                                    dictionaryCheckboxTrue.Add(valore, nomeSezione);
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
                            else if (elementoPropertiesValue is IList<string> elementoStringList)
                            {
                                // E' un array
                                foreach (string elementoListValue in elementoStringList)
                                {
                                    // Creo una checkbox
                                    CheckBox checkBox = GeneraCheckbox();
                                    myCheckboxList.Add(elementoListValue + ";" + nomeSezione, checkBox);

                                    // Crea una label
                                    Label label = new Label
                                    {
                                        Text = elementoListValue,
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

                                    // Salvo nel dictionary l'elemento
                                    dictionaryCheckboxTrue.Add(elementoListValue, nomeSezione);

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

        private void SaveClicked(object sender, EventArgs e)
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

            Navigation.PushAsync(new Preview(stagione, nomeVacanza, durata, categoria, categoriaMaiuscola, checkboxTrue));
        }
        
        private void checkAllClicked(object sender, EventArgs e)
        {
            if(checkAllButton.Text == "Seleziona tutto")
            {
                // Attivo tutte le checkbox
                foreach (CheckBox cb in myCheckboxList.Values)
                {
                    cb.IsChecked = true;
                }

                // Lo trasformo in deseleziona tutto
                checkAllButton.Text = "Deseleziona tutto";
            }
            else
            {
                // Disattivo tutte le checkbox
                foreach (CheckBox cb in myCheckboxList.Values)
                {
                    cb.IsChecked = false;
                }

                // Lo trasformo in deseleziona tutto
                checkAllButton.Text = "Seleziona tutto";
            }
        }
        
        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}