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

        public virtual DbSet<Apiary> Apiaries { get; set; } = null!;
        public virtual DbSet<ApiaryComputer> ApiaryComputers { get; set; } = null!;
        public virtual DbSet<ApiaryMod> ApiaryMods { get; set; } = null!;
        public virtual DbSet<BiomeInfo> BiomeInfos { get; set; } = null!;
        public virtual DbSet<ComputerBindRequest> ComputerBindRequests { get; set; } = null!;
        public virtual DbSet<Flower> Flowers { get; set; } = null!;
        public virtual DbSet<Inventory> Inventories { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemDropChance> ItemDropChances { get; set; } = null!;
        public virtual DbSet<Mod> Mods { get; set; } = null!;
        public virtual DbSet<MutationChance> MutationChances { get; set; } = null!;
        public virtual DbSet<MutationChancesHived> MutationChancesHiveds { get; set; } = null!;
        public virtual DbSet<Specie> Species { get; set; } = null!;
        public virtual DbSet<SpecieFull> SpecieFulls { get; set; } = null!;
        public virtual DbSet<SpecieNote> SpecieNotes { get; set; } = null!;
        public virtual DbSet<SpecieStat> SpecieStats { get; set; } = null!;
        public virtual DbSet<TransposerDatum> TransposerData { get; set; } = null!;
        public virtual DbSet<TransposerFlower> TransposerFlowers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=IVAN-DARKHOLME\\SQLEXPRESS;Database=BeeBreeder;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apiary>(entity =>
            {
                entity.ToTable("Apiary");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<ApiaryComputer>(entity =>
            {
                entity.ToTable("ApiaryComputer");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.InGameIdentifier).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.OpenComputersIdentifier).HasMaxLength(50);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.Apiary)
                    .WithMany(p => p.ApiaryComputers)
                    .HasForeignKey(d => d.ApiaryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_ApiaryComputer_Apiary");
            });

            modelBuilder.Entity<ApiaryMod>(entity =>
            {
                entity.ToTable("ApiaryMod");

                entity.HasOne(d => d.Apiary)
                    .WithMany(p => p.ApiaryMods)
                    .HasForeignKey(d => d.ApiaryId)
                    .HasConstraintName("FK_ApiaryMod_Apiary");

                entity.HasOne(d => d.Mod)
                    .WithMany(p => p.ApiaryMods)
                    .HasForeignKey(d => d.ModId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApiaryMod_Mod");
            });

            modelBuilder.Entity<BiomeInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BiomeInfo");

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<ComputerBindRequest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ComputerBindRequest");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.ComputerId).HasMaxLength(50);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(50);
            });

            modelBuilder.Entity<Flower>(entity =>
            {
                entity.ToTable("Flower");

                entity.Property(e => e.Name).HasMaxLength(15);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.TransposerId, e.Side });

                entity.ToTable("Inventory");

                entity.Property(e => e.TransposerId).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.InGameId).HasMaxLength(70);

                entity.Property(e => e.InGameLabel).HasMaxLength(70);

                entity.Property(e => e.Name).HasMaxLength(70);

                entity.HasOne(d => d.ItemUnder)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.ItemUnderId)
                    .HasConstraintName("FK_Inventory_Item");

                entity.HasOne(d => d.Transposer)
                    .WithMany(p => p.Inventories)
                    .HasForeignKey(d => d.TransposerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Inventory_TransposerData");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Name).HasMaxLength(40);
            });

            modelBuilder.Entity<ItemDropChance>(entity =>
            {
                entity.ToTable("ItemDropChance");

                entity.HasIndex(e => e.ItemId, "IX_ItemDropChance_ItemId");

                entity.HasIndex(e => e.SpecieId, "IX_ItemDropChance_SpecieId");

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

                entity.HasIndex(e => e.FirstId, "IX_MutationChance_FirstId");

                entity.HasIndex(e => e.ResultId, "IX_MutationChance_ResultId");

                entity.HasIndex(e => e.SecondId, "IX_MutationChance_SecondId");

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

            modelBuilder.Entity<MutationChancesHived>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MutationChancesHived");

                entity.Property(e => e.FirstName).HasMaxLength(30);

                entity.Property(e => e.ResultName).HasMaxLength(30);

                entity.Property(e => e.SecondName).HasMaxLength(30);
            });

            modelBuilder.Entity<Specie>(entity =>
            {
                entity.ToTable("Specie");

                entity.HasIndex(e => e.ModId, "IX_Specie_ModId");

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

            modelBuilder.Entity<SpecieFull>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("SpecieFull");

                entity.Property(e => e.DiscoveredBy)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Effect).HasMaxLength(50);

                entity.Property(e => e.Flowers).HasMaxLength(15);

                entity.Property(e => e.HumidTolerance)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.LatinName).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Notes).HasMaxLength(4000);

                entity.Property(e => e.TempTolerance)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Territory).HasMaxLength(10);
            });

            modelBuilder.Entity<SpecieNote>(entity =>
            {
                entity.ToTable("SpecieNote");

                entity.HasIndex(e => e.SpecieId, "IX_SpecieNote_SpecieId");

                entity.Property(e => e.Text).HasMaxLength(300);

                entity.HasOne(d => d.Specie)
                    .WithMany(p => p.SpecieNotes)
                    .HasForeignKey(d => d.SpecieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecieNote_Specie");
            });

            modelBuilder.Entity<SpecieStat>(entity =>
            {
                entity.HasIndex(e => e.SpecieId, "IX_SpecieStats_SpecieId");

                entity.Property(e => e.Effect).HasMaxLength(50);

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

            modelBuilder.Entity<TransposerDatum>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Biome).HasMaxLength(20);

                entity.Property(e => e.Description)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Computer)
                    .WithMany(p => p.TransposerData)
                    .HasForeignKey(d => d.ComputerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransposerData_ApiaryComputer");
            });

            modelBuilder.Entity<TransposerFlower>(entity =>
            {
                entity.ToTable("TransposerFlower");

                entity.Property(e => e.TransposerId).HasMaxLength(50);

                entity.HasOne(d => d.Flower)
                    .WithMany(p => p.TransposerFlowers)
                    .HasForeignKey(d => d.FlowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransposerFlower_Flower");

                entity.HasOne(d => d.Transposer)
                    .WithMany(p => p.TransposerFlowers)
                    .HasForeignKey(d => d.TransposerId)
                    .HasConstraintName("FK_TransposerFlower_TransposerData");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
