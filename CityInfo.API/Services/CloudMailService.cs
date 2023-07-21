namespace CityInfo.API.Services
{
    /// <summary>
    /// Class for sending fake email fom the cloud
    /// </summary>
    public class CloudMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        /// <summary>
        /// Constructor for injection and private member setting
        /// </summary>
        /// <param name="configuration">Access to appsettings</param>
        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        /// <summary>
        /// Sends a fake email
        /// </summary>
        /// <param name="subject">Subject of email</param>
        /// <param name="message">Contents of email</param>
        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " +
                $"with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
