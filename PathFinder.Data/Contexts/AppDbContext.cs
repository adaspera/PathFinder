using Microsoft.EntityFrameworkCore;
using PathFinder.Server.Models;

namespace PathFinder.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Stop> Stops { get; set; }
    public DbSet<Agency> Agencies { get; set; }
    public DbSet<Route> Routes { get; set; }
}