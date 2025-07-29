using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace screensound.core.data.dal;

public abstract class BaseDAL<T, U>(U context) where T : class where U : DbContext
{
    protected readonly U Context = context;

    public List<T> GetList() => GetListAsync().Result;
    public async Task<List<T>> GetListAsync()
    {
        List<T> output = [];
        await foreach (T item in Context.Set<T>())
            output.Add(item);
        return output;
    }

    public EntityEntry<T> Add(T item) => AddAsync(item).Result;
    public async Task<EntityEntry<T>> AddAsync(T item)
    {
        ValueTask<EntityEntry<T>> task = Context.Set<T>().AddAsync(item);
        EntityEntry<T> output = await task;
        Context.SaveChanges();
        return output;
    }

    public EntityEntry<T> Update(T item) => UpdateAsync(item).Result;
    public async Task<EntityEntry<T>> UpdateAsync(T item)
    {
        EntityEntry<T> result = Context.Set<T>().Update(item);
        await Context.SaveChangesAsync();
        return result;
    }

    public EntityEntry<T> Remove(T item) => RemoveAsync(item).Result;
    public async Task<EntityEntry<T>> RemoveAsync(T item)
    {
        EntityEntry<T> result = Context.Set<T>().Remove(item);
        await Context.SaveChangesAsync();
        return result;
    }

    public List<T> Where(Predicate<T> predicate) => WhereAsync(predicate).Result;
    public async Task<List<T>> WhereAsync(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        List<T> output = [];
        await foreach (T item in Context.Set<T>())
            if (predicate.Invoke(item))
                output.Add(item);
        return output;
    }

    public T? First(Predicate<T> predicate) => FirstAsync(predicate).Result;
    public async Task<T?> FirstAsync(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));

        await foreach (T item in Context.Set<T>())
            if (predicate.Invoke(item))
                return item;

        return null;
    }
}
