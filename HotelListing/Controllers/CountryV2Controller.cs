using HotelListing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace HotelListing.Controllers
{
    // set the api version
    [ApiVersion("2.0")]
    //[ApiVersion("2.0", Deprecated = true)]

    // set specific route
    // [Route("api/{v:apiversion}/country")] // use with api version in route
    [Route("api/country")] // use with api-version in header

    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        // Version-1 used _unitOfWork

        // V2 will use DB context directly
        private DatabaseContext _context;

        // Remark : Remember to add ...
        // services.AddTransient<IUnitOfWork, UnitOfWork > (); on Startup
        public CountryV2Controller(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
//        [ResponseCache(CacheProfileName = "120SecondsDuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(_context.Countries);
        }

    }
}
