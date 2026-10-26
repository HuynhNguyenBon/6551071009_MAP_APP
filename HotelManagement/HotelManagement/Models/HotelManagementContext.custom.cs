using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Models;

public partial class HotelManagementContext
{
    public HotelManagementContext()
    {
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Server=MSI\SQLEXPRESS;Database=HotelManagement;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}