using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Xunit.Sdk;
using ProfileServiceApp.Services;
using ProfileServiceApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestProject1
{
    public class Test_ProfileService
    {
        readonly IMongoCollection<ProfileModel> _profileCollection;
        readonly IMongoClient _mongoClient;
        readonly IMongoDatabase _database;
        readonly ProfileService _profileService;
        const string DATABASE_NAME = "TeammateTinder";

        public Test_ProfileService()
        {
            // load connection string from somewheere else
            var connectionString = "mongodb+srv://user7364605:8kgoQKCx8EwKx32I@teammatetinder.yp8q8nb.mongodb.net/?retryWrites=true&w=majority&appName=TeammateTinder";
            _mongoClient = new MongoClient(connectionString);
            _database = _mongoClient.GetDatabase(DATABASE_NAME);
            _profileService = new ProfileService(_mongoClient);
            _profileCollection = _profileService.GetProfilesFromDatabase(_database);
        }

        [Fact]
        public void Test_GetProfileFromDatabase()
        {
            // Setup
            IMongoCollection<ProfileModel> profileCollection;
            // Execution
            profileCollection = _profileService.GetProfilesFromDatabase(_database);
            // Assert
            Assert.NotNull(profileCollection);
        }

        /// <summary>
        /// Load in a checkpoint of the profilecollection, modify it, and compare the collection to the new updated collection
        /// </summary>
        [Fact]
        public void Test_SaveProfile()
        {
            // Setup
            IMongoCollection<ProfileModel>  profileCollection = _profileService.GetProfilesFromDatabase(_database);
            ProfileModel profile = new ProfileModel(12345, "username", 12345, [], []);
            // Execution
            _profileService.SaveProfile(profile);
            // Assert
            Assert.NotEqual(profileCollection, _profileCollection);
        } // Test_SaveProfile's succession is dependant on Test_GetProfilesFromDatabase's succession

        /// <summary>
        /// Load in a checkpoint of the profilecollection, modify it, and compare the collection to the new updated collection
        /// </summary>
        //[Fact]
        //public void Test_DeleteProfile()
        //{
        //    // Setup
        //    IMongoCollection<ProfileModel> profileCollection = _profileService.GetProfilesFromDatabase(_database);
        //    ProfileModel profile = new ProfileModel(12345, "username", 12345, [], []);
        //    // Execution
        //    _profileService.DeleteProfile(profile.Id); // deletes entry from _profileCollection
        //    // Assert
        //    Assert.NotEqual(profileCollection, _profileCollection);
        //} // Test_DeletesProfile's succession is dependant on Test_GetProfilesFromDatabase's succession
    }
}