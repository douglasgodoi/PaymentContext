using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;

namespace PaymentContext.Tests.Handlers
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            //Arrange
            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "";

            //Act
            command.Validate();

            //Assert
            Assert.IsTrue(command.Invalid);
        }
    }
}