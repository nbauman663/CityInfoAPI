using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities
{
    /// <summary>
    /// Object representing a point of interest
    /// </summary>
    public class PointOfInterest
    {
        /// <summary>
        /// Point of interest Id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Point of interest name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Point of interest description
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// City point of interest is in
        /// </summary>
        [ForeignKey("CityId")]
        public City? City { get; set; }

        /// <summary>
        /// City Id of city point of interest is in
        /// </summary>
        public int CityId   { get; set; }

        /// <summary>
        /// Constructor for point of interest
        /// </summary>
        /// <param name="name">Name of point of interest</param>
        public PointOfInterest(string name)
        {
            Name = name;
        }
    }
}
