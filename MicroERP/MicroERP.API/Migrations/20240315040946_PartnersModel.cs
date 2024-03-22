using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroERP.API.Migrations
{
    /// <inheritdoc />
    public partial class PartnersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerAddress_Partners_PartnerId",
                table: "PartnerAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerContact_Partners_PartnerId",
                table: "PartnerContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerContact",
                table: "PartnerContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerAddress",
                table: "PartnerAddress");

            migrationBuilder.RenameTable(
                name: "PartnerContact",
                newName: "PartnerContacts");

            migrationBuilder.RenameTable(
                name: "PartnerAddress",
                newName: "PartnerAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerContact_PartnerId",
                table: "PartnerContacts",
                newName: "IX_PartnerContacts_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerAddress_PartnerId",
                table: "PartnerAddresses",
                newName: "IX_PartnerAddresses_PartnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerContacts",
                table: "PartnerContacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerAddresses",
                table: "PartnerAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerAddresses_Partners_PartnerId",
                table: "PartnerAddresses",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerContacts_Partners_PartnerId",
                table: "PartnerContacts",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerAddresses_Partners_PartnerId",
                table: "PartnerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerContacts_Partners_PartnerId",
                table: "PartnerContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerContacts",
                table: "PartnerContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerAddresses",
                table: "PartnerAddresses");

            migrationBuilder.RenameTable(
                name: "PartnerContacts",
                newName: "PartnerContact");

            migrationBuilder.RenameTable(
                name: "PartnerAddresses",
                newName: "PartnerAddress");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerContacts_PartnerId",
                table: "PartnerContact",
                newName: "IX_PartnerContact_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerAddresses_PartnerId",
                table: "PartnerAddress",
                newName: "IX_PartnerAddress_PartnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerContact",
                table: "PartnerContact",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerAddress",
                table: "PartnerAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerAddress_Partners_PartnerId",
                table: "PartnerAddress",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerContact_Partners_PartnerId",
                table: "PartnerContact",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id");
        }
    }
}
