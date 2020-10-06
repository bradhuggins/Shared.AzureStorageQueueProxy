using System.Threading.Tasks;

namespace Shared.AzureStorageQueueProxy
{
    public interface IService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }
        string ConnectionString { get; set; }

        MessageFacade CreateMessage(string queueName, string message);
        Task<MessageFacade> CreateMessageAsync(string queueName, string message);
        void DeleteMessage(string queueName, string messageId, string popReceipt);
        Task DeleteMessageAsync(string queueName, string messageId, string popReceipt);
        MessageFacade PeekMessage(string queueName);
        Task<MessageFacade> PeekMessageAsync(string queueName);
        MessageFacade ReadMessage(string queueName, int? dequeueTimeoutSeconds = 30);
        Task<MessageFacade> ReadMessageAsync(string queueName, int? dequeueTimeoutSeconds = 30);

    }
}