using System;
using System.Collections.Generic;
using LyricInfoApi.Controllers;
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
            
            var classUnderTest = new ArtistsController(artistRepo.Object);
            
            var result = classUnderTest.Search(expected[0].Name);
            Assert.Equal(expected, result);
        }
    }
}