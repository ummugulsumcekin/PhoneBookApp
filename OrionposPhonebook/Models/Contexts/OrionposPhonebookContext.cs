using Microsoft.EntityFrameworkCore;
using OrionposPhonebook.Models.Entities;
using OrionposPhonebook.Models.Entities.Abstract;

namespace OrionposPhonebook.Models.Contexts;

public class OrionposPhonebookContext : DbContext
{
    private readonly IHttpContextAccessor _contextAccessor;

    public OrionposPhonebookContext(
        DbContextOptions<OrionposPhonebookContext> options,
        IHttpContextAccessor contextAccessor) : base(options)
    {
        _contextAccessor = contextAccessor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasQueryFilter(contact => contact.IsDeleted == false);

    }



    public override int SaveChanges()
    {
        int? userId = null;

        if (_contextAccessor.HttpContext.Request.Cookies.TryGetValue("UserId", out var userIdStr))
        {
            if (int.TryParse(userIdStr, out var userIdParsed))
            {
                userId = userIdParsed;
            }
        }

        var added = ChangeTracker.Entries()
            .Where(entry => entry.State == EntityState.Added)
            .Select(entry => entry.Entity);

        var modified = ChangeTracker.Entries()
            .Where(entry => entry.State == EntityState.Modified)
            .Select(entry => entry.Entity);

        var deletedEntries = ChangeTracker.Entries()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var entity in added)
        {
            if (entity is IAuditEntity)
            {
                var auditEntity = (IAuditEntity)entity;
                auditEntity.CreatedAt = DateTime.Now;
                auditEntity.CreatedById = userId.Value;
            }
        }

        foreach (var entity in modified)
        {
            if (entity is IAuditEntity)
            {
                var auditEntity = (IAuditEntity)entity;
                auditEntity.UpdatedAt = DateTime.Now;
                auditEntity.UpdatedById = userId.Value;
            }
        }

        foreach (var entry in deletedEntries)
        {
            if (entry.Entity is IAuditEntity)
            {
                var auditEntity = (IAuditEntity)entry.Entity;
                auditEntity.DeletedAt = DateTime.Now;
                auditEntity.DeletedById = userId.Value;
            }

            if (entry.Entity is ISoftDeleteEntity)
            {
                var softDeleteEntity = (ISoftDeleteEntity)entry.Entity;
                softDeleteEntity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }

        return base.SaveChanges();
    }
}