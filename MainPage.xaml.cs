namespace Pack__n__Go
{
    public partial class MainPage : ContentPage
    {
        int durata;

        public MainPage()
        {
            InitializeComponent();

            // Dato che l'utente NON ha ancora scelto una durata disattivo gli imagebutton
            hotImage.IsEnabled = false;
            coldImage.IsEnabled = false;

            
            SizeChanged += OnPageSizeChanged;
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            SetEntryWidth();
            SetSliderWidth();
        }

        private void SetEntryWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthEntry = widthSchermo * 0.4;
            nomeVacanzaEntry.WidthRequest = widthEntry;
        }
        
        private void SetSliderWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthSlider = widthSchermo * 0.2;
            slider.WidthRequest = widthSlider;
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            // Prendo il valore dello slider
            int value = Convert.ToInt32(args.NewValue);

            // Lo mostro nel layout
            durataLabel.Text = String.Format("La vacanza durerà {0} {1}", value, "giorni");

            durata = value;

            // Dato che l'utente ha scelto una durata attivo gli imagebutton
            hotImage.IsEnabled = true;
            coldImage.IsEnabled = true;
        }

        private void HotImageClicked(object sender, EventArgs e)
        {
            string nomeVacanza = nomeVacanzaEntry.Text;

            if (nomeVacanza == null || nomeVacanza == "")
            {
                nomeVacanza = "Vacanza Test " + DateTime.Now.ToString();
            }
            Navigation.PushAsync(new Categoria("estate", nomeVacanza, durata));
        }

        private void ColdImageClicked(object sender, EventArgs e)
        {
            string nomeVacanza = nomeVacanzaEntry.Text;

            if (nomeVacanza == null || nomeVacanza == "")
            {
                nomeVacanza = "Vacanza Test " + DateTime.Now.ToString();
            }
            Navigation.PushAsync(new Categoria("inverno", nomeVacanza, durata));
        }
    }
}