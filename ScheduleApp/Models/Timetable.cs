using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class Timetable
{
    public int TimetableId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<ShareToken> ShareTokens { get; set; } = new List<ShareToken>();

    public virtual User? User { get; set; }
}
