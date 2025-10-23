using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Prescription
{
    public int PrescriptionId { get; set; }

    public int RecordId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<PrescriptionDetail> PrescriptionDetails { get; set; } = new List<PrescriptionDetail>();

    public virtual MedicalRecord Record { get; set; } = null!;
}
