namespace CityInfo.API.Models
{
    /// <summary>
    /// City data transfer object
    /// </summary>
    public class CityDto
    {
        /// <summary>
        /// Id of city
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of city
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of city
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Count of the total amount of points of interest in the city
        /// </summary>
        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        /// <summary>
        /// List of points of interest
        /// </summary>
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; }
            = new List<PointOfInterestDto>();
    }
}
