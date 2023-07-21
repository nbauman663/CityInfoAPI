namespace CityInfo.API.Services
{
    /// <summary>
    /// Interface for mail service
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends fake email
        /// </summary>
        /// <param name="subject">Subject of fake email</param>
        /// <param name="message">Message of fake email</param>
        void Send(string subject, string message);
    }
}