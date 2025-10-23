using System;
using System.Collections.Generic;

namespace ClinicApi.Data.Models;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public DateTime? ExpenseDate { get; set; }

    public string? Description { get; set; }

    public decimal? Amount { get; set; }

    public int? RecordedBy { get; set; }

    public virtual Employee? RecordedByNavigation { get; set; }
}
