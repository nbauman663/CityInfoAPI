using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers
{
    /// <summary>
    /// Controler for file IO with API
    /// </summary>
    [Route("api/files")]
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        /// <summary>
        /// Constructor for dependency injection
        /// </summary>
        /// <param name="fileExtensionContentTypeProvider">File IO service</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public FilesController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        /// <summary>
        /// Gets file with input file name from project root dir
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <response code="200">Returns the hardcoded file</response>
        /// <response code="404">File not found</response>
        [HttpGet("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetFile(string fileName)
        {
            var pathToFile = "";
            if (fileName.IsNullOrEmpty())
            {
                pathToFile = "TextFile.txt";
            }
            else
            {
                pathToFile = fileName;
            }

            // check whether the file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
