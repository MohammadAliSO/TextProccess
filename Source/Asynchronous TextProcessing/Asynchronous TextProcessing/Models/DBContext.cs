using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Asynchronous_TextProcessing.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PermissionT> PermissionTs { get; set; } = null!;
        public virtual DbSet<RequestT> RequestTs { get; set; } = null!;
        public virtual DbSet<ResultT> ResultTs { get; set; } = null!;
        public virtual DbSet<UserPermissionT> UserPermissionTs { get; set; } = null!;
        public virtual DbSet<UserT> UserTs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-TQD5Q14\\SQL2019;Initial Catalog=Tasks_DB;MultipleActiveResultSets=true;User ID=sa;Password=22816321;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermissionT>(entity =>
            {
                entity.HasKey(e => e.PermissionId);

                entity.ToTable("Permission_t");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.Content)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RequestT>(entity =>
            {
                entity.ToTable("Request_t");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreateDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ResultId).HasColumnName("ResultID");

                entity.Property(e => e.Type).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Result)
                    .WithMany(p => p.RequestTs)
                    .HasForeignKey(d => d.ResultId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Request_t_Result_t");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RequestTs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Request_t_User_t");
            });

            modelBuilder.Entity<ResultT>(entity =>
            {
                entity.ToTable("Result_t");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(2000);

                entity.Property(e => e.EndProcessTime).HasColumnType("datetime");

                entity.Property(e => e.StartProcessTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserPermissionT>(entity =>
            {
                entity.HasKey(e => e.UserPermissionId);

                entity.ToTable("UserPermission_t");

                entity.Property(e => e.UserPermissionId).HasColumnName("UserPermissionID");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermissionTs)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_UserPermission_t_Permission_t");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermissionTs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserPermission_t_User_t");
            });

            modelBuilder.Entity<UserT>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("User_t");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
