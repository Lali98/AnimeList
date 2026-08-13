namespace AnimeList.Commands
{
    public class UpdateCommand(UpdateScoreCommand updateScoreCommand)
    {
        private readonly UpdateScoreCommand _updateScoreCommand = updateScoreCommand;

        public async Task ExecuteAsync (string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Meg kell adni, hogy melyik mezőt szeretnéd frissíteni.");
                Console.WriteLine("Használat: \n\tmal-score [--mal-id] <mal_id> / [--year] <year> [--season] <season>");
                Console.WriteLine("\ttitle");
                return;
            }

            switch (args[1])
            {
                case "mal-score":
                    await _updateScoreCommand.UpdateScore(args);
                    break;
                case "title":
                    Console.WriteLine("Title");
                    break;
                default:
                    Console.WriteLine($"Ismeretlen mező: {args[1]}");
                    break;
            }
        }
    }
}
