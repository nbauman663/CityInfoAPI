using Microsoft.EntityFrameworkCore.Query;
using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    /// <summary>
    /// City database access class
    /// </summary>
    public interface ICityInfoRepository
    {
        /// <summary>
        /// Gets all cities
        /// </summary>
        /// <returns>List of cities</returns>
        Task<IEnumerable<City>> GetCitiesAsync();

        /// <summary>
        /// Gets page of cities with optional search and filter arguments
        /// </summary>
        /// <param name="name">City name to filter on</param>
        /// <param name="searchQuery">String to search city names and descriptions for</param>
        /// <param name="pageNumber">The page number after the cities have been paginated by pageSize</param>
        /// <param name="pageSize">Amount of cities to return</param>
        /// <returns>Tuple containing list of cities and pagination metadata object</returns>
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

        /// <summary>
        /// Gets a city and optionally the points of interest for the city
        /// </summary>
        /// <param name="cityId">Id of the city to get</param>
        /// <param name="getPointsOfInterest">Boolean representing whether we want points of interest with the city or not</param>
        /// <returns>A City object if cityId exists or null</returns>
        Task<City?> GetCityAsync(int cityId, bool getPointsOfInterest);

        /// <summary>
        /// Does the city exist?
        /// </summary>
        /// <param name="cityId">The city to check if it exists</param>
        /// <returns>Whether the city exists or not</returns>
        Task<bool> CityExistsAsync(int cityId);

        /// <summary>
        /// Gets points of interests for given city Id
        /// </summary>
        /// <param name="cityId">City id of the city to get points of interest for</param>
        /// <returns>List of points of interest</returns>
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

        /// <summary>
        /// Gets a specific point of interest from a city
        /// </summary>
        /// <param name="cityId">Id of city to get point of interest from</param>
        /// <param name="pointOfInterestId">Id of point of interest to get</param>
        /// <returns>Point of interest if it exists or null</returns>
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
            int pointOfInterestId);
        
        /// <summary>
        /// Adds point of interest to city
        /// </summary>
        /// <param name="cityId">Id of city to add point of interest to</param>
        /// <param name="pointOfInterest">The point of interest to add</param>
        /// <returns>Task</returns>
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

        /// <summary>
        /// Deletes a specific point of interest
        /// </summary>
        /// <param name="pointOfInterest">The point of interest to delete</param>
        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        /// <summary>
        /// Does the city name provided match the city id provided?
        /// </summary>
        /// <param name="cityName">The city name to check</param>
        /// <param name="cityId">The city id to check</param>
        /// <returns>Whether or not the city id matches the city name</returns>
        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangesAsync();
    }
}
