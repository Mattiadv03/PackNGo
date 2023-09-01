namespace Pack__n__Go
{
    public partial class Categoria : ContentPage
    {
        string stagione;
        string nomeVacanza;
        int durata;

        public Categoria(string stagione, string nomeVacanza, int durata)
        {
            InitializeComponent();

            this.stagione = stagione;
            this.nomeVacanza = nomeVacanza;
            this.durata = durata;

            SizeChanged += OnPageSizeChanged;
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

        private void MareImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, nomeVacanza, durata, "mare"));
        }
        
        private void MontagnaImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, nomeVacanza, durata, "montagna"));
        }
        
        private void LagoImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, nomeVacanza, durata, "lago"));
        }
        
        private void VisitaImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, nomeVacanza, durata, "visitaCitta"));
        }
        
        private void EscursioneImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Valigia(stagione, nomeVacanza, durata, "escursione", "Escursione in montagna", "escursioneInMontagna.json", null));
        }

        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}