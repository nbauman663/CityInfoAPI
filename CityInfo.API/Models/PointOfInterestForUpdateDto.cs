using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    /// <summary>
    /// Data transfer object for updating point of interest
    /// </summary>
    public class PointOfInterestForUpdateDto
    {
        /// <summary>
        /// Name of point of interest
        /// </summary>
        [Required(ErrorMessage = "You should provide a name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of point of interest
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
