using SIE.Domain;
using SIE.ManagedProperty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Core.Common
{
    /// <summary>
    /// 实体扩展类
    /// </summary>
    public static class DataEntityCoreExtension
    {
        /// <summary>
        /// 获取完整子列表数据，子列表数据包括已保存未修改过、删除的、修改过的数据。
        /// #使用场景例子：单据保存后提交。部分从表数据已保存，提交时实体只有修改过或新增的数据，在后台验证时当对整个单据的从表数据验证时，可以使用该方法获取完整的从表数据。
        /// </summary>
        /// <typeparam name="T">主表类型</typeparam>
        /// <typeparam name="TChild">从表类型</typeparam>
        /// <param name="parent">主表实体</param>
        /// <param name="isForce">是否必须重新加载</param>
        /// <returns>从表实体集合</returns>
        public static void GetAllChildData<T, TChild>(this T parent, bool isForce = false) where T : Entity where TChild : Entity
        {
            if (parent==null || parent.PersistenceStatus == PersistenceStatus.New) return;
            var curChildDatas = parent.GetChildProperty<TChild>() as EntityList<TChild>;
            if (!isForce && curChildDatas.Any(p => p.PersistenceStatus == PersistenceStatus.Unchanged))
                return;  //从表包含未修改数据，认为之前已加载过，默认不再重新加载。
            var srcParent = RF.GetById<T>(parent.GetId());
            if (srcParent == null) return;
            foreach (var srcChildData in srcParent.GetChildProperty<TChild>())
            {
                var child = srcChildData as TChild;
                if (curChildDatas.All(x => (double)x.GetId() != (double)child.GetId())
                    && curChildDatas.DeletedList.All(x => (double)x.GetId() != (double)child.GetId()))
                {
                    curChildDatas.Add(child);
                    child.MarkSaved(); //标记成已保存，作为列表已加载过的标记
                }
            }
        }

        /// <summary>
        /// 对列表加载指定的贪婪属性。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="eagerLoadProperties">所有需要贪婪加载的属性。</param>
        public static void EagerLoad(EntityList list, IList<ConcreteProperty> eagerLoadProperties)
        {
            if (list.Count > 0 && eagerLoadProperties.Count > 0)
            {
                //为了不修改外面传入的列表，这里缓存一个新的列表。
                var eagerCache = eagerLoadProperties.ToList();

                //找到这个列表需要加载的所有贪婪加载属性。
                var listEagerProperties = new List<ConcreteProperty>();
                for (int i = eagerCache.Count - 1; i >= 0; i--)
                {
                    var item = eagerCache[i];
                    if (item.Owner.IsAssignableFrom(list.EntityType))
                    {
                        listEagerProperties.Add(item);
                        eagerCache.RemoveAt(i);
                    }
                }

                //对于每一个属性，直接查询出该属性对应实体的所有实体对象。
                foreach (var property in listEagerProperties)
                {
                    var mp = property.Property;
                    var listProperty = mp as IListProperty;
                    if (listProperty != null)
                    {
                        EagerLoadChildren(list, listProperty, eagerCache);
                    }
                    else
                    {
                        var refProperty = mp as IRefProperty;
                        if (refProperty != null)
                        {
                            EagerLoadRef(list, refProperty, eagerCache);
                        }
                        else
                        {
                            throw new InvalidOperationException("贪婪加载属性只支持引用属性和列表属性两种。");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 对实体列表中每一个实体都贪婪加载出它的所有子实体。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listProperty">贪婪加载的列表子属性。</param>
        /// <param name="eagerLoadProperties">所有还需要贪婪加载的属性。</param>
        public static void EagerLoadChildren(EntityList list, IListProperty listProperty, List<ConcreteProperty> eagerLoadProperties)
        {
            try
            {
                //查询一个大的实体集合，包含列表中所有实体所需要的所有子实体。
                var idList = new List<object>(10);
                list.EachNode(e =>
                {
                    idList.Add(e.GetId());
                    return false;
                });
                if (idList.Count == 0)
                    return;
                var targetRepo = RepositoryFactoryHost.Factory.FindByEntity(listProperty.ListEntityType);
                EntityList allChildren = null;
                //切割查询语句进行查询每次查不大于 999 笔数据
                int indexer = 0;
                while (indexer < idList.Count)
                {
                    var ids = idList.Skip(indexer).Take(999).ToArray();
                    var tempChildren = targetRepo.GetByParentIdList(ids.ToArray());
                    if (allChildren == null)
                        allChildren = tempChildren;
                    else
                        allChildren.AddRange(tempChildren.OfType<Entity>());
                    indexer += 999;
                }
                if (allChildren == null || allChildren.Count == 0)
                {
                    return;
                }

                //var allChildren = targetRepo.GetByParentIdList(idList.ToArray());

                //继续递归加载它的贪婪属性。
                EagerLoad(allChildren, eagerLoadProperties);

                #region 把父实体全部放到排序列表中

                //由于数据量可能较大，所以需要进行排序后再顺序加载。
                IList<Entity> sortedList = null;

                //if (SupportTree)
                //{
                //    var sortedParents = new List<Entity>();
                //    list.EachNode(p =>
                //    {
                //        sortedParents.Add(p);
                //        return false;
                //    });
                //    sortedList = sortedParents.ToList();
                //}
                //else
                //{
                sortedList = list.OfType<Entity>().ToList();
                //}

                #endregion

                //把大的实体集合，根据父实体 Id，分拆到每一个父实体的子集合中。
                var parentProperty = targetRepo.FindParentPropertyInfo(true).ManagedProperty as IRefProperty;
                var parentIdProperty = parentProperty.RefIdProperty;
                var allEntityChildren = allChildren.OfType<Entity>();
                //循环填充子列表数据
                foreach (Entity parent in sortedList)
                {
                    var children = targetRepo.NewList();
                    var childs = allEntityChildren.Where(p => object.Equals(p.GetRefId(parentIdProperty), parent.GetId()));
                    children.AddRange(childs);
                    children.SetParentEntity(parent);
                    parent.LoadProperty(listProperty, children);
                }
            }
            catch
            {
                //贪婪加载异常不处理
            }
        }

        /// <summary>
        /// 对实体列表中每一个实体都贪婪加载出它的所有引用实体。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="refProperty">贪婪加载的引用属性。</param>
        /// <param name="eagerLoadProperties">所有还需要贪婪加载的属性。</param>
        public static void EagerLoadRef(EntityList list, IRefProperty refProperty, List<ConcreteProperty> eagerLoadProperties)
        {
            try
            {
                var refIdProperty = refProperty.RefIdProperty;
                //查询一个大的实体集合，包含列表中所有实体所需要的所有引用实体。
                var idList = new List<object>();
                list.EachNode(e =>
                {
                    var refId = e.GetRefNullableId(refIdProperty);
                    if (refId != null)
                    {
                        idList.Add(refId);
                    }
                    return false;
                });
                idList = idList.Distinct().ToList();
                if (idList.Count == 0)
                    return;

                var targetRepo = RepositoryFactoryHost.Factory.FindByEntity(refProperty.RefEntityType);
                EntityList refList = targetRepo.NewList();
                int indexer = 0;
                while (indexer < idList.Count)
                {
                    var ids = idList.Skip(indexer).Take(999).ToArray();
                    refList.AddRange(targetRepo.GetByIdList(ids).OfType<Entity>());
                    indexer += 999;
                }
                //继续递归加载它的贪婪属性。
                EagerLoad(refList, eagerLoadProperties);
                if (refList.Count == 0)
                    return;

                #region 把实体全部放到排序列表中

                //由于数据量可能较大，所以需要进行排序后再顺序加载。
                IList<Entity> sortedList = null;

                //if (SupportTree)
                //{
                //    sortedList = new List<Entity>(list.Count);
                //    list.EachNode(p =>
                //    {
                //        sortedList.Add(p);
                //        return false;
                //    });
                //}
                //else
                //{
                sortedList = list.OfType<Entity>().ToList();
                //}

                #endregion

                //把所有的引用属性填充到列表
                var refEntityProperty = refProperty.RefEntityProperty;
                foreach (Entity refEntity in refList)
                {
                    var refId = refEntity.GetId();
                    var entityResult = sortedList.Where(p => refId.Equals(p.GetRefNullableId(refProperty.RefIdProperty)));
                    entityResult.ForEach(p => { p.LoadProperty(refEntityProperty, refEntity); });
                }
            }
            catch
            {
                //贪婪加载异常不处理
            }
        }
    }
}
