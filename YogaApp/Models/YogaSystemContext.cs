using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;

namespace YogaApp.Models
{
    public partial class YogaSystemContext : DbContext
    {
        public YogaSystemContext()
        {
        }

        public YogaSystemContext(DbContextOptions<YogaSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApprovalStatusLkp> ApprovalStatusLkps { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<ClassParticipantLink> ClassParticipantLinks { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<CourseStatusLkp> CourseStatusLkps { get; set; } = null!;
        public virtual DbSet<EnrollmentStatusLkp> EnrollmentStatusLkps { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<LocationAdministratorLink> LocationAdministratorLinks { get; set; } = null!;
        public virtual DbSet<LocationClassLink> LocationClassLinks { get; set; } = null!;
        public virtual DbSet<LocationInstructorLink> LocationInstructorLinks { get; set; } = null!;
        public virtual DbSet<Participant> Participants { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionStatusLkp> TransactionStatusLkps { get; set; } = null!;
        public virtual DbSet<TransactionTypeLkp> TransactionTypeLkps { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatusLkp>(entity =>
            {
                entity.HasKey(e => e.ApprovalStatusId);

                entity.ToTable("ApprovalStatusLkp");

                entity.Property(e => e.ApprovalStatusId).HasColumnName("ApprovalStatusID");

                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<ClassParticipantLink>(entity =>
            {
                entity.HasKey(e => e.ClassParticipantId);

                entity.ToTable("ClassParticipantLink");

                entity.Property(e => e.ClassParticipantId).HasColumnName("ClassParticipantID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EnrollmentStatusId).HasColumnName("EnrollmentStatusID");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassParticipantLinks)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_ClassParticipantLink_Course");

                entity.HasOne(d => d.EnrollmentStatus)
                    .WithMany(p => p.ClassParticipantLinks)
                    .HasForeignKey(d => d.EnrollmentStatusId)
                    .HasConstraintName("FK_ClassParticipantLink_EnrollmentStatusLkp");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.ClassParticipantLinks)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_ClassParticipantLink_Participant");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseId).HasColumnName("CourseID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.PreviousOfferingCourseId).HasColumnName("PreviousOfferingCourseID");

                entity.Property(e => e.PricePerSession).HasColumnType("money");

                entity.Property(e => e.RequiredExpertise).HasColumnType("text");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Title).HasColumnType("text");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Course_Location");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Course_CourseStatusLkp");
            });

            modelBuilder.Entity<CourseStatusLkp>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("CourseStatusLkp");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EnrollmentStatusLkp>(entity =>
            {
                entity.HasKey(e => e.EnrollmentStatusId);

                entity.ToTable("EnrollmentStatusLkp");

                entity.Property(e => e.EnrollmentStatusId).HasColumnName("EnrollmentStatusID");

                entity.Property(e => e.EntrollmentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("Instructor");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.Body).HasColumnType("text");

                entity.Property(e => e.ContactInfo).HasColumnType("text");

                entity.Property(e => e.Heading)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Photo).HasColumnType("image");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.AddressFull).HasColumnType("text");

                entity.Property(e => e.AddressPostcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactInfo).HasColumnType("text");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.OpeningTimes).HasColumnType("text");
            });

            modelBuilder.Entity<LocationAdministratorLink>(entity =>
            {
                entity.HasKey(e => e.AdministratorId);

                entity.ToTable("LocationAdministratorLink");

                entity.Property(e => e.AdministratorId)
                    .ValueGeneratedNever()
                    .HasColumnName("AdministratorID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationAdministratorLinks)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_LocationAdministratorLink_Location");
            });

            modelBuilder.Entity<LocationClassLink>(entity =>
            {
                entity.ToTable("LocationClassLink");

                entity.Property(e => e.LocationClassLinkId).HasColumnName("LocationClassLinkID");

                entity.Property(e => e.ClassId).HasColumnName("ClassID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");
            });

            modelBuilder.Entity<LocationInstructorLink>(entity =>
            {
                entity.ToTable("LocationInstructorLink");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.ApprovalStatusId).HasColumnName("ApprovalStatusID");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.RemovalDate).HasColumnType("datetime");

                entity.HasOne(d => d.ApprovalStatus)
                    .WithMany(p => p.LocationInstructorLinks)
                    .HasForeignKey(d => d.ApprovalStatusId)
                    .HasConstraintName("FK_LocationInstructorLink_ApprovalStatusLkp");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.LocationInstructorLinks)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_LocationInstructorLink_Instructor");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.LocationInstructorLinks)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_LocationInstructorLink_Location");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("Participant");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.AboutMe).HasColumnType("text");

                entity.Property(e => e.LastUpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.EntryDate).HasColumnType("datetime");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ReviewText).HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_Review_Instructor");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK_Review_Location");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Review_Participant");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Tid);

                entity.ToTable("Transaction");

                entity.Property(e => e.Tid).HasColumnName("TID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.InstructorId).HasColumnName("InstructorID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.TriggerDate).HasColumnType("datetime");

                entity.Property(e => e.TstatusId).HasColumnName("TStatusID");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_Transaction_Instructor");

                entity.HasOne(d => d.Participant)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ParticipantId)
                    .HasConstraintName("FK_Transaction_Participant");

                entity.HasOne(d => d.Tstatus)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TstatusId)
                    .HasConstraintName("FK_Transaction_TransactionStatusLkp");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Transaction_TransactionTypeLkp");
            });

            modelBuilder.Entity<TransactionStatusLkp>(entity =>
            {
                entity.HasKey(e => e.TransactionStatusId);

                entity.ToTable("TransactionStatusLkp");

                entity.Property(e => e.TransactionStatusId).HasColumnName("TransactionStatusID");

                entity.Property(e => e.TransactionStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TransactionTypeLkp>(entity =>
            {
                entity.HasKey(e => e.TransactionTypeId);

                entity.ToTable("TransactionTypeLkp");

                entity.Property(e => e.TransactionTypeId).HasColumnName("TransactionTypeID");

                entity.Property(e => e.TransactionType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
