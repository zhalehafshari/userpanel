using Common.Model;
using Infrastructure.Efcore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.AspcoreIdentity
{
    public class ProjectRoleStore : IQueryableRoleStore<Role>
    {
       
        private readonly ProjectDbContext db;

        public IQueryable<Role> Roles =>db.Roles;

        public ProjectRoleStore(ProjectDbContext db)
        {
            this.db = db;
        }
        public void Dispose()
        {

        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            IdentityResult result;
            try
            {
                await db.Roles.AddAsync(role);
                await db.SaveChangesAsync(cancellationToken);
                result = IdentityResult.Success;
            }
            catch (Exception e)
            {
                var error = new IdentityError{ Description="Can Not Added the Roles"};
                result = IdentityResult.Failed(error);
                
            }
            return result;
        }

        
        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role =  db.Roles.Include(ur=>ur.users) .FirstOrDefault(r => r.Id == roleId);
            return Task.FromResult(role);
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return FindByIdAsync(normalizedRoleName, cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id = normalizedName);
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id = roleName);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            IdentityResult result;
            try
            {
                db.Roles.Update(role);
                await db.SaveChangesAsync();
                result = IdentityResult.Success;
            }
            catch (Exception )
            {
                var error = new IdentityError { Description = "can not update" };
                result = IdentityResult.Failed(error);
            }
            return result;
        }
        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
