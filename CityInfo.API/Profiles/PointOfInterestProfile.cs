using AutoMapper;

namespace CityInfo.API.Profiles
{
    /// <summary>
    /// Profile for mapping point of interest objects using AutoMapper
    /// </summary>
    public class PointOfInterestProfile : Profile
    {
        /// <summary>
        /// Point of interest object maps
        /// </summary>
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
        }
    }
}
