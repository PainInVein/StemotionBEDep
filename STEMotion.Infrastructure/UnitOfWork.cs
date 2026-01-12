using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Domain.Entities;
using STEMotion.Infrastructure.DBContext;
using STEMotion.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StemotionContext _context;

        public UnitOfWork(StemotionContext context)
        {
            _context = context;
        }



        // IDisposable Implementation
        private bool disposed = false;

        private IUserRepository _userRepository;

        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

        private IRoleRepository _roleRepository;
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
