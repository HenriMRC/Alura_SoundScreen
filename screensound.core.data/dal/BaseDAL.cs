using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace screensound.core.data.dal
{
    public abstract class BaseDAL<T, U> where T : class where U : DbContext
    {
        protected readonly U context;

        protected BaseDAL(U context)
        {
            this.context = context;
        }

        public List<T> GetList() => GetListAsync().Result;
        public async Task<List<T>> GetListAsync()
        {
            List<T> output = new();
            await foreach (T item in context.Set<T>())
                output.Add(item);
            return output;
        }

        public EntityEntry<T> Add(T item) => AddAsync(item).Result;
        public async Task<EntityEntry<T>> AddAsync(T item)
        {
            ValueTask<EntityEntry<T>> task = context.Set<T>().AddAsync(item);
            EntityEntry<T> output = await task;
            context.SaveChanges();
            return output;
        }

        public EntityEntry<T> Update(T item) => UpdateAsync(item).Result;
        public async Task<EntityEntry<T>> UpdateAsync(T item)
        {
            EntityEntry<T> result = context.Set<T>().Update(item);
            await context.SaveChangesAsync();
            return result;
        }

        public EntityEntry<T> Remove(T item) => RemoveAsync(item).Result;
        public async Task<EntityEntry<T>> RemoveAsync(T item)
        {
            EntityEntry<T> result = context.Set<T>().Remove(item);
            await context.SaveChangesAsync();
            return result;
        }

        public List<T> Where(Predicate<T> predicate) => WhereAsync(predicate).Result;
        public async Task<List<T>> WhereAsync(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            List<T> output = new();
            await foreach (T item in context.Set<T>())
                if (predicate.Invoke(item))
                    output.Add(item);
            return output;
        }

        public T? First(Predicate<T> predicate) => FirstAsync(predicate).Result;
        public async Task<T?> FirstAsync(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            await foreach (T item in context.Set<T>())
                if (predicate.Invoke(item))
                    return item;

            return null;
        }
    }
}
