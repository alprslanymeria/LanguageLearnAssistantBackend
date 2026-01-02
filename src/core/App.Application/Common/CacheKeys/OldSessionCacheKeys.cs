namespace App.Application.Common.CacheKeys;

/// <summary>
/// CACHE KEY DEFINITIONS FOR OLD SESSIONS.
/// </summary>
public static class OldSessionCacheKeys
{
    public static string ReadingPrefix => "readingOldSession";
    public static string WritingPrefix => "writingOldSession";
    public static string FlashcardPrefix => "flashcardOldSession";
    public static string ListeningPrefix => "listeningOldSession";

    public static string GetSessionsKey(string prefix, string userId) => $"{prefix}.user.{userId}";
}
