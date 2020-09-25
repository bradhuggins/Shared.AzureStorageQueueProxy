using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.AzureStorageQueueProxy;
using System;
using System.Threading.Tasks;

namespace Shared.AzureStorageQueueProxyTests
{
    [TestClass]
    public class ServiceTests
    {
        private const string _connectionString = "[ENTER CONNECTION STRING HERE]";
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
        public async Task CreateMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            
            // Act
            await target.CreateMessage(_queueName, this.SampleMessage);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task PeekMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            var actual = await target.PeekMessage(_queueName);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task ReadMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            var actual = await target.ReadMessage(_queueName);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task DeleteMessage_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            await target.CreateMessage(_queueName, this.SampleMessage);
            var queueMessage = await target.ReadMessage(_queueName);

            // Act
            await target.DeleteMessage();

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task ResetMessageVisibility_Test()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            await target.CreateMessage(_queueName, this.SampleMessage);
            var queueMessage = await target.ReadMessage(_queueName);

            // Act
            await target.ResetMessageVisibility();

            // Assert
            Assert.IsFalse(target.HasError);
        }

    }
}
