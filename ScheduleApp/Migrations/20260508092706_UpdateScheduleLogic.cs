using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScheduleApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("subjects_pkey", x => x.subject_id);
                    table.ForeignKey(
                        name: "subjects_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    teacher_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("teachers_pkey", x => x.teacher_id);
                    table.ForeignKey(
                        name: "teachers_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    task_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    deadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("assignments_pkey", x => x.task_id);
                    table.ForeignKey(
                        name: "assignments_subject_id_fkey",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id");
                });

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    note_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<int>(type: "integer", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("notes_pkey", x => x.note_id);
                    table.ForeignKey(
                        name: "notes_subject_id_fkey",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id");
                });

            migrationBuilder.CreateTable(
                name: "schedule",
                columns: table => new
                {
                    entry_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<int>(type: "integer", nullable: true),
                    teacher_id = table.Column<int>(type: "integer", nullable: true),
                    link = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    time_start = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    time_finish = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    repeat_interval = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("schedule_pkey", x => x.entry_id);
                    table.ForeignKey(
                        name: "schedule_subject_id_fkey",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id");
                    table.ForeignKey(
                        name: "schedule_teacher_id_fkey",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id");
                });

            migrationBuilder.CreateTable(
                name: "schedule_events",
                columns: table => new
                {
                    event_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entry_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    event_date = table.Column<DateOnly>(type: "date", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("schedule_events_pkey", x => x.event_id);
                    table.ForeignKey(
                        name: "schedule_events_entry_id_fkey",
                        column: x => x.entry_id,
                        principalTable: "schedule",
                        principalColumn: "entry_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_assignments_subject_id",
                table: "assignments",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_notes_subject_id",
                table: "notes",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_subject_id",
                table: "schedule",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_teacher_id",
                table: "schedule",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_events_entry_id",
                table: "schedule_events",
                column: "entry_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_user_id",
                table: "subjects",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_user_id",
                table: "teachers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "schedule_events");

            migrationBuilder.DropTable(
                name: "schedule");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
