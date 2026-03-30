using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class ScheduleEvent
{
    public int EventId { get; set; }

    public int? EntryId { get; set; }

    public string? Title { get; set; }

    public DateOnly? EventDate { get; set; }

    public string? Description { get; set; }

    public virtual Schedule? Entry { get; set; }
}
