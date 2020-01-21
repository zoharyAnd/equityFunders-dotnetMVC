using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace cFunding.Migrations
{
    public partial class completeDataset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    categoryname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    userfname = table.Column<string>(nullable: true),
                    userlname = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true),
                    useremail = table.Column<string>(nullable: true),
                    userpassword = table.Column<string>(nullable: true),
                    userphoto = table.Column<string>(nullable: true),
                    userdob = table.Column<DateTimeOffset>(nullable: false),
                    useraddress = table.Column<string>(nullable: true),
                    usercountry = table.Column<string>(nullable: true),
                    companyname = table.Column<string>(nullable: true),
                    companylogo = table.Column<string>(nullable: true),
                    companydescription = table.Column<string>(nullable: true),
                    nbshareordinary = table.Column<int>(nullable: false),
                    sharevalueordinary = table.Column<double>(nullable: false),
                    descordinary = table.Column<string>(nullable: true),
                    additionalordinary = table.Column<string>(nullable: true),
                    nbsharepreferencial = table.Column<int>(nullable: false),
                    sharevaluepreferencial = table.Column<double>(nullable: false),
                    descpreferencial = table.Column<string>(nullable: true),
                    additionalpreferencial = table.Column<string>(nullable: true),
                    nbsharenonvoting = table.Column<int>(nullable: false),
                    sharevaluenonvoting = table.Column<double>(nullable: false),
                    descnonvoting = table.Column<string>(nullable: true),
                    additionalnonvoting = table.Column<string>(nullable: true),
                    nbshareredeemable = table.Column<int>(nullable: false),
                    sharevalueredeemable = table.Column<double>(nullable: false),
                    descredeemable = table.Column<string>(nullable: true),
                    additionalredeemable = table.Column<string>(nullable: true),
                    userassets = table.Column<double>(nullable: false),
                    isadmin = table.Column<bool>(nullable: false),
                    validatedUser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contactsus",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    fkuserid = table.Column<int>(nullable: true),
                    sendername = table.Column<string>(nullable: true),
                    senderemail = table.Column<string>(nullable: true),
                    mailsubject = table.Column<string>(nullable: true),
                    mailmessage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactsus", x => x.id);
                    table.ForeignKey(
                        name: "FK_contactsus_users_fkuserid",
                        column: x => x.fkuserid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    projectname = table.Column<string>(nullable: true),
                    projectdescription = table.Column<string>(nullable: true),
                    projectamounttoraise = table.Column<double>(nullable: false),
                    projectamountraised = table.Column<double>(nullable: false),
                    projectcreationdate = table.Column<DateTimeOffset>(nullable: false),
                    projectclosingdate = table.Column<DateTimeOffset>(nullable: false),
                    projectimage1 = table.Column<string>(nullable: true),
                    projectimage2 = table.Column<string>(nullable: true),
                    projectimage3 = table.Column<string>(nullable: true),
                    projectimage4 = table.Column<string>(nullable: true),
                    fkuserid = table.Column<int>(nullable: true),
                    fkcategoryid = table.Column<int>(nullable: true),
                    nbshareordinary = table.Column<int>(nullable: false),
                    nbsharepreferencial = table.Column<int>(nullable: false),
                    nbsharenonvoting = table.Column<int>(nullable: false),
                    nbshareredeemable = table.Column<int>(nullable: false),
                    validatedProject = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_projects_categories_fkcategoryid",
                        column: x => x.fkcategoryid,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_projects_users_fkuserid",
                        column: x => x.fkuserid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    fkuserid = table.Column<int>(nullable: true),
                    questiondate = table.Column<DateTimeOffset>(nullable: false),
                    questionmessage = table.Column<string>(nullable: true),
                    validatedQuestion = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                    table.ForeignKey(
                        name: "FK_questions_users_fkuserid",
                        column: x => x.fkuserid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    fkuserid = table.Column<int>(nullable: true),
                    fkprojectid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorites", x => x.id);
                    table.ForeignKey(
                        name: "FK_favorites_projects_fkprojectid",
                        column: x => x.fkprojectid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_favorites_users_fkuserid",
                        column: x => x.fkuserid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    fkprojectid = table.Column<int>(nullable: true),
                    fkuserid = table.Column<int>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    transactiondate = table.Column<DateTimeOffset>(nullable: false),
                    accountemail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_projects_fkprojectid",
                        column: x => x.fkprojectid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transactions_users_fkuserid",
                        column: x => x.fkuserid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    fkquestionid = table.Column<int>(nullable: true),
                    fkuserid = table.Column<int>(nullable: true),
                    answerdate = table.Column<DateTimeOffset>(nullable: false),
                    answermessage = table.Column<string>(nullable: true),
                    validatedAnswer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.id);
                    table.ForeignKey(
                        name: "FK_answers_questions_fkquestionid",
                        column: x => x.fkquestionid,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_answers_users_fkuserid",
                        column: x => x.fkuserid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_answers_fkquestionid",
                table: "answers",
                column: "fkquestionid");

            migrationBuilder.CreateIndex(
                name: "IX_answers_fkuserid",
                table: "answers",
                column: "fkuserid");

            migrationBuilder.CreateIndex(
                name: "IX_contactsus_fkuserid",
                table: "contactsus",
                column: "fkuserid");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_fkprojectid",
                table: "favorites",
                column: "fkprojectid");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_fkuserid",
                table: "favorites",
                column: "fkuserid");

            migrationBuilder.CreateIndex(
                name: "IX_projects_fkcategoryid",
                table: "projects",
                column: "fkcategoryid");

            migrationBuilder.CreateIndex(
                name: "IX_projects_fkuserid",
                table: "projects",
                column: "fkuserid");

            migrationBuilder.CreateIndex(
                name: "IX_questions_fkuserid",
                table: "questions",
                column: "fkuserid");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_fkprojectid",
                table: "transactions",
                column: "fkprojectid");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_fkuserid",
                table: "transactions",
                column: "fkuserid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "contactsus");

            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
