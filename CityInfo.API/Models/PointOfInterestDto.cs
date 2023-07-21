namespace CityInfo.API.Models
{
    /// <summary>
    /// Data transfer object for point of interest
    /// </summary>
    public class PointOfInterestDto
    {
        /// <summary>
        /// Id of point of interest
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of point of interest
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of point of interest
        /// </summary>
        public string? Description { get; set; }
    }
}
