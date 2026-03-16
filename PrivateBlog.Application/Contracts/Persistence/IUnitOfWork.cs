using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        public Task CommitAsync();
        public Task RollbackAsync();
    }
}
