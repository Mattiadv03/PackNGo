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

            
            //SizeChanged += OnPageSizeChanged;
        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            SetSliderWidth();
        }

        private void SetSliderWidth()
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            double widthSchermo = displayInfo.Width;
            double widthSlider = widthSchermo * 0.2; // Impostiamo la larghezza al 80% dello schermo
            slider.WidthRequest = widthSlider;
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            // Prendo il valore dello slider
            double value = Convert.ToInt32(args.NewValue);

            // Lo mostro nel layout
            durataLabel.Text = String.Format("La vacanza durerà {0} {1}", value, "giorni");

            // Dato che l'utente ha scelto una durata attivo gli imagebutton
            hotImage.IsEnabled = true;
            coldImage.IsEnabled = true;
        }

        private void HotImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Categoria("estate", Convert.ToInt32(durataLabel.Text)));
        }

        private void ColdImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Categoria("inverno", Convert.ToInt32(durataLabel.Text)));
        }
    }
}