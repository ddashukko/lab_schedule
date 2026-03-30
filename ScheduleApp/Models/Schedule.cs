using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class Schedule
{
    public int EntryId { get; set; }

    public int? TimetableId { get; set; }

    public int? SubjectId { get; set; }

    public int? TeacherId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeFinish { get; set; }

    public int? RepeatInterval { get; set; }

    public virtual ICollection<ScheduleEvent> ScheduleEvents { get; set; } = new List<ScheduleEvent>();

    public virtual Subject? Subject { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual Timetable? Timetable { get; set; }
}
