#region Using Statements
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
#endregion

namespace Shared.AzureStorageQueueProxy
{
    // https://github.com/Azure/azure-storage-net/tree/master/Samples/GettingStarted/VisualStudioQuickStarts/DataStorageQueue

    [ExcludeFromCodeCoverage]
    public class Service : IService
    {
        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new System.Exception("Error: ConnectionString not set!");
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        private CloudQueue _currentQueue;
        private CloudQueueMessage _currentMessage;
       
        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount = null;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                this.ErrorMessage = "Invalid storage account information provided in config.";

            }
            catch (ArgumentException)
            {
                this.ErrorMessage = "Invalid storage account information provided in config.";
            }
            return storageAccount;
        }

        private async Task<CloudQueue> GetQueueReference(string queueName)
        {
            CloudQueue cloudQueue = null;
            try
            {
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(this.ConnectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                cloudQueue = queueClient.GetQueueReference(queueName);
                await cloudQueue.CreateIfNotExistsAsync();
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return cloudQueue;
        }

        /// <summary>
        /// Create a new messaage in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <param name="message">Any text message; including json</param>
        /// <returns></returns>
        public async Task CreateMessage(string queueName, string message)
        {
            try
            {
                CloudQueue cloudQueue = await this.GetQueueReference(queueName);
                await cloudQueue.AddMessageAsync(new CloudQueueMessage(message));
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

        /// <summary>
        /// Peek the next message in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <returns></returns>
        public async Task<string> PeekMessage(string queueName)
        {
            string toReturn = null;
            try
            {
                CloudQueue cloudQueue = await this.GetQueueReference(queueName);
                CloudQueueMessage peekedMessage = await cloudQueue.PeekMessageAsync();
                if(peekedMessage != null)
                {
                    toReturn = peekedMessage.AsString;
                }
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        /// <summary>
        /// Read the next message in the queue.
        /// </summary>
        /// <param name="queueName">Name of the queue</param>
        /// <returns></returns>
        public async Task<string> ReadMessage(string queueName)
        {
            string toReturn = null;
            try
            {
                _currentQueue = await this.GetQueueReference(queueName);
                _currentMessage = await _currentQueue.GetMessageAsync();
                if (_currentMessage != null)
                {
                    toReturn = _currentMessage.AsString;
                }
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        /// <summary>
        /// Delete the current message.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteMessage()
        {
            try
            {
                if (_currentQueue == null)
                {
                    this.ErrorMessage = "Current queue not set.";
                    return;
                }

                if (_currentMessage != null)
                {
                    await _currentQueue.DeleteMessageAsync(_currentMessage);
                }
                else
                {
                    this.ErrorMessage = "Current messasge not set.";
                }
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

        /// <summary>
        /// Reset the current message's visibility.
        /// </summary>
        /// <returns></returns>
        public async Task ResetMessageVisibility()
        {
            try
            {
                if (_currentQueue == null)
                {
                    this.ErrorMessage = "Current queue not set.";
                    return;
                }

                if (_currentMessage != null)
                {
                    await _currentQueue.UpdateMessageAsync(
                        _currentMessage,
                        TimeSpan.Zero,  // update visible immediately
                        MessageUpdateFields.Content |
                        MessageUpdateFields.Visibility);
                }
                else
                {
                    this.ErrorMessage = "Current messasge not set.";
                }
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

    }
}
