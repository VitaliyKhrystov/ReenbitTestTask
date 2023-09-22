using Moq;
using WebAppReenbitTest.Controllers;
using WebAppReenbitTest.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using WebAppReenbitTest;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace WebAppReenbitTest_Tests
{
    public class FilesControllerTest
    {

        private readonly Mock<ILogger<FilesController>> logger;
        private FileService fileService;
        private FilesController controller;

        public FilesControllerTest()
        {
            logger = new Mock<ILogger<FilesController>>();
            BlobCredential.Account = "reenbitstoragetest";
            BlobCredential.Key = "ZzFHPQ+J3VgZBXAadzv9eioNzVq58PjZn93u37EnnDYA+6DyTlaljjuP0EgQ5ooqT0XQC2d84W4b+AStaqXhjA==";
            fileService = new FileService();
            controller = new FilesController(fileService, logger.Object);
        }
        [Fact]
        public async Task UploadAsync_CheckStatus_ShouldReturnOk()
        {
            //Arrange

            int status = 201;
            int rand = new Random().Next(1, 10000);

            var fileName = $"test{rand}.docx";
            var fileContent = Encoding.UTF8.GetBytes($"This is the content from {fileName}");

            var memoryStream = new MemoryStream(fileContent);

            var file = new FormFile(
                baseStream: memoryStream,
                baseStreamOffset: 0,
                length: memoryStream.Length,
                name: "file",
                fileName: fileName
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };

            var email = "vitalii.khrystov.it@gmail.com";
            var httpcontext = new DefaultHttpContext();
            httpcontext.Request.Form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "email", new StringValues(email)}
            });
            controller.ControllerContext = new ControllerContext { HttpContext= httpcontext };


            //Act
            var result = await controller.UploadAsync(file);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(status, okResult.Value );
        }

        [Fact]
        public async Task UploadAsync_CheckStatus_ShouldReturnStatus500InternalServerError()
        {
            //Arrange
            int rand = new Random().Next(1, 10000);

            var fileName = $"test{rand}.docx";
            var fileContent = Encoding.UTF8.GetBytes("");

            var memoryStream = new MemoryStream(fileContent);

            var file = new FormFile(
                baseStream: memoryStream,
                baseStreamOffset: 0,
                length: memoryStream.Length,
                name: "file",
                fileName: fileName
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };

            string email = "";
            var httpcontext = new DefaultHttpContext();
            httpcontext.Request.Form = new FormCollection(new Dictionary<string, StringValues>
            {
                { "email", new StringValues(email)}
            });
            controller.ControllerContext = new ControllerContext { HttpContext = httpcontext };

            //Act
            var result = await controller.UploadAsync(file);

            //Assert

            Assert.Equal(StatusCodes.Status500InternalServerError, (result as ObjectResult).Value);

        }
    }
}