using Microsoft.AspNetCore.Mvc;
using ProfileServiceApp.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Cors;
using ProfileServiceApp.Services; // Import the Cors namespace

namespace ProfileServiceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowSpecificOrigin")] // Apply CORS policy to the controller
    public class ProfileController : ControllerBase
    {
        private readonly IMongoCollection<ProfileModel> _profileCollection;
        private readonly ProfileService _profileService;

        public ProfileController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("TeammateTinder");
            _profileCollection = database.GetCollection<ProfileModel>("Profiles");
            _profileService = new ProfileService(mongoClient);
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
            ProfileModel profile = _profileService.ReadProfileById(userId);
            if (profile == null) { return NotFound(); }
            return Ok(profile);
        }

        [HttpGet("read/all")]
        public IActionResult ReadProfiles()
        {
            var profiles = _profileCollection.Find(Builders<ProfileModel>.Filter.Empty).ToList();
            return Ok(profiles);
        }

        //[HttpPut("overwrite/{userId}")]
        //public IActionResult UpdateProfile(int swiperID, [FromBody] ProfileModel updatedProfile)
        //{
        //    var filter = Builders<ProfileModel>.Filter.Eq(p => p.Id, swiperID);
        //    var existingProfile = _profileCollection.Find(filter).FirstOrDefault();

        //    if (existingProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update the existing profile with the data from the updatedProfile object
        //    existingProfile.Username = updatedProfile.Username;
        //    existingProfile.PowerRanking = updatedProfile.PowerRanking;

        //    var updateResult = _profileCollection.ReplaceOne(filter, existingProfile);

        //    if (updateResult.IsAcknowledged && updateResult.ModifiedCount > 0)
        //    {
        //        return Ok(existingProfile);
        //    }
        //    else
        //    {
        //        return StatusCode(500);
        //    }
        //}


        [HttpPatch("add/swipedno/{userId}")]
        public IActionResult AddToSwipedNo(int swiperID, [FromBody] int id2)
        {
            UpdateResult result = _profileService.AddIDToSwipedNo(swiperID, id2);

            // Check if the update was successful
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return Ok($"Added {id2} to SwipedNo array for user with ID {swiperID}");
            }
            else
            {
                return NotFound($"User with ID {swiperID} not found");
            }
        }

        [HttpPatch("add/swipedyes/{userId}")]
        public IActionResult AddToSwipedYes(int swiperID, [FromBody] int id2)
        {
            UpdateResult result = _profileService.AddIDToSwipedYes(swiperID, id2);

            // Check if the update was successful
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return Ok($"Added {id2} to SwipedNo array for user with ID {swiperID}");
            }
            else
            {
                return NotFound($"User with ID {swiperID} not found");
            }
        }

        [HttpDelete("delete/{userId}")]
        public IActionResult DeleteProfile(int userId)
        {
            var profile = _profileService.DeleteProfile(userId);
            return Ok(profile);
        }

    }
}
