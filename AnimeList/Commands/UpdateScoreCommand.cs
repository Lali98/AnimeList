using AnimeList.Data;
using AnimeList.Models;
using AnimeList.Service;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Commands
{
    public class UpdateScoreCommand(AppDbContext db, MalApiService mal)
    {
        private readonly AppDbContext _db = db;
        private readonly MalApiService _mal = mal;

        public async Task UpdateScore(string[] args)
        {
            int? malId = null;
            int? year = null;
            AnimeSeason? season = null;

            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--mal-id":
                        if (i + 1 >= args.Length)
                        {
                            Console.WriteLine("A --mal-id paraméterhez meg kell adni egy MAL ID-t.");
                            return;
                        }
                        if (!int.TryParse(args[++i], out var parsedMalId))
                        {
                            Console.WriteLine("A --mal-id értéke érvénytelen.");
                            return;
                        }

                        malId = parsedMalId;
                        break;
                    case "--year":
                        if (i + 1 >= args.Length)
                        {
                            Console.WriteLine("A --year paraméterhez meg kell adni egy évet.");
                            return;
                        }
                        if (!int.TryParse(args[++i], out var parsedYear))
                        {
                            Console.WriteLine("A --year értéke érvénytelen.");
                            return;
                        }

                        year = parsedYear;
                        break;
                    case "--season":
                        if (i + 1 >= args.Length)
                        {
                            Console.WriteLine("A --season paraméterhez meg kell adni egy évszakot.");
                            return;
                        }
                        if (!Enum.TryParse<AnimeSeason>(args[++i], true, out var parsedSeason))
                        {
                            Console.WriteLine("Érvénytelen season. Használható: Winter, Spring, Summer, Fall.");
                            return;
                        }

                        season = parsedSeason;
                        break;
                    default:
                        Console.WriteLine($"Ismeretlen argumentum: {args[i]}");
                        return;
                }
            }

            if (malId.HasValue && (year.HasValue || season.HasValue))
            {
                Console.WriteLine("A --mal-id nem használható együtt a --year/--season paraméterekkel.");
                return;
            }

            if ((year.HasValue && !season.HasValue) || (!year.HasValue && season.HasValue))
            {
                Console.WriteLine("A --year és --season paramétereket együtt kell használni.");
                return;
            }

            if (!malId.HasValue && !year.HasValue && !season.HasValue)
            {
                Console.WriteLine("Meg kell adni egy keresési feltételt: --mal-id vagy --year + --season.");
                return;
            }

            var query = _db.Animes.AsQueryable();

            if (malId.HasValue)
            {
                query = query.Where(a =>
                    a.MalId == malId.Value);
            }
            else if (year.HasValue && season.HasValue)
            {
                query = query.Where(a =>
                    a.Year == year.Value &&
                    a.Season == season.Value);
            }

            var animes = await query.ToListAsync();

            if (animes.Count == 0)
            {
                Console.WriteLine("Nincs találat a megadott paraméterekkel.");
                return;
            }

            Console.WriteLine($"{animes.Count} anime frissítése...");

            bool hasChanges = false;

            foreach (var anime in animes)
            {
                try
                {
                    await Task.Delay(1500);
                    var malAnime = await _mal.GetAnimeByIdAsync(anime.MalId);

                    var oldScore = anime.MalScore;
                    var newScore = malAnime.Mean;

                    if (oldScore == newScore)
                    {
                        Console.WriteLine($"= {anime.Titles.JpRomaji}: nincs változás ({oldScore})");
                        continue;
                    }

                    anime.MalScore = newScore;
                    hasChanges = true;

                    Console.WriteLine($"~ {anime.Titles.JpRomaji}: {oldScore} -> {newScore}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"! Hiba ({anime.MalId}): {ex.Message}");
                }
            }

            if (hasChanges)
            {
                await _db.SaveChangesAsync();
                Console.WriteLine("Frissítés befejezve.");
            }
            else
            {
                Console.WriteLine("Nem volt szükség adatbázis-frissítésre.");
            }
        }
    }
}