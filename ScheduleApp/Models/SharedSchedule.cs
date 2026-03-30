using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class SharedSchedule
{
    public int ShareId { get; set; }

    public int? TokenId { get; set; }

    public int? ViewerId { get; set; }

    public DateTime? GrantedAt { get; set; }

    public virtual ShareToken? Token { get; set; }

    public virtual User? Viewer { get; set; }
}
