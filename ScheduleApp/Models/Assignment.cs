using System;
using System.ComponentModel.DataAnnotations;

namespace ScheduleApp.Models;

public partial class Assignment
{
    [Display(Name = "ID Завдання")]
    public int TaskId { get; set; }

    [Display(Name = "Предмет")]
    public int? SubjectId { get; set; }

    [Required(ErrorMessage = "Опис обов'язковий")]
    [Display(Name = "Опис завдання")]
    public string? Description { get; set; }

    [Display(Name = "Дедлайн")]
    public DateTime? Deadline { get; set; }

    [Display(Name = "Предмет")]
    public virtual Subject? Subject { get; set; }
}