using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    /// <summary>
    /// Controller implementing token based authentication where username
    /// and password do not matter; every token works.
    /// </summary>
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Class representing an authentication request
        /// </summary>
        public class AuthenticationRequestBody
        {
            /// <summary>
            /// Username of user making authentication request
            /// </summary>
            public string? UserName { get; set; }

            /// <summary>
            /// Password of user making authentication request
            /// </summary>
            public string? Password { get; set; }
        }

        /// <summary>
        /// Class containing API user info 
        /// </summary>
        private class CityInfoUser
        {
            /// <summary>
            /// A user's user id
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// A user's username
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// A user's first name
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            /// A user's last name
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// A user's city of residence
            /// </summary>
            public string City { get; set; }

            public CityInfoUser(
                int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }   
        }

        private IConfiguration _configuration;

        /// <summary>
        /// Constructor for dependancy injection onto AuthenticationController
        /// class
        /// </summary>
        /// <param name="configuration">IConfiguration injecting access to appsettings.json files</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Authenticates a user and provides them with a token used for API access
        /// </summary>
        /// <param name="authenticationRequestBody">An object containing a user's username and password</param>
        /// <returns>ActionResult</returns>
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
            AuthenticationRequestBody authenticationRequestBody)
        {
            // Step 1: validate the username/password
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Step 2: create a token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        /// <summary>
        /// Creates a CityInfoUser object given a user's username and password.
        /// There is no user database so some of the members of CityInfoUser that
        /// would typically be set dynamically are hardcoded. 
        /// </summary>
        /// <param name="userName">A user's username</param>
        /// <param name="password">A user's password</param>
        /// <returns></returns>
        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            // we don't have a user DB or table. So for demo purposes we assume the credentials are valid.
            // typically we would check the username and password against the DB and get the user data

            return new CityInfoUser(
                1,
                userName ?? "",
                "Nate",
                "Bauman",
                "New York City");
        }
    }
}
