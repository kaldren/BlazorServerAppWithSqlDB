using Microsoft.EntityFrameworkCore;

namespace DemoBlazorServerApp.Data;

public class UserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ICollection<User>> GetUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }
}
