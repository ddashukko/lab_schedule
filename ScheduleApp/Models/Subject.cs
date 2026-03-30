using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? ColorCode { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual User? User { get; set; }
}
