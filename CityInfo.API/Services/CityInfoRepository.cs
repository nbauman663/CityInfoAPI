using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    /// <summary>
    /// Class for accessing CityInfo DB
    /// </summary>
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="context">Database context</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets all cities ordered by city name
        /// </summary>
        /// <returns>A list of cities</returns>
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        /// <summary>
        /// Gets list or page of cities matching an optional name filter, an optional search 
        /// on name and description, and a page number and page size.
        /// </summary>
        /// <param name="name">City name to filter on</param>
        /// <param name="searchQuery">String to search city name and city description for</param>
        /// <param name="pageNumber">What page should be returned after all cities are paginated 
        /// by page size</param>
        /// <param name="pageSize">Size of page to return</param>
        /// <returns>A tuple containing a list of cities and pagination metadata</returns>
        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name
            ,string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => (c.Name.Contains(searchQuery))
                    || (c.Description != null && c.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn =  await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        /// <summary>
        /// Gets a city and optionally the cities points of interest
        /// </summary>
        /// <param name="cityId">The id of the city to get</param>
        /// <param name="getPointsOfInterest">Whether or not to get the points of intrest for the city</param>
        /// <returns>A City or null</returns>
        public async Task<City?> GetCityAsync(int cityId, bool getPointsOfInterest)
        {
            if (getPointsOfInterest)
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Cities.Where(c => c.Id == cityId)
                    .FirstOrDefaultAsync();
            }
        }

        /// <summary>
        /// Gets whether a city exists or not given its id
        /// </summary>
        /// <param name="cityId">Id of city to check</param>
        /// <returns>A boolean representing whether a city exists or not</returns>
        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        /// <summary>
        /// Gets a point of interest given a city id and a point of interest id
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="pointOfInterestId"></param>
        /// <returns>The point of interest requested or null if it does not exist</returns>
        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId,
            int pointOfInterestId)
        {
            return await _context.PointOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets points of interest of a city
        /// </summary>
        /// <param name="cityId">City id to get points of interest for</param>
        /// <returns>A list of points of interests</returns>
        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointOfInterest
                .Where(p => p.CityId == cityId).ToListAsync();
        }

        /// <summary>
        /// Adds point of interest to a city
        /// </summary>
        /// <param name="cityId">City id to add point of interest to</param>
        /// <param name="pointOfInterest">The point of interest to add</param>
        /// <returns></returns>
        public async Task AddPointOfInterestForCityAsync(int cityId,
            PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        /// <summary>
        /// Deletes a point of interest
        /// </summary>
        /// <param name="pointOfInterest">The point of interest to delete</param>
        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointOfInterest.Remove(pointOfInterest);
        }

        /// <summary>
        /// Returns boolean of whether input city name has the input city id
        /// </summary>
        /// <param name="cityName">City name to check</param>
        /// <param name="cityId">City id to check</param>
        /// <returns>Whether or not the city id matches the city name</returns>
        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }

        /// <summary>
        /// Saves changes to database
        /// </summary>
        /// <returns>boolean representing success or fail</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
