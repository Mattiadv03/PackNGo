namespace Pack__n__Go
{
    public partial class Preview : ContentPage
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
        StackLayout stackLayoutBase;


        public Preview(string stagione, string nomeVacanza, int durata, string categoria, string categoriaMaiuscola, List<string> checkboxTrue)
        {
            InitializeComponent();

            this.stagione = stagione;
            this.nomeVacanza = nomeVacanza;
            this.durata = durata;
            this.categoria = categoria;
            this.categoriaMaiuscola = categoriaMaiuscola;
            this.checkboxTrue = checkboxTrue;

            SizeChanged += OnPageSizeChanged;

            ControllaInternet(categoriaMaiuscola);
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            SetBackWidth();
        }

        private void SetBackWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthEntry = widthSchermo * 0.2;
            backButton.WidthRequest = widthEntry;
        }

        private async Task ControllaInternet(string categoriaMaiuscola)
        {
            // Controllo se è connesso a internet
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                GeneraXaml(categoriaMaiuscola);

                // IN VALIIGIIA  NON VVENGONO FATTI TUTTI I SUCCESSIVI A DEFAULT
            }
            else
            {
                await DisplayAlert("Errore", "Connettiti a internet per utilizzare l'applicazione", "OK");
            }
        }

        private void GeneraXaml(string categoriaMaiuscola)
        {
            // Genero la label con il titolo della categoria scelta
            Label categoriaScelta = new Label
            {
                Text = "Valigia " + categoriaMaiuscola + " - " + nomeVacanza,
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center
            };
            StackLayout stackLayoutCategoria = this.FindByName<StackLayout>("dynamicCategoria");
            stackLayoutCategoria.Children.Add(categoriaScelta);

            // Genero tutte le checkbox ma non cliccabili
            foreach (string element in checkboxTrue)
            {
                if (true)
                {
                    // Creo il titolo di questa sezione
                    Label labelTitolo = new Label
                    {
                        Text = element,
                        TextColor = Color.FromHex("#BB86FC"),
                        FontSize = 30,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(0, 30, 0, 0) // Applica un margine top di 30
                    };
                }
                else
                {

                }

                // Genero una checkbox
                CheckBox checkBox = GeneraCheckbox();

                // Genero la label
                Label label = new Label
                {
                    Text = element,
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

        private CheckBox GeneraCheckbox()
        {
            return new CheckBox
            {
                IsChecked = true,
                IsEnabled = false,
                Color = Color.FromHex("#BB86FC"),
                VerticalOptions = LayoutOptions.Center,
                //InputTransparent = true // Diventa non più cliccabile
            };
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new Salva(stagione, nomeVacanza, durata, categoria, categoriaMaiuscola, checkboxTrue));
        }
        
        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}