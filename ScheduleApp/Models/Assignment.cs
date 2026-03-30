using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class Assignment
{
    public int TaskId { get; set; }

    public int? SubjectId { get; set; }

    public string? Description { get; set; }

    public DateTime? Deadline { get; set; }

    public virtual Subject? Subject { get; set; }
}
