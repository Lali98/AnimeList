# AnimeList

![build-badge](https://img.shields.io/badge/build-passing-brightgreen) ![dotnet-10](https://img.shields.io/badge/.NET-10-blue)

Kis .NET 10 web/CLI alkalmazás anime-adatok kezelésére és importálására. Az adatbázist EF Core (Npgsql) kezeli, a projekt támogatja a MyAnimeList integrációt és egyszerű CLI parancsokat importáláshoz/frissítéshez.

Tartalomjegyzék
-----------------
- [Főbb komponensek](#főbb-komponensek)
- [Követelmények](#követelmények)
- [Konfiguráció](#konfiguráció)
- [Gyors indítás](#gyors-indítás)
- [CLI parancsok](#cli-parancsok)
- [API végpontok](#api-végpontok)
- [Migrációk](#migrációk)

Főbb komponensek
-----------------

- AppDbContext (AnimeList/Data): EF Core DbContext és konfigurációk.
- Models (AnimeList/Models): Anime, Genre, Studio, FanSub, BaseEntity.
- Services (AnimeList/Service): üzleti logika, import és MyAnimeList API kliens.
- Commands (AnimeList/Commands): CLI parancsok (import, update, update-score stb.).
- Controllers (AnimeList/Controllers): Web API végpontok.
- Migrations: EF Core migrációk.

Követelmények
------------

- .NET 10 SDK
- PostgreSQL (alapértelmezett: Npgsql)
- (Opcionális) MyAnimeList Client ID az API integrációhoz

Konfiguráció
-------------

Állítsd be az adatbázis-kapcsolatot és a MyAnimeList kliens azonosítót az `appsettings.json`-ban vagy környezeti változóként.

Példa appsettings.json részlet:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Host=localhost;Database=animelist;Username=youruser;Password=yourpass"
  },
  "MyAnimeList": {
	"ClientId": "<your-mal-client-id>"
  }
}
```

Gyors indítás
------------

Web API módban:

1. dotnet restore
2. dotnet build
3. dotnet run

CLI mód (példa import):

```bash
dotnet run -- import 2024 Summer
```

CLI parancsok
-------------

- Import: import <year> <season>
  - Példa: ``dotnet run -- import 2024 Summer``
- Update: update <field> [options]
  - Példa MAL-pontszám frissítése egy adott MAL ID-val:
	- ``dotnet run -- update mal-score --mal-id 12345``
  - Példa év/évszak alapján:
	- ``dotnet run -- update mal-score --year 2024 --season Summer``

API végpontok
-------------

- GET /api/anime/season/2026/summer — lista az adott év/évszak animeiről.
- GET /api/anime/id/1 — animét ad vissza adatbázis ID alapján.
- GET /api/anime/mal/62883 — animét ad vissza MyAnimeList ID alapján.

Migrációk
---------

A migrációk az `Migrations` mappában találhatók. Alkalmazás:

```bash
dotnet ef database update
```