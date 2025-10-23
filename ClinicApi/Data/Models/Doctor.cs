using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string? Specialty { get; set; }

    public string? Qualification { get; set; }

    public int? ExperienceYears { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Employee DoctorNavigation { get; set; } = null!;
}
