﻿using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public int AppointmentId { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? Advice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
