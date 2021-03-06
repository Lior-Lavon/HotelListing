using AutoMapper;
using HotelListing.Core.DTOs;
using HotelListing.Core.Models;
using HotelListing.Core.Repository;
using HotelListing.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
        {
            try
            {
                var hotels = await _unitOfWork.Hotels.GetAll(); //_unitOfWork.Hotels.GetAll(requestParams);
                var result = _mapper.Map<List<HotelDTO>>(hotels);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError("Something went wrong in GetHotels");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}", Name ="GetHotel")]  // Name = "GetHotel" is a like a nickname for this endpoint for CreatedAtRoute
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        {
            try
            {
                var hotel = await _unitOfWork.Hotels.Get(op => op.Id==id, new List<string> { "Country" });
                var hotelDTO = _mapper.Map<HotelDTO>(hotel);
                return Ok(hotelDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong in GetHotel");
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO createHotelDTO)
        {
            // validate the request
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attemt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                Hotel hotel = _mapper.Map<Hotel>(createHotelDTO);
                await _unitOfWork.Hotels.Insert(hotel); // insert
                await _unitOfWork.Save();               // save

                // CreatedAtRoute : user the route GetHotel , fetch data based on hotel.id
                // hotel originally has no Id, but after the save operation it will have the Id
                return CreatedAtRoute("GetHotel", new { id = hotel.Id}, hotel); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(CreateHotel)}");
                return StatusCode(500, "Internal server error !");
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] UpdateHotelDTO hotelDTO)
        {
            if (!ModelState.IsValid || id<1)
            {
                _logger.LogError($"Invalid update attempt {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = await _unitOfWork.Hotels.Get(h => h.Id == id);
                if (hotel == null)
                    return NotFound($"Submitted data is invalid in {nameof(UpdateHotel)}");

                // merge hotelDTO with -> hotel
                _mapper.Map(hotelDTO, hotel);
                // now hotel is the most updated
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return NoContent(); // after update -> return 204 NoContent
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(UpdateHotel)}");
                return StatusCode(500, "Internal server error.");
            }


        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attemp");
                return BadRequest();
            }

            try
            {
                Hotel hotel = await _unitOfWork.Hotels.Get(h => h.Id==id);
                if(hotel == null)
                {
                    _logger.LogError($"Hotel with {id} not found");
                    return NotFound($"Hotel with {id} not found");
                }

                await _unitOfWork.Hotels.Delete(id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong while Delete in {nameof(DeleteHotel)}");
                return StatusCode(500, $"Internal server error");
            }
        }
    }
}
