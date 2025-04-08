using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SGCP.Models;

public partial class SGCP_DbContext : DbContext
{
    public SGCP_DbContext()
    {
    }

    public SGCP_DbContext(DbContextOptions<SGCP_DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Component> Components { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryMovement> InventoryMovements { get; set; }

    public virtual DbSet<LaborCost> LaborCosts { get; set; }

    public virtual DbSet<LaborType> LaborTypes { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductComponent> ProductComponents { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RegionalsPrice> RegionalsPrices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=jnp_janus;Database=SGCP;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07A13FC1A7");

            entity.ToTable("Categories", "Cat");

            entity.HasIndex(e => e.Type, "IX_Categories_Type");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Component>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC277A1FE1F3");

            entity.ToTable("Components", "Prod");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UnitCost).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Components)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Components_Categories");

            entity.HasOne(d => d.Unit).WithMany(p => p.Components)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Component__UnitI__797309D9");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Currenci__3214EC07087C912A");

            entity.ToTable("Currencies", "Cat");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Symbol).HasMaxLength(10);
            entity.Property(e => e.TasaCambio).HasColumnType("numeric(10, 6)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC07511D161D");

            entity.ToTable("Inventories", "Inv");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.EntityTipe).HasMaxLength(50);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Min)
                .HasDefaultValue(0.0m)
                .HasColumnType("numeric(10, 2)");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0.0m)
                .HasColumnType("numeric(10, 2)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Unit).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventori__UnitI__73BA3083");
        });

        modelBuilder.Entity<InventoryMovement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC07CFF80618");

            entity.ToTable("InventoryMovements", "Inv");

            entity.HasIndex(e => e.Date, "IX_InventoryMovements_Date");

            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MovementType).HasMaxLength(50);
            entity.Property(e => e.Quantity).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Inventory).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.InventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Inven__74AE54BC");

            entity.HasOne(d => d.Location).WithMany(p => p.InventoryMovements)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMovements_Locations");
        });

        modelBuilder.Entity<LaborCost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LaborCos__3214EC07D50800C4");

            entity.ToTable("LaborCost", "Prod");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserInsert).HasMaxLength(100);
            entity.Property(e => e.UserUpdate).HasMaxLength(100);

            entity.HasOne(d => d.LaborType).WithMany(p => p.LaborCosts)
                .HasForeignKey(d => d.LaborTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LaborCost_LaborTypes");

            entity.HasOne(d => d.Product).WithMany(p => p.LaborCosts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LaborCost_Products");

            entity.HasOne(d => d.Unit).WithMany(p => p.LaborCosts)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LaborCost_Units");
        });

        modelBuilder.Entity<LaborType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LaborTyp__3214EC07D4D61436");

            entity.ToTable("LaborTypes", "Prod");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.HourlyCost).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07929B62B2");

            entity.ToTable("Locations", "Cat");

            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC071C9009D1");

            entity.ToTable("Permissions", "Sec");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07783DB36A");

            entity.ToTable("Products", "Prod");

            entity.HasIndex(e => e.Name, "IX_Products_Name");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.FinalPrice)
                .HasDefaultValue(0.0m)
                .HasColumnType("numeric(10, 2)");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__7D439ABD");
        });

        modelBuilder.Entity<ProductComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC0750712BC7");

            entity.ToTable("ProductComponents", "Prod");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Component).WithMany(p => p.ProductComponents)
                .HasForeignKey(d => d.ComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCo__Compo__7B5B524B");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductComponents)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCo__Produ__7C4F7684");

            entity.HasOne(d => d.Unit).WithMany(p => p.ProductComponents)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductComponents_Units");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductP__3214EC07199E84BE");

            entity.ToTable("ProductPrices", "Prod");

            entity.HasIndex(e => e.ProductId, "UQ__ProductP__B40CC6CC71E25535").IsUnique();

            entity.Property(e => e.Cost).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RetailRealPrice).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.RetailSuggestedPrice).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserInsert).HasMaxLength(100);
            entity.Property(e => e.UserUpdate).HasMaxLength(100);
            entity.Property(e => e.WholesaleRealPrice).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.WholesaleSuggestedPrice).HasColumnType("decimal(18, 4)");

            entity.HasOne(d => d.Product).WithOne(p => p.ProductPrice)
                .HasForeignKey<ProductPrice>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPrices_Products");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07832F3CE2");

            entity.ToTable("RefreshTokens", "Sec");

            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(512);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshTokens_Users");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Regions__3214EC27D475998F");

            entity.ToTable("Regions", "Cat");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RegionalsPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Regional__3214EC0716938F54");

            entity.ToTable("RegionalsPrices", "Prec");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");
            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Currency).WithMany(p => p.RegionalsPrices)
                .HasForeignKey(d => d.CurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Regionals__Curre__76969D2E");

            entity.HasOne(d => d.Product).WithMany(p => p.RegionalsPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Regionals__Produ__778AC167");

            entity.HasOne(d => d.Region).WithMany(p => p.RegionalsPrices)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Regionals__Regio__787EE5A0");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07A24EDFDA");

            entity.ToTable("Roles", "Sec");

            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("PK__RolePerm__6400A1A8FB7D272F");

            entity.ToTable("RolePermissions", "Sec");

            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__RolePermi__Permi__70A8B9AE");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__RolePermi__RoleI__6FB49575");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Units__3214EC07B2084E9F");

            entity.ToTable("Units", "Cat");

            entity.Property(e => e.ConversionBase)
                .HasDefaultValue(1.0m)
                .HasColumnType("numeric(10, 6)");
            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Symbol).HasMaxLength(10);
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC076C46F3ED");

            entity.ToTable("Users", "Sec");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Enable).HasDefaultValue(true);
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(512);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("PK__UserRole__AF2760AD3A506C57");

            entity.ToTable("UserRoles", "Sec");

            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRoles__RoleI__7849DB76");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoles__UserI__7755B73D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
