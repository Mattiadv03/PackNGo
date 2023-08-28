namespace Pack__n__Go
{
    public partial class Categoria : ContentPage
    {
        string stagione;
        int durata;

        public Categoria(string stagione, int durata)
        {
            InitializeComponent();

            this.stagione = stagione;
            this.durata = durata;
        }

        private void MareImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, durata, "mare"));
        }
        
        private void MontagnaImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, durata, "montagna"));
        }
        
        private void LagoImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, durata, "lago"));
        }
        
        private void VisitaImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Optionals(stagione, durata, "visitaCitta"));
        }

        private void BackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}