using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "monsters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    attack = table.Column<int>(type: "integer", nullable: false),
                    defense = table.Column<int>(type: "integer", nullable: false),
                    hp = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_monsters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "battles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    monster_a = table.Column<Guid>(type: "uuid", nullable: false),
                    monster_b = table.Column<Guid>(type: "uuid", nullable: false),
                    monster_winner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_battles", x => x.id);
                    table.ForeignKey(
                        name: "fk_battles_monsters_monster_a",
                        column: x => x.monster_a,
                        principalTable: "monsters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_battles_monsters_monster_b",
                        column: x => x.monster_b,
                        principalTable: "monsters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_battles_monsters_monster_winner_id",
                        column: x => x.monster_winner_id,
                        principalTable: "monsters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_battles_monster_a",
                table: "battles",
                column: "monster_a");

            migrationBuilder.CreateIndex(
                name: "ix_battles_monster_b",
                table: "battles",
                column: "monster_b");

            migrationBuilder.CreateIndex(
                name: "ix_battles_monster_winner_id",
                table: "battles",
                column: "monster_winner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battles");

            migrationBuilder.DropTable(
                name: "monsters");
        }
    }
}
