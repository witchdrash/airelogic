using System;
using System.Collections.Generic;
using LyricInfoApi.Controllers;
using LyricInfoApi.Models;
using LyricInfoApi.Repositories;
using LyricInfoApi.Services;
using Moq;
using Xunit;

namespace LyricInfoApi.Test
{
    public class TestArtistsController
    {
        [Fact]
        public void WhenSearchingForAValidArtistThatReturnsOnlyTheSingleEntryTheExpectedValuesAreReturned()
        {
            var expected = new[] {new Artist("testy", "123")};
            
            var artistRepo = new Mock<IArtistRepository>();
            
            artistRepo.Setup(x => x.SearchFor(expected[0].Name)).Returns(expected);
            
            var classUnderTest = new ArtistsController(artistRepo.Object, null);
            
            var result = classUnderTest.Search(expected[0].Name);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WhenRequestingDataForASpecificArtistThatIsValidTheCorrectDataIsExpectedDataIsReturned()
        {

            var expected = new LyricStatistics {For = "1234-ABCD", AverageLyricsPerSong = 123.47m};
            
            var lyricStatisticsService = new Mock<ILyricStatisticsService>();
            lyricStatisticsService.Setup(x => x.CalculateFor(expected.For)).Returns(expected);
            
            var classUnderTest = new ArtistsController(null, lyricStatisticsService.Object);

            var result = classUnderTest.GetLyricStats(expected.For);
            
            Assert.Equal(expected, result);
            
        }
    }
}