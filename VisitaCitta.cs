using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class VestitiEstateCitta
    {
        [JsonProperty("Magliette normali")]
        public string Magliettenormali { get; set; }

        [JsonProperty("Pantaloni corti normali")]
        public string Pantalonicortinormali { get; set; }
        public string Felpe { get; set; }

        [JsonProperty("Pantaloni lunghi")]
        public string Pantalonilunghi { get; set; }
    }

    public class VestitiInvernoCitta
    {
        [JsonProperty("Magliette normali maniche lunghe")]
        public string Magliettenormalimanichelunghe { get; set; }

        [JsonProperty("Magliette termiche maniche lunghe")]
        public string Magliettetermichemanichelunghe { get; set; }

        [JsonProperty("Canutiere normali")]
        public string Canutierenormali { get; set; }

        [JsonProperty("Pantaloni corti normali")]
        public int Pantalonicortinormali { get; set; }
        public string Felpe { get; set; }

        [JsonProperty("Pantaloni lunghi")]
        public string Pantalonilunghi { get; set; }
        public int Berretto { get; set; }
        public int Sciarpa { get; set; }
        public int Scaldacollo { get; set; }
    }
    public class VisitaCitta
    {
        [JsonProperty("Vestiti estate citta")]
        public VestitiEstateCitta Vestitiestatecitta { get; set; }

        [JsonProperty("Vestiti inverno citta")]
        public VestitiInvernoCitta Vestitiinvernocitta { get; set; }
    }
}
