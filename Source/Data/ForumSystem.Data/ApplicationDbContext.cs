namespace ForumSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    using Microsoft.AspNet.Identity.EntityFramework;

    using ForumSystem.Data.Common.Models;
    using ForumSystem.Data.Migrations;
    using ForumSystem.Data.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IDbSet<Tag> Tags { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            // automathic CreatedOn, ModifiedOn  data fields
            // ChangeTracker -> have information for everything changed in the Models
            // EntityState.Added -> just have been added
            // EntityState.Modified -> just have been modified
            // e.Entity is IAuditInfo -> implement IAuditInfo
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                // add new entry
                if (entry.State == EntityState.Added)
                {
                    // for new posted data, new rocord
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                // modify existing entry
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            // Automatic DeletedOn data field
            // when delete -> set DeletedOn, IsDeleted and State to modified (do not delet it from db)
            foreach (var entry in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IDeletableEntity))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
