namespace Pack__n__Go
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Dato che l'utente NON ha ancora scelto una durata disattivo gli imagebutton
            hotImage.IsEnabled = false;
            coldImage.IsEnabled = false;
        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            // Prendo il valore dello slider
            double value = Convert.ToInt32(args.NewValue);

            // Lo mostro nel layout
            displayLabel.Text = String.Format("La vacanza durerà {0} {1}", value, "giorni");

            // Dato che l'utente ha scelto una durata attivo gli imagebutton
            hotImage.IsEnabled = true;
            coldImage.IsEnabled = true;
        }

        private void HotImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Categoria("estate", Convert.ToInt32(displayLabel.Text)));
        }

        private void ColdImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Categoria("inverno", Convert.ToInt32(displayLabel.Text)));
        }
    }
}