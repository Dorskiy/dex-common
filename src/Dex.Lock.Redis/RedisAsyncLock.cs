using System;
using System.Threading.Tasks;
using Dex.Lock.Async;
using Dex.Lock.Async.Impl;
using StackExchange.Redis;

namespace Dex.Lock.Redis
{
    internal class RedisAsyncLock : IAsyncLock
    {
        private const int MaxTries = 50;
        private const int IterationDelay = 50;
        private readonly IDatabase _database;
        private readonly string _key;

        // TODO check database for support, dirty connection close, Danilov
        internal RedisAsyncLock(IDatabase database, string key)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public async Task LockAsync(Func<Task> asyncAction)
        {
            if (asyncAction == null) throw new ArgumentNullException(nameof(asyncAction));
            var c = MaxTries;
            while (c-- > 0)
            {
                var success = await _database.LockTakeAsync(_key, string.Empty, TimeSpan.MaxValue).ConfigureAwait(false);
                if (success)
                {
                    try
                    {
                        await asyncAction().ConfigureAwait(false);
                    }
                    finally
                    {
                        await RemoveLockObject().ConfigureAwait(false);
                    }

                    return;
                }

                await Task.Delay(IterationDelay).ConfigureAwait(false);
            }

            throw new TimeoutException("[" + IterationDelay * MaxTries + "ms]");
        }

        internal Task<bool> RemoveLockObject()
        {
            return _database.LockReleaseAsync(_key, string.Empty);
        }

        public async Task LockAsync(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            Task Act()
            {
                action();
                return Task.FromResult(true);
            }

            await LockAsync(Act).ConfigureAwait(false);
        }

        public ValueTask<LockReleaser> LockAsync()
        {
            throw new NotImplementedException();
        }
    }
}