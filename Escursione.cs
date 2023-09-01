using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Vestiti
    {
        [JsonProperty("Magliette tecniche")]
        public int Magliettetecniche { get; set; }

        [JsonProperty("Pantaloni tecnici")]
        public int Pantalonitecnici { get; set; }

        [JsonProperty("Cintura pantaloni tecnici")]
        public int Cinturapantalonitecnici { get; set; }

        [JsonProperty("Canutiere tecniche")]
        public int Canutieretecniche { get; set; }

        [JsonProperty("Calze tecniche altezza media")]
        public int Calzetecnichealtezzamedia { get; set; }
    }

    public class Escursione
    {
        public List<string> Scarponi { get; set; }
        public Vestiti Vestiti { get; set; }

        [JsonProperty("Freddo/Pioggia")]
        public List<string> FreddoPioggia { get; set; }
        public List<string> Utility { get; set; }

        [JsonProperty("Crema solare")]
        public List<string> Cremasolare { get; set; }
        public List<string> Acqua { get; set; }
        public string Snack { get; set; }
        public int Pila { get; set; }
        public List<string> Powerbank { get; set; }

        [JsonProperty("Coltello per i panini")]
        public int Coltelloperipanini { get; set; }
        public List<string> Necessary { get; set; }
    }
}
