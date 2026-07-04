using SIE.Common.Configs;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Packages.Boxs.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages.Boxs
{
    /// <summary>
    /// 箱控制器
    /// </summary>
    public partial class BoxController : DomainController
    {
        #region 周转箱
        /// <summary>
        /// 获取周转箱
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="type">类型</param>
        /// <returns>周转箱</returns>
        /// <exception cref="ArgumentNullException">编码为空、类型为空</exception>
        public virtual TurnoverBox GetTurnoverBox(string code, string type)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ValidationException("周转箱条码不能为空".L10N());
            }
            if (type.IsNullOrEmpty())
            {
                throw new ValidationException("全局配置项生产周转箱类型不能为空".L10N());
            }
            return Query<TurnoverBox>().Where(x => x.Code == code && x.Type == type).FirstOrDefault();
        }

        /// <summary>
        /// 获取周转箱
        /// </summary>
        /// <param name="code">编码</param>     
        /// <returns>周转箱</returns>
        /// <exception cref="ArgumentNullException">编码为空</exception>
        public virtual TurnoverBox GetTurnoverBox(string code)
        {
            Check.NotNullOrEmpty(code, nameof(code));
            return Query<TurnoverBox>().Where(x => x.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 判断周转箱是否存在
        /// </summary>
        /// <param name="code">条码</param>
        /// <param name="type">类型</param>
        /// <returns>存返回true，不存在返回false</returns>
        /// <exception cref="ArgumentNullException">编码为空、类型为空</exception>
        public virtual bool ExistsTurnoverBox(string code, string type)
        {
            Check.NotNullOrEmpty(code, nameof(code));
            Check.NotNullOrEmpty(type, nameof(type));
            return Query<TurnoverBox>().Where(p => p.Code == code && p.Type == type).Count() > 0;
        }

        /// <summary>
        /// 获取周转箱集合
        /// </summary>
        /// <param name="turnIds">周转箱Id集合</param>
        /// <returns>周转箱集合</returns>
        public virtual EntityList<TurnoverBox> GetTurnoverBoxes(List<double> turnIds)
        {
            if (turnIds == null || !turnIds.Any())
                throw new ValidationException("查询参数不能为空!".L10N());
            var querys = Query<TurnoverBox>().Where(x => turnIds.Contains(x.Id)).ToList();
            return querys;
        }

        /// <summary>
        /// 获取闲置的周转箱
        /// </summary>
        /// <returns>周转箱</returns>
        public virtual EntityList<TurnoverBox> GetTurnoverBoxes()
        {
            return Query<TurnoverBox>().Where(p => p.State == BoxState.Unused).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 修改周转箱集合的状态
        /// </summary>
        /// <param name="turnIds">周转箱Id集合</param>
        /// <param name="state">周转箱状态</param>
        public virtual void SaveTurnoverBoxsState(List<double> turnIds, BoxState state)
        {
            var turnBoxs = GetTurnoverBoxes(turnIds);
            if (turnBoxs != null && turnBoxs.Any())
            {
                using (var tran = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
                {
                    foreach (var item in turnBoxs)
                    {
                        item.State = state;
                        RF.Save(item);
                    }

                    tran.Complete();
                }
            }
        }


        /// <summary>
        /// 更新周转箱状态
        /// </summary>
        /// <param name="boxCodes">周转箱号</param>
        /// <param name="state">状态</param>
        public virtual void UpdateTurnoverBox(List<string> boxCodes, BoxState state)
        {
            DB.Update<TurnoverBox>().Where(a => boxCodes.Contains(a.Code)).Set(p => p.State, state).Execute();
        }

        /// <summary>
        /// 更新周转箱状态
        /// </summary>
        /// <param name="boxCode">周转箱号</param>
        /// <param name="state">状态</param>
        public virtual void UpdateTurnoverBox(string boxCode, BoxState state)
        {
            TurnoverBox turnoverBox = GetTurnoverBox(boxCode);
            turnoverBox.State = state;
            RF.Save(turnoverBox);
        }

        /// <summary>
        /// 更新周转箱状态
        /// </summary>
        /// <param name="turnoverBoxId">周转箱ID</param>
        /// <param name="state">状态</param>
        public virtual void UpdateTurnoverBox(double turnoverBoxId, BoxState state)
        {
            DB.Update<TurnoverBox>().Set(p => p.State, state).Execute();
        }



        /// <summary>
        /// 获取周转工具绑定明细列表
        /// </summary>
        /// <param name="toolId">周转工具ID</param>
        /// <param name="isUnbinding">是否已解绑</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <param name="orderInfos">排序条件</param>
        /// <returns>绑定明细列表</returns>
        public virtual EntityList<TurnoverBoxBinding> GetTurnoverBoxs(double toolId, bool? isUnbinding, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var query = Query<TurnoverBoxBinding>()
                .Where(p => p.TurnoverBoxId == toolId);
            if (isUnbinding != null)
                query.Where(p => p.IsUnbinding == isUnbinding);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="toolId">周转工具</param>
        /// <param name="pagingInfo">分页</param>
        /// <param name="orderInfos">排序</param>
        /// <returns></returns>
        public virtual EntityList<TurnoverBoxActionLog> GetTurnoverBoxActionLogs(double toolId, PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            var query = Query<TurnoverBoxActionLog>().Where(c => c.TurnoverBoxId == toolId);
            if (orderInfos != null && orderInfos.Count > 0)
                query.OrderBy(orderInfos);
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 解绑明细
        /// </summary>
        /// <param name="box"></param>
        public virtual void UnbindTurnoverBoxBinding(TurnoverBox box)
        {
            using (var tran = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                var binds = Query<TurnoverBoxBinding>().Where(p => p.TurnoverBoxId == box.Id).ToList();
                binds.ForEach(p =>
                {
                    p.PersistenceStatus = PersistenceStatus.Deleted;
                    CreateActionLog(p);
                });
                RF.Save(binds);
                box.State = BoxState.Unused;
                RF.Save(box);
                tran.Complete();
            }
        }

        /// <summary>
        /// 出库周转箱明细回写
        /// </summary>
        /// <param name="boxCode">周转箱号</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="qty">数量</param>
        public virtual void BoxItemBindingFromOut(string boxCode, double itemId, decimal qty)
        {
            var box = GetTurnoverBox(boxCode);
            TurnoverBoxBinding bind = new TurnoverBoxBinding()
            {
                TurnoverBoxId = box.Id,
                ItemId = itemId,
                Qty = qty.ToString(),
                BarcodeType = Core.Barcodes.BarcodeType.ContainerNo,
                BindingDate = DateTime.Now,
                BindingOperatorId = RT.IdentityId,
                IsBindFinish = true,
            };
            RF.Save(bind);
            CreateActionLog(bind, TurnoverType.OutStorage);
        }

        /// <summary>
        /// 创建日志
        /// </summary>
        /// <param name="bind">绑定的明细</param>
        /// <param name="turnoverType">类型</param>
        public virtual void CreateActionLog(TurnoverBoxBinding bind, TurnoverType turnoverType = TurnoverType.UnBinding)
        {
            decimal qty = 0;
            decimal.TryParse(bind.Qty, out qty);
            TurnoverBoxActionLog log = new TurnoverBoxActionLog()
            {
                TurnoverBoxId = bind.TurnoverBoxId,
                ItemId = bind.ItemId,
                Qty = qty,
                Sn = bind.Sn,
                TurnoverType = turnoverType,
            };
            RF.Save(log);
        }

        /// <summary>
        /// 创建周转箱日志
        /// </summary>
        /// <param name="boxId">周转箱Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="qty">数量</param>
        /// <param name="sn">条码</param>
        /// <param name="turnoverType">周转操作类型</param>
        public virtual void CreateActionLog(double boxId, double? itemId, decimal? qty, string sn, TurnoverType turnoverType)
        {
            TurnoverBoxActionLog log = new TurnoverBoxActionLog
            {
                TurnoverBoxId = boxId,
                ItemId = itemId,
                Qty = qty,
                Sn = sn,
                TurnoverType = turnoverType
            };
            RF.Save(log);
        }

        /// <summary>
        /// 创建周转箱日志
        /// </summary>
        /// <param name="boxId">周转箱Id</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="qty">数量</param>
        /// <param name="sn">条码</param>
        /// <param name="turnoverType">周转操作类型</param>
        public virtual TurnoverBoxActionLog ReCreateActionLog(double boxId, double? itemId, decimal? qty, string sn, TurnoverType turnoverType)
        {
            TurnoverBoxActionLog log = new TurnoverBoxActionLog
            {
                TurnoverBoxId = boxId,
                ItemId = itemId,
                Qty = qty,
                Sn = sn,
                TurnoverType = turnoverType
            };
            return log;
        }
        #endregion

        #region 产品容量
        /// <summary>
        /// 是否已存在默认载具
        /// </summary>
        /// <param name="cap">产品容量</param>
        /// <returns>默认产品容量</returns>
        public virtual bool HasDefaultProductCapacity(ProductCapacity cap)
        {
            return GetDefaultProductCapacity(cap.ItemId, cap.Id).Count > 0;
        }

        /// <summary>
        /// 获取物料对应的默认容量
        /// </summary>
        /// <param name="itemId">物料Id</param>、
        /// <param name="filterId">排除Id</param>
        /// <returns>默认产品容量</returns>
        public virtual EntityList<ProductCapacity> GetDefaultProductCapacity(double itemId, double? filterId)
        {
            var query = Query<ProductCapacity>().Where(p => p.IsDefault && p.ItemId == itemId);
            if (filterId != null)
                query.Where(p => p.Id != filterId);
            return query.ToList();
        }

        /// <summary>
        /// 获取载具容量，优先匹配载具底下的产品容量，不存在取载具默认容量
        /// </summary>
        /// <param name="containerNo">载具号</param>
        /// <param name="productId">产品ID</param>
        /// <returns>容量</returns>
        public virtual decimal GetProductCapacity(string containerNo, double productId)
        {
            if (containerNo.IsNullOrEmpty())
                throw new ArgumentNullException("载具号不能为空".L10N());
            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
            var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(containerNo, config.BoxType);
            if (box == null)
                throw new ValidationException("[{0}]不存在".L10nFormat(containerNo));
            if (box.State == BoxState.Scrap)
                throw new ValidationException("[{0}]已经报废".L10nFormat(containerNo));
            var boxCapacity = box.CapacityList.FirstOrDefault(p => p.ItemId == productId);
            if (boxCapacity != null)
                return boxCapacity.Capacity;
            return box.Capacity;
        }

        /// <summary>
        /// 设置默认产品容量为非默认
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="capId">排除Id</param>
        public virtual void UpdateDefaultProductCapacitys(double itemId, double capId)
        {
            var update = DB.Update<ProductCapacity>().Set(p => p.IsDefault, false);
            update.Where(p => p.ItemId == itemId && p.Id != capId).Execute();
        }

        /// <summary>
        /// 产品容量添加命令物料查询
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>物料列表</returns>
        public virtual EntityList<Item> GetItems(ProdCpyItemCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            var query = Query<Item>().Where(p => p.State == State.Enable);
            if (criteria.Code.IsNotEmpty())
                query.Where(p => p.Code.Contains(criteria.Code));
            if (criteria.Name.IsNotEmpty())
                query.Where(p => p.Name.Contains(criteria.Name));
            if (criteria.Type.HasValue)
                query.Where(p => p.Type == criteria.Type);
            if (criteria.ItemSourceType.HasValue)
                query.Where(p => p.ItemSourceType == criteria.ItemSourceType);
            if (criteria.ProductFamilyId.HasValue)
                query.Join<ProductModel>((x, y) => x.ModelId == y.Id && y.ProductFamilyId == criteria.ProductFamilyId);
            if (criteria.ProductModelId.HasValue)
                query.Where(p => p.ModelId == criteria.ProductModelId.Value);
            if (criteria.FilterIds.Length > 0)
                query.Where(p => !criteria.FilterIds.Contains(p.Id));

            return query.ToList(criteria.PagingInfo, eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        /// <summary>
        /// 周转箱型号
        /// </summary>
        /// <returns></returns>
        public virtual TurnoverBoxModel GetTurnoverBoxModel(string code)
        {
            return Query<TurnoverBoxModel>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 保存新增的周转箱数据到垛表
        /// </summary>
        /// <param name="boxes"></param>
        public virtual void SaveTurnoverBoxs(List<TurnoverBox> boxes)
        {
            //保存时将新增的周转箱数据同步新增到WMS垛表中
        }

        /// <summary>
        /// 获取周转箱型号
        /// </summary>
        /// <param name="modelIds">ID集合</param>
        /// <returns>周转箱型号</returns>
        public virtual EntityList<TurnoverBoxModel> GetTurnoverBoxModels(List<double> modelIds)
        {
            if (modelIds == null || modelIds.Count == 0)
            {
                return new EntityList<TurnoverBoxModel>();
            }
            return modelIds.SplitContains(sons =>
            {
                return Query<TurnoverBoxModel>().Where(p => sons.Contains(p.Id)).ToList();
            });
        }
    }
}
