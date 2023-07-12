using Microsoft.EntityFrameworkCore;
using WonnaKnow.Models;
using System;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<People> Birthdays { get; set; }
}
