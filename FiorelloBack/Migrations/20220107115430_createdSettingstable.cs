using Microsoft.EntityFrameworkCore.Migrations;

namespace FiorelloBack.Migrations
{
    public partial class createdSettingstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(maxLength: 150, nullable: true),
                    SearchIcon = table.Column<string>(nullable: false),
                    BasketIcon = table.Column<string>(nullable: false),
                    ParallaxImage = table.Column<string>(maxLength: 150, nullable: true),
                    ParallaxTitle = table.Column<string>(maxLength: 50, nullable: true),
                    InstagramUrl = table.Column<string>(maxLength: 150, nullable: true),
                    FacebookUrl = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
