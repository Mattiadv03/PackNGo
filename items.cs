using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Acne
    {
        [JsonProperty("Crema serale")]
        public int CremaSerale { get; set; }

        [JsonProperty("Crema mattutina")]
        public int CremaMattutina { get; set; }

        [JsonProperty("Compresse pranzo")]
        public string CompressePranzo { get; set; }
    }

    public class Beauty
    {
        public List<string> Denti { get; set; }
        public List<string> Doccia { get; set; }
        public Acne Acne { get; set; }

        [JsonProperty("Bellezza personale")]
        public BellezzaPersonale BellezzaPersonale { get; set; }
        public Medicinali Medicinali { get; set; }

        [JsonProperty("Igienizzante spray")]
        public int IgienizzanteSpray { get; set; }

        [JsonProperty("Borsetta roba onta")]
        public int BorsettaRobaOnta { get; set; }
    }

    public class BellezzaPersonale
    {
        [JsonProperty("Cotton Fioc")]
        public string CottonFioc { get; set; }
        public int Profumo { get; set; }

        [JsonProperty("Lametta barba")]
        public int LamettaBarba { get; set; }
        public string Cera { get; set; }
    }

    public class Bicicletta
    {
        [JsonProperty("Maglia tecnica")]
        public string MagliaTecnica { get; set; }

        [JsonProperty("Pantaloni da bici")]
        public string PantaloniDaBici { get; set; }

        [JsonProperty("Calze tecniche")]
        public string CalzeTecniche { get; set; }

        [JsonProperty("Scarpe da ginnastica")]
        public int ScarpeDaGinnastica { get; set; }
    }

    public class Elettronica
    {
        public List<string> Smartphone { get; set; }
        public List<string> Orologio { get; set; }
        public List<string> Powerbank { get; set; }
        public int Auricolari { get; set; }
        public int Multipla { get; set; }
    }

    public class Medicinali
    {
        public int Tachipirina { get; set; }

        [JsonProperty("Pasta fissan")]
        public int PastaFissan { get; set; }

        [JsonProperty("Cerotti normali")]
        public int CerottiNormali { get; set; }

        [JsonProperty("Cerotti Vesciche")]
        public int CerottiVesciche { get; set; }
    }

    public class Piscina
    {
        public string Costume { get; set; }
        public int Cuffia { get; set; }

        [JsonProperty("Telo mare")]
        public int TeloMare { get; set; }

        [JsonProperty("Borsetta roba onta")]
        public int BorsettaRobaOnta { get; set; }

        [JsonProperty("Crema solare faccia")]
        public int CremaSolareFaccia { get; set; }

        [JsonProperty("Crema solare corpo")]
        public int CremaSolareCorpo { get; set; }
    }

    public class Svago
    {
        public List<string> Carte { get; set; }
        public int Libro { get; set; }
    }

    public class Utility
    {
        [JsonProperty("Borraccia da 1.5 lt")]
        public int BorracciaDa15lt { get; set; }

        [JsonProperty("Borraccia da 0.5 lt")]
        public int BorracciaDa05lt { get; set; }

        [JsonProperty("Mascherina ffp2")]
        public int MascherinaFfp2 { get; set; }

        [JsonProperty("Occhiali da sole")]
        public int OcchialiDaSole { get; set; }
        public int Berretto { get; set; }

        [JsonProperty("Tappi per le orecchie")]
        public int TappiPerLeOrecchie { get; set; }
        public string Fazzoletti { get; set; }
    }

    public class VestitiEstate
    {
        public string Magliette { get; set; }

        [JsonProperty("Pantaloni corti")]
        public string Pantalonicorti { get; set; }

        [JsonProperty("Pantaloni lunghi")]
        public string Pantalonilunghi { get; set; }
        public int Cintura { get; set; }
        public int Felpe { get; set; }
        public string Mutande { get; set; }
        public string Calze { get; set; }
        public List<string> Pigiama { get; set; }

        [JsonProperty("Borsetta roba onta")]
        public int Borsettarobaonta { get; set; }
    }

    public class VestitiInverno
    {
        public string Magliette { get; set; }

        [JsonProperty("Pantaloni lunghi")]
        public string Pantalonilunghi { get; set; }
        public int Cintura { get; set; }
        public string Felpe { get; set; }
        public int Kway { get; set; }
        public string Mutande { get; set; }
        public string Calze { get; set; }
        public List<string> Pigiama { get; set; }

        [JsonProperty("Borsetta roba onta")]
        public int Borsettarobaonta { get; set; }
    }

    public class Root
    {
        public Beauty Beauty { get; set; }
        public Bicicletta Bicicletta { get; set; }
        public List<string> Campeggio { get; set; }
        public Elettronica Elettronica { get; set; }
        public Piscina Piscina { get; set; }
        public Svago Svago { get; set; }
        public Utility Utility { get; set; }

        [JsonProperty("Vestiti Estate")]
        public VestitiEstate VestitiEstate { get; set; }

        [JsonProperty("Vestiti Inverno")]
        public VestitiInverno VestitiInverno { get; set; }
    }
}
