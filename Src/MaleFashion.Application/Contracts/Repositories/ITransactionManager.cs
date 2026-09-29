using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{
    public interface ITransactionManager
    {
        Task BeginTransactionAsync(
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(
            CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(
            CancellationToken cancellationToken = default);
    }
}
