using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? GuardianName { get; set; }

    public string? GuardianPhone { get; set; }

    public string? HealthInsuranceNo { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<RevisitReminder> RevisitReminders { get; set; } = new List<RevisitReminder>();
}
