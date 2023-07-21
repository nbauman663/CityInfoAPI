using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CityInfo.API.Entities
{
    /// <summary>
    /// Object representing a city
    /// </summary>
    public class City
    {
        /// <summary>
        /// City Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// City name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// City description
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Array of points of interest in city
        /// </summary>
        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
            = new List<PointOfInterest>();

        /// <summary>
        /// Constructor for city
        /// </summary>
        /// <param name="name">Name of city</param>
        public City(string name)
        {
            Name = name;
        }
    }
}
