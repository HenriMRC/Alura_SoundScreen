using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace screensound.database.dal
{
    public abstract class DAL<T, U> where T : class where U : DbContext
    {
        protected readonly U context;

        protected abstract DbSet<T> DbSet { get; }

        protected DAL(U context)
        {
            this.context = context;
        }

        public List<T> GetList() => GetListAsync().Result;
        public async Task<List<T>> GetListAsync()
        {
            List<T> output = new();
            await foreach (T item in DbSet)
                output.Add(item);
            return output;
        }


        public EntityEntry<T> Add(T item) => AddAsync(item).Result;
        public async Task<EntityEntry<T>> AddAsync(T item)
        {
            ValueTask<EntityEntry<T>> task = DbSet.AddAsync(item);
            EntityEntry<T> output = await task;
            context.SaveChanges();
            return output;
        }

        public EntityEntry<T> Update(T item)
        {
            EntityEntry<T> result = DbSet.Update(item);
            context.SaveChanges();
            return result;
        }

        public EntityEntry<T> Remove(T item)
        {
            EntityEntry<T> result = DbSet.Remove(item);
            context.SaveChanges();
            return result;
        }
    }
}
