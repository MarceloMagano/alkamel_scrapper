namespace Alkamel_Scrapper;

public static class Alkamel
{
    // /Alkamel data
    //
    // Seasons
    // season 12 = 2023
    // season 13 = 2024
    //
    // Events
    // event 01 = Losail
    // event 02 = Imola
    // event 03 = Spa
    // ... fill in the rest after 

    private static int SeasonNumber(int year) => year < 2022 ? DateTime.UtcNow.Year - 2000 - 11 : year - 2000 - 11;
    public static string Season(int year) => $"{SeasonNumber(year)}_{year}";

    public static string Event(string eventNumber) => $"{eventNumber}_SPA+FRANCORCHAMPS"; // todo: fill in correct event names
}
