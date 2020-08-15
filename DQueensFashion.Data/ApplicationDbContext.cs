using DQueensFashion.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<WishList> WishLit { get; set; }
        public DbSet<LineItem> LineItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<ImageFile> Image { get; set; }
        public DbSet<MailingList> MailingList { get; set; }
        public DbSet<GeneralValues> GeneralValues { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<OutfitSample> OutfitSample { get; set; }
        public DbSet<OutfitSampleImageFile> OutfitSampleImageFile { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}
