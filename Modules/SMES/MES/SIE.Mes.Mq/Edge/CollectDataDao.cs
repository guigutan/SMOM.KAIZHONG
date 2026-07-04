using SIE.Core.Barcodes;
using SIE.Core.Common.IService;
using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 采集数据Dao类
    /// </summary>
    public class CollectDataDao : ICollectDataDao
    {
        private readonly IRepositoryFactoryService rfs;
        private readonly object lockObj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public CollectDataDao(IRepositoryFactoryService rfs)
        {
            this.rfs = rfs;
        }

        /// <summary>
        /// 保存采集数据到数据库
        /// </summary>
        /// <param name="pp"></param>
        /// <param name="version"></param>
        /// <param name="pd"></param>
        public void SaveCollectData(WipProductProcess pp, WipProductVersion version, WipProduct pd)
        {
            using (var tran = Domain.DB.TransactionScope(MesMqEntityDataProvider.ConnectionStringName))
            {
                rfs.Save(pd);
                rfs.Save(version);
                rfs.Save(pp);
                tran.Complete();
            }
        }


        /// <summary>
        /// 获取产品生产版本
        /// </summary>
        /// <param name="puid">产品ID</param>
        /// <returns>产品生产版本</returns>
        public WipProductVersion GetWipProductVersion(string puid)
        {
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                var verId = rfs.Query<WipProduct>().Where(p => p.Puid == puid).Select(p => p.CurrentVersionId).FirstOrDefault()?.CurrentVersionId;
                if (verId != null)
                    return rfs.GetById<WipProductVersion>(verId);
                return null;
            }
        }


        /// <summary>
        /// 查找最后一个产品版本
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="barcodeType">采集类型</param>
        /// <returns>生产产品版本</returns>
        public virtual WipProductVersion FindLastWipProductVersion(string barcode, BarcodeType barcodeType)
        {
            var query = rfs.Query<WipProductVersion>();
            switch (barcodeType)
            {
                case BarcodeType.CSN:
                    query.Where(x => x.Csns == barcode);
                    break;
                case BarcodeType.TurnoverBox:
                case BarcodeType.ContainerNo:
                    query.Where(x => x.BoxNo == barcode);
                    break;
                case BarcodeType.SN:
                case BarcodeType.BatchBarocde:
                    query.Where(x => x.Sn == barcode);
                    break;
                case BarcodeType.KeyLabel:
                    query.Where(x => x.KeyLabel == barcode);
                    break;
                case BarcodeType.CombinedCode:
                    query.Where(x => x.CombinedCode == barcode);
                    break;
                default:
                    break;
            }

            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                return query.OrderByDescending(p => p.CreateDate).FirstOrDefault();
            }
        }


        /// <summary>
        /// 创建产品在制信息
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="pd"></param>
        public void SaveWipProductVersion(WipProductVersion pv, WipProduct pd)
        {
            using (var tran = Domain.DB.TransactionScope(MesMqEntityDataProvider.ConnectionStringName))
            {
                rfs.Save(pd);
                rfs.Save(pv);
                tran.Complete();
            }
        }

        /// <summary>
        /// 取员工的登录用户ID
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public double? GetUserId(double employeeId)
        {
            var emp = rfs.GetById<Resources.Employees.Employee>(employeeId);
            return emp?.UserId;
        }

        /// <summary>
        /// 更新物料标签
        /// </summary>
        /// <param name="decreaseQty"></param>
        /// <param name="label"></param>
        public void UpdateItemLabel(decimal decreaseQty, string label)
        {
            lock (lockObj)
            {
                var il = rfs.Query<ItemLabel>().Where(p => p.Label == label).FirstOrDefault();
                il.Qty -= decreaseQty;
                if (il.Qty <= 0)
                {
                    il.Qty = 0;
                }
                rfs.Save(il);
            }
        }

        /// <summary>
        /// 更新工单状态
        /// </summary>
        /// <param name="woId">工单Id</param>
        /// <param name="state">状态</param>
        public void UpdateWorkOrderState(double woId, Core.WorkOrders.WorkOrderState state)
        {
            DB.Update<WorkOrder>().Set(p => p.State, state).Where(p => p.Id == woId).Execute();
        }

        /// <summary>
        /// 更新工单完工数
        /// </summary>
        /// <param name="woId">标签号</param>
        /// <param name="qty">完工数量</param>
        public void UpdateWorkOrderQty(double woId, decimal qty) 
        {
            var wo = RF.GetById<WorkOrder>(woId);
            wo.FinishQty += qty;
            wo.State = wo.FinishQty >= wo.PlanQty ? Core.WorkOrders.WorkOrderState.Finish : wo.State;
            RF.Save(wo);
        }
    }
}
