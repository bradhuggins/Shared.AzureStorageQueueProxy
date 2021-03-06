using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.AzureStorageQueueProxy;
using System;
using System.Threading.Tasks;

namespace Shared.AzureStorageQueueProxyTests
{
    [TestClass]
    public class ServiceTests
    {
        private const string _connectionString = "UseDevelopmentStorage=true;";
        private const string _queueName = "unittestingqueue";
        private string SampleMessage 
        { 
            get 
            { 
                return Guid.NewGuid().ToString()
                + "/t" + DateTime.UtcNow
                + "/t" + "unit test"; 
            } 
        }

        [TestMethod]
        public void HasError_True_Test()
        {
            // Arrange
            Service target = new Service();
            target.ErrorMessage = "error";

            // Act
            var actual = target.HasError;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void HasError_False_Test()
        {
            // Arrange
            Service target = new Service();

            // Act
            var actual = target.HasError;

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ConnectionString_Error_Test()
        {
            // Arrange
            Service target = new Service();
            bool hasError = false;
            // Act
            try
            {
                var connectionString = target.ConnectionString;
            }
            catch (Exception ex)
            {
                hasError = true;
            }

            // Assert
            Assert.IsTrue(hasError);
        }

        [TestMethod]
        public async Task CreateMessageAsync_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            
            // Act
            await target.CreateMessageAsync(_queueName, this.SampleMessage);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task PeekMessageAsync_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            var actual = await target.PeekMessageAsync(_queueName);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task ReadMessageAsync_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            var actual = await target.ReadMessageAsync(_queueName);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task DeleteMessageAsync_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            await target.CreateMessageAsync(_queueName, this.SampleMessage);
            var queueMessage = await target.ReadMessageAsync(_queueName);

            // Act
            await target.DeleteMessageAsync(_queueName, queueMessage.MessageId, queueMessage.PopReceipt);

            // Assert
            Assert.IsFalse(target.HasError);
        }


        [TestMethod]
        public void CreateMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            target.CreateMessage(_queueName, this.SampleMessage);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public void PeekMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            var actual = target.PeekMessage(_queueName);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public void ReadMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            var actual = target.ReadMessage(_queueName);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public void DeleteMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            target.CreateMessage(_queueName, this.SampleMessage);
            var queueMessage = target.ReadMessage(_queueName);

            // Act
            target.DeleteMessage(_queueName, queueMessage.MessageId, queueMessage.PopReceipt);

            // Assert
            Assert.IsFalse(target.HasError);
        }

    }
}
