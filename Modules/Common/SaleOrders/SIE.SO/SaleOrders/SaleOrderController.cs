using SIE.Common.Catalogs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Common;
using SIE.CSM.Customers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.APS.SaleOrderEvents;
using SIE.Pcb.SO;
using SIE.Resources.Employees;
using SIE.SO.SaleOrders.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SIE.SO.SaleOrders
{
    /// <summary>
    /// 销售订单控制器
    /// </summary>
    public class SaleOrderController : DomainController
    {
        /// <summary>
        /// 行业类型快码
        /// </summary>
        private Dictionary<string, string> IndustryTypeCodeDic = null;

        /// <summary>
        /// 订单类型快码
        /// </summary>
        private Dictionary<string, string> OrderTypeCodeDic = null;

        /// <summary>
        /// 产品类型快码
        /// </summary>
        private Dictionary<string, string> ProductTypeCodeDic = null;

        /// <summary>
        /// 产品等级快码
        /// </summary>
        private Dictionary<string, string> ProductLevelCodeDic = null;

        /// <summary>
        /// 查询销售订单列表
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>返回生产订单列表</returns>
        public virtual EntityList<SaleOrder> GetSalesOrderList(SaleOrderCriteria criteria)
        {
            var query = Query<SaleOrder>();
            if (criteria.Code.IsNotEmpty())
            {
                query.Where(p => p.Code.Contains(criteria.Code));
            }
            //客户名称
            if (criteria.CustomerId.HasValue && criteria.CustomerId > 0)
            {
                query.Where(p => p.Customer.Id == criteria.CustomerId);
            }
            //销售人员
            if (criteria.EmployeeId.HasValue && criteria.EmployeeId > 0)
            {
                query.Where(p => p.EmployeeId == criteria.EmployeeId);
            }
            //登记时间
            if (criteria.RegisterDateTime.BeginValue.HasValue)
            {
                query.Where(p => p.RegisterDateTime >= criteria.RegisterDateTime.BeginValue);
            }
            if (criteria.RegisterDateTime.EndValue.HasValue)
            {
                query.Where(p => p.RegisterDateTime <= criteria.RegisterDateTime.EndValue);
            }
            if (criteria.IsExport)
            {
                //是导出则不需要分页
                criteria.PagingInfo = null;
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            elo.LoadWith(SaleOrder.SaleOrderDetailListProperty);

            var list = query.Exists<SaleOrderDetail>((a, b) => b.Where(c => c.SaleOrderId == a.Id)
              .WhereIf(criteria.TargetOrderCode.IsNotEmpty(), c => c.TargetOrderCode.Contains(criteria.TargetOrderCode))
              .WhereIf(criteria.LineState.HasValue, c => c.LineState == criteria.LineState)
              .WhereIf(criteria.ItemCode.IsNotEmpty(), c => c.Item.Code.Contains(criteria.ItemCode))
              .WhereIf(criteria.ItemName.IsNotEmpty(), c => c.Item.Name.Contains(criteria.ItemName))
              .WhereIf(criteria.EnterpriseId.HasValue && criteria.EnterpriseId > 0, c => c.EnterpriseId == criteria.EnterpriseId)
              .WhereIf(criteria.RequireDelivery.BeginValue.HasValue, c => c.RequireDelivery >= criteria.RequireDelivery.BeginValue)
              .WhereIf(criteria.RequireDelivery.EndValue.HasValue, c => c.RequireDelivery <= criteria.RequireDelivery.EndValue)
            ).OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, elo);

            foreach (var item in list)
            {
                item.OrderRowsQty = item.SaleOrderDetailList.Count();
                item.TotalQty = item.SaleOrderDetailList.Sum(c => c.Qty);
                item.DetailSum = item.SaleOrderDetailList.Where(s => s.LineState != LineState.NEW).Count();
            }
            return list;
        }

        /// <summary>
        /// 获取销售订单行
        /// </summary>
        /// <param name="productId">产品物料ID</param>
        /// <param name="itemRevision">成品版本</param>
        /// <param name="customerId">客户</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyWord">关键字</param>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetSaleOrderDetails(double productId, string itemRevision, double? customerId,
            PagingInfo pagingInfo = null, string keyWord = null)
        {
            //成品 + 版本 + 客户 + 承诺交期不为空 + 未打包完成】获取可选的销售订单行
            //    ，并且按承诺交期升序排列再按数量升序，
            //    下拉框的字段包含销售订单编号、行号、承诺交期、数量
            var query = Query<SaleOrderDetail>()
                .Where(x => x.ItemId == productId)
                .WhereIf(itemRevision.IsNullOrEmpty(), x => x.ItemRevision == null)
                .WhereIf(!itemRevision.IsNullOrEmpty(), x => x.ItemRevision == itemRevision)
                .Where(x => x.SaleOrder.CustomerId == customerId)
                .Where(x => x.PackagedQty < x.Qty)
                .Where(x => x.PromiseDelivery != null)
                .WhereIf(!keyWord.IsNullOrEmpty(), x => x.SaleOrder.Code.Contains(keyWord + '%'));

            if (pagingInfo == null)
            {
                var saleOrderDetails = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());

                var saleOrderDetailsSortted = new EntityList<SaleOrderDetail>();

                saleOrderDetailsSortted
                    .AddRange(saleOrderDetails.OrderBy(x => x.PromiseDelivery).ThenBy(x => x.Qty));

                return saleOrderDetailsSortted;
            }
            else
            {
                query.OrderBy(x => x.PromiseDelivery).OrderBy(x => x.Qty);
                return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }

        }

        /// <summary>
        /// 获取销售订单行
        /// </summary>
        /// <param name="productId">产品物料ID</param>
        /// <param name="itemRevision">成品版本</param>
        /// <param name="customerId">客户</param>
        /// <param name="saleOrderNo">销售订单号</param>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetSaleOrderDetailsBySaleOrderNo(
            double productId, string itemRevision, double customerId, string saleOrderNo)
        {
            //成品 + 版本 + 客户 + 承诺交期不为空 + 未打包完成】获取可选的销售订单行
            //    ，并且按承诺交期升序排列再按数量升序，
            //    下拉框的字段包含销售订单编号、行号、承诺交期、数量
            var query = Query<SaleOrderDetail>()
                .Where(x => x.ItemId == productId)
                .WhereIf(itemRevision.IsNullOrEmpty(), x => x.ItemRevision == null)
                .WhereIf(!itemRevision.IsNullOrEmpty(), x => x.ItemRevision == itemRevision)
                .Where(x => x.SaleOrder.CustomerId == customerId)
                .Where(x => x.PackagedQty < x.Qty)
                .Where(x => x.PromiseDelivery != null)
                .Where(x => x.SaleOrder.Code == saleOrderNo)
                .OrderBy(x => x.PromiseDelivery).OrderBy(x => x.Qty);

            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());


        }

        /// <summary>
        /// 获取匹配的销售订单行
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="itemRevision"></param>
        /// <param name="customerId"></param>        
        /// <returns></returns>
        public virtual SaleOrderDetail GetSaleOrderDetail(double productId, string itemRevision, double? customerId)
        {
            //成品 + 版本 + 客户 + 承诺交期不为空 + 未打包完成】获取可选的销售订单行
            //    ，并且按承诺交期升序排列再按数量升序，
            //    下拉框的字段包含销售订单编号、行号、承诺交期、数量
            var query = Query<SaleOrderDetail>()
                .Where(x => x.ItemId == productId)
                .WhereIf(itemRevision.IsNullOrEmpty(), x => x.ItemRevision == null)
                .WhereIf(!itemRevision.IsNullOrEmpty(), x => x.ItemRevision == itemRevision)
                .Where(x => x.SaleOrder.CustomerId == customerId)
                .Where(x => x.PackagedQty < x.Qty)
                .Where(x => x.PromiseDelivery != null)
                .OrderBy(x => new { x.PromiseDelivery, x.Qty });

            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取销售订单的No
        /// </summary>
        /// <returns>销售订单No字符串</returns>
        public virtual string GetSalesOrderNo()
        {
            return GetSalesOrderNos().FirstOrDefault();
        }

        /// <summary>
        /// 获取新的销售订单编号
        /// </summary>
        /// <param name="qty">生成多少个新的销售订单编号</param>
        /// <returns>返回销售订单编号集合</returns>
        public virtual List<string> GetSalesOrderNos(int qty = 1)
        {
            var config = ConfigService.GetConfig(new SaleOrderNoConfig(), typeof(SaleOrder));
            if (config == null || config.NumberRule == null)
            {
                throw new ValidationException("未找到销售订单编码配置规则，请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, qty).ToList();
        }

        /// <summary>
        /// 获取非禁用的客户信息
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回客户信息</returns>
        public virtual EntityList<Customer> GetSelfMadeOrOutMadeItem(PagingInfo pagingInfo, string keyword)
        {
            /*return Query<Customer>().Where(p => (p.ItemSourceType == ItemSourceType.SelfMade || p.ItemSourceType == ItemSourceType.OutMade) && p.Code.Contains(keyword)).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
              */
            return Query<Customer>().Where(p => p.State != State.Disable && (p.Code.Contains(keyword) || p.Name.Contains(keyword))).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //return Query<Customer>().ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 记录修改日志
        /// </summary>
        /// <param name="DetailList">明细数据集合</param>
        public virtual void GetSalesOrderDetailWriteLog(List<SaleOrderDetail> DetailList)
        {
            var DetailIdList = DetailList.Select(s => s.Id).ToList();
            List<SaleOrderDetail> DetailListCon = new List<SaleOrderDetail>();
            foreach (var item in DetailList)
            {
                if (item.LineState == LineState.CONFIRMED)
                {
                    DetailListCon.Add(item);
                }
            }
            if (!DetailListCon.Any()) return;
            Dictionary<Double, SaleOrderDetail> DetaiDicSource = DetailListCon.ToDictionary(key => key.Id, value => value);
            var DetailListdest = Query<SaleOrderDetail>().Where(s => DetailIdList.Contains(s.Id)).ToList();
            Dictionary<Double, SaleOrderDetail> salesOrderDetaiDicDest = DetailListdest.ToDictionary(key => key.Id, value => value);
            foreach (KeyValuePair<Double, SaleOrderDetail> item in DetaiDicSource)
            {
                if (salesOrderDetaiDicDest.ContainsKey(item.Key))
                {
                    List<SaleOrderLog> logList = ObjectCompare(item.Value, salesOrderDetaiDicDest[item.Key]);
                    if (logList.Any())
                    {
                        foreach (var log in logList)
                        {
                            log.LineNo = item.Value.LineNo;
                            log.Item = item.Value.Item;
                            log.SaleOrderId = item.Value.SaleOrderId;
                            RF.Save(log);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 比较二个类的public属性不同，source和dest类型必需相同
        /// </summary>
        /// <param name="source">修改后类</param>
        /// <param name="dest">修改前类</param>
        /// <returns>返回字符型比较结果不同</returns>
        public virtual List<SaleOrderLog> ObjectCompare(object source, object dest)
        {
            //获得对象的所有public属性
            PropertyInfo[] pis = source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //被修改的属性集合
            List<SaleOrderLog> logList = new List<SaleOrderLog>();
            SaleOrderLog log = new SaleOrderLog();

            if (pis != null)//如果获得了属性 
            {
                for (int i = 0, count = pis.Length; i < count; i++)//针对每一个属性进行循环 
                {
                    object val1, val2;
                    bool CompareTrue;//比较结果
                    string Name = pis[i].Name;//获取对应属性名
                    val1 = pis[i].GetValue(source, null);//获取源值
                    val2 = pis[i].GetValue(dest, null);//获取目标值
                    if (val1 != null)
                    {
                        //可以理解为没有赋值的不进行比较
                        if (Name == nameof(SaleOrderDetail.Qty))
                        {
                            CompareTrue = Compare(val1, val2, pis[i].PropertyType);
                            if (!CompareTrue)
                            {
                                log = new SaleOrderLog()
                                {
                                    ModifyBefore = val2.ToString(),
                                    ModifyAfter = val1.ToString(),
                                    UpdateItem = "数量变更"
                                };
                                logList.Add(log);
                            }
                        }
                        else if (Name == nameof(SaleOrderDetail.RequireDelivery))
                        {
                            CompareTrue = Compare(val1, val2, pis[i].PropertyType);
                            if (!CompareTrue)
                            {
                                log = new SaleOrderLog()
                                {
                                    ModifyBefore = val2.ToString(),
                                    ModifyAfter = val1.ToString(),
                                    UpdateItem = "日期变更"
                                };
                                logList.Add(log);
                            }
                        }
                        else
                        {
                            //其他类型不比较
                            continue;
                        }
                    }
                }
            }
            return logList;
        }

        /// <summary>
        /// 对比方法
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool Compare(object val1, object val2, Type type)
        {
            if (type == typeof(int))
            {
                return (int)val1 == (int)val2;
            }
            if (type == typeof(double))
            {
                return (double)val1 == (double)val2;
            }
            if (type == typeof(decimal))
            {
                return (decimal)val1 == (decimal)val2;
            }
            if (type == typeof(DateTime))
            {
                return (DateTime)val1 == (DateTime)val2;
            }
            return true;
        }

        /// <summary>
        /// 验证销售订单是否有明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Int32 ValidateSalesOrderDetail(double id)
        {
            return Query<SaleOrderDetail>().Where(s => s.SaleOrderId == id).Count();
        }

        /// <summary>
        /// 获得销售订单集合
        /// </summary>
        /// <param name="idList">ID集合</param>
        /// <returns>销售订单明细数据</returns>
        public virtual EntityList<SaleOrder> GetSalesOrders(List<double> idList)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWith(SaleOrder.EmployeeProperty);
            elo.LoadWith(SaleOrder.CustomerProperty);
            return GetSalesOrderIDList(idList, elo);
        }

        /// <summary>
        /// 获得销售订单集合
        /// </summary>
        /// <param name="salesOrderIds">Id列表</param>
        /// <param name="elo">贪婪对象</param>
        /// <returns>销售订单明细列表</returns>
        public virtual EntityList<SaleOrder> GetSalesOrderIDList(List<double> salesOrderIds, EagerLoadOptions elo)
        {
            return salesOrderIds.SplitContains((tmpIds) =>
            {
                return Query<SaleOrder>().Where(p => tmpIds.Contains(p.Id)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获得销售订单集合
        /// </summary>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="keyword">关键字</param>
        /// <returns>返回销售订单信息</returns>
        public virtual EntityList<SaleOrder> GetSalesOrderList(PagingInfo pagingInfo, string keyword)
        {

            return Query<SaleOrder>().Where(p => p.Code.Contains(keyword)).ToList(pagingInfo);
        }

        /// <summary>
        /// 获得销售订单集合
        /// </summary>
        /// <param name="CodeList">销售订单编码集合</param>
        /// <returns>返回销售订单信息</returns>
        public virtual EntityList<SaleOrder> GetSalesOrderList(List<string> CodeList)
        {
            using (SIE.Common.InvOrg.InvOrgs.WithAll(true))
            {
                return Query<SaleOrder>().Where(p => CodeList.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWith(SaleOrder.SaleOrderDetailListProperty));
            }
        }

        /// <summary>
        /// 保存销售订单
        /// </summary>
        /// <param name="data">销售订单集合</param>
        /// <returns>返回销售订单信息</returns>
        public virtual void SaveSaleOrder(EntityList data)
        {
            EntityList<SaleOrder> List = data as EntityList<SaleOrder>;
            List<SaleOrderFlatObject> Objectlist = new List<SaleOrderFlatObject>();
            SaleOrderFlatObject Object;
            foreach (var item in List)
            {
                foreach (var Udetail in item.SaleOrderDetailList)
                {
                    if (GetById<SaleOrderDetail>(Udetail.Id) != null && Udetail.LineState != LineState.NEW)
                    {
                        Object = new SaleOrderFlatObject();
                        Object.Code = item.Code;
                        Object.LineNo = Udetail.LineNo;
                        Object.Qty = Udetail.Qty;
                        Object.RequireDelivery = Udetail.RequireDelivery;
                        Object.Identification = FlatType.Alter;
                        Objectlist.Add(Object);
                    }
                }
            }
            using (var tran = DB.TransactionScope(SOEntityDataProvider.ConnectionStringName))
            {
                if (Objectlist.Any()) RT.EventBus.Publish(new AlterLoadingEvent() { SaleOrderFlatObjectList = Objectlist });
                RF.Save(data);
                tran.Complete();
            }
        }

        #region 导入相关方法
        /// <summary>
        /// 获取销售订单
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>返回抽样方案</returns>
        public virtual SaleOrder GetSaleOrderCode(string code)
        {
            return Query<SaleOrder>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 获取销售订单明细
        /// </summary>
        /// <param name="code">编码</param>
        /// <param name="lineNo">行号</param>
        /// <returns>返回抽样方案</returns>
        public virtual SaleOrderDetail GetSaleDetail(string code, string lineNo)
        {
            return Query<SaleOrderDetail>().Where(p => p.SaleOrder.Code == code && p.LineNo == lineNo).FirstOrDefault();
        }
        #endregion

        #region 导出相关方法
        /// <summary>
        /// 获取导出的模型数据
        /// </summary>
        /// <param name="criteria">销售订单查询实体</param>
        /// <returns></returns>
        public virtual List<SaleOrderReachViewModel> GetSaleOrderReachViewModel(SaleOrderCriteria criteria)
        {
            List<SaleOrderReachViewModel> modelList = new List<SaleOrderReachViewModel>();
            var SaleOrders = GetSalesOrderList(criteria);
            //加载快码字典
            InitCatalogDic(ref IndustryTypeCodeDic, SaleOrderDetail.INDUSTRYTYPE);
            InitCatalogDic(ref OrderTypeCodeDic, SaleOrderDetail.ORDERTYPE);
            InitCatalogDic(ref ProductTypeCodeDic, SaleOrderDetail.PRODUCTTYPE);
            InitCatalogDic(ref ProductLevelCodeDic, SaleOrderDetail.PRODUCTLEVEL);
            foreach (var item in SaleOrders)
            {
                modelList.AddRange(CreateModel(item));
            }
            return modelList;
        }

        /// <summary>
        /// 加载快码字典
        /// </summary>
        /// <param name="categoryRange">字典集合</param>
        /// <param name="CatalogType">快码类型</param>
        /// <returns>model</returns>
        private void InitCatalogDic(ref Dictionary<string, string> categoryRange, String CatalogType)
        {
            if (categoryRange == null)
            {
                categoryRange = new Dictionary<string, string>();
                EntityList<Catalog> categoryList = RT.Service.Resolve<CatalogController>().GetCatalogList(CatalogType);
                foreach (Catalog catalogItem in categoryList)
                {
                    categoryRange.Add(catalogItem.Code, catalogItem.Name);
                }
            }
        }

        /// <summary>
        /// 创建报表数据ViewModel
        /// </summary>
        /// <param name="sale">销售订单对象</param>
        /// <returns>model</returns>
        private List<SaleOrderReachViewModel> CreateModel(SaleOrder sale)
        {
            List<SaleOrderReachViewModel> modelList = new List<SaleOrderReachViewModel>();
            var temp = sale;
            SaleOrderReachViewModel model;
            foreach (var item in temp.SaleOrderDetailList)
            {
                model = new SaleOrderReachViewModel();
                model.Code = sale.Code;
                model.Customer = sale.Customer.Code;
                model.Employee = sale.Employee.Name;
                model.LineNo = item.LineNo;
                model.ItemCode = item.Item.Code;
                model.ItemName = item.Item.Name;
                model.Version = item.Version;
                model.IndustryType = IndustryTypeCodeDic[item.IndustryType];
                model.OrderType = OrderTypeCodeDic[item.OrderType];
                model.ProductType = ProductTypeCodeDic[item.ProductType];
                model.ProductLevel = ProductLevelCodeDic[item.ProductLevel];
                model.IsNew = item.IsNew == true ? "是".L10N() : "否".L10N();
                model.Qty = item.Qty;
                model.Unit = item.Unit.Name;
                model.EnterpriseCode = item.EnterpriseCode;
                model.EnterpriseName = item.EnterpriseName;
                model.MiDateTime = item.MiDateTime;
                model.Area = item.Area;
                model.PlateSize = item.PlateSize;
                model.MaterialPnl = item.MaterialPnl;
                model.SetPnl = item.SetPnl;
                model.PcsPnl = item.PcsPnl;
                model.IsHangUp = item.IsHangUp == true ? "是".L10N() : "否".L10N();
                model.LineState = item.LineState.ToLabel();
                model.RequireDelivery = item.RequireDelivery;
                model.PromiseDelivery = item.PromiseDelivery;
                modelList.Add(model);
            }
            return modelList;
        }
        #endregion

        /// <summary>
        /// 获取销售订单列表
        /// </summary>
        /// <returns></returns>

        public virtual EntityList<SaleOrder> GetSaleOrderList()
        {
            return Query<SaleOrder>().Exists<SaleOrderDetail>((a, b) => b.Where(p => p.LineState != LineState.COMPLETE && p.SaleOrderId == a.Id))
                .ToList(null, new EagerLoadOptions().LoadWith(SaleOrder.SaleOrderDetailListProperty));
        }

        /// <summary>
        /// 获取销售订单列表
        /// </summary>
        /// <param name="CodeList">销售订单Code集合</param>
        /// <returns></returns>
        public virtual EntityList<SaleOrder> GetSaleOrderList(List<string> CodeList)
        {
            var SaleOrderList = CodeList.SplitContains((codes) =>
              {
                  return Query<SaleOrder>().Where(p => codes.Contains(p.Code)).ToList(null, null);
              });
            return SaleOrderList;
        }


        #region  厂别确认与订单评审获取销售订单与销售订单明细

        /// <summary>
        /// 获取销售订单列表(同步)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<SaleOrder> GetFactorySaleOrderList()
        {
            return Query<SaleOrder>()
                .Exists<SaleOrderDetail>((a, b) => b.Where(p => p.LineState != LineState.COMPLETE && p.SaleOrderId == a.Id && p.EnterpriseId != null))
                .ToList(null, null);
        }

        /// <summary>
        /// 获取销售订单明细列表(同步)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetFactorySaleOrderDateilList()
        {
            return Query<SaleOrderDetail>().Where(p => p.LineState != LineState.COMPLETE && p.EnterpriseId != null).ToList(null, null);
        }


        /// <summary>
        /// 获取销售订单列表(上传)
        /// </summary>
        /// <param name="ids">销售订单明细ids</param>
        /// <returns></returns>
        public virtual EntityList<SaleOrder> GetFactorySaleOrderList(List<double> ids)
        {
            return Query<SaleOrder>()
                .Exists<SaleOrderDetail>((a, b) => b.Where(p => ids.Contains(p.Id) && p.SaleOrderId == a.Id && p.LineState == LineState.CONFIRMED && p.EnterpriseId != null))
                .ToList(null, null);
        }
        /// <summary>
        /// 获取销售订单明细列表(上传)
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetFactorySaleOrderDateilList(List<double> ids)
        {
            return Query<SaleOrderDetail>().Where(p => ids.Contains(p.Id) && p.LineState == LineState.CONFIRMED && p.EnterpriseId != null).ToList(null, null);
        }


        #endregion


        /// <summary>
        /// 获取销售订单明细列表
        /// </summary>
        /// <param name="keys">查询条件（订单号+行号）的数组</param>
        /// <returns></returns>
        public virtual EntityList<SaleOrderDetail> GetSaleOrderDetialList(List<string> keys)
        {
            var querys = Query<SaleOrderDetail>().
             Join<SaleOrder>((q, p) => q.SaleOrderId == p.Id).
             Where<SaleOrder>((q, p) => keys.Contains(p.Code + q.LineNo)).ToList();

            return querys;
        }

        /// <summary>
        /// 销售订单明细是否引用物料
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <returns>返回是否存在销售订单明细引用物料</returns>
        public virtual bool IsExistsItem(double itemId)
        {
            return Query<SaleOrderDetail>().Where(p => p.ItemId == itemId).Count() > 0;
        }
    }
}
