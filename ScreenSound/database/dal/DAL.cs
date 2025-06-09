using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.database.dal
{
    internal abstract class DAL<T, U> where T : class where U : DbContext
    {
        protected readonly U context;

        protected abstract DbSet<T> DbSet { get; }

        protected DAL(U context)
        {
            this.context = context;
        }

        internal List<T> GetList() => GetListAsync().Result;
        internal async Task<List<T>> GetListAsync()
        {
            List<T> output = new();
            await foreach (T item in DbSet)
                output.Add(item);
            return output;
        }


        internal EntityEntry<T> Add(T artist)
        {
            EntityEntry<T> result = DbSet.Add(artist);
            context.SaveChanges();
            return result;
        }

        internal EntityEntry<T> Update(T artist)
        {
            EntityEntry<T> result = DbSet.Update(artist);
            context.SaveChanges();
            return result;
        }

        internal EntityEntry<T> Remove(T artist)
        {
            EntityEntry<T> result = DbSet.Update(artist);
            context.SaveChanges();
            return result;
        }
    }
}
