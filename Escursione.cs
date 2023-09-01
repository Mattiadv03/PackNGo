using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Attrezzatura
    {
        public List<string> Scarpe { get; set; }
        public Vestiti Vestiti { get; set; }

        [JsonProperty("Freddo/Pioggia")]
        public List<string> FreddoPioggia { get; set; }
        public List<string> Utility { get; set; }

        [JsonProperty("Creme solari")]
        public List<string> Cremesolari { get; set; }
        public List<string> Acque { get; set; }
        public string Snack { get; set; }
        public List<string> Powerbank { get; set; }

        [JsonProperty("Coltello per i panini")]
        public int Coltelloperipanini { get; set; }
        public List<string> Necessary { get; set; }
        public List<string> Facoltativi { get; set; }
    }

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
        public Attrezzatura Attrezzatura { get; set; }
    }
}
