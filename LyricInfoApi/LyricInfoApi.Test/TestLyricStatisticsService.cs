using LyricInfoApi.Repositories;
using LyricInfoApi.Services;
using Moq;
using Xunit;

namespace LyricInfoApi.Test
{
    public class TestLyricStatisticsService
    {
        [Fact]
        public void WhenSearchingForAnInvalidIdNullIsReturned()
        {
            var artistRepo = new Mock<IArtistRepository>();
            var classUnderTest = new LyricStatisticsService(artistRepo.Object);

            var result = classUnderTest.CalculateFor("invalid");
            
            Assert.Null(result);
        }
    }
}