using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack__n__Go
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Beauty
    {
        public List<string> Denti { get; set; }
        public List<string> Doccia { get; set; }

        [JsonProperty("Cotton Fioc")]
        public string CottonFioc { get; set; }
        public int Profumo { get; set; }

        [JsonProperty("Lametta barba")]
        public int Lamettabarba { get; set; }
        public string Cera { get; set; }
        public int Tachipirina { get; set; }

        [JsonProperty("Pasta fissan")]
        public int Pastafissan { get; set; }

        [JsonProperty("Cerotti normali")]
        public int Cerottinormali { get; set; }

        [JsonProperty("Cerotti Vesciche")]
        public int CerottiVesciche { get; set; }

        [JsonProperty("Igienizzante spray")]
        public int Igienizzantespray { get; set; }

        [JsonProperty("Borsetta roba onta jolly")]
        public int Borsettarobaontajolly { get; set; }
    }

    public class Elettronica
    {
        public List<string> Smartphone { get; set; }
        public List<string> Orologio { get; set; }
        public List<string> Powerbank { get; set; }
        public int Auricolari { get; set; }
        public int Multipla { get; set; }
    }

    public class Intimo
    {
        public string Mutande { get; set; }
        public string Calze { get; set; }
        public List<string> Pigiama { get; set; }

        [JsonProperty("Borsetta roba onta intimo")]
        public int Borsettarobaontaintimo { get; set; }
    }

    public class Utility
    {
        [JsonProperty("Borraccia da 1.5 lt")]
        public int Borracciada15lt { get; set; }

        [JsonProperty("Borraccia da 0.5 lt")]
        public int Borracciada05lt { get; set; }

        [JsonProperty("Mascherina ffp2")]
        public int Mascherinaffp2 { get; set; }

        [JsonProperty("Occhiali da sole")]
        public int Occhialidasole { get; set; }
        public int Cintura { get; set; }

        [JsonProperty("Tappi per le orecchie")]
        public int Tappiperleorecchie { get; set; }
        public string Fazzoletti { get; set; }
    }

    public class Default
    {
        public Beauty Beauty { get; set; }
        public Elettronica Elettronica { get; set; }
        public Utility Utility { get; set; }
        public Intimo Intimo { get; set; }
    }
}
