using AnimeList.Dtos.MyAnimeList;
using AnimeList.Models;
using System.Text.Json;

namespace AnimeList.Service
{
    public class MalApiService
    {
        private readonly HttpClient _http;

        public MalApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<MALSeasonResponseDto> GetSeasonAsync(int year, AnimeSeason season)
        {
            var seasonName = season.ToString().ToLower();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true,
            };

            var result = await _http.GetFromJsonAsync<MALSeasonResponseDto>($"anime/season/{year}/{seasonName}?limit=500&fields=id,title,main_picture,synopsis,genres,studios,alternative_titles,media_type,start_season,num_episodes,average_episode_duration,mean", options);

            if (result is null)
                throw new Exception("A MyAnimeList API nem adott vissza adatot.");

            return result;
        }

        public async Task<MALAnimeDto> GetAnimeByIdAsync(int id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true,
            };
            var result = await _http.GetFromJsonAsync<MALAnimeDto>($"anime/{id}?fields=id,title,main_picture,synopsis,genres,studios,alternative_titles,media_type,start_season,num_episodes,average_episode_duration,mean", options);
            if (result is null)
                throw new Exception("A MyAnimeList API nem adott vissza adatot.");
            return result;
        }
    }
}
