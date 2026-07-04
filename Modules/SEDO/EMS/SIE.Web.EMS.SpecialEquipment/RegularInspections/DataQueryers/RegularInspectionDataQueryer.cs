using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.EMS.SpecialEquipment.RegularInspections.Configs;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections.DataQueryers
{
    /// <summary>
    /// 特种设备定检数据查询类
    /// </summary>
    public class RegularInspectionDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取单据相关信息-(单据级别的一次获取，减少请求数)
        /// </summary>
        /// <param name="billId">单据Id</param>
        /// <returns></returns>
        public BillInfo GetBillInfoById(double billId)
        {
            var bill = RF.GetById<RegularInspection>(billId);
            Check.NotNull(bill, nameof(bill));

            BillInfo info = new BillInfo
            {
                VoInitialSamplingQty = GetInitialSamplingQty(bill),
                VoMaxValueListCount = GetMaxValueListCount(bill),
                VoAllQuantitativeValues = GetQuantitativeValueList(bill),
                VoDetailWithMaxValueList = GetDetailWithMaxValueList(bill),
            };
            return info;
        }

        /// <summary>
        /// 检验单据中拥有最多测试值的检验项目
        /// </summary>
        /// <param name="bill">检验单</param>
        /// <returns>检验项目</returns>
        public RegularInspectionDetail GetDetailWithMaxValueList(RegularInspection bill)
        {
            Check.NotNull(bill, nameof(bill));
            var list = bill.RegularInspectionDetailList;
            if (list.Count > 0)
            {
                int maxCount = list.Max(f => f.RegularInspectionValueList.Count);
                return list.FirstOrDefault(p =>
                {
                    return p.RegularInspectionValueList.Count == maxCount;
                });
            }
            else
            {
                return new RegularInspectionDetail();
            }
        }

        /// <summary>
        /// 检验中的单据获取所有检验项目中已生成数据列数的最大值
        /// </summary>
        /// <param name="bill">校验记录</param>
        /// <returns>最大数据数</returns>
        public int GetMaxValueListCount(RegularInspection bill)
        {
            var list = bill.RegularInspectionDetailList;
            if (list.Count > 0)
            {
                return list.Max(p => p.RegularInspectionValueList.Count);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取定量的检测值列表
        /// </summary>
        /// <param name="bill">成品单据实例</param>
        /// <returns></returns>
        public IReadOnlyList<RegularInspectionValue> GetQuantitativeValueList(RegularInspection bill)
        {
            Check.NotNull(bill, nameof(bill));
            var dtlIds = bill.RegularInspectionDetailList.Select(p => p.Id).ToList();
            return RT.Service.Resolve<RegularInspectionController>().GetDetailValues(dtlIds).ToList();
        }

        /// <summary>
        /// 配置数据列数量
        /// </summary>
        /// <param name="bill">校验记录</param>
        /// <returns>初始数据列数</returns>
        public int GetInitialSamplingQty(RegularInspection bill)
        {
            Check.NotNull(bill, nameof(bill));
            var config = ConfigService.GetConfig(new ValueConfig(), typeof(RegularInspection));
            if (!(config?.Qty).HasValue)
            {
                throw new ValidationException("数据列数量未配置。".L10N());
            }
            return config.Qty.Value;
        }

    }

    /// <summary>
    /// 单据相关信息
    /// </summary>
    public class BillInfo
    {
        //为了与单据实体属性区分，全部加上vo(view object)
        /// <summary>
        /// 初始生成数据列数量
        /// </summary>
        public int VoInitialSamplingQty { get; set; }
        /// <summary>
        /// 已生成数据数的最大值
        /// </summary>
        public int VoMaxValueListCount { get; set; }
        /// <summary>
        /// 最多检测值的检验项目
        /// </summary>
        public RegularInspectionDetail VoDetailWithMaxValueList { get; set; }
        /// <summary>
        /// 定量检测值列表
        /// </summary>
        public IReadOnlyList<RegularInspectionValue> VoAllQuantitativeValues { get; set; }
    }
}
