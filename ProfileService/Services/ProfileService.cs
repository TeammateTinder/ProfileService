using MongoDB.Driver;
using ProfileServiceApp.Models;

namespace ProfileServiceApp.Services
{
    public class ProfileService
    {
        private readonly IMongoCollection<ProfileModel> _profileCollection;

        public ProfileService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("TeammateTinder");
            _profileCollection = database.GetCollection<ProfileModel>("Profiles");
        }

        public void SaveProfile(ProfileModel profile)
        {
            _profileCollection.InsertOne(profile);
        }
    }
}
