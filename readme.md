# Code-First C#

### Add a migrations script

`dotnet ef migrations add <name for migration> -o <path>`
Skapar en migreringsfil med kommandon som ska köras mot databasen. OBSERVERA ska köras! Ingenting har skett emot databasen ännu.

flaggan -o betyder att vi kan bestämma vart vi vill ha skripten skapade och lagrade någonstans. Observera att sökvägen är relativ till vart vi står och kör kommandot.

Om vi behöver justera något kan vi ta bort aktuell migrering med
`dotnet ef migrations remove`

### Run migrations

`dotnet ef database update`
Ovanstående kommando kör nu migreringsskriptet mot databasen och utför kommandona i migrereringsskripten.
