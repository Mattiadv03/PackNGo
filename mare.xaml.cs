using Microsoft.Maui.Storage;

namespace Pack__n__Go
{
    public partial class Mare : ContentPage
    {
        public Mare()
        {
            InitializeComponent();

            GeneraCheckBox();
        }

        private void GeneraCheckBox()
        {
            CheckBox checkBox;

            // Leggo il numero di categorie
            //string directory = Path.GetDirectoryName("Resources/JSON/beauty.json");
            //int numeroDiCategorie = Directory.GetFiles("Resources/JSON/", "*.json").Length;

            int numeroDiCategorie = 0;

            string directory = Path.Combine(AppDomain.CurrentDomain.FriendlyName, "Resources", "JSON");

            if (Directory.Exists(directory))
            {
                string[] jsonFileNames = Directory.GetFiles(directory, "*.json").Select(Path.GetFileName).ToArray();

                foreach (string fileName in jsonFileNames)
                {
                    numeroDiCategorie++;
                }
            }


            // Creo le checkboox per ogni categoria
            for (int i = 0; i < numeroDiCategorie; i++)
            {
                checkBox = new CheckBox { IsChecked = false };
            }
        }

        private void backClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}