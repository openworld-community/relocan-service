using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReloCAN.Service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Outbox",
                schema: "Relocan",
                columns: table => new
                {
                    IdempotencyId = table.Column<string>(type: "text", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: true),
                    Events = table.Column<string>(type: "text", nullable: true),
                    Commands = table.Column<string>(type: "text", nullable: true),
                    IsDispatched = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox", x => x.IdempotencyId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outbox",
                schema: "Relocan");
        }
    }
}
