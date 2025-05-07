using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.API.Migrations
{
  /// <inheritdoc />
  public partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "Award",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Award", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "Character",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Character", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "Genre",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Genre", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "Publisher",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Publisher", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "Setting",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Setting", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "User",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
          password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
          username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
          name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
          profile_img = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
          created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__User", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "Book",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          publisher_id = table.Column<int>(type: "int", nullable: true),
          book_id = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
          title = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
          series = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
          description = table.Column<string>(type: "nvarchar(max)", nullable: true),
          language = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
          isbn = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
          book_format = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
          edition = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
          pages = table.Column<int>(type: "int", nullable: true),
          publish_date = table.Column<DateOnly>(type: "date", nullable: true),
          cover_img = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Book", x => x.id);
          table.ForeignKey(
            name: "FK_Book_Publisher",
            column: x => x.publisher_id,
            principalTable: "Publisher",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Token",
        columns: table => new
        {
          id = table.Column<int>(type: "int", nullable: false),
          refresh_token = table.Column<string>(
            type: "nvarchar(60)",
            maxLength: 60,
            nullable: false
          ),
          expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Token", x => x.id);
          table.ForeignKey(
            name: "FK__Token__User",
            column: x => x.id,
            principalTable: "User",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Book_Award",
        columns: table => new
        {
          book_id = table.Column<int>(type: "int", nullable: false),
          award_id = table.Column<int>(type: "int", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Book_Award", x => new { x.book_id, x.award_id });
          table.ForeignKey(
            name: "FK__Book_Award__Award",
            column: x => x.award_id,
            principalTable: "Award",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Book_Award__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Book_Character",
        columns: table => new
        {
          book_id = table.Column<int>(type: "int", nullable: false),
          character_id = table.Column<int>(type: "int", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Book_Character", x => new { x.book_id, x.character_id });
          table.ForeignKey(
            name: "FK__Book_Character__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Book_Character__Character",
            column: x => x.character_id,
            principalTable: "Character",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Book_Genre",
        columns: table => new
        {
          genre_id = table.Column<int>(type: "int", nullable: false),
          book_id = table.Column<int>(type: "int", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Book_Genre", x => new { x.genre_id, x.book_id });
          table.ForeignKey(
            name: "FK__Book_Genre__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Book_Genre__Genre",
            column: x => x.genre_id,
            principalTable: "Genre",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Book_Setting",
        columns: table => new
        {
          book_id = table.Column<int>(type: "int", nullable: false),
          setting_id = table.Column<int>(type: "int", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Book_Setting", x => new { x.book_id, x.setting_id });
          table.ForeignKey(
            name: "FK__Book_Setting__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Book_Settings__Setting",
            column: x => x.setting_id,
            principalTable: "Setting",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Rating",
        columns: table => new
        {
          book_id = table.Column<int>(type: "int", nullable: false),
          star_1 = table.Column<int>(type: "int", nullable: true),
          star_2 = table.Column<int>(type: "int", nullable: true),
          star_3 = table.Column<int>(type: "int", nullable: true),
          star_4 = table.Column<int>(type: "int", nullable: true),
          star_5 = table.Column<int>(type: "int", nullable: true),
          stars_average = table.Column<double>(type: "float", nullable: true),
          stars_total = table.Column<int>(type: "int", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Rating", x => x.book_id);
          table.ForeignKey(
            name: "FK__Rating__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Readlist",
        columns: table => new
        {
          user_id = table.Column<int>(type: "int", nullable: false),
          book_id = table.Column<int>(type: "int", nullable: false),
          created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Readlist", x => new { x.book_id, x.user_id });
          table.ForeignKey(
            name: "FK__Readlist__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Readlist__User",
            column: x => x.user_id,
            principalTable: "User",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Review",
        columns: table => new
        {
          id = table
            .Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
          user_id = table.Column<int>(type: "int", nullable: false),
          book_id = table.Column<int>(type: "int", nullable: false),
          text = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
          created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Review", x => x.id);
          table.ForeignKey(
            name: "FK__Review__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Review__User",
            column: x => x.user_id,
            principalTable: "User",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "UserRating",
        columns: table => new
        {
          user_id = table.Column<int>(type: "int", nullable: false),
          book_id = table.Column<int>(type: "int", nullable: false),
          rating = table.Column<int>(type: "int", nullable: false),
          created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__UserRating", x => new { x.user_id, x.book_id });
          table.ForeignKey(
            name: "FK__UserRating__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__UserRating__User",
            column: x => x.user_id,
            principalTable: "User",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "Wishlist",
        columns: table => new
        {
          user_id = table.Column<int>(type: "int", nullable: false),
          book_id = table.Column<int>(type: "int", nullable: false),
          created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
        },
        constraints: table =>
        {
          table.PrimaryKey("PK__Wishlist", x => new { x.user_id, x.book_id });
          table.ForeignKey(
            name: "FK__Wishlist__Book",
            column: x => x.book_id,
            principalTable: "Book",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "FK__Wishlist__User",
            column: x => x.user_id,
            principalTable: "User",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateIndex(
        name: "IX_Book_publisher_id",
        table: "Book",
        column: "publisher_id"
      );

      migrationBuilder.CreateIndex(
        name: "IX_Book_Award_award_id",
        table: "Book_Award",
        column: "award_id"
      );

      migrationBuilder.CreateIndex(
        name: "IX_Book_Character_character_id",
        table: "Book_Character",
        column: "character_id"
      );

      migrationBuilder.CreateIndex(
        name: "IX_Book_Genre_book_id",
        table: "Book_Genre",
        column: "book_id"
      );

      migrationBuilder.CreateIndex(
        name: "IX_Book_Setting_setting_id",
        table: "Book_Setting",
        column: "setting_id"
      );

      migrationBuilder.CreateIndex(
        name: "IX_Readlist_user_id",
        table: "Readlist",
        column: "user_id"
      );

      migrationBuilder.CreateIndex(name: "IX_Review_book_id", table: "Review", column: "book_id");

      migrationBuilder.CreateIndex(name: "IX_Review_user_id", table: "Review", column: "user_id");

      migrationBuilder.CreateIndex(
        name: "IX_UserRating_book_id",
        table: "UserRating",
        column: "book_id"
      );

      migrationBuilder.CreateIndex(
        name: "IX_Wishlist_book_id",
        table: "Wishlist",
        column: "book_id"
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "Book_Award");

      migrationBuilder.DropTable(name: "Book_Character");

      migrationBuilder.DropTable(name: "Book_Genre");

      migrationBuilder.DropTable(name: "Book_Setting");

      migrationBuilder.DropTable(name: "Rating");

      migrationBuilder.DropTable(name: "Readlist");

      migrationBuilder.DropTable(name: "Review");

      migrationBuilder.DropTable(name: "Token");

      migrationBuilder.DropTable(name: "UserRating");

      migrationBuilder.DropTable(name: "Wishlist");

      migrationBuilder.DropTable(name: "Award");

      migrationBuilder.DropTable(name: "Character");

      migrationBuilder.DropTable(name: "Genre");

      migrationBuilder.DropTable(name: "Setting");

      migrationBuilder.DropTable(name: "Book");

      migrationBuilder.DropTable(name: "User");

      migrationBuilder.DropTable(name: "Publisher");
    }
  }
}
