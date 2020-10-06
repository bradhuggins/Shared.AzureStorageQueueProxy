#region Using Statements
using System.Threading.Tasks;
#endregion

namespace Shared.AzureStorageQueueProxy
{
    public interface IServiceV11
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }
        string ConnectionString { get; set; }

        Task CreateMessage(string queueName, string message);
       
        Task DeleteMessage();
       
        Task<string> PeekMessage(string queueName);
       
        Task<string> ReadMessage(string queueName);
          
        Task ResetMessageVisibility();
    }
}
