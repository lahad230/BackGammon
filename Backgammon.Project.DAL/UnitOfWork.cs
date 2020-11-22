using Backgammon.Project.DAL.DataContext;
using Backgammon.Project.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backgammon.Project.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly BgDataContext _context;

        public UnitOfWork(BgDataContext context)
        {
            _context = context;
        }

        private UsersRepository _usersRepository;

        public UsersRepository UsersRepository
        {
            get
            {
                if (_usersRepository == null)
                {
                    _usersRepository = new UsersRepository(_context);
                }
                return _usersRepository;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
