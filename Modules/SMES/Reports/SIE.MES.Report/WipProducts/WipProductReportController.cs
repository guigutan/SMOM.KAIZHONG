using SIE.Common.InvOrg;
using SIE.Data;
using SIE.Data.Common;
using SIE.Domain;
using SIE.Domain.ORM;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品生产版本报表控制器
    /// </summary>
    public class WipProductReportController : DomainController
    {
        #region 生产通用报表
        /// <summary>
        /// 获取产品版本集合
        /// </summary>
        /// <param name="criteria">产品版本查询实体</param>
        /// <returns>产品版本集合</returns>
        public virtual EntityList<WipProductVersionReport> GetWipProductVersions(WipProductReportCriteria criteria)
        {
            if (!criteria.Sn.IsNullOrEmpty())  ////条码不为空情况下需要递归查询管理条码版本
                return GetWipProductVersionList(criteria);
            else
                return GetWipProductVersionListBySNIsNull(criteria);
        }

        /// <summary>
        /// 获取产品版本集合
        /// </summary>
        /// <param name="criteria">产品版本查询实体</param> 
        /// <returns>产品版本集合</returns>
        EntityList<WipProductVersionReport> GetWipProductVersionList(WipProductReportCriteria criteria)
        {
            var meta = RF.Find<WipProductVersionReport>().EntityMeta;
            var tableName = meta.TableMeta.TableName;
            var sn = meta.Property(WipProductVersionReport.SnProperty).ColumnMeta.ColumnName;
            var relevanceSn = meta.Property(WipProductVersionReport.RelevanceSnProperty).ColumnMeta.ColumnName;
            var id = meta.Property(Entity.IdProperty).ColumnMeta.ColumnName;
            var invOrgId = meta.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            var isPhantom = meta.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
            List<double> versionIds = new List<double>();
            string cycle = "CYCLE ID SET DUP_ID TO 'Y' DEFAULT 'N'";
            var setting = SIE.Domain.ORM.RdbDataProvider.Get(RF.Find<WipProductVersionReport>()).DbSetting;
            if (setting.IsPostgreSqlServer() || setting.IsSqlserverDbServer()||setting.IsVastDataServer())
            {
                cycle = "";
            }
            var sqlPre = "with version({0},{1},{2}) as".FormatArgs(id, sn, relevanceSn);
            if (setting.IsPostgreSqlServer() || setting.IsVastDataServer())
            {
                sqlPre = "with recursive version({0},{1},{2}) as".FormatArgs(id, sn, relevanceSn);
            }

            string sql = sqlPre + @" (
  select {0},{1},{3}
    from {2}
   where {1} like '{4}'  or RELEVANCE_SN = '{4}'
  union all (select v.{0},v.{1},v.{3}
               from {2} v
               inner join version vc on v.{1} = vc.{3} and v.{1} != vc.{3}
               where v.{5} ={7} and v.{6}=0)){8}
  select * from version".FormatArgs(id, sn, tableName, relevanceSn, criteria.Sn, invOrgId, isPhantom, RT.InvOrg, cycle);
            using (var db = DbAccesserFactory.Create(ReportEntityDataProvider.ConnectionStringName))
            {
                using (System.Data.IDataReader dr = db.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        versionIds.Add(dr.GetDecimal(0).ConvertTo<double>());
                    }
                }
            }
            var query = Query<WipProductVersionReport>();

            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(criteria.No));
            }
            //if (!criteria.Sn.IsNullOrEmpty())
            //{
            //    query.or(p => p.RelevanceSn ==criteria.Sn);
            //}

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

            //拼板码
            if (!string.IsNullOrEmpty(criteria.PanelCode))
            {
                query.Where(p => p.CombinedCode.Contains(criteria.PanelCode));
            }

            //组件条码
            if (!string.IsNullOrEmpty(criteria.KeyLabel))
            {
                query.Where(p => p.KeyLabel.Contains(criteria.KeyLabel));
            }

            if (criteria.ItemSN.IsNotEmpty())
            {
                query.Exists<WipProductProcessKeyItem>((x, y) => y.Where(p => p.Process.VersionId == x.Id && !p.IsUnbound && p.SourceCode == criteria.ItemSN));
            }

            var versionIdList = versionIds.Distinct();

            return query.Where(p => versionIdList.Contains(p.Id)).OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品版本集合
        /// </summary>
        /// <param name="criteria">产品版本查询实体</param>
        /// <returns>产品版本集合</returns>
        EntityList<WipProductVersionReport> GetWipProductVersionListBySNIsNull(WipProductReportCriteria criteria)
        {
            var query = Query<WipProductVersionReport>();

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

            //if (!criteria.No.IsNullOrEmpty() || !criteria.PanelWorkOrderNo.IsNullOrEmpty())
            //{
            //    query.Join<WorkOrder>("w", (x, w) => x.WorkOrderId == w.Id);

            //    if (!criteria.No.IsNullOrEmpty())
            //    {
            //        query.Where<WorkOrder>((x, w) => w.No.Contains(criteria.No));
            //    }

            //    if (!criteria.PanelWorkOrderNo.IsNullOrEmpty())
            //    {
            //        query
            //            .Join<WorkOrder, WorkOrder>("w1", (w, w1) => w.PanelWorkOrderId == w1.Id)
            //            .Where<WorkOrder, WorkOrder>((x, w, w1) => w1.No.Contains(criteria.PanelWorkOrderNo));
            //    }
            //}

            if (criteria.ProcessId.HasValue)
                //query.Where(p => p.CurrentProcess.ProcessId == criteria.ProcessId);
                query.Where(p => p.NowProcessId == criteria.ProcessId);
            if (criteria.StartDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.StartDate.BeginValue);
            if (criteria.StartDate.EndValue.HasValue)
                query.Where(p => p.CreateDate <= criteria.StartDate.EndValue);
            if (criteria.NextProcessId.HasValue)
                query.Where(p => p.NextProcessId == criteria.NextProcessId);

            //拼板码
            if (!string.IsNullOrEmpty(criteria.PanelCode))
            {
                query.Where(p => p.CombinedCode.Contains(criteria.PanelCode));
            }

            //组件条码
            if (!string.IsNullOrEmpty(criteria.KeyLabel))
            {
                query.Where(p => p.KeyLabel.Contains(criteria.KeyLabel));
            }

            if (criteria.ItemSN.IsNotEmpty())
            {
                query.Exists<WipProductProcessKeyItem>((x, y) => y.Where(p => p.Process.VersionId == x.Id && !p.IsUnbound && p.SourceCode == criteria.ItemSN));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion
    }
}
