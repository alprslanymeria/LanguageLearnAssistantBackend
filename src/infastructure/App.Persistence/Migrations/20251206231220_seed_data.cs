using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seed_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO Languages (Name, ImageUrl) VALUES
                ('english', '/images/flags/uk.png'),
                ('turkish', '/images/flags/tr.png'),
                ('german', '/images/flags/de.png'),
                ('russian', '/images/flags/rs.png');

                INSERT INTO Practices (LanguageId, Name) VALUES
                (1, 'flashcard'),
                (1, 'reading'),
                (1, 'writing'),
                (1, 'listening'),
                (2, 'flashcard'),
                (2, 'reading'),
                (2, 'writing'),
                (2, 'listening'),
                (3, 'flashcard'),
                (3, 'reading'),
                (3, 'writing'),
                (3, 'listening'),
                (4, 'flashcard'),
                (4, 'reading'),
                (4, 'writing'),
                (4, 'listening');
                "
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Practices WHERE LanguageId IN (1,2,3,4);
                DELETE FROM Languages WHERE Id IN (1,2,3,4);
                "
            );
        }
    }
}
