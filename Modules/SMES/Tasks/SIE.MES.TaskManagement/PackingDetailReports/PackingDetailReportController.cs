using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingQC;
using SIE.MES.WIP.Pressure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingDetailReports
{
    /// <summary>
    /// 包装QC确认明细报表 控制器
    /// </summary>
    public class PackingDetailReportController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<PackingDetailReport> Fetch(PackingDetailReportCriteria criteria)
        {
            var datas = new EntityList<PackingDetailReport>();

            // 1. 查询父表 PackingQc
            var q_PackingQc = Query<PackingQc>();

            // 查询条件          
            bool existsWhere=false;
                      
            if (!string.IsNullOrEmpty(criteria.BlueLabel))
            {
                existsWhere = true;               
                q_PackingQc.Where(p => p.BlueLabel == criteria.BlueLabel);
            }
            if (criteria.PackIdent.HasValue)
            {
                existsWhere = true;
                q_PackingQc.Where(p => p.PackIdent == criteria.PackIdent);
            }
            if (criteria.Confirm.HasValue)
            {
                existsWhere = true;
                q_PackingQc.Where(p => p.Confirm == criteria.Confirm);
            }
            if (criteria.ReportsType.HasValue)
            {
                existsWhere = true;
                q_PackingQc.Where(p => p.ReportsType == criteria.ReportsType);
            }
            if (criteria.ResourceId != null)
            {
                existsWhere = true;
                q_PackingQc.Where(p => p.ResourceId == criteria.ResourceId);
            }
            if (!string.IsNullOrEmpty(criteria.ItemCode))
            {
                existsWhere = true;             
                q_PackingQc.Where(p => p.Item.Code == criteria.ItemCode);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                existsWhere = true;
                q_PackingQc.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                existsWhere = true;
                q_PackingQc.Where(p => p.CreateDate <= criteria.CreateDate.EndValue);
            }

            // 子表关联条件（Exists 方式）
            if (!string.IsNullOrEmpty(criteria.ProductLabel))
            {
                existsWhere = true;
                q_PackingQc.Exists<PackingDetail>((a, b) =>
                    b.Where(p => p.PackingQcId == a.Id && p.ProductLabel == criteria.ProductLabel));
            }
            if (!string.IsNullOrEmpty(criteria.BatchLabel))
            {
                existsWhere = true;
                q_PackingQc.Exists<PackingDetail>((a, b) =>
                    b.Where(p => p.PackingQcId == a.Id && p.BatchLabel == criteria.BatchLabel));
            }
            if (!string.IsNullOrEmpty(criteria.WorkOrderNo))
            {
                existsWhere = true;
                q_PackingQc.Exists<PackingDetail>((a, b) =>
                    b.Where(p => p.PackingQcId == a.Id && p.WorkOrderNo == criteria.WorkOrderNo));
            }

            if (!existsWhere) { throw new ValidationException("数据量大，您最少要设置一个查询条件"); }







            // 2. 执行查询，使用 EagerLoadOptions 预加载视图属性，提高性能
            var packingQcList = q_PackingQc.ToList(
                new PagingInfo(),
                new EagerLoadOptions().LoadWithViewProperty()
            );

            // 3. 遍历父表，获取子表明细
            foreach (var packingQc in packingQcList)
            {
                
                var detailList = GetPackingDetailsByParentId(packingQc.Id);

                if (detailList != null && detailList.Count > 0)
                {
                    foreach (var detail in detailList)
                    {
                        var report = CreateReportFromEntities(packingQc, detail);
                        datas.Add(report);
                    }
                }
                else
                {
                    // 如果没有明细，也生成一条记录（明细字段为空）
                    var report = CreateReportFromEntities(packingQc, null);
                    datas.Add(report);
                }
            }

            return datas;
        }




        /// <summary>
        /// 根据父表ID获取子表明细
        /// </summary>
        /// <param name="packId">PackingQc ID</param>
        /// <returns>明细列表</returns>
        private EntityList<PackingDetail> GetPackingDetailsByParentId(double packId)
        {           
            var list = Query<PackingDetail>()
                .Where(p => p.PackingQcId == packId)
                .ToList(new PagingInfo(), new EagerLoadOptions().LoadWithViewProperty());

            // 如果是SN标签类型，获取测试值
            if (list.Count > 0 && list.FirstOrDefault().LabelType == LabelTypeEnum.SnLabel)
            {
                var sns = list.Select(p => p.ProductLabel).ToList();
                var snList = RT.Service.Resolve<WipPressureController>().GetWipPressureSns(sns);

                
                var resultList = new EntityList<PackingDetail>();
                foreach (var item in list)
                {
                    item.TestValue = snList.FirstOrDefault(p => p.Sn == item.ProductLabel)?.RawData;

                    // 如果是SN标签类型，获取测试值.并且立马只返回当前这一条,其他的数据都不要。                   
                    resultList.Add(item);
                    return resultList;  

                }
            }

            return list;
        }

     
        /// <summary>
        /// 从父表和子表实体创建报表对象
        /// </summary>
        /// <param name="packingQc">父表实体</param>
        /// <param name="detail">子表实体（可为空）</param>
        /// <returns>报表对象</returns>
        private PackingDetailReport CreateReportFromEntities(PackingQc packingQc, PackingDetail detail)
        {
            return new PackingDetailReport
            {
                // ===== 从 PackingQc（父表）赋值的字段 =====
                BlueLabel = packingQc.BlueLabel,
                OldBlueLabel = packingQc.OldBlueLabel,
                BlueLableNum = packingQc.BlueLableNum,
                BlueLablePackingNum = packingQc.PackingNum,
                UnboxedQty = (int)packingQc.UnboxedQty,
                PackIdent = packingQc.PackIdent ?? PackIdentEnum.FullTank,
                Confirm = packingQc.Confirm ?? ConfirmEnum.NO,
                ItemCode = packingQc.Item.Code,
                ItemName = packingQc.ItemName,
                ResourceCode = packingQc.Resource.Code,
                ResourceName = packingQc.Resource.Name,

                IsUploadSap = packingQc.IsUploadSap ?? false,
                UploadResult = packingQc.UploadResult,
                ReportsType = packingQc.ReportsType,
                CreateByName = packingQc.CreateByName,
                CreateDate = packingQc.CreateDate,
                UpdateByName = packingQc.UpdateByName,
                UpdateDate = packingQc.UpdateDate,

                // ===== 从 PackingDetail（子表）赋值的字段 =====
                WorkOrderNo = detail?.WorkOrderNo ?? string.Empty,
                ProductLabel = detail?.ProductLabel ?? string.Empty,
                BatchLabel = detail?.BatchLabel ?? string.Empty,
                PackingNum = detail?.PackingNum ?? 0,
                LabelType = detail?.LabelType ?? LabelTypeEnum.BatchLabel,
                TestValue = detail?.TestValue ?? string.Empty
            };
        }        
    

    }













}



