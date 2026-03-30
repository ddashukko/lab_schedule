using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public int? UserId { get; set; }

    public string? FullName { get; set; }

    public string? Link { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual User? User { get; set; }
}
