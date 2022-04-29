using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BeeBreeder.Data.Models
{
    public partial class BeeBreederContext : DbContext
    {
        public BeeBreederContext()
        {
        }

        public BeeBreederContext(DbContextOptions<BeeBreederContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemDropChance> ItemDropChances { get; set; } = null!;
        public virtual DbSet<Mod> Mods { get; set; } = null!;
        public virtual DbSet<MutationChance> MutationChances { get; set; } = null!;
        public virtual DbSet<Specie> Species { get; set; } = null!;
        public virtual DbSet<SpecieNote> SpecieNotes { get; set; } = null!;
        public virtual DbSet<SpecieStat> SpecieStats { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=RIT-W45\\SQLEXPRESS;Database=BeeBreeder;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Name).HasMaxLength(40);
            });

            modelBuilder.Entity<ItemDropChance>(entity =>
            {
                entity.ToTable("ItemDropChance");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemDropChances)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LootItem_Item");

                entity.HasOne(d => d.Specie)
                    .WithMany(p => p.ItemDropChances)
                    .HasForeignKey(d => d.SpecieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LootItem_Specie");
            });

            modelBuilder.Entity<Mod>(entity =>
            {
                entity.ToTable("Mod");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<MutationChance>(entity =>
            {
                entity.ToTable("MutationChance");

                entity.HasOne(d => d.First)
                    .WithMany(p => p.MutationChanceFirsts)
                    .HasForeignKey(d => d.FirstId)
                    .HasConstraintName("FK_MutationChance_Specie");

                entity.HasOne(d => d.Result)
                    .WithMany(p => p.MutationChanceResults)
                    .HasForeignKey(d => d.ResultId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MutationChance_Specie2");

                entity.HasOne(d => d.Second)
                    .WithMany(p => p.MutationChanceSeconds)
                    .HasForeignKey(d => d.SecondId)
                    .HasConstraintName("FK_MutationChance_Specie1");
            });

            modelBuilder.Entity<Specie>(entity =>
            {
                entity.ToTable("Specie");

                entity.Property(e => e.Branch).HasMaxLength(30);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.DiscoveredBy)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.LatinName).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.HasOne(d => d.Mod)
                    .WithMany(p => p.Species)
                    .HasForeignKey(d => d.ModId)
                    .HasConstraintName("FK_Specie_Mod");
            });

            modelBuilder.Entity<SpecieNote>(entity =>
            {
                entity.ToTable("SpecieNote");

                entity.Property(e => e.Text).HasColumnType("text");

                entity.HasOne(d => d.Specie)
                    .WithMany(p => p.SpecieNotes)
                    .HasForeignKey(d => d.SpecieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecieNote_Specie");
            });

            modelBuilder.Entity<SpecieStat>(entity =>
            {
                entity.Property(e => e.Fertility).HasDefaultValueSql("((1))");

                entity.Property(e => e.Flowers)
                    .HasMaxLength(15)
                    .HasDefaultValueSql("(N'Flowers')");

                entity.Property(e => e.HumidTolerance)
                    .HasMaxLength(3)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.IsDefault)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Lifespan).HasDefaultValueSql("((1))");

                entity.Property(e => e.Pollination).HasDefaultValueSql("((1))");

                entity.Property(e => e.Speed).HasDefaultValueSql("((1))");

                entity.Property(e => e.TempTolerance)
                    .HasMaxLength(3)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();

                entity.Property(e => e.Territory)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Specie)
                    .WithMany(p => p.SpecieStats)
                    .HasForeignKey(d => d.SpecieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecieStats_Specie");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
