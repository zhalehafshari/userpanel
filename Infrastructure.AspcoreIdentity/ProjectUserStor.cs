
using Common.Model;
using Infrastructure.Efcore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.AspcoreIdentity
{
    public class ProjectUserStor : IQueryableUserStore<User>,IUserPasswordStore<User>
    {
        private readonly ProjectDbContext db;
       

        public ProjectUserStor(ProjectDbContext db)
        {
            this.db = db;
          
        }
        public IQueryable<User> Users =>db.Users;
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken) 
        {
            IdentityResult result ;
            try
            {
               
                await db.Users.AddAsync(user, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                result = IdentityResult.Success;
            }
            catch (Exception e)
            {
                var Error = new IdentityError
                {
                    Description = e.Message
                };
                result = IdentityResult.Failed(Error);                
            }
            return result;
        }
        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
           
        }
        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = db.Users.SingleOrDefault(u => u.ID == userId);
            return Task.FromResult(user);
        }
        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user =await db.Users.SingleOrDefaultAsync(u => u.ID == normalizedUserName);
            return (user);
        }
        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ID.ToLower());
        }      
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ID);
        }
        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.ID);
        }
        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.ID = normalizedName;
            return Task.CompletedTask;
        }       
        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken) 
        {
            userName = user.ID;
            return Task.CompletedTask;
        }
        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
           
            return Task.FromResult(user.Password);
        }
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password =passwordHash;
            return Task.CompletedTask;

        }
        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }


}
