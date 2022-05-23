using System.Data.SQLite;

namespace TestPoleEmploi
{
    internal class ApiData
    {

        private const string cs = @"URI=file:.\PoleEmploi.db";

        public void CreateTable()
        {
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS job_offers(id TEXT PRIMARY KEY,
                                intitule TEXT, description TEXT, entreprise TEXT, type_contrat TEXT,
                                url TEXT)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("\njob_offers table created\n");

            con.Close();
        }

        public void FillTable(List<JobOffer> jobOffers)
        {
            using var con = new SQLiteConnection(cs);
            con.Open();
            using var cmd = new SQLiteCommand(con);

            foreach (JobOffer jobOffer in jobOffers)
            {
                cmd.CommandText = @"INSERT OR IGNORE INTO job_offers(id, intitule, description, entreprise, type_contrat, url) 
                                    VALUES ($id, $intitule, $description, $entreprise, $typeContrat, $url)";
                cmd.Parameters.AddWithValue("$id", jobOffer.id);
                cmd.Parameters.AddWithValue("$intitule", jobOffer.intitule);
                cmd.Parameters.AddWithValue("$description", jobOffer.description);
                cmd.Parameters.AddWithValue("$entreprise", jobOffer.entreprise?.nom);
                cmd.Parameters.AddWithValue("$typeContrat", jobOffer.typeContrat);
                cmd.Parameters.AddWithValue("$url", jobOffer.origineOffre?.urlOrigine);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("\njob_offers table filled\n");

            con.Close();
        }

        public void GetOffersFromDb()
        {
            using var con = new SQLiteConnection(cs);
            con.Open();

            string stm = "SELECT * FROM job_offers LIMIT 5";

            using var cmd = new SQLiteCommand(stm, con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            Console.WriteLine("\nFirst five offers in job_offers\n");

            while (rdr.Read())
            {
                Console.WriteLine($"Intitulé : {rdr.GetString(1)}, entreprise : {rdr.GetString(3)}, type de contrat : {rdr.GetString(4)}, url : {rdr.GetString(5)}");
            }

            con.Close();
        }

    }
}
