using System;
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
                    Title = "Some Song",
                    Disambiguation = "hello"
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
            
            Assert.Equal(expected, result.Works);
        }

        [Fact]
        public void WhenProducingAWorksIfAnyComebackWithADisambiguationUseThisInsteadOfLookingUpTheArtist()
        {
            const string artistId = "6fe07aa5-fec0-4eca";
            const string expectedArtistName = "Artist Name Goes Here";
            
            var works = new List<Works>
            {
                new Works
                {
                    Id = "the id",
                    Title = "Some Song",
                    Disambiguation = expectedArtistName
                }
            };
            
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            
            httpClientWrapper.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = works, WorkCount = 1, WorkOffset = 0 });
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0"))
                .Returns(httpClientWrapper.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
            var result = classUnderTest.GetWorksFor(artistId);
            
            Assert.Equal(expectedArtistName, result.ArtistName);
        }

        [Fact]
        public void WhereNoSongsHaveADisambiguationLookUpTheArtistNameUsingTheId()
        {
            const string artistId = "6fe07aa5-fec0-4eca";
            const string expectedArtistName = "Artist Name Goes Here";
            
            var works = new List<Works>
            {
                new Works
                {
                    Id = "the id",
                    Title = "Some Song"
                }
            };
            
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            httpClientWrapper.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = works, WorkCount = 1, WorkOffset = 0 });

            var httpClientWrapperNameLookup = new Mock<IHttpClientWrapper>();
            httpClientWrapperNameLookup.Setup(x => x.GetResponse<Artist>()).Returns(
                new Artist
                    {
                        Id = artistId,
                        Name = expectedArtistName
                    });
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0"))
                .Returns(httpClientWrapper.Object);
            
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/artist/{artistId}"))
                .Returns(httpClientWrapperNameLookup.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
            var result = classUnderTest.GetWorksFor(artistId);
            
            Assert.Equal(expectedArtistName, result.ArtistName);
        }
        
        [Fact]
        public void WhenSearchingForWorksAndThereAreMoreThan100RepeatedRequestsAreMadeAndTheEndResultIsAggregated()
        {
            const string artistId = "6fe07aa5-fec0-4eca";

            var page1 = new Works
            {
                Id = "the id",
                Title = "Some Song"
            };
            var page2 = new Works
            {
                Id = "second page",
                Title = "another song that's on a different page",
                Disambiguation = "hello"
            };
            var expected = new List<Works>
            {
                page1,
                page2
            };
            
            var httpClientWrapperPage1 = new Mock<IHttpClientWrapper>();
            
            httpClientWrapperPage1.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = new List<Works> { page1 }, WorkCount = 101, WorkOffset = 0 });
            
            var httpClientWrapperPage2 = new Mock<IHttpClientWrapper>();
            httpClientWrapperPage2.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = new List<Works> { page2 }, WorkCount = 101, WorkOffset = 100 });
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0"))
                .Returns(httpClientWrapperPage1.Object);
            
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=100"))
                .Returns(httpClientWrapperPage2.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);
            var result = classUnderTest.GetWorksFor(artistId);
            
            Assert.Collection(result.Works, 
                item => Assert.Equal(page1, item), 
                item => Assert.Equal(page2, item));
        }

        [Fact]
        public void WhenHittingTheApiRepeatedlySlowCallsToEnsureWereNotBannedByTheRateLimiter()
        {
            const string artistId = "6fe07aa5-fec0-4eca";

            var workData = new Works
            {
                Id = "the id",
                Title = "Some Song",
                Disambiguation = "hello"
            };
            
            var httpClientWrapperPage1 = new Mock<IHttpClientWrapper>();
            
            httpClientWrapperPage1.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = new List<Works> { workData }, WorkCount = 201, WorkOffset = 0 });
            
            var httpClientWrapperPage2 = new Mock<IHttpClientWrapper>();
            httpClientWrapperPage2.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = new List<Works> { workData }, WorkCount = 201, WorkOffset = 100 });
            
            var httpClientWrapperPage3 = new Mock<IHttpClientWrapper>();
            httpClientWrapperPage2.Setup(x => x.GetResponse<MusicBrainzArtistWorksCollection>())
                .Returns(new MusicBrainzArtistWorksCollection {Works = new List<Works> { workData }, WorkCount = 201, WorkOffset = 200 });
            
            var httpClientFactory = new Mock<IHttpClientFactoryWrapper>();
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=0"))
                .Returns(httpClientWrapperPage1.Object);
            
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=100"))
                .Returns(httpClientWrapperPage2.Object);
            
            httpClientFactory
                .Setup(x => x.CreateClient(HttpMethod.Get,
                    $"http://musicbrainz.org/ws/2/work?artist={artistId}&limit=100&offset=200"))
                .Returns(httpClientWrapperPage2.Object);
            
            var classUnderTest = new ArtistRepository(httpClientFactory.Object);

            var start = DateTime.Now;
            classUnderTest.GetWorksFor(artistId);
            var end = DateTime.Now.Subtract(start);
            
            Assert.True(end.TotalMilliseconds > 2000, $"Should be at least 2000 ms was {end.TotalMilliseconds}");
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