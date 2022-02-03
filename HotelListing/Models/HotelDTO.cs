using HotelListing.Data;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength:50, ErrorMessage ="Hotel Name too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Hotel Address too long")]
        public string Address { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Value for Rating must be between 1 and 5.")]
        public double Rating { get; set; }

        [Required]
        public int CountryId { get; set; }

    }

    public class HotelDTO : CreateHotelDTO
    {
        public int Id { get; set; }

        public CountryDTO Country { get; set; }
    }

}
