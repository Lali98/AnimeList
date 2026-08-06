using AnimeList.Models;

namespace AnimeList.Service
{
    public class AnimeImportService
    {
        private readonly MalApiService _mal;

        public AnimeImportService(MalApiService mal)
        {
            _mal = mal;
        }

        public async Task ImportSeasonAsync (int year, AnimeSeason season)
        {
            Console.WriteLine("Import inditása...");
            await _mal.GetSeasonAsync(year, season);
            Console.WriteLine("Import befejezve.");
        }
    }
}
