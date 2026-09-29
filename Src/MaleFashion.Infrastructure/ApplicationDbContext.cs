using Demo.Infrastructure.Identity;
using MaleFashion.Domain.Entities;
using MaleFashion.Infrastructure.Data.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;

namespace MaleFashion.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser,
      ApplicationRole,
      Guid,
      ApplicationUserClaim,
      ApplicationUserRole,
      ApplicationUserLogin,
      ApplicationRoleClaim,
      ApplicationUserToken>(options)
    {











        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<Category>()
    .ToTable("Categories");

            builder.Entity<Discount>()
    .ToTable("Discount");

            // =========================================================
            // CATEGORY -> PRODUCT
            // 1 : MANY
            // =========================================================

            builder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================================================
            // PRODUCT -> VARIANT
            // 1 : MANY
            // =========================================================

            builder.Entity<Product>()
                .HasMany(x => x.Variants)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================================================
            // VARIANT -> INVENTORY
            // 1 : 1
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasOne(x => x.Inventory)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey<Inventory>(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Inventory>()
                .HasIndex(x => x.ProductVariantId)
                .IsUnique();



            // =========================================================
            // VARIANT -> IMAGES
            // 1 : MANY
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasMany(x => x.Images)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================================================
            // USER -> CART
            // 1 : 1
            //
            // Domain only contains:
            // Guid UserId
            //
            // ApplicationUser is defined in Infrastructure.Identity
            // =========================================================

            builder.Entity<Cart>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Cart>()
                .HasIndex(x => x.UserId);
              


            // =========================================================
            // CART -> CART ITEMS
            // 1 : MANY
            // =========================================================

            builder.Entity<Cart>()
                .HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================================================
            // PRODUCT VARIANT -> CART ITEMS
            // 1 : MANY
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasMany(x => x.CartItems)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);



            // =========================================================
            // CART
            // ONE VARIANT CAN APPEAR ONLY ONCE IN A CART
            // =========================================================

            builder.Entity<CartItem>()
                .HasIndex(x => new
                {
                    x.CartId,
                    x.ProductVariantId
                })
                .IsUnique();


            // =========================================================
            // USER -> ORDER
            // 1 : MANY

            // One customer can have many orders.
            // =========================================================

            builder.Entity<Order>()
               .HasKey(x => x.Id);

            builder.Entity<Order>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================================================
            // ORDER -> ORDER ITEMS
            // 1 : MANY
            // =========================================================

            builder.Entity<Order>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            // =================================================
            // ORDER -> PAYMENT
            // ONE ORDER HAS ONE PAYMENT
            // =================================================

            builder.Entity<Order>()
                .HasOne(x => x.Payment)
                .WithOne(x => x.Order)
                .HasForeignKey<Payment>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);


            // =================================================
            // PAYMENT -> ORDER
            //
            // Payment.OrderId is unique because this is
            // a ONE-TO-ONE relationship.
            // =================================================

            builder.Entity<Payment>()
                .HasIndex(x => x.OrderId)
                .IsUnique();



            // =================================================
            // ORDER ITEM
            // =================================================

            builder.Entity<OrderItem>()
                .HasKey(x => x.Id);


            // =========================================================
            // PRODUCT VARIANT -> ORDER ITEMS
            // 1 : MANY
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);



            // =========================================================
            // USER -> WISHLIST
            // 1 : 1
            // =========================================================

            builder.Entity<WishList>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WishList>()
         .HasIndex(x => x.UserId);

            // =========================================================
            // WISHLIST -> WISHLIST ITEMS
            // 1 : MANY
            // =========================================================

            builder.Entity<WishList>()
                .HasMany(x => x.Items)
                .WithOne(x => x.Wishlist)
                .HasForeignKey(x => x.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);


            // =========================================================
            // PRODUCT VARIANT -> WISHLIST ITEMS
            // 1 : MANY
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasMany(x => x.WishlistItems)
                .WithOne(x => x.ProductVariant)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================================================
            // WISHLIST
            // NO DUPLICATE VARIANT
            // =========================================================

            builder.Entity<WishlistItem>()
                .HasIndex(x => new
                {
                    x.WishlistId,
                    x.ProductVariantId
                })
                .IsUnique();


            // =========================================================
            // PRODUCT VARIANT
            // UNIQUE SKU
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasIndex(x => x.Sku)
                .IsUnique();


            // =========================================================
            // PRODUCT VARIANT
            // UNIQUE PRODUCT + COLOR + SIZE
            //
            // =========================================================

            builder.Entity<ProductVariant>()
                .HasIndex(x => new
                {
                    x.ProductId,
                    x.Color,
                    x.Size
                })
                .IsUnique();


            // =========================================================
            // PRODUCT TAGS
            // sql server
            //

            builder.Entity<Product>()
            .Property(x => x.Tags)
            .HasConversion(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<string>>(
            v,
            (JsonSerializerOptions?)null
            ) ?? new List<string>()
          );


            // =========================================================
            // DISCOUNT CODE
            // UNIQUE
            // =========================================================

            builder.Entity<Discount>()
                .HasIndex(x => x.Code)
                .IsUnique();



            // =========================================================
            // ORDER PAYMENT TYPE
            // ENUM -> INTEGER
            // =========================================================

            builder.Entity<Payment>()
                .Property(x => x.PaymentType)
                .HasConversion<int>();

            builder.Entity<Payment>()
             .Property(x => x.Status)
             .HasConversion<int>();


            // =========================================================
            // DECIMAL PRECISION
            // MONEY / PRICE FIELDS
            // SQL SERVER
            // =========================================================

            builder.Entity<Product>()
                .Property(x => x.ProductPrize)
                .HasPrecision(18, 2);

            builder.Entity<CartItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(x => x.Subtotal)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(x => x.DiscountAmount)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(x => x.Total)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 2);

            builder.Entity<Discount>()
                .Property(x => x.DiscountPercentage)
                .HasPrecision(18, 2);


            //Payment
            builder.Entity<Payment>()
                .Property(x => x.Amount)
                .HasPrecision(18, 2);


            builder.Entity<Payment>()
                .Property(x => x.TransactionId)
                .HasMaxLength(200)
                .IsRequired(false);


            builder.Entity<Payment>()
                .Property(x => x.CreatedAt)
                .IsRequired();

            builder.Entity<Payment>()
                .Property(x => x.PaidAt)
                .IsRequired(false);




            //Seeding Data
            builder.Entity<ApplicationRole>().HasData(Data.Seeds.RoleSeeds.GetRoles());

            builder.Entity<ApplicationUser>().HasData(Data.Seeds.UserSeeds.GetUsers());

            builder.Entity<ApplicationUserRole>() .HasData(Data.Seeds.UserRoleSeeds.GetUserRoles());

        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }

        public DbSet<Discount> Discount { get; set; }
        public DbSet<WishList> WishLists { get; set; }

        public DbSet<WishlistItem> WishlistItems { get; set; }




    }
}

