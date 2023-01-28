using Microsoft.EntityFrameworkCore;

namespace AddressBook.Entities.Models
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {
        }
        public override int SaveChanges()
        {
            OnBeforeSaving();

            return base.SaveChanges();
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnBeforeSaving();

            return await base.SaveChangesAsync(cancellationToken);
        }
        protected virtual void OnBeforeSaving()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                entityEntry.Property("DateUpdated").CurrentValue = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property("DateCreated").CurrentValue = DateTime.UtcNow;
                    entityEntry.Property("IsActive").CurrentValue = true;
                }
            }


        }
        public virtual DbSet<AddressModel> Address { get; set; }
        public virtual DbSet<EmailModel> Email { get; set; }
        public virtual DbSet<FileModel> File { get; set; }
        public virtual DbSet<PhoneModel> Phone { get; set; }
        public virtual DbSet<UserModel> User { get; set; }
        public virtual DbSet<RefsetModel> RefSet {get; set;}
        public virtual DbSet<ReftermModel> RefTerm {get; set;}
        public virtual DbSet<SetreftermModel> SetRefTerm {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Common.OnModelCreation(modelBuilder);
        }
    }
    
}