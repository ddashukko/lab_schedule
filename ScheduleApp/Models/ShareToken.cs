using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class ShareToken
{
    public int TokenId { get; set; }

    public int? TimetableId { get; set; }

    public string? AccessToken { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<SharedSchedule> SharedSchedules { get; set; } = new List<SharedSchedule>();

    public virtual Timetable? Timetable { get; set; }
}
