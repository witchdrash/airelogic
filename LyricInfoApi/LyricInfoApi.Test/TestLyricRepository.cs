using System.Net.Http;
using LyricInfoApi.Models;
using LyricInfoApi.Repositories;
using LyricInfoApi.Wrappers;
using Moq;
using Xunit;

namespace LyricInfoApi.Test
{
    public class TestLyricRepository
    {
        [Fact]
        public void WhenSearchingForASongArtistCombinationThatDoesntExistReturnsNull()
        {
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            httpClientWrapper.Setup(x => x.GetResponse<LyricsApiResponse>()).Returns(default(LyricsApiResponse));
            
            var clientFactoryWrapper = new Mock<IHttpClientFactoryWrapper>();
            clientFactoryWrapper.Setup(x => x.CreateClient(HttpMethod.Get, $"https://api.lyrics.ovh/v1/invalid_artist/invalid_song"))
                .Returns(httpClientWrapper.Object);
            
            var classUnderTest = new LyricsRepository(clientFactoryWrapper.Object);
            var results = classUnderTest.GetFor("invalid_artist", "invalid_song");
            
            Assert.Null(results);
        }

        [Fact]
        public void WhenSeachingForAValidArtistSongCombinationTheLyricsAreReturned()
        {
            const string lyrics = "I'm some lyrics";
            const string Artist = "artist name";
            const string SongName = "a song name goes here";
            
            var httpClientWrapper = new Mock<IHttpClientWrapper>();
            httpClientWrapper.Setup(x => x.GetResponse<LyricsApiResponse>()).Returns(new LyricsApiResponse { Lyrics = lyrics});
            
            var clientFactoryWrapper = new Mock<IHttpClientFactoryWrapper>();
            clientFactoryWrapper.Setup(x => x.CreateClient(HttpMethod.Get, $"https://api.lyrics.ovh/v1/{Artist}/{SongName}"))
                .Returns(httpClientWrapper.Object);
            
            var classUnderTest = new LyricsRepository(clientFactoryWrapper.Object);
            var results = classUnderTest.GetFor(Artist, SongName);
            
            Assert.Equal(lyrics, results);
        }
    }
}