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
            Navigation.PushAsync(new Mare(stagione));
        }
        
        private void MontagnaImageClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void LagoImageClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void VisitaImageClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void backClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}