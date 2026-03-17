using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Persisntece
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        Task RollbackAsync();
    }
}
