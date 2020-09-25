#region Using Statements
using System.Threading.Tasks;
#endregion

namespace Shared.AzureStorageQueueProxy
{
    public interface IService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }
        string ConnectionString { get; set; }

        /// <summary>
        /// Create a new messaage in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <param name="message">Any text message; including json</param>
        /// <returns></returns>
        Task CreateMessage(string queueName, string message);
       
        /// <summary>
        /// Delete the current message.
        /// </summary>
        /// <returns></returns>
        Task DeleteMessage();
        
        /// <summary>
        /// Peek the next message in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <returns></returns>
        Task<string> PeekMessage(string queueName);

        /// <summary>
        /// Read the next message in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <returns></returns>
        Task<string> ReadMessage(string queueName);
        
        /// <summary>
        /// Reset the current message's visibility.
        /// </summary>
        /// <returns></returns>
        Task ResetMessageVisibility();
    }
}
