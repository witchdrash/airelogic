using System.Collections.Generic;
using LyricInfoApi.Models;
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
            var classUnderTest = new LyricStatisticsService(new Mock<IArtistRepository>().Object, null);

            var result = classUnderTest.CalculateFor("invalid");
            
            Assert.Null(result);
        }

        [Fact]
        public void WhenArtistHasASingleWorksThisIsSearchedForAndTheNumberOfLyricsCounted()
        {
            const string ArtistsId = "Easy Artist Id";
            const string ArtistsName = "Easy Artist Name";
            const string SongName = "Some Song";
            const string Lyrics = "I'm a simple song!";
            
            var works = new List<Works>
            {
                new Works
                {
                    Id = "id",
                    Title = SongName
                }
            };

            var worksCollection = new WorksCollection
            {
                Works = works,
                ArtistName = ArtistsName
            };
                
            
            var artistRepo = new Mock<IArtistRepository>();
            artistRepo.Setup(x => x.GetWorksFor(ArtistsId)).Returns(worksCollection);
            
            var lyricsRepo = new Mock<ILyricsRepository>();
            lyricsRepo.Setup(x => x.GetFor(ArtistsName, SongName)).Returns(Lyrics);
            
            var classUnderTest = new LyricStatisticsService(artistRepo.Object, lyricsRepo.Object);
            var results = classUnderTest.CalculateFor(ArtistsId);
            
            Assert.Equal(4, results.AverageLyricsPerSong);
        }
        
        [Fact]
        public void WhenArtistHasAMultipleWorksThisIsSearchedForAndTheNumberOfLyricsCountedAndAveraged()
        {
            const string ArtistsId = "Easy Artist Id";
            const string ArtistsName = "Easy Artist Name";
            const string SongName = "Some Song";
            const string SongName2 = "Some Song2";
            const string Lyrics = "I'm a simple song!";
            const string Lyrics2 = "I'm another very very simple song!";
            
            var works = new List<Works>
            {
                new Works
                {
                    Id = "id",
                    Title = SongName
                },
                new Works
                {
                    Id = "id2",
                    Title = SongName2
                }
            };

            var worksCollection = new WorksCollection
            {
                Works = works,
                ArtistName = ArtistsName
            };
            
            var artistRepo = new Mock<IArtistRepository>();
            artistRepo.Setup(x => x.GetWorksFor(ArtistsId)).Returns(worksCollection);
            
            var lyricsRepo = new Mock<ILyricsRepository>();
            lyricsRepo.Setup(x => x.GetFor(ArtistsName, SongName)).Returns(Lyrics);
            lyricsRepo.Setup(x => x.GetFor(ArtistsName, SongName2)).Returns(Lyrics2);
            
            var classUnderTest = new LyricStatisticsService(artistRepo.Object, lyricsRepo.Object);
            var results = classUnderTest.CalculateFor(ArtistsId);
            
            Assert.Equal(5, results.AverageLyricsPerSong);
        }
    }
}