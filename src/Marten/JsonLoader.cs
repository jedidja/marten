using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Marten.Internal.Sessions;
using Marten.Linq.QueryHandlers;
using Marten.Services;
#nullable enable
namespace Marten
{
    internal class JsonLoader: IJsonLoader
    {
        private readonly QuerySession _session;

        public JsonLoader(QuerySession session)
        {
            _session = session;
        }

        public string? FindById<T>(string id) where T : class
        {
            return findJsonById<T, string>(id);
        }

        public Task<string?> FindByIdAsync<T>(string id, CancellationToken token) where T : class
        {
            return findJsonByIdAsync<T, string>(id, token);
        }

        public Task<string?> FindJsonByIdAsync<T>(int id, CancellationToken token) where T : class
        {
            return findJsonByIdAsync<T, int>(id, token);
        }

        private string? findJsonById<T, TId>(TId id) where T : notnull where TId : notnull
        {
            var storage = _session.QueryStorageFor<T, TId>();
            var command = storage.BuildLoadCommand(id, _session.Tenant);

            return _session.Database.LoadOne(command, LinqConstants.StringValueSelector);
        }

        private Task<string?> findJsonByIdAsync<T, TId>(TId id, CancellationToken token) where T : notnull where TId : notnull
        {
            var storage = _session.QueryStorageFor<T, TId>();
            var command = storage.BuildLoadCommand(id, _session.Tenant);

            return _session.Database.LoadOneAsync(command, LinqConstants.StringValueSelector, token);
        }

        public string? FindById<T>(int id) where T : class
        {
            return findJsonById<T, int>(id);
        }

        public string? FindById<T>(long id) where T : class
        {
            return findJsonById<T, long>(id);
        }

        public string? FindById<T>(Guid id) where T : class
        {
            return findJsonById<T, Guid>(id);
        }

        public Task<string?> FindByIdAsync<T>(int id, CancellationToken token = new CancellationToken()) where T : class
        {
            return findJsonByIdAsync<T, int>(id, token);
        }

        public Task<string?> FindByIdAsync<T>(long id, CancellationToken token = new CancellationToken()) where T : class
        {
            return findJsonByIdAsync<T, long>(id, token);
        }

        public Task<string?> FindByIdAsync<T>(Guid id, CancellationToken token = new CancellationToken()) where T : class
        {
            return findJsonByIdAsync<T, Guid>(id, token);
        }

        public Task<bool> StreamById<T>(int id, Stream destination, CancellationToken token = default) where T : class
        {
            return streamJsonById<T, int>(id, destination, token);
        }

        private Task<bool> streamJsonById<T, TId>(TId id, Stream destination, CancellationToken token) where T : class where TId : notnull
        {
            var storage = _session.QueryStorageFor<T, TId>();
            var command = storage.BuildLoadCommand(id, _session.Tenant);

            return _session.Database.StreamOne(command, destination, token);
        }

        public Task<bool> StreamById<T>(long id, Stream destination, CancellationToken token = default) where T : class
        {
            return streamJsonById<T, long>(id, destination, token);
        }

        public Task<bool> StreamById<T>(string id, Stream destination, CancellationToken token = default) where T : class
        {
            return streamJsonById<T, string>(id, destination, token);
        }

        public Task<bool> StreamById<T>(Guid id, Stream destination, CancellationToken token = default) where T : class
        {
            return streamJsonById<T, Guid>(id, destination, token);
        }
    }
}
