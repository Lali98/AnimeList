using AnimeList.Models;
using AnimeList.Service;

namespace AnimeList.Commands
{
    public class ImportCommand
    {
        private readonly AnimeImportService _importService;

        public ImportCommand(AnimeImportService importService)
        {
            _importService = importService;
        }

        public async Task ExecuteAsync(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Használat: \n\tdotnet run -- import <year> <season>");
                return;
            }
            
            if (!int.TryParse(args[1], out var year))
            {
                Console.WriteLine("Hiba: A megadott év nem érvényes szám.");
                return;
            }

            if (!Enum.TryParse<AnimeSeason>(args[2], true, out var season))
            {
                Console.WriteLine("Hiba: A megadott évszak nem érvényes. \nHasználható értékek: Winter, Spring, Summer, Fall.");
                return;
            }

            await _importService.ImportSeasonAsync(year, season);
        }
    }
}
