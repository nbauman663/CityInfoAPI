using AutoMapper;

namespace CityInfo.API.Profiles
{
    /// <summary>
    /// Profile class that uses AutoMapper to map city objects
    /// </summary>
    public class CityProfile : Profile
    {
        /// <summary>
        /// Maps for city objects
        /// </summary>
        public CityProfile()
        {
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>();
        }
    }
}
