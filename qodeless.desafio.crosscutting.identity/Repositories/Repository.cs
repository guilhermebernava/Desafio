using Microsoft.EntityFrameworkCore;
using qodeless.desafio.domain.Entities;
using qodeless.desafio.domain.Interfaces;
using qodeless.desafio.domain.ViewModel;
using qodeless.desafio.Infra.CrossCutting.identity.Data;
using System;
using System.Linq;

namespace qodeless.desafio.crosscutting.identity.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext Db;

        protected readonly DbSet<TEntity> DbSet;

        public Repository(ApplicationDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        #region METHODS
        public bool Add(TEntity obj, bool bCommit = true)
        {
            DbSet.Add(obj);
            Db.Entry(obj).State = EntityState.Added;
            return !bCommit || SaveChanges() > 0;
        }

        public bool Any(Func<TEntity, bool> exp) => DbSet.AsNoTracking().Any(exp);

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool ForceDelete(Guid id, bool bCommit = true)
        {
            var obj = DbSet.Find(id);
            DbSet.Remove(obj);
            Db.Entry(obj).State = EntityState.Deleted;
            return !bCommit || SaveChanges() > 0;
        }

        public IQueryable<TEntity> GetAll() => DbSet.AsNoTracking();

        public IQueryable<TEntity> GetAllBy(Func<TEntity, bool> exp) => DbSet.Where(exp).AsQueryable().AsNoTracking();


        public IQueryable<UserViewModel> GetAllUsers()
        {
            return Db.Users.Select(x => new UserViewModel { Email = x.Email, Id = x.Id }).AsNoTracking();
        }

        public TEntity GetBy(Func<TEntity, bool> exp) => DbSet.AsNoTracking().FirstOrDefault(exp);


        public TEntity GetById(Guid id) => DbSet.Find(id);


        public bool None(Func<TEntity, bool> exp) => !DbSet.AsNoTracking().Any(exp);

        public bool Save(TEntity obj, bool bCommit = true)
        {
            var entity = obj as Entity;
            if (Guid.Empty == entity.Id)
                Add(obj, false);
            else
                Update(obj, false);

            return !bCommit || SaveChanges() > 0;
        }

        public int SaveChanges()
        {
            try
            {
                return Db.SaveChanges();
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public bool SoftDelete(TEntity obj)
        {
            var entity = obj as Entity;
            if (entity == null) return false;

            return ForceDelete(entity.Id, true);
        }

        public bool Update(TEntity obj, bool bCommit = true)
        {
            try
            {
                var entity = obj as Entity;
                if (entity == null) return false;

                Db.Entry(obj).State = EntityState.Detached;
                Db.SaveChanges();
                Db.Entry(obj).State = EntityState.Modified;
                entity.Update();
                DbSet.Update(obj);

                if (bCommit)
                    SaveChanges();

                Db.Entry(obj).State = EntityState.Detached;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return true;
        }

        public bool Upsert(TEntity obj, Func<TEntity, bool> exp, bool bCommit = true)
        {
            if (None(exp))
                return Add(obj, bCommit);
            else
                return Update(obj, bCommit);
        }
        #endregion
    }
}
