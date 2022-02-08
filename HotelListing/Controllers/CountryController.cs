using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    //    [ApiVersion("1.0")]
    //[Route("api/{v:apiversion}/[controller]")]
    [Route("api/[controller]")]

    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        // Remark : Remember to add ...
        // services.AddTransient<IUnitOfWork, UnitOfWork > (); on Startup
        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
//        [ResponseCache(CacheProfileName = "120SecondsDuration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries(
            [FromQuery] RequestParams requestParams)
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll(requestParams);
                var results = _mapper.Map<List<CountryDTO>>(countries);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal server error !");
            }
        }

        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
                Country country = await _unitOfWork.Countries.Get(q => q.Id==id, new List<string>() { "Hotels" });
                CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);
                return Ok(countryDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                //return StatusCode(500, "Internal server error !");
                return StatusCode(500, $"Internal server error !, Something went wrong in the {nameof(GetCountry)}");
            }
        }

        [Authorize(Roles = "Administrator")]
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Invalid POST attemt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }

            try
            {
                Country country = _mapper.Map<Country>(countryDTO);
                await _unitOfWork.Countries.Insert(country);
                await _unitOfWork.Save();
                return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Internal server error !");
                return StatusCode(500, "Internal server error !");
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid update param at {nameof(UpdateCountry)}");
                return BadRequest($"Invalid update param at { nameof(UpdateCountry)}");
            }

            try
            {
                // find the existing country
                Country country = await _unitOfWork.Countries.Get(c => c.Id == id, new List<string>() { "Hotels" });
                if(country == null)
                {
                    _logger.LogError($"Country with id {id} no found");
                    return NotFound($"Submitted data is invalid in {nameof(UpdateCountry)}");
                }

                _mapper.Map(countryDTO, country);
                _unitOfWork.Countries.Update(country);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(UpdateCountry)}");
                return StatusCode(500, "Something went wrong");
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attemp");
                return BadRequest();
            }

            try
            {
                Country country = await _unitOfWork.Countries.Get(c => c.Id == id);
                if (country == null)
                {
                    _logger.LogError($"Country with {id} not found");
                    return NotFound($"Country with {id} not found");
                }

                await _unitOfWork.Countries.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong while Delete in {nameof(DeleteCountry)}");
                return StatusCode(500, $"Internal server error");
            }
        }

    }
}
