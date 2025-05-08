using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Backend.Core.Context;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{

}
