using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScheduleApp.Models;

public partial class Subject
{
    [Display(Name = "ID Предмета")]
    public int SubjectId { get; set; }

    [Display(Name = "ID Користувача")]
    public int? UserId { get; set; }

    [Required(ErrorMessage = "Назва обов'язкова")]
    [Display(Name = "Назва предмета")]
    public string Name { get; set; } = null!;

    [Display(Name = "Колір (HEX)")]
    public string? ColorCode { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    public virtual User? User { get; set; }
}