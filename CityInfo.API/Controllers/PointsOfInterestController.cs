using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    /// <summary>
    /// Controller for CRUD operations on points of interest
    /// </summary>
    [Route("api/v{version:apiVersion}/cities/{cityId}/pointsofinterest")]
    [Authorize(Policy = "MustBeFromNewYorkCity")]
    [ApiVersion("2.0")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for dependancy injection
        /// </summary>
        /// <param name="logger">Logger service</param>
        /// <param name="mailService">Email service</param>
        /// <param name="cityInfoRepository">Database access service</param>
        /// <param name="mapper">AutoMapper service</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets all points of interest for a city only if the requestor lives in that city  
        /// </summary>
        /// <param name="cityId">Id number for the requested city</param>
        /// <returns>ActionResult task</returns>
        /// <response code="200">Returns points of interest for the city with the provided cityId</response>
        /// <response code="403">Access is forbidden because user does not live in requested city</response>
        /// <response code="404">City not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            // get metadata from user authentication to make it impossible to get
            // a cities point of interest if the user is not from that city.
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;

            if(!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId))
            {
                return Forbid();
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

           var pointsOfInterestForCity = await _cityInfoRepository
                .GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        /// <summary>
        /// Gets a specific point of interest
        /// </summary>
        /// <param name="cityId">Id of city of the point of interest</param>
        /// <param name="pointOfInterestId">Id of the point of interest</param>
        /// <returns>ActionResult task</returns>
        /// <response code="200">Returns points of interest requested</response>
        /// <response code="404">Point of interest requested is not found</response>
        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        /// <summary>
        /// Creates a point of interest
        /// </summary>
        /// <param name="cityId">Id of city the point of interest will be added to</param>
        /// <param name="pointOfInterest">Object representing a point of interest</param>
        /// <returns>ActionResult task</returns>
        /// <response code="200">Returns points of interest requested</response>
        /// <response code="404">Point of interest requested is not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(
                cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn =
                _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOFInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointOfInterestToReturn.Id
                },
                createdPointOfInterestToReturn);
        }


        /// <summary>
        /// Updates all fields on a point of interest
        /// </summary>
        /// <param name="cityId">City id of the point of interest to update</param>
        /// <param name="pointOfInterestId">Id of the point of interest to update</param>
        /// <param name="pointOfInterest">Object representing a point of interest</param>
        /// <returns>ActionResult task</returns>
        /// <response code="204">Point of interest updated successfully</response>
        /// <response code="404">Point of interest requested for update is not found</response>
        [HttpPut("{pointOfInterestId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Partially updates some or all fields on a point of interest
        /// </summary>
        /// <param name="cityId">City id of the point of interest to update</param>
        /// <param name="pointOfInterestId">Id of the point of interest to update</param>
        /// <param name="patchDocument">JSON patch document describing what to update</param>
        /// <returns>ActionResult task</returns>
        /// <response code="204">Point of interest updated successfully</response>
        /// <response code="400">Patch JSON is not valid</response>
        /// <response code="404">Point of interest requested for update is not found</response>
        [HttpPatch("{pointOfInterestId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Deletes requested point of interest
        /// </summary>
        /// <param name="cityId">City id of the point of interest to delete</param>
        /// <param name="pointOfInterestId">Id of the point of interest to delete</param>
        /// <returns>ActionResult task</returns>
        /// <response code="204">Point of interest deleted successfully</response>
        /// <response code="404">Point of interest requested for delete is not found</response>
        [HttpDelete("{pointOfInterestId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            _mailService.Send(
                "Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} " +
                $"with id {pointOfInterestEntity.Id} was deleted.");
            return NoContent();
        }
    }
}
