namespace ProfileServiceApp.Models
{
    // Add Brawlhalla ID?
    // Array with swiped ids
    // consider replacing ID value with Brawlhalla ID? will result in mega epic search possibilities discuss with marcel
    public class ProfileModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int PowerRanking { get; set; }
    }
}
