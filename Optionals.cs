using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Bicicletta
    {
        [JsonProperty("Maglia tecnica")]
        public string Magliatecnica { get; set; }

        [JsonProperty("Pantaloni da bici")]
        public string Pantalonidabici { get; set; }

        [JsonProperty("Calze tecniche")]
        public string Calzetecniche { get; set; }

        [JsonProperty("Scarpe da ginnastica")]
        public int Scarpedaginnastica { get; set; }
    }

    public class CamminareInMontagnaEstate
    {
        [JsonProperty("Magliette tecniche")]
        public string Magliettetecniche { get; set; }

        [JsonProperty("Canutiere tecniche")]
        public string Canutieretecniche { get; set; }

        [JsonProperty("Pantaloni tecnici")]
        public string Pantalonitecnici { get; set; }
    }

    public class Campeggio
    {
        [JsonProperty("Letto campeggio")]
        public List<string> Lettocampeggio { get; set; }
    }

    public class CuraPersonale
    {
        [JsonProperty("Crema serale")]
        public int Cremaserale { get; set; }

        [JsonProperty("Crema mattutina")]
        public int Cremamattutina { get; set; }

        [JsonProperty("Compresse pranzo")]
        public string Compressepranzo { get; set; }
    }

    public class Lavoro
    {
        public List<string> Computer { get; set; }
    }

    public class Neve
    {
        [JsonProperty("Giacca da neve")]
        public int Giaccadaneve { get; set; }

        [JsonProperty("Pantaloni da neve")]
        public int Pantalonidaneve { get; set; }

        [JsonProperty("Calze grosse da neve")]
        public string Calzegrossedaneve { get; set; }

        [JsonProperty("Scarponcini/Doposcii")]
        public int ScarponciniDoposcii { get; set; }

        [JsonProperty("Berretto da neve")]
        public int Berrettodaneve { get; set; }

        [JsonProperty("Calza maglia")]
        public int Calzamaglia { get; set; }
    }

    public class Palestra
    {
        [JsonProperty("Scarpe da ginnastica")]
        public int Scarpedaginnastica { get; set; }

        [JsonProperty("Asciugamano in microfibra")]
        public int Asciugamanoinmicrofibra { get; set; }

        [JsonProperty("Maglia tecnica")]
        public string Magliatecnica { get; set; }

        [JsonProperty("Pantaloni da ginnastica")]
        public string Pantalonidaginnastica { get; set; }
    }

    public class Pioggia
    {
        public int Kway { get; set; }
        public int Ombrello { get; set; }
    }

    public class PiscinaMare
    {
        public string Costume { get; set; }
        public int Cuffia { get; set; }

        [JsonProperty("Telo mare")]
        public int Telomare { get; set; }

        [JsonProperty("Borsetta roba onta")]
        public int Borsettarobaonta { get; set; }

        [JsonProperty("Crema solare faccia")]
        public int Cremasolarefaccia { get; set; }

        [JsonProperty("Crema solare corpo")]
        public int Cremasolarecorpo { get; set; }
    }

    public class Svago
    {
        public List<string> Carte { get; set; }
        public int Libro { get; set; }
    }

    public class Opzionali
    {
        [JsonProperty("Cura personale")]
        public CuraPersonale Curapersonale { get; set; }
        public Lavoro Lavoro { get; set; }

        [JsonProperty("Piscina/Mare")]
        public PiscinaMare PiscinaMare { get; set; }

        [JsonProperty("Camminare in montagna estate")]
        public CamminareInMontagnaEstate Camminareinmontagnaestate { get; set; }
        public Bicicletta Bicicletta { get; set; }
        public Palestra Palestra { get; set; }
        public Neve Neve { get; set; }
        public Svago Svago { get; set; }
        public Campeggio Campeggio { get; set; }
        public Pioggia Pioggia { get; set; }
    }
}
