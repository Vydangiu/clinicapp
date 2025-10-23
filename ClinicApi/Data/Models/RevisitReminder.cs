using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class RevisitReminder
{
    public int ReminderId { get; set; }

    public int PatientId { get; set; }

    public DateOnly NextVisitDate { get; set; }

    public bool? Sent { get; set; }

    public DateTime? SentDate { get; set; }

    public string? Notes { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
