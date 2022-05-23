using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPoleEmploi
{
    internal class ApiStats
    {
        public void ContractTypeStats(List<JobOffer> jobOffers)
        {
            int cdiCount = jobOffers.Where(x => x.typeContrat == "CDI")
                        .Count();
            int cddCount = jobOffers.Where(x => x.typeContrat == "CDD")
                        .Count();

            Console.WriteLine(cdiCount);

            Console.WriteLine("\nStats\n");

            Console.WriteLine("CDI : " + (int)((float)cdiCount / (float)jobOffers.Count() * 100.0) + "%");
            Console.WriteLine("CDD : " + (int)((float)cddCount / (float)jobOffers.Count() * 100.0) + "%\n");
        }
    }
}
