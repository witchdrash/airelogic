using System.Collections.Generic;
using System.Net.Http;
using LyricInfoApi.Models;
using LyricInfoApi.Repositories;
using LyricInfoApi.Wrappers;
using Moq;
using Xunit;

namespace LyricInfoApi.Test
{
    public class TestArtistRepository
    {
        [Fact]
        public void WhenSearchingForWorksWithAnInvalidIdNullIsReturned()
        {
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            //httpClientFactory.Setup(x => x.CreateClient()).Returns(new Mock<)
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
        }

        [Fact]
        public void WhenSearchingForArtistsTheExpectedValueIsReturned()
        {
            const string artistName = "artistsName";
            var expected = new List<Artist>
            {
                new Artist {Id = "abc", Name = artistName, SortName = artistName}
            };

            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            httpClientWrapper.Setup(x => x.GetResponse<MusicBrainzArtistsCollection>())
                .Returns(new MusicBrainzArtistsCollection {Artists = expected});
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory.Setup(x =>
                x.CreateClient(HttpMethod.Get, $"http://musicbrainz.org/ws/2/artist/?query=artist:{artistName}")).Returns(httpClientWrapper.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
            var result = classUnderTest.SearchFor(artistName);
            
            Assert.Equal(expected, result);
        }
    }
}