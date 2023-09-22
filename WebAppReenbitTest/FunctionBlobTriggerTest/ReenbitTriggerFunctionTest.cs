using FunctionBlobTrigger;
using Microsoft.Extensions.Logging;
using Moq;

namespace FunctionBlobTriggerTest
{
    public class ReenbitTriggerFunctionTest
    {
        [Fact]
        public void TestBlobStorageTriggerFunction_CompareLogInformationMessage_ReturnedMessageMustBeEqualToVerificationMessage()
        {
            // Arrange
            ReenbitTriggerFunction triggerFunction = new ReenbitTriggerFunction();
            var loggerMock = new Mock<ILogger>();
            var stream = new MemoryStream();
            var name = "v@mail.ua#myblob.txt";
            var shortName = "myblob.txt";
            var message = $"C# Blob trigger function Processed blob\n Name:{shortName} \n Size: {0} Bytes";
            loggerMock.Setup(x => x.Log(LogLevel.Information, 0, It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>())).Verifiable();
            
            // Act
            triggerFunction.Run(stream, name, loggerMock.Object);
            var loggedMessages = loggerMock.Invocations
                .Where(x => (LogLevel)x.Arguments[0] == LogLevel.Information)
                .Select(x => x.Arguments[2].ToString()).First();

            // Assert
            Assert.Equal(loggedMessages, message);
        }
    }
}