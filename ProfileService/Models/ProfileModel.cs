namespace ProfileServiceApp.Models
{
    // Add Brawlhalla ID?
    // Array with swiped ids
    // consider replacing ID value with Brawlhalla ID? will result in mega epic search possibilities discuss with marcel
    public class ProfileModel
    {
        public ProfileModel(int id, string username, int powerRanking, List<int> swipedYes, List<int> swipedNo)
        {
            Id = id;
            Username = username;
            PowerRanking = powerRanking;
            SwipedYes = swipedYes;
            SwipedNo = swipedNo;
        }
        public ProfileModel()
        {
            SwipedYes = new List<int>();
            SwipedNo = new List<int>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public int PowerRanking { get; set; }
        public List<int> SwipedYes { get; set; }
        public List<int> SwipedNo { get; set; }
    }
}
