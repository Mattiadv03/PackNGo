namespace Pack__n__Go
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = Convert.ToInt32(args.NewValue);
            displayLabel.Text = String.Format("The Slider value is {0}", value);
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