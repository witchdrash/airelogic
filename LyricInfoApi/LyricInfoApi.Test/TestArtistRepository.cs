using System.Collections.Generic;
using System.Data;
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
            const string artistId = "invalid";
            
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            httpClientWrapper.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>()).Returns(default(MusicBrainzArtistWorksCollection));
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0"))
                .Returns(httpClientWrapper.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
            var result = classUnderTest.GetWorksFor(artistId);
            
            Assert.Null(result);
        }
        
        [Fact]
        public void WhenSearchingForAWorksThatIsValidAndDoesntReturnMoreThan100TheExpectedValueIsReturned()
        {
            const string artistId = "6fe07aa5-fec0-4eca";
            
            var expected = new List<Works>
            {
                new Works
                {
                    Id = "the id",
                    Name = "Some Song"
                }
            };
            
            
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            
            httpClientWrapper.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = expected, WorkCount = 1, WorkOffset = 0 });
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0"))
                .Returns(httpClientWrapper.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
            var result = classUnderTest.GetWorksFor(artistId);
            
            Assert.Equal(expected, result);
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