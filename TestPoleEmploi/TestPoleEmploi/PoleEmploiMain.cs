namespace TestPoleEmploi
{
    class PoleEmploiMain
    {
        static void Main(string[] args)
        {
            // Get job offers
            ApiRequest request = new ApiRequest();
            List<JobOffer> jobOffers = request.ContactApi();

            // Store in DB
            ApiData data = new ApiData();
            data.CreateTable();
            data.FillTable(jobOffers);

            // Retrieve from DB
            data.GetOffersFromDb();

            // Generate stats
            ApiStats stats = new ApiStats();
            stats.ContractTypeStats(jobOffers);
        }

    }
}