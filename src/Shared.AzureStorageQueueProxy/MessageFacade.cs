namespace Shared.AzureStorageQueueProxy
{
    public class MessageFacade
    {
        public string MessageId { get; set; }

        public string MessageText { get; set; }

        public string PopReceipt { get; set; }
    }
}
