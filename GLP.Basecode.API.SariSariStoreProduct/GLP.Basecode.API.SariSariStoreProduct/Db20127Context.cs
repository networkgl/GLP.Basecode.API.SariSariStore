using System;
using System.Collections.Generic;
using GLP.Basecode.API.SariSariStoreProduct.Models;
using Microsoft.EntityFrameworkCore;

namespace GLP.Basecode.API.SariSariStoreProduct;

public partial class Db20127Context : DbContext
{
    public Db20127Context()
    {
    }

    public Db20127Context(DbContextOptions<Db20127Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<VwGetProductBy> VwGetProductBies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=db20127.public.databaseasp.net;User ID=db20127;Password=7Df#K%4eg3R_;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Barcode)
                .IsUnicode(false)
                .HasColumnName("barcode");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("product_name");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<VwGetProductBy>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_getProductBy");

            entity.Property(e => e.Barcode)
                .IsUnicode(false)
                .HasColumnName("barcode");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("product_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
