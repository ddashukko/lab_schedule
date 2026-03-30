using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class Note
{
    public int NoteId { get; set; }

    public int? SubjectId { get; set; }

    public string? Content { get; set; }

    public virtual Subject? Subject { get; set; }
}
