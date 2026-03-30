using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ScheduleApp.Models;

public partial class IsttpContext : DbContext
{
    public IsttpContext()
    {
    }

    public IsttpContext(DbContextOptions<IsttpContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleEvent> ScheduleEvents { get; set; }

    public virtual DbSet<ShareToken> ShareTokens { get; set; }

    public virtual DbSet<SharedSchedule> SharedSchedules { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<Timetable> Timetables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("assignments_pkey");

            entity.ToTable("assignments");

            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.Deadline)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deadline");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("assignments_subject_id_fkey");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("notes_pkey");

            entity.ToTable("notes");

            entity.Property(e => e.NoteId).HasColumnName("note_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Notes)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("notes_subject_id_fkey");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("schedule_pkey");

            entity.ToTable("schedule");

            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.RepeatInterval).HasColumnName("repeat_interval");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.TimeFinish).HasColumnName("time_finish");
            entity.Property(e => e.TimeStart).HasColumnName("time_start");
            entity.Property(e => e.TimetableId).HasColumnName("timetable_id");

            entity.HasOne(d => d.Subject).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("schedule_subject_id_fkey");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("schedule_teacher_id_fkey");

            entity.HasOne(d => d.Timetable).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TimetableId)
                .HasConstraintName("schedule_timetable_id_fkey");
        });

        modelBuilder.Entity<ScheduleEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("schedule_events_pkey");

            entity.ToTable("schedule_events");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EntryId).HasColumnName("entry_id");
            entity.Property(e => e.EventDate).HasColumnName("event_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Entry).WithMany(p => p.ScheduleEvents)
                .HasForeignKey(d => d.EntryId)
                .HasConstraintName("schedule_events_entry_id_fkey");
        });

        modelBuilder.Entity<ShareToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("share_tokens_pkey");

            entity.ToTable("share_tokens");

            entity.HasIndex(e => e.AccessToken, "share_tokens_access_token_key").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.AccessToken)
                .HasMaxLength(255)
                .HasColumnName("access_token");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.TimetableId).HasColumnName("timetable_id");

            entity.HasOne(d => d.Timetable).WithMany(p => p.ShareTokens)
                .HasForeignKey(d => d.TimetableId)
                .HasConstraintName("share_tokens_timetable_id_fkey");
        });

        modelBuilder.Entity<SharedSchedule>(entity =>
        {
            entity.HasKey(e => e.ShareId).HasName("shared_schedules_pkey");

            entity.ToTable("shared_schedules");

            entity.Property(e => e.ShareId).HasColumnName("share_id");
            entity.Property(e => e.GrantedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("granted_at");
            entity.Property(e => e.TokenId).HasColumnName("token_id");
            entity.Property(e => e.ViewerId).HasColumnName("viewer_id");

            entity.HasOne(d => d.Token).WithMany(p => p.SharedSchedules)
                .HasForeignKey(d => d.TokenId)
                .HasConstraintName("shared_schedules_token_id_fkey");

            entity.HasOne(d => d.Viewer).WithMany(p => p.SharedSchedules)
                .HasForeignKey(d => d.ViewerId)
                .HasConstraintName("shared_schedules_viewer_id_fkey");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("subjects_pkey");

            entity.ToTable("subjects");

            entity.Property(e => e.SubjectId).HasColumnName("subject_id");
            entity.Property(e => e.ColorCode)
                .HasMaxLength(50)
                .HasColumnName("color_code");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("subjects_user_id_fkey");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("teachers_pkey");

            entity.ToTable("teachers");

            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Link).HasColumnName("link");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("teachers_user_id_fkey");
        });

        modelBuilder.Entity<Timetable>(entity =>
        {
            entity.HasKey(e => e.TimetableId).HasName("timetables_pkey");

            entity.ToTable("timetables");

            entity.Property(e => e.TimetableId).HasColumnName("timetable_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Timetables)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("timetables_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
