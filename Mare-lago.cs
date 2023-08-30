using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class VestitiEstateMareLago
    {
        [JsonProperty("Magliette maniche lunghe")]
        public string Magliettemanichelunghe { get; set; }

        [JsonProperty("Pantaloni corti")]
        public string Pantalonicorti { get; set; }

        [JsonProperty("Pantaloni lunghi")]
        public int Pantalonilunghi { get; set; }
        public int Felpe { get; set; }
    }

    public class VestitiInvernoMareLago
    {
        [JsonProperty("Magliette maniche lunghe")]
        public string Magliettemanichelunghe { get; set; }

        [JsonProperty("Pantaloni lunghi")]
        public string Pantalonilunghi { get; set; }
        public string Felpe { get; set; }
        public int Berretto { get; set; }
        public int Sciarpa { get; set; }
        public int Scaldacollo { get; set; }
    }

    public class Mare_lago
    {
        [JsonProperty("Vestiti estate mare-lago")]
        public VestitiEstateMareLago Vestitiestatemarelago { get; set; }

        [JsonProperty("Vestiti inverno mare-lago")]
        public VestitiInvernoMareLago Vestitiinvernomarelago { get; set; }
    }
}
