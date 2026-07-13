# AceGuessr — Daily Tennis Guessing Game

A Wordle-style daily guessing game for ATP tennis players. Guess the mystery
player in 8 tries; after each guess, get color-coded feedback across 7
attributes (playing hand, backhand type, country, Grand Slam titles,
career-high ranking, turned-pro year, career titles) with directional
arrows on numeric clues.

## Status

The .NET 10 upgrade and the AI trivia feature (short blurb about the
mystery player shown on game over) are both implemented and merged.
`dotnet build` (backend) and `npm run build` (frontend) have been verified
to pass cleanly.

## Features

- **Daily puzzle** — one shared mystery player per day, no repeats for 14 days
- **Practice modes** — Easy / Medium / Hard, unlimited plays, difficulty
  scoped to different eras of the player pool (current stars → 2005-2015
  golden era → pre-2005 legends)
- **Shareable results** — Wordle-style emoji grid via the Web Share API /
  clipboard, no social media API integration required
- **Generic, sport-agnostic schema** — tennis is fully playable standalone
  today; the data model (Sport → Player → Attribute → PlayerAttributeValue)
  is designed so additional sports could be added later as data, not code

## Tech Stack

| Layer      | Technology                                  |
|------------|-----------------------------------------------|
| Frontend   | React 18, TypeScript, Vite, Tailwind CSS      |
| Backend    | C#, ASP.NET Core 10 Web API, Entity Framework Core |
| Database   | PostgreSQL                                    |
| Deployment | Docker & Docker Compose                       |

## Architecture

```
tennis-guessr/
├── backend/TennisGuessr.Api/
│   ├── Models/          # Sport, Player, AttributeDefinition, PlayerAttributeValue, DailyPuzzle
│   ├── Data/            # GameDbContext, DataSeeder (20-player ATP sample dataset)
│   ├── Services/        # GameService — comparison logic, daily puzzle selection, practice sessions
│   ├── Controllers/     # PlayersController, GameController
│   └── Dtos/            # Request/response contracts
└── frontend/src/
    ├── api/             # Axios client
    ├── components/      # PlayerSearch, ClueGrid, ModeSelector, ShareResult
    ├── pages/           # GameBoard (main game screen)
    └── types/
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Node.js 18+
- PostgreSQL (via Docker or a local install)

### 1. Start PostgreSQL

Easiest via Docker:
```bash
docker compose up -d postgres
```
Or point `ConnectionStrings:DefaultConnection` in `appsettings.json` at your
own local Postgres instance.

### 2. Set up the database (EF Core migrations)

This project does not yet include a `Migrations/` folder — generate it locally:
```bash
cd backend/TennisGuessr.Api
dotnet tool install --global dotnet-ef   # if you don't have it already
dotnet ef migrations add InitialCreate
dotnet ef database update
```
The app also calls `db.Database.Migrate()` on startup, so once migrations
exist, the schema stays in sync automatically. The 20-player sample dataset
seeds itself automatically on first run (see `Data/DataSeeder.cs`).

### 3. (Optional) Enable AI trivia blurbs

On game over, the app can show a short AI-generated trivia blurb about the
mystery player. This is optional — without a key configured, the game works
exactly the same, just without the blurb. Set your Anthropic API key via
`dotnet user-secrets` rather than committing it to `appsettings.json`:
```bash
cd backend/TennisGuessr.Api
dotnet user-secrets init
dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-..."
```

### 4. Run the backend
```bash
dotnet run
```
API will be available at `http://localhost:5080`, with interactive OpenAPI
docs (via Scalar) at `http://localhost:5080/scalar/v1`.

### 5. Run the frontend
```bash
cd frontend
npm install
npm run dev
```
App will be available at `http://localhost:5174`.

## API Endpoints

- `GET /api/sports/tennis/players` — full player pool (for guess autocomplete)
- `POST /api/sports/tennis/game/start?difficulty=easy|medium|hard` — start a practice game, returns a `sessionId`
- `POST /api/sports/tennis/game/guess?guessNumber=N` — submit a guess, returns per-attribute clue feedback

## Notes on the dataset

The current dataset is a **20-player sample** (mix of current stars,
2005–2015 era, and pre-2005 legends) meant to prove the game end-to-end.
The full design targets 200 players (~110 current / ~60 2005–2015 / ~30
legends, career-high ranking ≤125) — expanding `DataSeeder.cs` with more
entries is the main remaining step to reach that.

Player stats (Grand Slam counts, titles, etc.) are accurate as of mid-2026
but will drift for active players over time — treat this as a dataset that
needs periodic manual refreshes, not a live feed.
