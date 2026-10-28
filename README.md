# BudgetBook

Persönliche Einnahmen- und Ausgabenverwaltung als ASP.NET Core MVC-Anwendung.
Jede angemeldete Person sieht und bearbeitet ausschließlich die eigenen Buchungen.

Es ist ein Schulprojekt, wodurch es auch minimal geahalten wurde. Das Ziel war es NICHT Production Ready Produkt zu bauen, sondern verschiedene .NET Libraries zu Testen bzw. kennenlernen. 


## Video

https://github.com/user-attachments/assets/de66e1cc-ef80-42ef-94aa-64b3205aecd5



## Technik

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core mit SQLite
- ASP.NET Core Identity

## Starten

```
dotnet restore
dotnet build BudgetBook.slnx
dotnet run --project BudgetBook
```

Die Datenbank `BudgetBook/BudgetBookContext.db` wird beim ersten Start angelegt und
mit Kategorien, zwei Test-Benutzern und Beispielbuchungen befüllt
(siehe `BudgetBook/Models/SeedData.cs`). Die Datei liegt bewusst nicht im Repository.

## Test-Zugangsdaten

> **Hinweis:** Das Seed-Passwort steht im Klartext in `SeedData.cs`. Das ist eine
> bewusste Entscheidung für dieses Prüfungsprojekt: Es handelt sich um öffentliche
> Demo-Konten einer lokalen SQLite-Datenbank ohne echte Daten. In einem produktiven
> Projekt gehört das Passwort in User Secrets oder in eine Umgebungsvariable.

| Benutzer | Passwort |
|---|---|
| usera@example.com | Test123! |
| userb@example.com | Test123! |

## Funktionen

### Buchungen

- Eigene Buchungen anlegen, anzeigen, bearbeiten und löschen
- Löschen erst nach einer Bestätigungsseite
- Felder: Typ, Betrag, Datum, Kategorie, Beschreibung
- Liste nach Datum absteigend sortiert
- Serverseitige Validierung: Betrag größer als 0, Kategorie muss aktiv sein und zum Typ passen

### Statistik

Unter `/Statistics`, ebenfalls nur mit den eigenen Buchungen:

- Kennzahlen: Einnahmen, Ausgaben, Saldo, Top-Ausgabenkategorie
- Ausgaben nach Kategorie mit prozentualem Anteil
- Monatsübersicht mit Einnahmen, Ausgaben und Saldo je Monat
- Filter: Zeitraum von / bis, Typ, Kategorie

### Datenisolation

Alle Abfragen laufen über die `UserId` der angemeldeten Person:

- Buchungen werden nur über `FindOwnTransactionAsync` geladen (`TransactionsController`)
- Die Statistik baut alle Auswertungen auf `BuildOwnTransactionsQuery` auf (`StatisticsController`)

Der Aufruf einer fremden Buchungs-URL liefert `404 Not Found`.

## Datenmodell

```
ApplicationUser (1) ---- (n) Transaction (n) ---- (1) Category
```

Beträge werden als `decimal` gespeichert, nie als `double` oder `float`.
Die beiden Buchungsarten stehen als Konstanten in `Models/TransactionType.cs`.
