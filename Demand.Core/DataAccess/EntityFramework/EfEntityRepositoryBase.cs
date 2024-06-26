﻿using Demand.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Demand.Core.DataAccess.EntityFramework;

public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
where TEntity : class, IEntity, new()
where TContext : DbContext, new()
{
    public void Add(TEntity entity)
    {
        using (var context = new TContext())
        {
            var addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Added;
            context.SaveChanges();
        }
    }

    public void Delete(TEntity entity)
    {
        using (var context = new TContext())
        {
            var deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }
    }

    public TEntity Get(Expression<Func<TEntity, bool>> filter)
    {
        using (var context = new TContext())
        {
            return context.Set<TEntity>().SingleOrDefault(filter);
        }
    }

    public IList<TEntity> GetAll()
    {
        using (var context = new TContext())
        {
            try
            {
                return context.Set<TEntity>().ToList();

            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
    }

    public virtual IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
    {
        using (var context = new TContext())
        {
            try
            {
                return filter == null
                ? context.Set<TEntity>().ToList()
                : context.Set<TEntity>().Where(filter).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
  
        }
    }

    public virtual TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
    {
        using (var context = new TContext())
        {
            try
            {
                return context.Set<TEntity>().FirstOrDefault(filter);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

    public void Update(TEntity entity)
    {
        using (var context = new TContext())
        {
            var updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}

