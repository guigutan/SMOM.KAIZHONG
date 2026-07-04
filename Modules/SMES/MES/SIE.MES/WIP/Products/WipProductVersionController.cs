using SIE.Common;
using SIE.Common.InvOrg;
using SIE.Core.Barcodes;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Data;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.PPSNs;
using SIE.EventMessages.MES.WorkOrders.Models;
using SIE.MES.QTimes;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Repairs;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using Barcode = SIE.Barcodes.Barcode;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品生产版本控制器
    /// </summary>
    public class WipProductVersionController : DomainController
    {

        /// <summary>
        /// 获取产品版本集合
        /// </summary>
        /// <param name="criteria">产品版本查询实体</param> 
        /// <returns>产品版本集合</returns>
        EntityList<WipProductVersion> GetWipProductVersionList(WipProductVersionCriteria criteria)
        {
            var meta = RF.Find<WipProductVersion>().EntityMeta;
            var tableName = meta.TableMeta.TableName;
            var sn = meta.Property(WipProductVersion.SnProperty).ColumnMeta.ColumnName;
            var relevanceSn = meta.Property(WipProductVersion.RelevanceSnProperty).ColumnMeta.ColumnName;
            var id = meta.Property(Entity.IdProperty).ColumnMeta.ColumnName;
            var invOrgId = meta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            var isPhantom = meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
            List<double> versionIds = new List<double>();
            string cycle = "CYCLE ID SET DUP_ID TO 'Y' DEFAULT 'N'";
            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<WipProductVersion>()).DbSetting;
            if (setting.IsPostgreSqlServer() || setting.IsSqlserverDbServer()|| setting.IsVastDataServer())
            {
                cycle = "";
            }
            var sqlPre = "with version({0},{1},{2}) as".FormatArgs(id, sn, relevanceSn);
            if (setting.IsPostgreSqlServer()||setting.IsVastDataServer())
            {
                sqlPre = "with recursive version({0},{1},{2}) as".FormatArgs(id, sn, relevanceSn);
            }
            string sql = sqlPre + @" (
  select {0},{1},{3}
    from {2}
   where {1}='{4}'
  union all (select v.{0},v.{1},v.{3}
               from {2} v
               inner join version vc on v.{1} = vc.{3} 
               where v.{5} ={7} and v.{6}=0)){8}
  select * from version".FormatArgs(id, sn, tableName, relevanceSn, criteria.Sn, invOrgId, isPhantom, RT.InvOrg, cycle);
            using (var db = DbAccesserFactory.Create(MesCoreEntityDataProvider.ConnectionStringName))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        versionIds.Add(dr.GetDecimal(0).ConvertTo<double>());
                    }
                }
            }

            var query = Query<WipProductVersion>();

            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.No));
            }

            if (!criteria.ProductCode.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            }

            if (!criteria.ProductName.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.Product.Name.Contains(criteria.ProductName));
            }

            if (criteria.IsFinish != null)
            {
                if (criteria.IsFinish == YesNo.Yes)
                    query.Where(p => p.IsFinish);
                else
                    query.Where(p => !p.IsFinish);
            }

            if (!criteria.PanelWorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.PanelWorkOrder.No.Contains(criteria.PanelWorkOrderNo));
            }

            if (criteria.ProcessId.HasValue)
                //query.Where(p => p.CurrentProcess.ProcessId == criteria.ProcessId);
                query.Where(p => p.NowProcessId == criteria.ProcessId);
            if (criteria.StartDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.StartDate.BeginValue);
            if (criteria.StartDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.StartDate.EndValue);
            if (criteria.NextProcessId.HasValue)
                query.Where(p => p.NextProcessId == criteria.NextProcessId);
            if (criteria.ItemSN.IsNotEmpty())
            {
                query.Exists<WipProductProcessKeyItem>((x, y) => y.Where(p => p.Process.VersionId == x.Id && !p.IsUnbound && p.SourceCode == criteria.ItemSN));
            }
            var versionIdList = versionIds.Distinct();
            return query.Where(p => versionIdList.Contains(p.Id))
                    .OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品版本集合
        /// </summary>
        /// <param name="criteria">产品版本查询实体</param>
        /// <returns>产品版本集合</returns>
        EntityList<WipProductVersion> GetWipProductVersionListBySNIsNull(WipProductVersionCriteria criteria)
        {
            var query = Query<WipProductVersion>();

            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.No));
            }

            if (!criteria.ProductCode.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.Product.Code.Contains(criteria.ProductCode));
            }

            if (!criteria.ProductName.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.Product.Name.Contains(criteria.ProductName));
            }

            if (criteria.IsFinish != null)
            {
                if (criteria.IsFinish == YesNo.Yes)
                    query.Where(p => p.IsFinish);
                else
                    query.Where(p => !p.IsFinish);
            }

            if (!criteria.PanelWorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.PanelWorkOrder.No.Contains(criteria.PanelWorkOrderNo));
            }

            if (criteria.ProcessId.HasValue)
                //query.Where(p => p.CurrentProcess.ProcessId == criteria.ProcessId);
                query.Where(p => p.NowProcessId == criteria.ProcessId);

            if (criteria.StartDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.StartDate.BeginValue);

            if (criteria.StartDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.StartDate.EndValue);

            if (criteria.NextProcessId.HasValue)
                query.Where(p => p.NextProcessId == criteria.NextProcessId);

            if (criteria.ItemSN.IsNotEmpty())
            {
                query.Exists<WipProductProcessKeyItem>((x, y) => y.Where(p => p.Process.VersionId == x.Id && !p.IsUnbound && p.SourceCode == criteria.ItemSN));
            }

            try
            {
                return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            catch (Exception ex)
            {
                if (ex.Message == "数据集超过50000条记录，请使用分页条件进行限制。或者设置DB.DataLimit参数(不建议)")
                {
                    throw new ValidationException("查询数量超出最大50000条，导出失败，请分批导出或联系管理员导出！".L10N());
                }
                return new EntityList<WipProductVersion>();
            }
        }

        /// <summary>
        /// 查询产品生产版本
        /// </summary> 
        /// <param name="query">标准查询实体</param>
        /// <returns>返回生产产品列表</returns>
        public virtual EntityList<WipProductVersion> GetWipProductVersions(CriteriaQuery query)
        {
            return Query<WipProductVersion>().Where(query.Criteria).ToList(query.PagingInfo);
        }

        /// <summary>
        /// 获取产品版本集合
        /// </summary>
        /// <param name="criteria">产品版本查询实体</param>
        /// <returns>产品版本集合</returns>
        public virtual EntityList<WipProductVersion> GetWipProductVersions(WipProductVersionCriteria criteria)
        {
            if (criteria != null && !criteria.Sn.IsNullOrEmpty())  ////条码不为空情况下需要递归查询管理条码版本
            {
                return GetWipProductVersionList(criteria);
            }
            else
            {
                return GetWipProductVersionListBySNIsNull(criteria);
            }
        }

        /// <summary>
        /// 获取产品当前生产版本
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <returns>返回生产产品版本列表</returns>
        public virtual WipProductVersion GetWipProductVersion(string sn)
        {
            var eagerLoad = new EagerLoadOptions().LoadWithViewProperty();
            return Query<WipProductVersion>()
                .Where(p => p.Product.CurrentVersionId == p.Id && p.Sn == sn)
                .FirstOrDefault(eagerLoad: eagerLoad);
        }

        /// <summary>
        /// 获取产品当前生产版本
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <returns>返回生产产品版本列表</returns>
        public virtual WipProductVersion GetWipProductVersion(CollectBarcode collectBarcode)
        {
            WipProductVersion wipProductVersion = null;
            var eagerLoad = new EagerLoadOptions().LoadWithViewProperty();

            switch (collectBarcode.Type)
            {
                case BarcodeType.SN:
                    {
                        wipProductVersion = Query<WipProductVersion>()
                            .Where(p => p.Product.CurrentVersionId == p.Id && p.Sn == collectBarcode.Code)
                            .FirstOrDefault(eagerLoad: eagerLoad);
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                case BarcodeType.TurnoverBox:
                    break;
                case BarcodeType.KeyLabel:
                    {
                        wipProductVersion = Query<WipProductVersion>()
                            .Where(p => p.Product.CurrentVersionId == p.Id && p.KeyLabel == collectBarcode.Code)
                            .FirstOrDefault(eagerLoad: eagerLoad);
                    }
                    break;
                case BarcodeType.BatchBarocde:
                    break;
                case BarcodeType.ContainerNo:
                    break;
                case BarcodeType.CombinedCode:
                    {
                        wipProductVersion = Query<WipProductVersion>()
                            .Where(p => p.Product.CurrentVersionId == p.Id && p.CombinedCode == collectBarcode.Code)
                            .FirstOrDefault(eagerLoad: eagerLoad);
                    }
                    break;
                default:
                    break;
            }

            return wipProductVersion;
        }

        /// <summary>
        /// 获取产品当前生产版本，贪婪加载工序记录 bs使用
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <returns>返回生产产品版本列表</returns>
        public virtual WipProductVersion GetWipProductVersionEagerLoadProcess(string sn)
        {
            var eagerLoad = new EagerLoadOptions().LoadWithViewProperty().LoadWith(WipProductVersion.ProcessListProperty);
            return Query<WipProductVersion>()
                .Where(p => p.Product.CurrentVersionId == p.Id && p.Sn == sn)
                .FirstOrDefault(eagerLoad: eagerLoad);
        }

        /// <summary>
        /// Check产品当前生产版本
        /// </summary>
        /// <param name="sn">返工工单条码</param>
        /// <returns>true: 已置换；false:未置换</returns>
        public virtual bool CheckReworkBarcodeHavePermuted(string sn)
        {
            bool result = true;
            var wipPrcVersion = GetWipProductVersion(sn);
            if (wipPrcVersion == null)
                result = false;
            else if (wipPrcVersion.RelevanceSn.IsNotEmpty())
                result = true;
            else
                result = false;

            return result;
        }

        /// <summary>
        /// 获取关键件
        /// </summary>
        /// <param name="sourceCode">来源条码</param>
        /// <returns>返回生产关键件</returns>
        public virtual WipProductProcessKeyItem GetWipKeyItem(string sourceCode)
        {
            return Query<WipProductProcessKeyItem>().Where(p => p.SourceCode == sourceCode && !p.IsUnbound && p.Qty > 0).FirstOrDefault();
        }

        /// <summary>
        /// 获取关键件列表
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>关键件列表</returns>
        public virtual EntityList<WipProductProcessKeyItem> GetWipKeyItems(string barcode)
        {
            EntityList<WipProductProcessKeyItem> wipPrcKeyItems = null;
            var curWipPrcVersion = Query<WipProductVersion>().Where(p => p.Sn == barcode
                                  && p.RelevanceSn == null && p.WorkOrder.Type != WorkOrderType.Rework).FirstOrDefault();
            if (curWipPrcVersion != null)
                wipPrcKeyItems = curWipPrcVersion.ProcessList.SelectMany(x => x.KeyItemList).Where(p => !p.IsUnbound).AsEntityList();
            return wipPrcKeyItems;
        }

        /// <summary>
        /// 获取关键件列表
        /// </summary>
        /// <param name="wipProductVersionId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="reelId"></param>
        /// <param name="itemCode"></param>        
        /// <returns>关键件列表</returns>
        public virtual EntityList<WipProductProcessKeyItem> GetWipKeyItems(double wipProductVersionId,
            string reelId, string itemCode, PagingInfo pagingInfo = null)
        {
            EntityList<WipProductProcessKeyItem> wipPrcKeyItems = null;

            var query = Query<WipProductProcessKeyItem>()
                .Join<WipProductProcess>((x, y) => x.ProcessId == y.Id)
                .Where<WipProductProcess>((x, y) => !x.IsUnbound && y.VersionId == wipProductVersionId);


            if (!reelId.IsNullOrEmpty())
            {
                query.Where(x => x.SourceCode.Contains(reelId));
            }

            if (!itemCode.IsNullOrEmpty())
            {
                query.Where(x => x.Item.Code.Contains(itemCode));
            }

            if (pagingInfo != null)
            {
                wipPrcKeyItems = query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            }
            else
            {
                wipPrcKeyItems = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            }

            return wipPrcKeyItems;
        }

        /// <summary>
        /// 获取产品生产关健件
        /// </summary>
        /// <param name="wipKeyItemIds">产品生产关健件Id集合</param>
        /// <returns>产品生产关键件集合</returns>
        public virtual EntityList<WipProductProcessKeyItem> GetWipKeyItems(List<double> wipKeyItemIds)
        {
            var querys = Query<WipProductProcessKeyItem>().Where(p => wipKeyItemIds.Contains(p.Id)).ToList();
            return querys;
        }

        /// <summary>
        /// 递归获取关键件列表
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>关键件列表</returns>
        public virtual EntityList<WipProductProcessKeyItem> GetRecursionWipKeyItems(string barcode)
        {
            EntityList<WipProductProcessKeyItem> wipPrcKeyItems = null;
            WipProductVersionCriteria criteria = new WipProductVersionCriteria();
            criteria.Sn = barcode;
            criteria.No = string.Empty;
            criteria.PagingInfo = null;
            var wipPrcVersionList = GetWipProductVersionList(criteria);
            if (wipPrcVersionList != null && wipPrcVersionList.Any())
            {
                wipPrcKeyItems = new EntityList<WipProductProcessKeyItem>();
                foreach (var curWipPrcVersion in wipPrcVersionList)
                {
                    var curWipPrcKeyItems = curWipPrcVersion.ProcessList.SelectMany(x => x.KeyItemList).Where(p => !p.IsUnbound&&p.Qty>0).ToList();
                    if (curWipPrcKeyItems != null && curWipPrcKeyItems.Any())
                    {
                        wipPrcKeyItems.AddRange(curWipPrcKeyItems);
                    }
                }
            }

            return wipPrcKeyItems;
        }

        /// <summary>
        /// 获取产品检验记录
        /// </summary>
        /// <param name="wipProductVersionId">生产产品版本ID列表</param>
        /// <returns>返回产品检验记录列表</returns>
        public virtual EntityList<WipProductInspectionItem> GetWipProductInspectionItems(List<double> wipProductVersionId)
        {
            var wipProductInspectionItemList = new EntityList<WipProductInspectionItem>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<WipProductInspectionItem>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductInspectionItemList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductInspectionItemList;
        }

        /// <summary>
        /// 获取产品生产采集记录
        /// </summary>
        /// <param name="sn">条码</param>
        /// <returns>返回生产采集记录列表</returns>
        public virtual EntityList<WipProductProcess> GetWipProductProcess(string sn)
        {
            return Query<WipProductProcess>()
                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == sn)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品生产采集记录
        /// </summary>
        /// <param name="wipProductVersionId">生产产品版本ID列表</param>
        /// <returns>返回生产采集记录列表</returns>
        public virtual EntityList<WipProductProcess> GetWipProductProcess(List<double> wipProductVersionId)
        {
            var wipProductProcessList = new EntityList<WipProductProcess>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<WipProductProcess>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductProcessList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductProcessList;
        }

        /// <summary>
        /// 获取生产采集记录
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <param name="resourceId">产线</param>
        /// <param name="processId">工序</param>
        /// <returns>生产采集记录集合</returns>
        public virtual EntityList<WipProductProcess> GetWipProductProcess(string barcode, BarcodeType type, double resourceId, double processId)
        {
            var wipProductProcessList = new EntityList<WipProductProcess>();
            switch (type)
            {
                case BarcodeType.SN:
                    {
                        wipProductProcessList = Query<WipProductProcess>()
                                            .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == barcode)
                                            .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                            .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.State == WipProductProcessState.Finish)
                                            .OrderByDescending(p => p.CreateDate)
                                            .ToList();
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                case BarcodeType.TurnoverBox:
                    break;
                case BarcodeType.KeyLabel:
                    {
                        wipProductProcessList = Query<WipProductProcess>()
                                            .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.KeyLabel == barcode)
                                            .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                            .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.State == WipProductProcessState.Finish)
                                            .OrderByDescending(p => p.CreateDate)
                                            .ToList();
                    }
                    break;
                case BarcodeType.BatchBarocde:
                    break;
                case BarcodeType.ContainerNo:
                    break;
                case BarcodeType.CombinedCode:
                    {
                        wipProductProcessList = Query<WipProductProcess>()
                                            .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.CombinedCode == barcode)
                                            .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                            .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.State == WipProductProcessState.Finish)
                                            .OrderByDescending(p => p.CreateDate)
                                            .ToList();
                    }
                    break;
                default:
                    break;
            }

            return wipProductProcessList;
        }

        /// <summary>
        /// 获取产品关键件清单记录
        /// </summary>
        /// <param name="wipProductProcessId">生产采集记录ID列表</param>
        /// <returns>返回产品关键件清单列表</returns>
        public virtual EntityList<WipProductProcessKeyItem> GetWipProductProcessKeyItems(List<double> wipProductProcessId)
        {
            var wipProductProcessKeyItemList = new EntityList<WipProductProcessKeyItem>();
            for (int i = 0; i < Math.Ceiling((double)wipProductProcessId.Count / 1000); i++)
            {
                var queryRsult = Query<WipProductProcessKeyItem>().Where(p => wipProductProcessId.Skip(i * 1000).Take(1000).Contains(p.ProcessId));
                wipProductProcessKeyItemList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductProcessKeyItemList;
        }

        /// <summary>
        /// 获取产品测试结果记录
        /// </summary>
        /// <param name="wipProductProcessId">生产采集记录ID列表</param>
        /// <returns>返回产品测试结果记录列表</returns>
        public virtual EntityList<WipProductTestResult> GetWipProductTestResults(List<double> wipProductProcessId)
        {
            var wipProductTestResultList = new EntityList<WipProductTestResult>();
            for (int i = 0; i < Math.Ceiling((double)wipProductProcessId.Count / 1000); i++)
            {
                var queryRsult = Query<WipProductTestResult>().Where(p => wipProductProcessId.Skip(i * 1000).Take(1000).Contains(p.ProcessId));
                wipProductTestResultList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductTestResultList;
        }

        /// <summary>
        /// 获取产品维修记录
        /// </summary>
        /// <param name="wipProductVersionId">生产产品版本ID列表</param>
        /// <returns>返回产品维修记录列表</returns>
        public virtual EntityList<WipProductRepair> GetWipProductRepairs(List<double> wipProductVersionId)
        {
            var wipProductRepairList = new EntityList<WipProductRepair>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<WipProductRepair>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductRepairList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductRepairList;
        }

        /// <summary>
        /// 获取产品缺陷记录
        /// </summary>
        /// <param name="wipProductVersionId">生产产品版本ID列表</param>
        /// <returns>返回产品缺陷记录列表</returns>
        public virtual EntityList<WipProductDefect> GetWipProductDefects(List<double> wipProductVersionId)
        {
            var wipProductDefectList = new EntityList<WipProductDefect>();
            for (int i = 0; i < Math.Ceiling((double)wipProductVersionId.Count / 1000); i++)
            {
                var queryRsult = Query<WipProductDefect>().Where(p => wipProductVersionId.Skip(i * 1000).Take(1000).Contains(p.VersionId));
                wipProductDefectList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipProductDefectList;
        }

        /// <summary>
        /// 获取产品缺陷责任记录
        /// </summary>
        /// <param name="productDefectIds">产品缺陷记录ID列表</param>
        /// <returns>返回产品缺陷责任列表</returns>
        public virtual EntityList<WipDefectResponsibility> GetWipDefectResponsibilitys(List<double> productDefectIds)
        {
            var wipDefectResponsibilityList = new EntityList<WipDefectResponsibility>();
            for (int i = 0; i < Math.Ceiling((double)productDefectIds.Count / 1000); i++)
            {
                var queryRsult = Query<WipDefectResponsibility>().Where(p => productDefectIds.Skip(i * 1000).Take(1000).Contains(p.WipProductDefectId));
                wipDefectResponsibilityList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipDefectResponsibilityList;
        }

        /// <summary>
        /// 获取产品缺陷维修措施记录
        /// </summary>
        /// <param name="productDefectIds">产品缺陷记录ID列表</param>
        /// <returns>返回产品缺陷维修措施记录列表</returns>
        public virtual EntityList<WipDefectMeasure> GetWipDefectMeasures(List<double> productDefectIds)
        {
            var wipDefectMeasureList = new EntityList<WipDefectMeasure>();
            for (int i = 0; i < Math.Ceiling((double)productDefectIds.Count / 1000); i++)
            {
                var queryRsult = Query<WipDefectMeasure>().Where(p => productDefectIds.Skip(i * 1000).Take(1000).Contains(p.WipProductDefectId));
                wipDefectMeasureList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return wipDefectMeasureList;
        }

        /// <summary>
        /// 获取产品生产采集记录
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="type">条码类型</param>
        /// <returns>返回条数</returns>
        public virtual int GetWipProductProcessCountBySn(string sn, BarcodeType type)
        {
            var rownum = 0;
            switch (type)
            {
                case BarcodeType.SN:
                    {
                        rownum = Query<WipProductProcess>()
                            .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == sn)
                            .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                            .Where(p => p.State == WipProductProcessState.Finish)
                            .Count();
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                case BarcodeType.TurnoverBox:
                    break;
                case BarcodeType.KeyLabel:
                    {
                        rownum = Query<WipProductProcess>()
                            .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.KeyLabel == sn)
                            .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                            .Where(p => p.State == WipProductProcessState.Finish)
                            .Count();
                    }
                    break;
                case BarcodeType.BatchBarocde:
                    break;
                case BarcodeType.ContainerNo:
                    break;
                case BarcodeType.CombinedCode:
                    {
                        rownum = Query<WipProductProcess>()
                            .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.CombinedCode == sn)
                            .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                            .Where(p => p.State == WipProductProcessState.Finish)
                            .Count();
                    }
                    break;
                default:
                    break;
            }

            return rownum;
        }
        /// <summary>
        /// 判断是否工序重复过站
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <param name="resourceId">产线</param>
        /// <param name="processId">工序</param>
        /// <returns>重复过站返回true，否则返回false</returns>
        public virtual bool IsRepeatProcess(string barcode, BarcodeType type, double resourceId, double processId)
        {
            var isRepeat = false;
            switch (type)
            {
                case BarcodeType.SN:
                    {
                        isRepeat = Query<WipProductProcess>()
                                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == barcode)
                                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.State == WipProductProcessState.Finish)
                                .OrderByDescending(p => p.CreateDate)
                                .Count() > 0;
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                case BarcodeType.TurnoverBox:
                    break;
                case BarcodeType.KeyLabel:
                    {
                        isRepeat = Query<WipProductProcess>()
                                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.KeyLabel == barcode)
                                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.State == WipProductProcessState.Finish)
                                .OrderByDescending(p => p.CreateDate)
                                .Count() > 0;
                    }
                    break;
                case BarcodeType.BatchBarocde:
                    break;
                case BarcodeType.ContainerNo:
                    break;
                case BarcodeType.CombinedCode:
                    {
                        isRepeat = Query<WipProductProcess>()
                                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.CombinedCode == barcode)
                                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.State == WipProductProcessState.Finish)
                                .OrderByDescending(p => p.CreateDate)
                                .Count() > 0;
                    }
                    break;
                default:
                    break;
            }

            return isRepeat;
        }

        /// <summary>
        /// 判断条码是否在工序重复成功过站
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        /// <param name="resourceId">产线</param>
        /// <param name="processId">工序</param>
        /// <returns>重复成功过站返回true，否则返回false</returns>
        public virtual bool IsProcessMoveOutSuccess(string barcode, BarcodeType type, double resourceId, double processId)
        {
            var isProcessMoveOutSuccess = false;
            switch (type)
            {
                case BarcodeType.SN:
                    {
                        isProcessMoveOutSuccess = Query<WipProductProcess>()
                                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == barcode)
                                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.Result != ResultType.Fail && p.State == WipProductProcessState.Finish)
                                .Count() > 1;
                    }
                    break;
                case BarcodeType.CSN:
                    break;
                case BarcodeType.TurnoverBox:
                    break;
                case BarcodeType.KeyLabel:
                    {
                        isProcessMoveOutSuccess = Query<WipProductProcess>()
                                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.KeyLabel == barcode)
                                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.Result != ResultType.Fail && p.State == WipProductProcessState.Finish)
                                .Count() > 1;
                    }
                    break;
                case BarcodeType.BatchBarocde:
                    break;
                case BarcodeType.ContainerNo:
                    break;
                case BarcodeType.CombinedCode:
                    {
                        isProcessMoveOutSuccess = Query<WipProductProcess>()
                                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.CombinedCode == barcode)
                                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                                .Where(p => p.ResourceId == resourceId && p.ProcessId == processId && p.Result != ResultType.Fail && p.State == WipProductProcessState.Finish)
                                .Count() > 1;
                    }
                    break;
                default:
                    break;
            }

            return isProcessMoveOutSuccess;
        }

        /// <summary>
        /// 获取工单工序过站数量（去除重复过站）
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="processId">工序Id</param>
        /// <returns>工单工序过站数量</returns>
        public virtual int GetWorkOrderProcessQty(double workOrderId, double processId)
        {
            return Query<WipProductProcess>()
                .Join<WipProductVersion>((p, v) => p.VersionId == v.Id && v.WorkOrderId == workOrderId && p.ProcessId == processId)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                .Where(p => p.State == WipProductProcessState.Finish)
                .ToList()
                .GroupBy(p => p.Barcode)
                .Count();
        }

        /// <summary>
        /// 获取SN上一次未修复不良录入时间
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <returns>不存在缺陷/未修复缺陷返回null</returns>
        public virtual DateTime? GetLastDefectInputDate(double workOrderId, string sn)
        {
            //获取SN上一次未修复不良录入时间
            return Query<WipProductDefect>()
                    .Join<WipProductVersion>((d, v) => v.Id == d.VersionId && v.WorkOrderId == workOrderId && v.Sn == sn)
                    .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                    .Where(p => !p.IsFixed && !p.IsMisjudgment)
                    .OrderByDescending(p => p.CreateDate)
                    .FirstOrDefault()?.CreateDate;
        }

        /// <summary>
        /// 获取产品最后一次采集结果
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <returns>采集结果</returns>
        public virtual ResultType? GetLastWipResult(double workOrderId, string sn)
        {
            return Query<WipProductProcess>()
                .Join<WipProductVersion>((p, v) => p.VersionId == v.Id && v.WorkOrderId == workOrderId && v.Sn == sn)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                .Where(p => p.State == WipProductProcessState.Finish)
                .OrderByDescending(p => p.OperateTime)
                .FirstOrDefault()?.Result;
        }

        /// <summary>
        /// 判断产品是否完工下线
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <returns>不存在产品返回null，产品完工返回true，未产品返回false</returns>
        public virtual bool? IsSnDownline(string sn)
        {
            return Query<WipProductVersion>()
                .Where(p => p.Sn == sn && p.Product.CurrentVersionId == p.Id)
                .Select(p => p.IsFinish)
                .FirstOrDefault()?.IsFinish;
        }

        /// <summary>
        /// 获取未下线的产品条码列表
        /// </summary>
        /// <param name="sns">产品条码列表</param>
        /// <returns>未下线的产品条码列表</returns>
        public virtual IList<string> GetNoDownlineSn(List<string> sns)
        {
            return Query<WipProductVersion>()
                .Where(p => sns.Contains(p.Sn) && !p.IsFinish && p.Product.CurrentVersionId == p.Id)
                .Select(p => p.Sn)
                .ToList<string>();
        }

        /// <summary>
        /// 获取有产品版本的产品条码列表
        /// </summary>
        /// <param name="sns">产品条码列表</param>
        /// <returns>有产品版本的产品条码列表</returns>
        public virtual IList<string> GetHaveVersionSn(List<string> sns)
        {
            return Query<WipProductVersion>()
                .Where(p => sns.Contains(p.Sn) && p.Product.CurrentVersionId == p.Id)
                .Select(p => p.Sn)
                .ToList<string>();
        }

        /// <summary>
        /// 获取生产工序图片路径
        /// </summary>
        /// <param name="sn">产品条码</param>
        /// <param name="defectId">缺陷ID</param>
        /// <returns>图片路径列表</returns>
        public virtual IList<string> GetWipProcessPhotoUrls(string sn, double defectId)
        {
            return Query<WipProductProcessPhoto>()
                .Join<WipProductDefect>((ph, p) => ph.DefectId == p.Id && p.DefectId == defectId && !p.IsMisjudgment)
                .Join<WipProductDefect, WipProductVersion>((d, v) => d.VersionId == v.Id && v.Sn == sn)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                .Select(p => p.Url)
                .ToList<string>();
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <returns>产品缺陷记录</returns>
        public virtual EntityList<WipProductDefect> GetWipProductDefectList(double workOrderId)
        {
            return Query<WipProductDefect>()
                .Join<WipProductVersion>((a, b) => a.VersionId == b.Id && b.WorkOrderId == workOrderId)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
                .Where(p => !p.IsMisjudgment)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        /// <param name="workShopId">车间Id</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>产品缺陷记录</returns>
        public virtual EntityList<WipProductDefect> GetWorkShopWipProductDefectList(double workShopId, DateTime startTime, DateTime endTime)
        {
            return Query<WipProductDefect>()
                .Join<WipResource>((x, y) => x.ResourceId == y.Id && y.WorkShopId == workShopId)
                .Where(x => x.CreateDate >= startTime && x.CreateDate <= endTime)
                .Where(p => !p.IsMisjudgment)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品当前工序最新连续的采集记录
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">条码</param>
        /// <param name="processId">工序</param> 
        /// <returns>采集胜制记录</returns>
        public virtual string GetVictoryRecordString(double workOrderId, string sn, double processId)
        {
            var wipProcessList = Query<WipProductProcess>()
               .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.WorkOrderId == workOrderId && y.Sn == sn)
               .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
               .Where(p => p.State == WipProductProcessState.Finish)
               .ToList();

            IEnumerable<WipProductProcess> wipProductProcesses;

            //非当前工序的采集记录
            var notSameProcessRecords = wipProcessList.Where(p => p.ProcessId != processId);

            if (!notSameProcessRecords.Any())
            {
                wipProductProcesses = wipProcessList.Where(p => p.ProcessId == processId);
            }
            else
            {
                //取非当前工序最新创建时间，之后的当前工序的采集记录未当前工序连续采集记录（跳转取最新连续的工序记录）
                var notSameProcessRecordsMaxDate = notSameProcessRecords.Max(p => p.CreateDate);
                wipProductProcesses = wipProcessList
                    .Where(p => p.CreateDate > notSameProcessRecordsMaxDate && p.ProcessId == processId);
            }

            string victoryRecordString = string.Empty;
            if (wipProductProcesses.Any())
            {
                //不合格为0，合格为1
                victoryRecordString = string.Join(string.Empty, wipProductProcesses
                  .OrderBy(p => p.CreateDate)
                  .Select(p => p.Result == ResultType.Fail ? "0" : "1"));
            }

            return victoryRecordString;
        }

        /// <summary>
        /// 判断产品是否经过维修
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <returns>产品维修过返回true，未维修过返回false</returns>
        public virtual bool IsSnHasRepair(double workOrderId, string sn)
        {
            return Query<WipProductRepair>()
                .Join<WipProductVersion>((r, v) => r.VersionId == v.Id && v.Sn == sn && v.WorkOrderId == workOrderId)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && p.CurrentVersionId == v.Id)
                .Join<WipProductRepairDefect>((x, y) => x.Id == y.WipProductRepairId)
                .Join<WipProductRepairDefect, WipProductDefect>((r, d) => r.WipProductDefectId == d.Id && !d.IsMisjudgment)
                .Count() > 0;
        }

        /// <summary>
        /// 判断产品是否经过维修且维修的缺陷为当前工序录入
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <param name="processId">检验工序ID</param>
        /// <returns>产品维修过返回true，未维修过返回false</returns>
        public virtual bool IsSnHasRepairInProcess(double workOrderId, string sn, double processId)
        {
            return Query<WipProductRepair>()
                .Join<WipProductVersion>((r, v) => r.VersionId == v.Id && v.Sn == sn && v.WorkOrderId == workOrderId)
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && p.CurrentVersionId == v.Id)
                .Join<WipProductRepairDefect>((x, y) => x.Id == y.WipProductRepairId)
                .Join<WipProductRepairDefect, WipProductDefect>((r, d) => r.WipProductDefectId == d.Id && d.ProcessId == processId && !d.IsMisjudgment)
                .Count() > 0;
        }

        #region 拼板码绑定条码打印记录
        /// <summary>
        /// 添加拼板码绑定条码打印记录
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="panelCode">拼板码</param>
        /// <param name="barcodes">绑定条码列表</param>
        public virtual void AddToBindingSnPrintRecord(double workOrderId, string panelCode, EntityList<Barcode> barcodes)
        {
            EntityList<ToBindingSnPrintRecord> records = new EntityList<ToBindingSnPrintRecord>();
            barcodes.ForEach(barcode =>
            {
                records.Add(new ToBindingSnPrintRecord()
                {
                    Barcode = barcode,
                    WorkOrderId = workOrderId,
                    PanelCode = panelCode
                });
            });
            RF.Save(records);
        }

        /// <summary>
        /// 获取并删除拼板码已绑定待打印的条码列表
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="panelCode">拼板码</param>
        /// <returns></returns>
        public virtual EntityList<Barcode> GetAndDeleteToBePrintedSnList(double workOrderId, string panelCode)
        {
            var records = Query<ToBindingSnPrintRecord>().Where(p => p.WorkOrderId == workOrderId && p.PanelCode == panelCode).ToList();
            DB.Delete<ToBindingSnPrintRecord>().Where(p => p.WorkOrderId == workOrderId && p.PanelCode == panelCode).Execute();
            return records.Select(p => p.Barcode).AsEntityList();
        }
        #endregion


        #region 产品追溯

        /// <summary>
        /// 获取PPID集合
        /// </summary>
        /// <param name="queryInfo">PPID查询信息</param>
        /// <returns>PPID集合</returns>
        public virtual List<PPSNInfo> GetPPSNInfos(PPSNQueryInfo queryInfo)
        {
            var query = Query<WipProductProcessKeyItem>()
                .Where(p => (queryInfo.SnList.Contains(p.SourceCode) || queryInfo.ItemBatchList.Contains(p.SourceCode)) && p.ItemId == queryInfo.ItemId);
            query.Join<WipProductProcess>((k, p) => p.Id == k.ProcessId && !k.IsUnbound)
                .Join<WipProductProcess, WipProductVersion>("version", (p, v) => p.VersionId == v.Id)
                .Join<WipProductVersion, WorkOrder>("workOrder", (v, w) => v.WorkOrderId == w.Id)
                .Join<WorkOrder, Item>("Item", (w, item) => w.ProductId == item.Id);

            query.Select<WipProductProcess, WipProductVersion, WorkOrder, Item>((k, p, version, w, item) => new
            {
                BindingDate = k.CreateDate,
                Operator = k.CreateBy,
                ProductSN = version.Sn,
                PPSN = k.SourceCode,
                WorkOrderId = w.Id,
                WorkOrderNo = w.No,
                CombinedCode = version.CombinedCode,
                ProductId = item.Id,
                ProductName = item.Name,
            });
            var result = query.ToList<PPSNInfo>().ToList();
            return result;
        }

        /// <summary>
        /// 获取产品待维修缺陷记录
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <returns>产品缺陷记录列表</returns>
        public virtual List<RepairDefectViewModel> GetDefectRecords(double workOrderId, string sn)
        {
            var lastestTime = DateTime.MinValue; //RT.Service.Resolve<WashBoardController>().GetLastestWashBoardTime(workOrderId, sn);
            var defects = Query<WipProductDefect>()
               .Join<WipProductVersion>((d, v) => d.VersionId == v.Id && v.WorkOrderId == workOrderId
                    && (v.Sn == sn || v.CombinedCode == sn || v.KeyLabel == sn))
                .Join<WipProductVersion, WipProduct>((v, p) => v.ProductId == p.Id && v.Id == p.CurrentVersionId)
               .Where(p => !p.IsFixed && !p.IsMisjudgment && p.CreateDate > lastestTime)
               .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var defectVms = defects.Select(p => new RepairDefectViewModel()
            {
                WorkOrderId = workOrderId,
                Sn = sn,
                WipProductDefectId = p.Id,
                ProcessName = p.ProcessName,
                ProcessId = p.ProcessId,
                Defect = p.Defect,
                DefectCode = p.DefectCode,
                DefectDesc = p.DefectDesc,
                DefectLocation = p.Location,
                Remark = p.Remark
            }).ToList();

            if (defectVms.Count == 0)
                return defectVms;

            var repairMainRecord = RT.Service.Resolve<RepairController>().GetRepairRecord(workOrderId, sn);

            if (repairMainRecord != null && repairMainRecord.RepairDefectRecordList.Count > 0)
            {
                foreach (var defectVm in defectVms)
                {
                    var record = repairMainRecord.RepairDefectRecordList
                        .FirstOrDefault(p => p.ProductDefectId == defectVm.WipProductDefectId);

                    if (record == null)
                        continue;

                    defectVm.ReloadBarcode = record.ReloadBarcode;
                    defectVm.ActualDefect = record.ActualDefect;
                    defectVm.ActualDefectCode = record.ActualDefectCode;
                    defectVm.ActualDefectDesc = record.ActualDefectDesc;
                    defectVm.RepairSolution = record.RepairSolution;
                    defectVm.RepairLocation = record.RepairLocation;
                    defectVm.Remark = record.Remark;
                    defectVm.MeasureList.AddRange(record.MeasureList.Select(p => p.Measure));
                    defectVm.ResponsibilityList.AddRange(record.ResponseList.Select(p => p.Responsibility));
                    defectVm.MeasureCode = string.Join(";", defectVm.MeasureList.Select(p => p.Code));
                    defectVm.Responsibility = string.Join(";", defectVm.ResponsibilityList.Select(p => p.Description));
                }

                repairMainRecord.RepairDefectRecordList
                    .Where(p => p.IsNewAdd)
                    .ForEach(record =>
                    {
                        var defectVm = new RepairDefectViewModel()
                        {
                            WorkOrderId = repairMainRecord.WorkOrderId,
                            Sn = repairMainRecord.Sn,
                            ProcessId = record.ProcessId,
                            ProcessName = record.ProcessName,
                            Defect = record.Defect,
                            DefectCode = record.DefectCode,
                            DefectDesc = record.DefectDesc,
                            ActualDefect = record.ActualDefect,
                            ActualDefectCode = record.ActualDefectCode,
                            ActualDefectDesc = record.ActualDefectDesc,
                            ReloadBarcode = record.ReloadBarcode,
                            RepairSolution = record.RepairSolution,
                            RepairLocation = record.RepairLocation,
                            Remark = record.Remark,
                            IsNewAdd = record.IsNewAdd
                        };

                        defectVm.MeasureList.AddRange(record.MeasureList.Select(p => p.Measure));
                        defectVm.ResponsibilityList.AddRange(record.ResponseList.Select(p => p.Responsibility));
                        defectVms.Add(defectVm);
                    });
            }
            return defectVms;
        }
        #endregion

        #region 工单制造档案
        /// <summary>
        /// 根据工单ids获取关键件
        /// </summary>
        /// <param name="sameWoIds"></param>
        /// <returns></returns>
        public virtual List<WipProductKeyItem> GetWipKeyItemByWoIds(List<double> sameWoIds)
        {
            List<WipProductKeyItem> singleKeyItemList = new List<WipProductKeyItem>();
            sameWoIds.SplitDataExecute(tempIds =>
            {
                var list = DB.Query<WipProductProcessKeyItem>().Where(p => !p.IsUnbound)
                .Join<WipProductProcess>((x, y) => x.ProcessId == y.Id)
                .Join<WipProductProcess, WipProductVersion>((x, y) => x.VersionId == y.Id && tempIds.Contains(y.WorkOrderId))
                .GroupBy<WipProductProcess, WipProductVersion>((k, p, v) => new
                {
                    k.ItemId,
                    k.ItemExtProp,
                    v.WorkOrderId,
                })
                .Select<WipProductProcess, WipProductVersion>((k, p, v) => new
                {
                    ItemId = k.ItemId,
                    ItemExtProp = k.ItemExtProp,
                    WoOrderId = v.WorkOrderId,
                    Qty = k.Qty.SUM(),
                }).ToList<WipProductKeyItem>().ToList();
                singleKeyItemList.AddRange(list);
            });
            return singleKeyItemList;
        }

        /// <summary>
        /// 根据工单id获取关键件清单
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual List<WipProductKeyItem> GetWipKeyItemByWoId(double woId)
        {
            return DB.Query<WipProductProcessKeyItem>().Where(p => !p.IsUnbound)
                .Join<WipProductProcess>((x, y) => x.ProcessId == y.Id)
                .Join<WipProductProcess, WipProductVersion>((x, y) => x.VersionId == y.Id && woId == y.WorkOrderId)
                .GroupBy<WipProductProcess, WipProductVersion>((k, p, v) => new
                {
                    k.ItemId,
                    k.ItemExtProp,
                })
                .Select<WipProductProcess, WipProductVersion>((k, p, v) => new
                {
                    ItemId = k.ItemId,
                    ItemExtProp = k.ItemExtProp,
                    Qty = k.Qty.SUM(),
                }).ToList<WipProductKeyItem>().ToList();
        }
        #endregion

        /// <summary>
        /// 获取特定时间范围内的生产采集记录
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual List<WipProductVersionInfo> GetWipProductVersionInfos(QTimeReportViewModelCriteria criteria)
        {
            var wipRecords = Query<WipProductProcess>()
                .Join<WipProductVersion>((x, y) => x.VersionId == y.Id)
                .Where(p => p.OperateTime >= criteria.CollectTime.BeginValue && p.OperateTime <= criteria.CollectTime.EndValue)
                .WhereIf<WipProductVersion>(criteria.WipResourceId != null, (x, y) => x.ResourceId == criteria.WipResourceId)
                .WhereIf<WipProductVersion>(criteria.WorkOrderId != null, (x, y) => y.WorkOrderId == criteria.WorkOrderId)
                .WhereIf<WipProductVersion>(criteria.Sn.IsNotEmpty(), (x, y) => y.Sn.Contains(criteria.Sn))
                .WhereIf<WipProductVersion>(criteria.ProCode.IsNotEmpty(), (x, y) => y.WorkOrder.Product.Code.Contains(criteria.ProCode))
                .WhereIf<WipProductVersion>(criteria.ProName.IsNotEmpty(), (x, y) => y.WorkOrder.Product.Name.Contains(criteria.ProName))
                .Select<WipProductVersion>((x, y) => new
                {
                    ParentId = y.Id,
                    Id = x.Id,
                    Sn = y.Sn,
                    WorkOrderId = y.WorkOrderId,
                    WoNo = y.WorkOrder.No,
                    WipResourceId = x.ResourceId,
                    WipResourceCode = x.Resource.Code,
                    ProductId = y.WorkOrder.ProductId,
                    ProductCode = y.WorkOrder.Product.Code,
                    ProductName = y.WorkOrder.Product.Name,
                    ProcessId = x.ProcessId,
                    ProcessName = x.Process.Name,
                    State = x.State,
                    OperateTime = x.OperateTime,
                    Qty = 1,
                    IsBatch = false,
                }).ToList<WipProductVersionInfo>().ToList();
            return wipRecords;
        }
    }
}