﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SmarterThanYou.Data.Contracts
{
    public interface IGenericRepository<T>
        where T : class
    {
        T GetById(object id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression);

        IEnumerable<T> GetAll<T1>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T1>> sortExpression);

        IEnumerable<T2> GetAll<T1, T2>(Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, T1>> sortExpression,
            Expression<Func<T, T2>> selectExpression);

        IEnumerable<T> GetAll<T1>(Expression<Func<T, T1>> sortExpression,
            int? skip = null,
            int? take = null);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
