namespace Pack__n__Go
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void HotImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Categoria("estate"));
        }

        private void ColdImageClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Categoria("inverno"));
        }
    }
}