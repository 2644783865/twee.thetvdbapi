﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace twee.thetvdbapi.test
{
    public class TvTheDbClientTest
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly string _token;
        private readonly TheTvDbClient _tvDbClient;
        public TvTheDbClientTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;

            var builder = new ConfigurationBuilder()
                             .AddEnvironmentVariables()
                    .AddUserSecrets("aspnet-twee-thetvdb-api-asdasdasd-shr4e63-asdad-9b77-235245212");


            var configuration = builder.Build();

            var apiKey = configuration["ApiKey"];

            _tvDbClient = new TheTvDbClient();

            var response = Task.Run(() => _tvDbClient.Authentication.Login(apiKey)).Result;
            _token = response.Token;

        }

        [Theory]
        [InlineData(281662, "Marvel's Daredevil")]
        [InlineData(114701,"The League")]
        [InlineData(138531, "Solsidan")]
        public async Task CanAuthWithTvDbApiAndGetSerie(int serieid,string seriename)
        {

            var serie = await _tvDbClient.Series.GetById(serieid, _token);

            Assert.Equal(seriename, serie.Data.SeriesName);
        }

        [Theory]
        [InlineData(281662)]
        [InlineData(114701)]
        [InlineData(138531)]
        public async Task CanAuthWithTvDbApiAndGetEpisodes(int serieid)
        {

            var episodes = await _tvDbClient.Series.GetEpisodesBySerieId(serieid, _token);

            Assert.True(episodes.Data.Any());
        }

        [Theory]
        [InlineData(281662)]
        [InlineData(114701)]
        [InlineData(138531)]
        public async Task CanAuthWithTvDbApiAndGetActors(int serieid)
        {

            var episodes = await _tvDbClient.Series.GetActorsBySerieId(serieid, _token);

            Assert.True(episodes.Data.Any());
        }

        [Theory]
        [InlineData(281662)]
        [InlineData(114701)]
        [InlineData(138531)]
        public async Task CanAuthWithTvDbApiAndGetImageSummary(int serieid)
        {


            var episodes = await _tvDbClient.Series.GetImageSummaryBySerieId(serieid, _token);

            Assert.NotEqual(0,episodes.Data.Poster);
        }

        [Fact]
        public async Task CanAuthWithTvDbApiAndGetImageQueryParams()
        {
            var serieId = 281662;

            var imageParams = await _tvDbClient.Series.GetImageQueryParams(serieId, _token);

            Assert.True(imageParams.Data.Any());
        }

        [Fact]
        public async Task CanAuthWithTvDbApiAndGetImageQuery()
        {
            var serieId = 281662;

            var images = await _tvDbClient.Series.GetImageQuery(serieId,"poster", _token);

            Assert.True(images.Data.Any());
        }
    }
}
