using AnimeList.Data;
using AnimeList.Models;
using AnimeList.Service;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Commands
{
    public class UpdateScoreCommand
    {
        private readonly AppDbContext _db;
        private readonly MalApiService _mal;

        public UpdateScoreCommand(
            AppDbContext db,
            MalApiService mal)
        {
            _db = db;
            _mal = mal;
        }

        public async Task ExecuteAsync(string[] args)
        {
            int? malId = null;
            int? year = null;
            AnimeSeason? season = null;

            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--mal-id":
                        malId = int.Parse(args[++i]);
                        break;

                    case "--year":
                        year = int.Parse(args[++i]);
                        break;

                    case "--season":
                        if (!Enum.TryParse<AnimeSeason>(
                            args[++i],
                            true,
                            out var parsedSeason))
                        {
                            Console.WriteLine("Érvénytelen season.");
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

            if ((year.HasValue && !season.HasValue) ||
                (!year.HasValue && season.HasValue))
            {
                Console.WriteLine("A --year és --season paramétereket együtt kell használni.");

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

            foreach (var anime in animes)
            {
                try
                {
                    var malAnime =
                        await _mal.GetAnimeByIdAsync(anime.MalId);

                    var oldScore = anime.MalScore;

                    anime.MalScore = malAnime.Mean;

                    Console.WriteLine($"~ {anime.Titles.JpRomaji}: {oldScore} -> {anime.MalScore}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"! Hiba ({anime.MalId}): {ex.Message}");
                }
            }

            await _db.SaveChangesAsync();

            Console.WriteLine("Frissítés befejezve.");
        }
    }
}