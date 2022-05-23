using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPoleEmploi
{
    internal class JobOffer
    {
        public string? id { get; set; }
        public string? intitule { get; set; }
        public string? description { get; set; }
        public Entreprise? entreprise { get; set; }
        public string? typeContrat { get; set; }
        public Salaire? salaire { get; set; }
        public string? dureeTravailLibelle { get; set; }
        public string? dureeTravailLibelleConverti { get; set; }
        public Contact? contact { get; set; }
        public OrigineOffre? origineOffre { get; set; }

    }
}
