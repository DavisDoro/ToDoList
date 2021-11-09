//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using ToDoList.Controllers;
//using Xunit;

//namespace UnitTestApp.Tests
//{
//    public class HomeControllerTests
//    {
//        private ILogger<HomeController> logger;

//        [Fact]
//        public void PRIVACYViewDataMessage()
//        {
//            // Arrange
//            HomeController controller = new HomeController(logger);

//            // Act
//            ViewResult result = controller.Privacy() as ViewResult;

//            // Assert
//            Assert.Equal("Use this page to detail your site's privacy policy.", result?.ViewData["Message"]);
//        }

//        [Fact]
//        public void IndexViewResultNotNull()
//        {
//            // Arrange
//            HomeController controller = new HomeController(logger);
//            // Act
//            ViewResult result = controller.Privacy() as ViewResult;
//            // Assert
//            Assert.NotNull(result);
//        }

//        [Fact]
//        public void PrivacyViewNameEqualIndex()
//        {
//            // Arrange
//            HomeController controller = new HomeController(logger);
//            // Act
//            ViewResult result = controller.Privacy() as ViewResult;
//            // Assert
//            Assert.Equal("Privacy", result?.ViewName);
//        }
//    }
//}
