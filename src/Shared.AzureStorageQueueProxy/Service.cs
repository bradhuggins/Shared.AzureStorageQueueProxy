#region Using Statements
using Azure.Storage.Queues; // Namespace for Queue storage types
using Azure.Storage.Queues.Models; // Namespace for PeekedMessage
using System;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Shared.AzureStorageQueueProxy
{
    // https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues

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

        public async Task<MessageFacade> CreateMessageAsync(string queueName, string message)
        {
            MessageFacade toReturn = null;
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    // Send a message to the queue
                    var receipt = await queueClient.SendMessageAsync(message);
                    toReturn = new MessageFacade()
                    {
                        MessageId = receipt.Value.MessageId,
                        MessageTexty = message,
                        PopReceipt = receipt.Value.PopReceipt
                    };
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task<MessageFacade> PeekMessageAsync(string queueName)
        {
            MessageFacade toReturn = null;
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    PeekedMessage[] peekedMessage = await queueClient.PeekMessagesAsync();
                    if (peekedMessage != null && peekedMessage.ToList().Count > 0)
                    {
                        toReturn = new MessageFacade()
                        {
                            MessageId = peekedMessage[0].MessageId,
                            MessageTexty = peekedMessage[0].MessageText
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task<MessageFacade> ReadMessageAsync(string queueName, int? dequeueTimeoutSeconds = 30)
        {
            MessageFacade toReturn = null;
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    QueueMessage[] retrievedMessage = await queueClient.ReceiveMessagesAsync(1, TimeSpan.FromSeconds((double)dequeueTimeoutSeconds));
                    if (retrievedMessage != null && retrievedMessage.ToList().Count > 0)
                    {
                        toReturn = new MessageFacade()
                        {
                            MessageId = retrievedMessage[0].MessageId,
                            MessageTexty = retrievedMessage[0].MessageText,
                            PopReceipt = retrievedMessage[0].PopReceipt
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task DeleteMessageAsync(string queueName, string messageId, string popReceipt)
        {
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    await queueClient.DeleteMessageAsync(messageId, popReceipt);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }


        public MessageFacade CreateMessage(string queueName, string message)
        {
            MessageFacade toReturn = null;
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    // Send a message to the queue
                    var receipt = queueClient.SendMessage(message);
                    toReturn = new MessageFacade()
                    {
                        MessageId = receipt.Value.MessageId,
                        MessageTexty = message,
                        PopReceipt = receipt.Value.PopReceipt
                    };
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public MessageFacade PeekMessage(string queueName)
        {
            MessageFacade toReturn = null;
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    PeekedMessage[] peekedMessage = queueClient.PeekMessages();
                    if (peekedMessage != null && peekedMessage.ToList().Count > 0)
                    {
                        toReturn = new MessageFacade()
                        {
                            MessageId = peekedMessage[0].MessageId,
                            MessageTexty = peekedMessage[0].MessageText
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public MessageFacade ReadMessage(string queueName, int? dequeueTimeoutSeconds = 30)
        {
            MessageFacade toReturn = null;
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    QueueMessage[] retrievedMessage = queueClient.ReceiveMessages(1, TimeSpan.FromSeconds((double)dequeueTimeoutSeconds));
                    if (retrievedMessage != null && retrievedMessage.ToList().Count > 0)
                    {
                        toReturn = new MessageFacade()
                        {
                            MessageId = retrievedMessage[0].MessageId,
                            MessageTexty = retrievedMessage[0].MessageText,
                            PopReceipt = retrievedMessage[0].PopReceipt
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public void DeleteMessage(string queueName, string messageId, string popReceipt)
        {
            try
            {
                QueueClient queueClient = new QueueClient(this.ConnectionString, queueName);
                if (queueClient.Exists())
                {
                    queueClient.DeleteMessage(messageId, popReceipt);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

    }
}
