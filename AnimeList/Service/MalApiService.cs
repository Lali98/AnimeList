using AnimeList.Models;

namespace AnimeList.Service
{
    public class MalApiService
    {
        public async Task GetSeasonAsync(int year, AnimeSeason season)
        {
            Console.WriteLine($"Lekérdezés: {year} {season}");

            await Task.CompletedTask;
        }
    }
}
