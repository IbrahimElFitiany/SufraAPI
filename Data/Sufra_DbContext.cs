using Microsoft.EntityFrameworkCore;
using System;
using Sufra_MVC.Models;
using Sufra_MVC.Models.Emps;
using Models.Reservation;
using Models.Orders;
using Sufra_MVC.Models.RestaurantModels;
using DTOs.ReservationDTOs;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.Orders;

namespace Sufra_MVC.Data
{
    public class Sufra_DbContext : DbContext
    {
        public Sufra_DbContext(DbContextOptions<Sufra_DbContext> options) : base(options) {
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantManager> RestaurantManagers { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }


        public DbSet<District> Districts { get; set; }
        public DbSet<Gov> Govs { get; set; }
        public DbSet<Customer> Customers { get; set; }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuSection> MenuSections { get; set; }


        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }


        public DbSet<Review> Reviews { get; set; }
        public DbSet<Complaint> Complaints { get; set; }


        public DbSet<SufraEmp> Sufra_Emps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // 1-to-1: Restaurant <-> RestaurantManager
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Manager)
                .WithOne(m => m.Restaurant)
                .HasForeignKey<Restaurant>(r => r.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: District <-> Restaurant
            modelBuilder.Entity<District>()
                .HasMany(d => d.Restaurants)
                .WithOne(r => r.District)
                .HasForeignKey(r => r.DistrictId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: Gov <-> District
            modelBuilder.Entity<Gov>()
                .HasMany(g => g.Districts)
                .WithOne(d => d.Gov)
                .HasForeignKey(d => d.GovId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: Cuisine <-> Restaurant
            modelBuilder.Entity<Cuisine>()
                .HasMany(c => c.Restaurants)
                .WithOne(r => r.Cuisine)
                .HasForeignKey(r => r.CuisineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cuisine>()
                .HasIndex(c => c.Name)
                .IsUnique();


            // 1-to-Many: Restaurant <-> MenuItem
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.MenuItems)
                .WithOne(mi => mi.Restaurant)
                .HasForeignKey(mi => mi.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: MenuSection <-> MenuItems
            modelBuilder.Entity<MenuSection>()
                .HasMany(ms => ms.MenuItems)
                .WithOne(mi => mi.MenuSection)
                .HasForeignKey(mi => mi.MenuSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuItem>()
                .HasIndex(m => new { m.RestaurantId, m.Name })
                .IsUnique();

            // Unique constraint: RestaurantId + Name
            modelBuilder.Entity<MenuSection>()
                .HasIndex(ms => new { ms.RestaurantId, ms.Name })
                .IsUnique();

            // 1-to-Many: Customer <-> Order
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: Order <-> OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);



            // One-to-One relationship between Customer and Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne() 
                .HasForeignKey<Cart>(c => c.CustomerId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .Navigation(c => c.CartItems)
                .HasField("_cartItems"); // Tell EF to use the private field _cartItems

            // One-to-Many relationship between Cart and Restaurant
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Restaurant)
                .WithMany()  
                .HasForeignKey(c => c.RestaurantId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring the foreign key for MenuItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.MenuItem)
                .WithMany() // Each MenuItem can be in many CartItems
                .HasForeignKey(ci => ci.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of MenuItems if they are in a CartItem

            // Configuring decimal precision for Price in CartItem
            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Price)
                .HasColumnType("decimal(18,2)");

            // Configuring range for Quantity in CartItem
            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Quantity)
                .HasDefaultValue(1); // Default value for quantity if not specified



            // 1-to-Many: Customer <-> Reservation
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Reservations)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: Restaurant <-> Reservation
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Reservations)
                .WithOne(rs => rs.Restaurant)
                .HasForeignKey(rs => rs.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);



            // 1-to-Many: Restaurant <-> Table
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Tables)
                .WithOne()
                .HasForeignKey("RestaurantId");

            modelBuilder.Entity<Restaurant>()
                .Navigation(r => r.Tables)
                .HasField("_tables");


            // 1-to-Many: Restaurant <-> OpeningHours
            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.OpeningHours)
                .WithOne()
                .HasForeignKey("RestaurantId");

            modelBuilder.Entity<Restaurant>()
                .Navigation(r => r.OpeningHours)
                .HasField("_openingHours");




            // 1-to-Many: Table <-> Reservation
            modelBuilder.Entity<Table>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Table)
                .HasForeignKey(r => r.TableId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
            .Property(r => r.Status)
            .HasConversion<string>();  // Store as string

            // 1-to-Many: Customer <-> Complaint
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Complaints)
                .WithOne(cp => cp.Customer)
                .HasForeignKey(cp => cp.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: Customer <-> Review
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // 1-to-Many: MenuItem <-> Review
            modelBuilder.Entity<MenuItem>()
                .HasMany(mi => mi.Reviews)
                .WithOne(r => r.MenuItem)
                .HasForeignKey(r => r.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<SufraEmp>()
            .HasCheckConstraint("CHK_Role", "\"Role\" IN ('Admin', 'Emp', 'Support')");

            modelBuilder.Entity<RestaurantOpeningHours>()
            .Property(roh => roh.DayOfWeek)
            .HasConversion<string>();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }

    }

