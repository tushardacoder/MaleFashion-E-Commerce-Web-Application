using MaleFashion.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{
    public class EfTransactionManager
    : ITransactionManager
    {
        private readonly ApplicationDbContext _context;

        private IDbContextTransaction? _transaction;

        public EfTransactionManager(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(
            CancellationToken cancellationToken )
        {
            _transaction =
                await _context.Database
                    .BeginTransactionAsync(
                        cancellationToken);
        }

        public async Task CommitTransactionAsync(
            CancellationToken cancellationToken)
        {
            if (_transaction == null)
            {
                return;
            }

            await _transaction.CommitAsync(
                cancellationToken);

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackTransactionAsync(
            CancellationToken cancellationToken)
        {
            if (_transaction == null)
            {
                return;
            }

            await _transaction.RollbackAsync(
                cancellationToken);

            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }
   }
