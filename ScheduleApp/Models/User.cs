using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<SharedSchedule> SharedSchedules { get; set; } = new List<SharedSchedule>();

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}
