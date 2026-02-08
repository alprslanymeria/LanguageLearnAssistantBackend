using App.Application.Contracts.Infrastructure.Files;

namespace App.Application.Features.WritingBooks.Dtos;

// THESE TYPES ARE NOT PRESENT IN PURE NEXT NEXTJS BECAUSE THERE, DATA IS RETRIEVED DIRECTLY FROM FORM DATA.
// WE COULD DO THE SAME WITH IFORM COLLECTION HERE. THE PROBLEM IS THAT IFORM COLLECTION IS A WEB-SPECIFIC STRUCTURE.
// WHEN WE TRY TO DEFINE THIS IN CLASS LIBRARIES, WE WOULD NEED TO INSTALL EXTRA DEPENDENCIES.
// THIS CAUSES PROBLEMS IN TERMS OF ARCHITECTURE. THAT'S WHY WE DEFINED EXTRA TYPES FOR THESE STRUCTURES:
// DeckWords
// FlashcardCategories
// ReadingBooks
// WritingBooks

// THE SAME APPLIES TO THE USER SIDE AS WELL, THIS HAS ALREADY BEEN COMPLETED ON THE OAUTH SERVER SIDE..

public record CreateWritingBookRequest(

    string UserId,
    int LanguageId,
    string BookName,
    string Practice,
    IFileUpload ImageFile,
    IFileUpload SourceFile
    );

public record UpdateWritingBookRequest(

    int ItemId,
    string UserId,
    int LanguageId,
    string BookName,
    string Practice,
    IFileUpload ImageFile,
    IFileUpload SourceFile
    );
