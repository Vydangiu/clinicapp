using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicApi.Data.Models;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicineImport> MedicineImports { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

    public virtual DbSet<RevisitReminder> RevisitReminders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VwExpensesByMonth> VwExpensesByMonths { get; set; }

    public virtual DbSet<VwPatientByMonth> VwPatientByMonths { get; set; }

    public virtual DbSet<VwPatientsNotRevisited> VwPatientsNotRevisiteds { get; set; }

    public virtual DbSet<VwRevenueByDay> VwRevenueByDays { get; set; }

    public virtual DbSet<VwTopMedicine> VwTopMedicines { get; set; }

    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Server=DESKTOP-PFEO40A;Database=ClinicDB;User Id=sa;Password=ThaoVy;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2C911A62F");

            entity.HasIndex(e => e.AppointmentDate, "IX_Appointments_Date");
            
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa khám");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Appointments_Patient");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDF3595F2D5");

            entity.Property(e => e.DoctorId)
                .ValueGeneratedNever()
                .HasColumnName("DoctorID");
            entity.Property(e => e.Qualification).HasMaxLength(100);
            entity.Property(e => e.Specialty).HasMaxLength(100);

            entity.HasOne(d => d.DoctorNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Employee");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1C080D3A8");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Position).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");
            
            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Employees_Role");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expenses__1445CFF31EA07654");

            entity.Property(e => e.ExpenseId).HasColumnName("ExpenseID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ExpenseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.RecordedBy)
                .HasConstraintName("FK_Expenses_Employee");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__MedicalR__FBDF78C9E459D8DD");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");

            entity.Property(e => e.Advice).HasMaxLength(255);
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Diagnosis).HasMaxLength(255);
            entity.Property(e => e.Symptoms).HasMaxLength(255);

            entity.HasOne(d => d.Appointment).WithMany(p => p.MedicalRecords)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicalRecords_Appointment");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__4F2128F01DAEB46C");

            entity.HasIndex(e => e.MedicineName, "IX_Medicines_Name");

            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.MedicineName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue(0);
            entity.Property(e => e.SideEffects).HasMaxLength(255);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.Usage).HasMaxLength(255);
        });

        modelBuilder.Entity<MedicineImport>(entity =>
        {
            entity.HasKey(e => e.ImportId).HasName("PK__Medicine__8697678A8EBB3DA9");

            entity.ToTable("MedicineImport");

            entity.Property(e => e.ImportId).HasColumnName("ImportID");
            entity.Property(e => e.ImportDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.TotalPrice)
                .HasComputedColumnSql("([Quantity]*[UnitPrice])", false)
                .HasColumnType("decimal(29, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicineImports)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineImport_Medicine");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC34623B9A7E9");

            entity.HasIndex(e => e.FullName, "IX_Patients_Name");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.GuardianName).HasMaxLength(100);
            entity.Property(e => e.GuardianPhone).HasMaxLength(20);
            entity.Property(e => e.HealthInsuranceNo).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A589F1B0E3B");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Appointment");

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ProcessedBy)
                .HasConstraintName("FK_Payments_Employee");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__40130812CCEC7CF0");

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.RecordId).HasColumnName("RecordID");

            entity.HasOne(d => d.Record).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prescriptions_Record");
        });

        modelBuilder.Entity<PrescriptionDetail>(entity =>
        {
            entity.HasKey(e => e.PrescriptionDetailId).HasName("PK__Prescrip__6DB7668A9052071B");

            entity.Property(e => e.PrescriptionDetailId).HasColumnName("PrescriptionDetailID");
            entity.Property(e => e.Dosage).HasMaxLength(100);
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.UsageInstruction).HasMaxLength(255);

            entity.HasOne(d => d.Medicine).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionDetails_Medicine");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.PrescriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionDetails_Prescription");
        });

        modelBuilder.Entity<RevisitReminder>(entity =>
        {
            entity.HasKey(e => e.ReminderId).HasName("PK__RevisitR__01A830A7D4E3C368");

            entity.Property(e => e.ReminderId).HasColumnName("ReminderID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Sent).HasDefaultValue(false);
            entity.Property(e => e.SentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Patient).WithMany(p => p.RevisitReminders)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RevisitReminders_Patient");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AD7D807F5");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.IsAdmin).HasDefaultValue(false);

            // SEED: vài vai trò hiển thị (dropdown), chỉ Admin có IsAdmin=true
            entity.HasData(
                new Role { RoleId = 1, RoleName = "Admin", Description = "Quản trị", IsAdmin = true },
                new Role { RoleId = 2, RoleName = "Bác sĩ", Description = "Khám chữa", IsAdmin = false },
                new Role { RoleId = 3, RoleName = "Điều dưỡng", Description = "Chăm sóc", IsAdmin = false },
                new Role { RoleId = 4, RoleName = "Thu ngân", Description = "Thanh toán", IsAdmin = false },
                new Role { RoleId = 5, RoleName = "Kế toán", Description = "Tài chính", IsAdmin = false }
            );
        });

        // modelBuilder.Entity<User>(entity =>
        // {
        //     entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACCF8FEFA6");

        //     entity.HasIndex(e => e.Username, "UQ__Users__536C85E42A2DF4DB").IsUnique();

        //     entity.Property(e => e.UserId).HasColumnName("UserID");
        //     entity.Property(e => e.CreatedAt)
        //         .HasDefaultValueSql("(getdate())")
        //         .HasColumnType("datetime");
        //     entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
        //     entity.Property(e => e.IsActive).HasDefaultValue(true);
        //     entity.Property(e => e.PasswordHash).HasMaxLength(255);
        //     entity.Property(e => e.Username).HasMaxLength(50);

        //     entity.HasOne(d => d.Employee).WithMany(p => p.Users)
        //         .HasForeignKey(d => d.EmployeeId)
        //         .HasConstraintName("FK_Users_Employee");
        // });
        
        modelBuilder.Entity<User>(entity =>
    {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACCF8FEFA6");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E42A2DF4DB").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);

            // NEW: map cột RoleID (FK)
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            // Quan hệ cũ: User -> Employee (giữ nguyên)
            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_Users_Employee");

            // NEW: Quan hệ User -> Role
            entity.HasOne(d => d.Role).WithMany(r => r.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Users_Role");
        });



        modelBuilder.Entity<VwExpensesByMonth>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ExpensesByMonth");

            entity.Property(e => e.TổngChiPhíVnđ)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Tổng chi phí (VNĐ)");
        });

        modelBuilder.Entity<VwPatientByMonth>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PatientByMonth");

            entity.Property(e => e.TổngBệnhNhânĐăngKýMới).HasColumnName("Tổng bệnh nhân đăng ký mới");
        });

        modelBuilder.Entity<VwPatientsNotRevisited>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PatientsNotRevisited");

            entity.Property(e => e.NgàyHẹnTáiKhám).HasColumnName("Ngày hẹn tái khám");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.SốNgàyTrễ).HasColumnName("Số ngày trễ");
            entity.Property(e => e.TênBệnhNhân)
                .HasMaxLength(100)
                .HasColumnName("Tên bệnh nhân");
        });

        modelBuilder.Entity<VwRevenueByDay>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RevenueByDay");

            entity.Property(e => e.NgàyThuTiền).HasColumnName("Ngày thu tiền");
            entity.Property(e => e.SốPhiếuThu).HasColumnName("Số phiếu thu");
            entity.Property(e => e.TổngDoanhThuVnđ)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Tổng doanh thu (VNĐ)");
        });

        modelBuilder.Entity<VwTopMedicine>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TopMedicines");

            entity.Property(e => e.TênThuốc)
                .HasMaxLength(100)
                .HasColumnName("Tên thuốc");
            entity.Property(e => e.TổngDoanhThuVnđ)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("Tổng doanh thu (VNĐ)");
            entity.Property(e => e.TổngSlBán).HasColumnName("Tổng SL bán");
        });

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__WorkSche__9C8A5B6945770287");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.Shift).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkSchedules)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkSchedules_Employee");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
