using Microsoft.AspNetCore.Mvc;
using ProfileServiceApp.Models;
using MongoDB.Driver;

namespace ProfileServiceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IMongoCollection<ProfileModel> _profileCollection;

        public ProfileController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("TeammateTinder");
            _profileCollection = database.GetCollection<ProfileModel>("Profiles");
        }

        [HttpPost("create")]
        public IActionResult CreateProfile([FromBody] ProfileModel profile)
        {
            _profileCollection.InsertOne(profile);
            return Ok();
        }

        [HttpGet("read/{userId}")]
        public IActionResult ReadProfileById(int userId)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, userId);
            var profile = _profileCollection.Find(filter).FirstOrDefault();

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpGet("read/all")]
        public IActionResult ReadProfiles()
        {
            var profiles = _profileCollection.Find(Builders<ProfileModel>.Filter.Empty).ToList();
            return Ok(profiles);
        }

        [HttpPut("update/{userId}")]
        public IActionResult UpdateProfile(int userId, [FromBody] ProfileModel updatedProfile)
        {
            var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, userId);
            var existingProfile = _profileCollection.Find(filter).FirstOrDefault();

            if (existingProfile == null)
            {
                return NotFound();
            }

            // Update the existing profile with the data from the updatedProfile object
            existingProfile.Username = updatedProfile.Username;
            existingProfile.PowerRanking = updatedProfile.PowerRanking;

            var updateResult = _profileCollection.ReplaceOne(filter, existingProfile);

            if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
            {
                return Ok(existingProfile);
            }
            else
            {
                return StatusCode(500);
            }
        }

    }
}
