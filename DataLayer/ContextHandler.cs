using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ContextHandler<TEntity> where TEntity: class
    {

        #region Create
        public async Task<TEntity> Add(TEntity item)
        {
            return (await this.AddRange(new[] { item }))[0];
        }

        public async Task<TEntity[]> AddRange(params TEntity[] items)
        {
            using (var context = new Entities())
            {
                context.Set<TEntity>().AddRange(items);
                await context.SaveChangesAsync();
                return items;
            }
        }
        #endregion

        #region Read
        public async Task<TEntity> Get (params object[] ids)
        {
            using (var context = new Entities())
            {
                return await context.Set<TEntity>().FindAsync(ids);
            }
        }

        public TEntity Find(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            return this.FindAll(where, includes).FirstOrDefault();
        }

        public ICollection<TEntity> FindAll (Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new Entities())
            {
                var result = context.Set<TEntity>().Where(where).AsQueryable();
                result = includes.Aggregate(result, (current, include) => current.Include(include));
                return result.ToList();
            }
        }
        #endregion

        #region Update
        public async Task<TEntity> Update (TEntity updated, params string[] objectIds)
        {
            return (await this.Update(new [] { updated }, objectIds))[0];
        }

        public async Task<TEntity[]> Update(TEntity[] updated, params string[] objectIds)
        {
            using (var context = new Entities())
            {
                var noOfIds = objectIds.Length;
                foreach (var item in updated)
                {
                    var ids = new List<object>();
                    foreach (var id in objectIds) { ids.Add(item.GetType().GetProperty(id).GetValue(item)); }

                    var dbItem = await context.Set<TEntity>().FindAsync(ids.ToArray());
                    context.Entry(dbItem).CurrentValues.SetValues(item);
                }
                await context.SaveChangesAsync();
                return updated;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> Delete (params TEntity[] entities)
        {
            using (var context = new Entities())
            {
                foreach (var entity in entities) { context.Set<TEntity>().Attach(entity); }
                context.Set<TEntity>().RemoveRange(entities);
                return (await context.SaveChangesAsync()) == entities.Length;
            }
        }
        #endregion
    }
}
