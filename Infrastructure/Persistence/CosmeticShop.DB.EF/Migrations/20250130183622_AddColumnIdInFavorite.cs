﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticShop.DB.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIdInFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Favorites",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Favorites");
        }
    }
}
