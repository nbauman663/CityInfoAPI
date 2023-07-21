namespace CityInfo.API.Services
{
    /// <summary>
    /// Class for sending fake email locally
    /// </summary>
    public class LocalMailService : IMailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;
        
        /// <summary>
        /// Constructor for injection and private member setting
        /// </summary>
        /// <param name="configuration">Access to appsettings</param>
        public LocalMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        /// <summary>
        /// Sends fake email
        /// </summary>
        /// <param name="subject">Subject of fake email</param>
        /// <param name="message">Message of fake email</param>
        public void Send(string subject, string message)
        {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " +
                $"with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
