using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int AppointmentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public int? ProcessedBy { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Employee? ProcessedByNavigation { get; set; }
}
