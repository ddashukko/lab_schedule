using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScheduleApp.Models;

public partial class Schedule
{
    public int EntryId { get; set; }

    public int? SubjectId { get; set; }

    public int? TeacherId { get; set; }

    public string? Link { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public TimeOnly? TimeStart { get; set; }

    public TimeOnly? TimeFinish { get; set; }
    [Display(Name = "Інтервал повторення")]
    [Range(0, 365, ErrorMessage = "Інтервал повторення не може бути від'ємним!")]
    public int? RepeatInterval { get; set; }

    public virtual ICollection<ScheduleEvent> ScheduleEvents { get; set; } = new List<ScheduleEvent>();

    public virtual Subject? Subject { get; set; }

    public virtual Teacher? Teacher { get; set; }
}