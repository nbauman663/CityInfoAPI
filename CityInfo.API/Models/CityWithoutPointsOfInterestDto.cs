﻿namespace CityInfo.API.Models
{
    /// <summary>
    /// A data transfer object for a city without points of interest
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        /// <summary>
        /// The id of the city
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the city
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The description of the city
        /// </summary>
        public string? Description { get; set; }
    }
}
