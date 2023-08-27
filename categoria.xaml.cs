namespace Pack__n__Go
{
    public partial class Categoria : ContentPage
    {
        string stagione;

        public Categoria(string stagione)
        {
            InitializeComponent();

            this.stagione = stagione;
        }

        private void MareImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, "mare"));
        }
        
        private void MontagnaImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, "montagna"));
        }
        
        private void LagoImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, "lago"));
        }
        
        private void VisitaImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, "visitaCitta"));
        }

        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}