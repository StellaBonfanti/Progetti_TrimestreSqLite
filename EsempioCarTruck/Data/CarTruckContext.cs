using System;
using EsempioCarTruck.Model;
using Microsoft.EntityFrameworkCore;

namespace EsempioCarTruck.Data;

public class CarTruckContext:DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Car1> Cars1 { get; set; }
    public DbSet<Truck> Trucks { get; set; }
    public string DbPath { get; }
    public CarTruckContext()
    {
        var folder = AppContext.BaseDirectory; 
        var path = Path.Combine(folder, "../../../CarsTrucks.db");
        DbPath = path;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        =>optionsBuilder.UseSqlite($" Data Source = {DbPath}");
}
 