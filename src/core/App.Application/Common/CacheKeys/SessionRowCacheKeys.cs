namespace App.Application.Common.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR SESSION ROWS.
/// </summary>
public static class SessionRowCacheKeys
{
    public static string ReadingPrefix => "readingSessionRow";
    public static string WritingPrefix => "writingSessionRow";
    public static string FlashcardPrefix => "flashcardSessionRow";
    public static string ListeningPrefix => "listeningSessionRow";

    public static string GetRowsBySessionKey(string prefix, string sessionId) => $"{prefix}.session.{sessionId}";
}
