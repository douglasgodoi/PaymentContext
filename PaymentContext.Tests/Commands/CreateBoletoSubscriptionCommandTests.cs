using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;

namespace PaymentContext.Tests.Commands
{
    [TestClass]
    public class CreateBoletoSubscriptionCommandTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenNameIsValid()
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