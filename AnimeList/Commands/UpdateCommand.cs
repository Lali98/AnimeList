using AnimeList.Service;

namespace AnimeList.Commands
{
    public class UpdateCommand(UpdateAnimeService updateAnimeService)
    {
        private readonly UpdateAnimeService _updateAnimeService = updateAnimeService;

        public async Task ExecuteAsync (string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Meg kell adni, hogy melyik mezőt szeretnéd frissíteni.");
                Console.WriteLine("Használat: \n\tmal-score/image/episode [--mal-id] <mal_id> / [--year] <year> [--season] <season>");
                return;
            }

            switch (args[1].ToLower())
            {
                case "mal-score":
                case "image":
                case "episode":
                    await _updateAnimeService.Update(args, args[1].ToLower());
                    break;
                default:
                    Console.WriteLine($"Ismeretlen mező: {args[1].ToLower()}");
                    break;
            }
        }
    }
}
