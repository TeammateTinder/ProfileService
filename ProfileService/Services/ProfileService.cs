using MongoDB.Driver;
using ProfileServiceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MongoDB.Bson; // Import the Cors namespace

namespace ProfileServiceApp.Services
{
    public class ProfileService
    {
        private const string DATABASE_NAME = "TeammateTinder";
        private readonly IMongoCollection<ProfileModel> _profileCollection;

        public ProfileService(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(DATABASE_NAME);
            _profileCollection = GetProfilesFromDatabase(database);
        }
        public ProfileModel ReadProfileById(int id)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, id);
            var profile = _profileCollection.Find(filter).FirstOrDefault();
            return profile;
        }

        public void SaveProfile(ProfileModel profile)
        {
            _profileCollection.InsertOne(profile);
        }
        
        public ProfileModel DeleteProfile(int userId)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, userId);
            ProfileModel deletedProfile = _profileCollection.FindOneAndDelete(filter);
            return deletedProfile;
        }

        public UpdateResult AddIDToSwipedNo(int swiperID, int id2)
        {
            // Create a filter to find the user by ID
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, swiperID);

            // Create an update definition to push the number to the SwipedNo array
            var update = Builders<ProfileModel>.Update.Push(p => p.SwipedNo, id2);

            // Update the document in the collection
            var result = _profileCollection.UpdateOne(filter, update);

            return result;
        }

        public UpdateResult AddIDToSwipedYes(int swiperID, int id2)
        {
            // Create a filter to find the user by ID
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, swiperID);

            // Create an update definition to push the number to the SwipedYes array
            var update = Builders<ProfileModel>.Update.Push(p => p.SwipedYes, id2);

            // Update the document in the collection
            var result = _profileCollection.UpdateOne(filter, update);

            return result;
        }

        public IMongoCollection<ProfileModel> GetProfilesFromDatabase(IMongoDatabase database)
        {
            Console.WriteLine("Loading Profiles...");
            return database.GetCollection<ProfileModel>("Profiles");
        }
    }
}
