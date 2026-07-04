using SIE.Common.NumberRules;
using SIE.Core.WorkOrders;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages.ItemLabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Packages
{
    /// <summary>
    /// 包装关系控制器
    /// </summary>
    public partial class PackingRelationController : DomainController
    {
        /// <summary>
        /// 往包装里加入一个产品
        /// </summary>
        /// <param name="relation">包装关系</param>
        /// <param name="itemLabel">物料标签</param>
        public virtual void AddItem(PackingRelation relation, ItemLabel itemLabel)
        {
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));
            if (itemLabel == null)
                throw new ArgumentNullException(nameof(itemLabel));
            using (var trans = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                LessenItemSourcePackingRelation(itemLabel);
                UpdateCurrentPackingRelation(relation, itemLabel);
                var parentRelation = GetById<PackingRelation>(relation.GetTreePId());
                UpdateParentPackageItemQty(parentRelation, itemLabel.Qty);
                itemLabel.RelationId = relation.Id;
                RF.Save(itemLabel);
                trans.Complete();
            }
        }

        /// <summary>
        /// 往包装里加入一个包装
        /// </summary>
        /// <param name="parentPackage">parentPackage</param>
        /// <param name="childPackage">childPackage</param>
        public virtual void AddPackage(PackingRelation parentPackage, PackingRelation childPackage)
        {
            if (parentPackage == null)
                throw new ArgumentNullException(nameof(parentPackage));
            if (childPackage == null)
                throw new ArgumentNullException(nameof(childPackage));
            using (var trans = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                LessenPackingRelationSourcePackingRelation(childPackage);
                UpdateCurrentPackingRelation(parentPackage, childPackage);
                UpdateParentPackageItemQty((GetById<PackingRelation>(parentPackage.GetTreePId())), childPackage.ItemQty);
                DB.Update<PackingRelation>()
                    .Set(f => f.RootId, parentPackage.RootId)
                    .Where(f => f.RootId == childPackage.RootId).Execute();

                childPackage.TreePId = parentPackage.Id;
                childPackage.RootId = parentPackage.RootId;
                RF.Save(childPackage);

                trans.Complete();
            }
        }

        /// <summary>
        /// 更新父包装物料数量
        /// </summary>
        /// <param name="parentPackage">包装关系</param>
        /// <param name="itemQty">物料数量</param>
        /// <exception cref="ArgumentOutOfRangeException">物料数量为0</exception>
        /// <exception cref="ValidationException">更新外包装失败</exception>
        private void UpdateParentPackageItemQty(PackingRelation parentPackage, decimal itemQty)
        {
            if (parentPackage == null)
            {
                return;
            }

            if (itemQty <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(itemQty), "必须大于0");
            }

            var isSucess = DB.Update<PackingRelation>()
                    .Set(f => f.ItemQty, parentPackage.ItemQty + itemQty)
                    .Where(f => f.ItemQty == parentPackage.ItemQty
                        && f.PackedQty == parentPackage.PackedQty
                        && f.Id == parentPackage.Id
                        && f.UpdateDate == parentPackage.UpdateDate
                        && f.UpdateBy == parentPackage.UpdateBy).Execute() == 1;

            if (!isSucess)
            {
                throw new ValidationException("更新外包装失败,{0}.Id = {2},PackageNo = {3}"
                    .L10nFormat(nameof(parentPackage), parentPackage.Id, parentPackage.PackageNo));
            }

            if (parentPackage.ItemQty + itemQty == 0)
            {
                parentPackage.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(parentPackage);
            }

            UpdateParentPackageItemQty((GetById<PackingRelation>(parentPackage.GetTreePId())), itemQty);
        }

        /// <summary>
        /// 减少物料标签对应包装关系的包装数量
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        private void LessenItemSourcePackingRelation(ItemLabel itemLabel)
        {
            if (itemLabel.Relation != null)
            {
                var isSucess = DB.Update<PackingRelation>()
                    .Set(f => f.PackedQty, itemLabel.Relation.PackedQty - 1)
                    .Where(f => f.PackedQty == itemLabel.Relation.PackedQty
                        && f.Id == itemLabel.RelationId
                        && f.UpdateDate == itemLabel.Relation.UpdateDate
                        && f.UpdateBy == itemLabel.Relation.UpdateBy).Execute() == 1;
                if (!isSucess)
                    throw new ValidationException("更新产品条码[{0}]的包装[{1}({2})]失败".L10nFormat(itemLabel.Label, itemLabel.Relation.PackageUnit.Name, itemLabel.Relation.PackageNo));

                UpdateParentPackageItemQty(GetById<PackingRelation>(itemLabel.RelationId), -itemLabel.Qty);
            }
        }

        /// <summary>
        /// 减少包装关系包装数量
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        private void LessenPackingRelationSourcePackingRelation(PackingRelation packingRelation)
        {
            var sourcePackingRelation = GetById<PackingRelation>(packingRelation.GetTreePId());
            if (sourcePackingRelation != null)
            {
                var isSucess = DB.Update<PackingRelation>()
                    .Set(f => f.PackedQty, sourcePackingRelation.PackedQty - 1)
                    .Where(f => f.PackedQty == sourcePackingRelation.PackedQty
                        && f.Id == sourcePackingRelation.Id
                        && f.UpdateDate == sourcePackingRelation.UpdateDate
                        && f.UpdateBy == sourcePackingRelation.UpdateBy).Execute() == 1;
                if (!isSucess)
                    throw new ValidationException("更新产品包装[{0}]的父包装[{1}({2})]失败".L10nFormat(packingRelation.PackageNo, sourcePackingRelation.PackageUnit.Name.L10N(), sourcePackingRelation.PackageNo));

                UpdateParentPackageItemQty(GetById<PackingRelation>(sourcePackingRelation.Id), -packingRelation.ItemQty);
            }
        }

        /// <summary>
        /// 更新当前包装关系
        /// </summary>
        /// <param name="parentPackage">parentPackage</param>
        /// <param name="itemLabel">物料标签</param>
        private void UpdateCurrentPackingRelation(PackingRelation parentPackage, ItemLabel itemLabel)
        {
            var isSuscess = DB.Update<PackingRelation>()
                 .Set(f => f.ItemQty, parentPackage.ItemQty + itemLabel.Qty)
                 .Set(f => f.PackedQty, parentPackage.PackedQty + 1)
                 .Set(f => f.Id, parentPackage.Id)
                 .Where(f => f.Id == parentPackage.Id)
                 .Execute() == 1;
            if (!isSuscess)
                throw new ValidationException("更新外包装异常,{0}.Id = {1},{0}.PackageNo = {2}"
                    .L10nFormat(nameof(parentPackage), parentPackage.Id, parentPackage.PackageNo));
        }

        /// <summary>
        /// 更新当前包装关系
        /// </summary>
        /// <param name="parentPackage">包装关系</param>
        /// <param name="innerPackage">innerPackage</param>
        private void UpdateCurrentPackingRelation(PackingRelation parentPackage, PackingRelation innerPackage)
        {
            var isSuscess = DB.Update<PackingRelation>()
                 .Set(f => f.ItemQty, parentPackage.ItemQty + innerPackage.ItemQty)
                 .Set(f => f.PackedQty, parentPackage.PackedQty + 1)
                 .Set(f => f.Id, parentPackage.Id)
                 .Where(f => f.Id == parentPackage.Id)
                 .Execute() == 1;

            if (!isSuscess)
            {
                throw new ValidationException("更新外包装异常,{0}.Id = {1},{0}.PackageNo = {2}"
                    .L10nFormat(nameof(parentPackage), parentPackage.Id, parentPackage.PackageNo));
            }
        }

        /// <summary>
        /// 打包
        /// 生成包装关系包装号
        /// </summary>
        /// <param name="packingRelationId">包装关系ID</param>
        /// <param name="numberRuleId">编码规则ID</param>
        /// <param name="packingBy">打包人员</param>
        /// <returns>打包后的包装关系</returns>
        public virtual PackingRelation DoPacking(double packingRelationId, double numberRuleId, double packingBy)
        {
            using (var trans = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                if (packingRelationId == 0)
                    throw new ArgumentOutOfRangeException(nameof(packingRelationId), "packingRelationId = 0");
                if (numberRuleId == 0)
                    throw new ArgumentOutOfRangeException(nameof(numberRuleId), "numberRuleId = 0");
                if (packingBy == 0)
                    throw new ArgumentOutOfRangeException(nameof(packingBy), "packingBy = 0");
                var packageNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId, 1).FirstOrDefault();
                if (packageNo.IsNullOrWhiteSpace())
                    throw new ValidationException("生成包装号失败，请在编码规则功能里配置规则明细".L10N());
                var isSucceed = DB.Update<PackingRelation>()
                   .Set(f => f.PackageNo, packageNo)
                   .Set(f => f.PackingBy, packingBy)
                   .Set(f => f.PackedDate, DateTime.Now)
                   .Where(f => f.PackageNo == null && f.Id == packingRelationId)
                   .Execute() == 1;

                if (!isSucceed)
                    throw new ValidationException("打包失败,因为此包装已被打包,不能重复打包".L10N());
                trans.Complete();
                return GetById<PackingRelation>(packingRelationId);

            }
        }

        /// <summary>
        /// 是否已被打包
        /// </summary>
        /// <param name="innerPackage">innerPackage</param>
        /// <returns>bool</returns>
        public virtual bool IsNotBeDoPacking(PackingRelation innerPackage)
        {
            if (innerPackage == null) throw new ArgumentNullException(nameof(innerPackage));

            var packingRelation = RF.GetById<PackingRelation>(innerPackage.GetTreePId());
            while (packingRelation != null)
            {
                if (packingRelation.PackageNo.IsNotEmpty()) return false;
                packingRelation = RF.GetById<PackingRelation>(packingRelation.GetTreePId());
            }

            return true;
        }

        /// <summary>
        /// 是否已打包
        /// </summary>
        /// <param name="innerPackage">innerPackage</param>
        /// <returns>bool</returns>
        public virtual bool IsNotDoPacking(PackingRelation innerPackage)
        {
            if (innerPackage == null) throw new ArgumentNullException(nameof(innerPackage));
            var packingRelation = innerPackage;

            while (packingRelation != null)
            {
                if (packingRelation.PackageNo.IsNotEmpty()) return false;
                packingRelation = RF.GetById<PackingRelation>(packingRelation.GetTreePId());
            }

            return true;
        }

        /// <summary>
        /// 是否放进包装
        /// </summary>
        /// <param name="innerPackage">innerPackage</param>
        /// <returns>bool</returns>
        public virtual bool IsNotInPackage(PackingRelation innerPackage)
        {
            if (innerPackage == null) throw new ArgumentNullException(nameof(innerPackage));
            var pkg = GetById<PackingRelation>(innerPackage.Id);

            if (pkg.GetTreePId() != null) return false;
            return true;
        }

        /// <summary>
        /// 获取包装对应的工单信息
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <returns>工单</returns>
        [IgnoreProxy]
        public virtual WorkOrder GetWorkOrder(PackingRelation packingRelation)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));

            var workOrderId = RT.Service.Resolve<ItemLabelController>().GetRootPackingRelationItemLabel(packingRelation.RootId)?.WorkOrderId;
            if (!workOrderId.HasValue) return null;
            return RF.GetById<WorkOrder>(workOrderId);
        }

        /// <summary>
        /// 获取包装关系父节点列表
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <returns>包装关系列表</returns>
        public virtual EntityList<PackingRelation> GetTreeParentList(PackingRelation packingRelation)
        {
            var parent = GetById<PackingRelation>(packingRelation.TreePId);
            if (parent != null)
                return RF.Find<PackingRelation>().GetByTreePId(parent.Id) as EntityList<PackingRelation>;
            return new EntityList<PackingRelation>();
        }

        /// <summary>
        /// 根据根节点查询条件递归获取下面所有子节点包装关系
        /// </summary>
        /// <param name="querySql">根节点筛选查询条件</param>
        /// <returns>包装关系列表</returns>
        public virtual EntityList<PackingRelation> GetRelationAllNodes(string querySql)
        {
            EntityList<PackingRelation> list = new EntityList<PackingRelation>();
            var query = Query<PackingRelation>();
            var meta = RF.Find<PackingRelation>().EntityMeta;

            List<object> idList = new List<object>();
            var sqlPre = "with relation_child(id) as";
            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<PackingRelation>()).DbSetting;
            if (setting.IsPostgreSqlServer()||setting.IsVastDataServer())
            {
                sqlPre = "with recursive relation_child as";
            }
            if (setting.IsMysqlDbServer())
            {
                sqlPre = "with  recursive  relation_child(id) as";
            }
            var sql = sqlPre + string.Format(@"
                     (select {2}
                        from {0}
                       where 1=1 {3}
                      UNION ALL (SELECT a.{2}
                                  from {0} a
                                 inner join relation_child b
                                    on a.{1} = b.{2}))
                    SELECT * FROM relation_child",
                    meta.TableMeta.TableName, meta.Property(PackingRelation.TreePIdProperty).ColumnMeta.ColumnName,
                    meta.Property(PackingRelation.IdProperty).ColumnMeta.ColumnName, querySql);

            using (var db = DbAccesserFactory.Create(PackageEntityDataProvider.ConnectionStringName))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        idList.Add(dr.GetValue(0));
                    }
                }
            }

            if (idList.Count > 0)
            {
                list = query.Where(p => idList.Contains(p.Id))
                    .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }

            return list;
        }

        /// <summary>
        /// 根据根节点ID查找下面所有子节点列表
        /// </summary>
        /// <param name="rootId">根节点id</param>
        /// <returns>包装关系列表</returns>
        public virtual EntityList<PackingRelation> GetRelationAllNodesByPtreeId(object rootId)
        {
            StringBuilder sb = new StringBuilder();
            var meta = RF.Find<PackingRelation>().EntityMeta;
            sb.Append(" and {0} ={1}".FormatArgs(meta.Property(PackingRelation.IdProperty).ColumnMeta.ColumnName, rootId));
            return GetRelationAllNodes(sb.ToString());
        }

        /// <summary>
        /// 根据根节点包装号查找下面所有子节点列表
        /// </summary>
        /// <param name="packageNos">根节点</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns>包装关系列表</returns>
        public virtual EntityList<PackingRelation> GetRelationByRootNo(List<string> packageNos, EagerLoadOptions elo = null)
        {
            var result = new EntityList<PackingRelation>();
            var meta = RF.Find<PackingRelation>().EntityMeta;
            for (int i = 0; i < Math.Ceiling((double)packageNos.Count / 1000); i++)
            {
                StringBuilder sb = new StringBuilder();
                var tempNos = packageNos.Skip(i * 1000).Take(1000).ToList();
                var nos = string.Join("','", tempNos);
                sb.Append(" and {0} in( '{1}')".FormatArgs(meta.Property(PackingRelation.PackageNoProperty).ColumnMeta.ColumnName, nos));
                var datas = GetRelationAllNodes(sb.ToString());
                result.AddRange(datas);
            }

            return result;
        }

        /// <summary>
        /// 更新包装关系物流状态
        /// </summary>
        /// <param name="relationId">包装关系ID</param>
        /// <param name="state">物流状态</param>
        public virtual void UpdateRelationState(double relationId, LogisticState state)
        {
            DB.Update<PackingRelation>().Set(p => p.State, state).Where(p => p.Id == relationId).Execute();
        }

        /// <summary>
        /// 判断是否是一个包装
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <returns>存在返回true，否则返回false</returns>
        public virtual bool IsPackingRelation(string packageNo)
        {
            return GetBatchPackingRelation(packageNo, false) != null;
        }

        /// <summary>
        /// 获取包装关系信息
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="throwException">包装不存在是否抛异常，默认true</param>
        /// <exception cref="ValidationException">包装不存在</exception>
        /// <exception cref="ArgumentNullException">包装号为空</exception>
        /// <returns>包装关系</returns>
        public virtual BatchPackingRelation GetBatchPackingRelation(string packageNo, bool throwException = true)
        {
            if (packageNo.IsNullOrWhiteSpace())
            {
                throw new ValidationException("必须具有一个包装号".L10N());
            }

            var batchPackingRelation = Query<BatchPackingRelation>().Where(p => p.PackageNo == packageNo)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            if (batchPackingRelation == null && throwException)
            {
                throw new ValidationException("采集失败,无此包装号:[{0}]".L10nFormat(packageNo));
            }

            return batchPackingRelation;
        }

        /// <summary>
        /// 获取包装关系信息
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="throwException">包装不存在是否抛异常，默认true</param>
        /// <returns>包装关系</returns>
        /// <exception cref="ValidationException"></exception>
        public virtual PackingRelation GetPackingRelation(string packageNo, bool throwException = true)
        {
            if (packageNo.IsNullOrWhiteSpace())
            {
                throw new ValidationException("必须具有一个包装号".L10N());
            }

            var packingRelation = Query<BatchPackingRelation>().Where(p => p.PackageNo == packageNo)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            if (packingRelation == null && throwException)
            {
                throw new ValidationException("采集失败,无此包装号:[{0}]".L10nFormat(packageNo));
            }

            return packingRelation;
        }

        /// <summary>
        /// 获取包装关系
        /// </summary>
        /// <param name="relationId">包装关系ID</param>
        /// <exception cref="EntityNotFoundException">包装关系不存在</exception>
        /// <returns>包装关系</returns>
        public virtual PackingRelation GetPackingRelation(double relationId)
        {
            var relation = RF.GetById<PackingRelation>(relationId);
            if (relation == null)
                throw new EntityNotFoundException(typeof(PackingRelation), relationId);
            return relation;
        }


        /// <summary>
        /// 递归获取该包装上级包装（包括传入包装）
        /// </summary>
        /// <param name="list">包装列表</param>
        /// <returns>包装列表</returns>
        public virtual EntityList<BatchPackingRelation> GetAllPacingRelation(EntityList<BatchPackingRelation> list)
        {
            if (list.Count == 0) return new EntityList<BatchPackingRelation>();
            var relation = list[0];
            if (relation.TreePId == null || relation.TreePId == 0) return list;
            list.Insert(0, (RF.GetById<BatchPackingRelation>(relation.TreePId)));
            return GetAllPacingRelation(list);
        }

        /// <summary>
        /// 工位工序未完成包装
        /// </summary>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <returns>未完成包装关系列表</returns>
        public virtual EntityList<PackingRelation> FindWorkPackingRelationByStation(double? processId, double? stationId)
        {
            var meta = RF.Find<PackingRelation>().EntityMeta;
            StringBuilder querySql = new StringBuilder();

            querySql.Append(" and {0} is null and {1} = 0 "
                .FormatArgs(meta.Property(PackingRelation.TreePIdProperty).ColumnMeta.ColumnName,
                    meta.Property(PackingRelation.IsProcessFinishProperty).ColumnMeta.ColumnName));

            if (processId.HasValue)
            {
                querySql.Append(@" and {0} = {1} "
                    .FormatArgs(meta.Property(PackingRelation.ProcessIdProperty).ColumnMeta.ColumnName, processId.Value));
            }

            if (stationId.HasValue)
            {
                querySql.Append(@" and {0} = {1}"
                    .FormatArgs(meta.Property(PackingRelation.StationIdProperty).ColumnMeta.ColumnName, stationId.Value));
            }

            var list = RT.Service.Resolve<PackingRelationController>().GetRelationAllNodes(querySql.ToString());
            return list;
        }

        /// <summary>
        /// 获取标签外包装关系
        /// </summary>
        /// <param name="label">标签号</param>
        /// <returns>外包装关系</returns>
        public virtual PackingRelation GetLabelPackingRelation(string label)
        {
            return Query<PackingRelation>()
                .Join<ItemLabel>((p, i) => p.Id == i.RelationId && i.Label == label)
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据包装关系Id获取最外层的包装关系
        /// </summary>
        /// <param name="rootId">最外层包装根节点Id</param>
        /// <returns>最外层的包装关系</returns>
        public virtual PackingRelation GetRootPackingRelation(double rootId)
        {
            return Query<PackingRelation>().Where(p => p.Id == rootId).FirstOrDefault();
        }

        /// <summary>
        /// 获取所有的包装关系
        /// </summary>
        /// <param name="relationNos">包装条码</param>
        /// <returns>包装关系</returns>
        /// <param name="eLoad">是否加载试图属性</param>
        public virtual EntityList<PackingRelation> GetAllPackingRelations(List<string> relationNos, bool eLoad = false)
        {
            EntityList<PackingRelation> packingRelations = new EntityList<PackingRelation>();
            packingRelations.AddRange(GetPackingRelations(relationNos));
            var rootIds = packingRelations.Select(p => p.Id).Distinct().ToList();
            packingRelations.AddRange(GetPackingRelationsByRootIds(rootIds));
            var ids = packingRelations.Select(p => p.Id).Distinct().ToList();
            return GetPackingRelations(ids, eLoad);
        }

        /// <summary>
        /// 获取包装关系
        /// </summary>
        /// <param name="relationNos">包装条码</param>
        /// <returns>包装关系</returns>
        public virtual EntityList<PackingRelation> GetPackingRelations(List<string> relationNos)
        {
            return relationNos.SplitContains((tempNos) =>
            {
                return Query<PackingRelation>().Where(p => tempNos.Contains(p.PackageNo)).ToList();
            });
        }


        /// <summary>
        /// 获取包装关系
        /// </summary>
        /// <param name="ids">根ID</param>
        /// <param name="eLoad">是否加载试图属性</param>
        /// <returns>包装关系</returns>
        public virtual EntityList<PackingRelation> GetPackingRelations(List<double> ids, bool eLoad = false)
        {
            if (eLoad)
            {
                return ids.SplitContains((tempIds) =>
                {
                    return Query<PackingRelation>().Where(p => tempIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
            }
            else
            {
                return ids.SplitContains((tempIds) =>
                {
                    return Query<PackingRelation>().Where(p => tempIds.Contains(p.Id)).ToList();
                });
            }
        }
        /// <summary>
        /// 获取包装关系
        /// </summary>
        /// <param name="rootIds">根ID</param>
        /// <returns>包装关系</returns>
        public virtual EntityList<PackingRelation> GetPackingRelationsByRootIds(List<double> rootIds)
        {
            return rootIds.SplitContains((tempIds) =>
            {
                return Query<PackingRelation>().Where(p => tempIds.Contains(p.RootId)).ToList();
            });
        }
    }
}