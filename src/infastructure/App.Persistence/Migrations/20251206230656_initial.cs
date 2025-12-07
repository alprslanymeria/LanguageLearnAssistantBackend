using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Practices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practices_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PracticeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flashcards_Practices_PracticeId",
                        column: x => x.PracticeId,
                        principalTable: "Practices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Listenings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PracticeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listenings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listenings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listenings_Practices_PracticeId",
                        column: x => x.PracticeId,
                        principalTable: "Practices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Readings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PracticeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Readings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Readings_Practices_PracticeId",
                        column: x => x.PracticeId,
                        principalTable: "Practices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Writings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    PracticeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Writings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Writings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Writings_Practices_PracticeId",
                        column: x => x.PracticeId,
                        principalTable: "Practices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlashcardCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlashcardId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardCategories_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeningCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListeningId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningCategories_Listenings_ListeningId",
                        column: x => x.ListeningId,
                        principalTable: "Listenings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingBooks_Readings_ReadingId",
                        column: x => x.ReadingId,
                        principalTable: "Readings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WritingBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WritingId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeftColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WritingBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WritingBooks_Writings_WritingId",
                        column: x => x.WritingId,
                        principalTable: "Writings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlashcardCategoryId = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckWords_FlashcardCategories_FlashcardCategoryId",
                        column: x => x.FlashcardCategoryId,
                        principalTable: "FlashcardCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardOldSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FlashcardId = table.Column<int>(type: "int", nullable: false),
                    FlashcardCategoryId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardOldSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardOldSessions_FlashcardCategories_FlashcardCategoryId",
                        column: x => x.FlashcardCategoryId,
                        principalTable: "FlashcardCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardOldSessions_Flashcards_FlashcardId",
                        column: x => x.FlashcardId,
                        principalTable: "Flashcards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeckVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListeningCategoryId = table.Column<int>(type: "int", nullable: false),
                    Correct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SourceUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckVideos_ListeningCategories_ListeningCategoryId",
                        column: x => x.ListeningCategoryId,
                        principalTable: "ListeningCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeningOldSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListeningId = table.Column<int>(type: "int", nullable: false),
                    ListeningCategoryId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningOldSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningOldSessions_ListeningCategories_ListeningCategoryId",
                        column: x => x.ListeningCategoryId,
                        principalTable: "ListeningCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListeningOldSessions_Listenings_ListeningId",
                        column: x => x.ListeningId,
                        principalTable: "Listenings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReadingOldSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReadingId = table.Column<int>(type: "int", nullable: false),
                    ReadingBookId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingOldSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingOldSessions_ReadingBooks_ReadingBookId",
                        column: x => x.ReadingBookId,
                        principalTable: "ReadingBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingOldSessions_Readings_ReadingId",
                        column: x => x.ReadingId,
                        principalTable: "Readings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WritingOldSessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WritingId = table.Column<int>(type: "int", nullable: false),
                    WritingBookId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WritingOldSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WritingOldSessions_WritingBooks_WritingBookId",
                        column: x => x.WritingBookId,
                        principalTable: "WritingBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WritingOldSessions_Writings_WritingId",
                        column: x => x.WritingId,
                        principalTable: "Writings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FlashcardSessionRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlashcardOldSessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardSessionRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardSessionRows_FlashcardOldSessions_FlashcardOldSessionId",
                        column: x => x.FlashcardOldSessionId,
                        principalTable: "FlashcardOldSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListeningSessionRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListeningOldSessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListenedSentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Similarity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningSessionRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListeningSessionRows_ListeningOldSessions_ListeningOldSessionId",
                        column: x => x.ListeningOldSessionId,
                        principalTable: "ListeningOldSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingSessionRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadingOldSessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedSentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerTranslate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Similarity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingSessionRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadingSessionRows_ReadingOldSessions_ReadingOldSessionId",
                        column: x => x.ReadingOldSessionId,
                        principalTable: "ReadingOldSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WritingSessionRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WritingOldSessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedSentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnswerTranslate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Similarity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WritingSessionRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WritingSessionRows_WritingOldSessions_WritingOldSessionId",
                        column: x => x.WritingOldSessionId,
                        principalTable: "WritingOldSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckVideos_ListeningCategoryId",
                table: "DeckVideos",
                column: "ListeningCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckWords_FlashcardCategoryId",
                table: "DeckWords",
                column: "FlashcardCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardCategories_FlashcardId",
                table: "FlashcardCategories",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardOldSessions_FlashcardCategoryId",
                table: "FlashcardOldSessions",
                column: "FlashcardCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardOldSessions_FlashcardId",
                table: "FlashcardOldSessions",
                column: "FlashcardId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_LanguageId",
                table: "Flashcards",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_PracticeId",
                table: "Flashcards",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSessionRows_FlashcardOldSessionId",
                table: "FlashcardSessionRows",
                column: "FlashcardOldSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningCategories_ListeningId",
                table: "ListeningCategories",
                column: "ListeningId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningOldSessions_ListeningCategoryId",
                table: "ListeningOldSessions",
                column: "ListeningCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningOldSessions_ListeningId",
                table: "ListeningOldSessions",
                column: "ListeningId");

            migrationBuilder.CreateIndex(
                name: "IX_Listenings_LanguageId",
                table: "Listenings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Listenings_PracticeId",
                table: "Listenings",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_ListeningSessionRows_ListeningOldSessionId",
                table: "ListeningSessionRows",
                column: "ListeningOldSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Practices_LanguageId",
                table: "Practices",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingBooks_ReadingId",
                table: "ReadingBooks",
                column: "ReadingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingOldSessions_ReadingBookId",
                table: "ReadingOldSessions",
                column: "ReadingBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingOldSessions_ReadingId",
                table: "ReadingOldSessions",
                column: "ReadingId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_LanguageId",
                table: "Readings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Readings_PracticeId",
                table: "Readings",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingSessionRows_ReadingOldSessionId",
                table: "ReadingSessionRows",
                column: "ReadingOldSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WritingBooks_WritingId",
                table: "WritingBooks",
                column: "WritingId");

            migrationBuilder.CreateIndex(
                name: "IX_WritingOldSessions_WritingBookId",
                table: "WritingOldSessions",
                column: "WritingBookId");

            migrationBuilder.CreateIndex(
                name: "IX_WritingOldSessions_WritingId",
                table: "WritingOldSessions",
                column: "WritingId");

            migrationBuilder.CreateIndex(
                name: "IX_Writings_LanguageId",
                table: "Writings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Writings_PracticeId",
                table: "Writings",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_WritingSessionRows_WritingOldSessionId",
                table: "WritingSessionRows",
                column: "WritingOldSessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckVideos");

            migrationBuilder.DropTable(
                name: "DeckWords");

            migrationBuilder.DropTable(
                name: "FlashcardSessionRows");

            migrationBuilder.DropTable(
                name: "ListeningSessionRows");

            migrationBuilder.DropTable(
                name: "ReadingSessionRows");

            migrationBuilder.DropTable(
                name: "WritingSessionRows");

            migrationBuilder.DropTable(
                name: "FlashcardOldSessions");

            migrationBuilder.DropTable(
                name: "ListeningOldSessions");

            migrationBuilder.DropTable(
                name: "ReadingOldSessions");

            migrationBuilder.DropTable(
                name: "WritingOldSessions");

            migrationBuilder.DropTable(
                name: "FlashcardCategories");

            migrationBuilder.DropTable(
                name: "ListeningCategories");

            migrationBuilder.DropTable(
                name: "ReadingBooks");

            migrationBuilder.DropTable(
                name: "WritingBooks");

            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "Listenings");

            migrationBuilder.DropTable(
                name: "Readings");

            migrationBuilder.DropTable(
                name: "Writings");

            migrationBuilder.DropTable(
                name: "Practices");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
