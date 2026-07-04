using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIE.Core.Common
{
    /// <summary>
    /// 表达式扩展（扩展OR + AND）
    /// </summary>
    public static class ExpressionEx
    {
        private const string CONTAINS = "Contains";

        /// <summary>
        /// 参数绑定器
        /// </summary>
        public class ParameterRebinder : ExpressionVisitor
        {
            private readonly Dictionary<ParameterExpression, ParameterExpression> dicMap;

            private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                dicMap = dicMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// 替换参数
            /// </summary>
            /// <param name="map"></param>
            /// <param name="exp"></param>
            /// <returns></returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
                Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// 访问参数
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (dicMap.TryGetValue(node, out replacement))
                    node = replacement;
                return base.VisitParameter(node);
            }
        }

        /// <summary>
        ///  以 Expression.AndAlso 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            if (left == null)
                return right;
            return left.Compose(right, Expression.AndAlso);
        }

        /// <summary>
        /// 以 Expression.AndAlso 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, T1, bool>> And<T, T1>(this Expression<Func<T, T1, bool>> left,
            Expression<Func<T, T1, bool>> right)
        {
            if (left == null)
                return right;
            return left.Compose(right, Expression.AndAlso);
        }

        /// <summary>
        /// 以 Expression.AndAlso 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, T1, T2, bool>> And<T, T1, T2>(this Expression<Func<T, T1, T2, bool>> left,
            Expression<Func<T, T1, T2, bool>> right)
        {
            if (left == null)
                return right;
            return left.Compose(right, Expression.AndAlso);
        }

        /// <summary>
        /// 以 Expression.OrElse 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            if (left == null)
                return right;
            return left.Compose(right, Expression.OrElse);
        }


        /// <summary>
        /// 以 Expression.OrElse 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, T1, bool>> Or<T, T1>(this Expression<Func<T, T1, bool>> left,
            Expression<Func<T, T1, bool>> right)
        {
            if (left == null)
                return left;

            return left.Compose(right, Expression.OrElse);
        }

        /// <summary>
        /// 以 Expression.OrElse 组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <returns>组合后的表达式</returns>
        public static Expression<Func<T, T1, T2, bool>> Or<T, T1, T2>(this Expression<Func<T, T1, T2, bool>> left,
            Expression<Func<T, T1, T2, bool>> right)
        {
            if (left == null)
                return left;

            return left.Compose(right, Expression.OrElse);
        }

        /// <summary>
        /// 以特定的条件运行组合两个Expression表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型</typeparam>
        /// <param name="left">第一个Expression表达式</param>
        /// <param name="right">要组合的Expression表达式</param>
        /// <param name="merge">组合条件运算方式</param>
        /// <returns>组合后的表达式</returns>
        private static Expression<T> Compose<T>(this Expression<T> left, Expression<T> right,
            Func<Expression, Expression, Expression> merge)
        {
            var map = left.Parameters.Select((f, i) => new { f, s = right.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, right.Body);
            return Expression.Lambda<T>(merge(left.Body, secondBody), left.Parameters);
        }

        private static Expression<Func<T, T1, bool>> Compose<T, T1>(this Expression<Func<T, T1, bool>> left, Expression<Func<T, T1, bool>> right,
          Func<Expression, Expression, Expression> merge)
        {
            var map = left.Parameters.Select((f, i) => new { f, s = right.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, right.Body);
            return Expression.Lambda<Func<T, T1, bool>>(merge(left.Body, secondBody), left.Parameters);

        }

        private static Expression<Func<T, T1, T2, bool>> Compose<T, T1, T2>(this Expression<Func<T, T1, T2, bool>> left, Expression<Func<T, T1, T2, bool>> right,
          Func<Expression, Expression, Expression> merge)
        {
            var map = left.Parameters.Select((f, i) => new { f, s = right.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, right.Body);
            return Expression.Lambda<Func<T, T1, T2, bool>>(merge(left.Body, secondBody), left.Parameters);

        }


        /// <summary>
        /// 返回IN 表达式
        /// （经过项目（ORACLE）使用验证，遇到大量数据（20000笔数据）时会报错，建议不要使用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeT">lambda T类型标记</param>
        /// <param name="propertyName"></param>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateContainsExpression<T>(this List<double> lstIds, string typeT, string propertyName)
        {
            Expression<Func<T, bool>> exp = null;
            var parameter = Expression<Func<T, bool>>.Parameter(typeof(T), typeT);
            for (int i = 0; i < Math.Ceiling((double)lstIds.Count / 1000); i++)
            {
                var ids = lstIds.Skip(i * 1000).Take(1000).ToList();
                var body = Expression.Call(Expression.Constant(ids, typeof(List<double>)), typeof(List<double>).GetMethod(CONTAINS), Expression.Property(parameter, propertyName));
                exp = exp == null ? Expression.Lambda<Func<T, bool>>(body, parameter) : exp.Or(Expression.Lambda<Func<T, bool>>(body, parameter));
            }
            return exp;
        }

        /// <summary>
        /// 返回IN 表达式
        /// （经过项目（ORACLE）使用验证，遇到大量数据（20000笔数据）时会报错，建议不要使用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeT">lambda T类型标记</param>
        /// <param name="propertyName"></param>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateContainsExpression<T>(this List<double?> lstIds,
            string typeT, string propertyName)
        {
            Expression<Func<T, bool>> exp = null;
            var parameter = Expression<Func<T, bool>>.Parameter(typeof(T), typeT);
            for (int i = 0; i < Math.Ceiling((double)lstIds.Count / 1000); i++)
            {
                var ids = lstIds.Skip(i * 1000).Take(1000).ToList();
                var body = Expression.Call(Expression.Constant(ids, typeof(List<double?>)),
                    typeof(List<double?>).GetMethod(CONTAINS),
                    Expression.Property(parameter, propertyName));
                exp = exp == null ? 
                    Expression.Lambda<Func<T, bool>>(body, parameter) 
                    : exp.Or(Expression.Lambda<Func<T, bool>>(body, parameter));
            }
            return exp;
        }

        /// <summary>
        /// 返回IN 表达式
        /// （经过项目（ORACLE）使用验证，遇到大量数据（20000笔数据）时会报错，建议不要使用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeT">lambda T类型标记</param>
        /// <param name="propertyName"></param>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateContainsExpression<T>(this List<string> lstIds, string typeT, string propertyName)
        {
            Expression<Func<T, bool>> exp = null;
            var parameter = Expression<Func<T, bool>>.Parameter(typeof(T), typeT);
            for (int i = 0; i < Math.Ceiling((double)lstIds.Count / 1000); i++)
            {
                var ids = lstIds.Skip(i * 1000).Take(1000).ToList();
                var body = Expression.Call(Expression.Constant(ids, typeof(List<string>)), typeof(List<string>).GetMethod(CONTAINS), Expression.Property(parameter, propertyName));
                exp = exp == null ? Expression.Lambda<Func<T, bool>>(body, parameter) : exp.Or(Expression.Lambda<Func<T, bool>>(body, parameter));
            }
            return exp;
        }

        /// <summary>
        /// 返回IN 表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型T</typeparam>
        /// <typeparam name="T1">表达式的主实体类型T</typeparam>
        /// <param name="t">此类型需为T或T1</param>
        /// <param name="typeT">lambda T类型标记</param>
        /// <param name="typeT1">lambda T1类型标记</param>
        /// <param name="propertyName"></param>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        public static Expression<Func<T, T1, bool>> CreateContainsExpression<T, T1>(this List<double> lstIds, Type t, string typeT, string typeT1, string propertyName)
        {
            Expression<Func<T, T1, bool>> exp = null;
            bool isT = (t == typeof(T));
            var parameter = isT ? Expression<Func<T, T1, bool>>.Parameter(t, typeT) : Expression<Func<T, T1, bool>>.Parameter(t, typeT1);
            var parameter1 = isT ? Expression<Func<T, T1, bool>>.Parameter(typeof(T1), typeT1) : Expression<Func<T, T1, bool>>.Parameter(typeof(T), typeT);
            List<ParameterExpression> lstParameter = new List<ParameterExpression>() { parameter, parameter1 };
            if (!isT)
                lstParameter.Reverse();
            for (int i = 0; i < Math.Ceiling((double)lstIds.Count / 1000); i++)
            {
                var ids = lstIds.Skip(i * 1000).Take(1000).ToList();
                var body = Expression.Call(Expression.Constant(ids, typeof(List<double>)), typeof(List<double>).GetMethod(CONTAINS), Expression.Property(parameter, propertyName));
                exp = exp == null ? Expression.Lambda<Func<T, T1, bool>>(body, lstParameter) : exp.Or(Expression.Lambda<Func<T, T1, bool>>(body, lstParameter));
            }
            return exp;
        }

        /// <summary>
        /// 返回IN 表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型T</typeparam>
        /// <typeparam name="T1">表达式的主实体类型T</typeparam>
        /// <param name="t">此类型需为T或T1</param>
        /// <param name="typeT">lambda T类型标记</param>
        /// <param name="typeT1">lambda T1类型标记</param>
        /// <param name="propertyName"></param>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        public static Expression<Func<T, T1, bool>> CreateContainsExpression<T, T1>(this List<string> lstIds, Type t, string typeT, string typeT1, string propertyName)
        {
            Expression<Func<T, T1, bool>> exp = null;
            bool isT = (t == typeof(T));
            var parameter = isT ? Expression<Func<T, T1, bool>>.Parameter(t, typeT) : Expression<Func<T, T1, bool>>.Parameter(t, typeT1);
            var parameter1 = isT ? Expression<Func<T, T1, bool>>.Parameter(typeof(T1), typeT1) : Expression<Func<T, T1, bool>>.Parameter(typeof(T), typeT);
            List<ParameterExpression> lstParameter = new List<ParameterExpression>() { parameter, parameter1 };
            if (!isT)
                lstParameter.Reverse();
            for (int i = 0; i < Math.Ceiling((double)lstIds.Count / 1000); i++)
            {
                var ids = lstIds.Skip(i * 1000).Take(1000).ToList();
                var body = Expression.Call(Expression.Constant(ids, typeof(List<string>)), typeof(List<string>).GetMethod(CONTAINS), Expression.Property(parameter, propertyName));
                exp = exp == null ? Expression.Lambda<Func<T, T1, bool>>(body, lstParameter) : exp.Or(Expression.Lambda<Func<T, T1, bool>>(body, lstParameter));
            }
            return exp;
        }


        /// <summary>
        /// 返回IN 表达式
        /// </summary>
        /// <typeparam name="T">表达式的主实体类型T</typeparam>
        /// <typeparam name="T1">表达式的主实体类型T</typeparam>
        /// <param name="t">此类型需为T或T1</param>
        /// <param name="typeT">lambda T类型标记</param>
        /// <param name="typeT1">lambda T1类型标记</param>
        /// <param name="propertyName"></param>
        /// <param name="lstIds"></param>
        /// <returns></returns>
        public static Expression<Func<T, T1, bool>> CreateContainsExpression1<T, T1>(this List<double?> lstIds, Type t, string typeT, string typeT1, string propertyName)
        {
            Expression<Func<T, T1, bool>> exp = null;
            bool isT = (t == typeof(T));
            var parameter = isT ? Expression<Func<T, T1, bool>>.Parameter(t, typeT) : Expression<Func<T, T1, bool>>.Parameter(t, typeT1);
            var parameter1 = isT ? Expression<Func<T, T1, bool>>.Parameter(typeof(T1), typeT1) : Expression<Func<T, T1, bool>>.Parameter(typeof(T), typeT);
            List<ParameterExpression> lstParameter = new List<ParameterExpression>() { parameter, parameter1 };
            if (!isT)
                lstParameter.Reverse();
            for (int i = 0; i < Math.Ceiling((double)lstIds.Count / 1000); i++)
            {
                var ids = lstIds.Skip(i * 1000).Take(1000).ToList();
                var body = Expression.Call(Expression.Constant(ids, typeof(List<double?>)), typeof(List<double?>).GetMethod(CONTAINS), Expression.Property(parameter, propertyName));
                exp = exp == null ? Expression.Lambda<Func<T, T1, bool>>(body, lstParameter) : exp.Or(Expression.Lambda<Func<T, T1, bool>>(body, lstParameter));
            }
            return exp;
        }
    }
}
