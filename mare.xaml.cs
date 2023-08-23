using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using System.Reflection;

namespace Pack__n__Go
{
    public partial class Mare : ContentPage
    {
        string stagione;

        public Mare(string stagione)
        {
            InitializeComponent();

            this.stagione = stagione;

            GeneraCheckBox();
        }

        private void GeneraCheckBox()
        {
            CheckBox checkBox;

            // Ottieni l'assembly corrente
            var assembly = Assembly.GetExecutingAssembly();

            // Ottieni il nome completo della risorsa (deve includere il namespace)
            var resourceName = "Pack__n__Go.Resources.JSON.items.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string jsonContent = reader.ReadToEnd();

                        Root jsonDeserialized = JsonConvert.DeserializeObject<Root>(jsonContent);

                        // Ora puoi utilizzare l'oggetto 'jsonDeserialized' come necessario
                    }
                }
            }

            




            // Creo le checkboox per ogni categoria
            /*for (int i = 0; i < numeroDiCategorie; i++)
            {
                checkBox = new CheckBox { IsChecked = false };
            }*/
        }

        private void backClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}