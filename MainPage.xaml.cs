namespace Pack__n__Go
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MareImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Mare());
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
    }
}