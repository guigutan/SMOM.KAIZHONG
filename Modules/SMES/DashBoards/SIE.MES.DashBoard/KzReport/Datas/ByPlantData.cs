using SIE.Core.ApiModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 
    /// </summary>
    public static class ByPlantData 
    {
        /// <summary>
        /// 产品线或厂部维度时，只统计的工序
        /// </summary>
        public static IReadOnlyList<string> onlyProcesses= new List<string> { "成品包装", "押出" };




        /// <summary>
        /// 调试用的
        /// </summary>
        public static List<string> debugSettingUrl = new List<string>
        {
            "http://172.17.8.80:2151/api/dataportal/invoke",           
            "http://172.26.111.40:2030/api/dataportal/invoke",
            "http://10.6.0.35:2030/api/dataportal/invoke",
            "http://10.7.200.37:2010/api/dataportal/invoke",
            "http://172.17.12.192:2030/api/dataportal/invoke"
        };

        /// <summary>
        /// 调试用的
        /// </summary>
        public static List<DictionaryData> debugMrpDics = new List<DictionaryData>
        {
            new DictionaryData
            {
                DicKey = "1",
                DicValue = new List<string>() { "A01","A02","A03","A04","A06","A07","A08","A09","A10","B01","B02","B03","B04","B05","B06","B07","B08","B09","B10","B11","B12","B13","C01","C02","C11","D01","D02","D03","D04","D05","D08","D11","D13","D14","D21","E01","E02","E03","E04","E07","F01","F02","F03","F04","P01","R01","R02","R03","R04","R05","R06","R07","R08","R09" }
            },
            new DictionaryData
            {
                DicKey = "2",
                DicValue = new List<string>() { "A01","A02","A03","A04","A06","A09","D01","D05","R01","R02","R03","R04","R05","R06","R07","R08","R09" }
            },
            new DictionaryData
            {
                DicKey = "3",
                DicValue = new List<string>() { "A01","A02","A06","A07","A08","A09","A10","R01","R02","R03","R04","R05","R06","R07","R08","R09" }
            },
            new DictionaryData
            {
                DicKey = "4",
                DicValue = new List<string>() { "D01","D03","D05","D21","R01","R03","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "5",
                DicValue = new List<string>() { "D11","D13","D14","D21","P01","R01","R02","R03","R04","R05","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "6",
                DicValue = new List<string>() { "D04","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "7",
                DicValue = new List<string>() { "D03","D05","E01","R01","R02","R03","R05","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "8",
                DicValue = new List<string>() { "B01","B02","B03","B04","B05","B06","B07","B08","B13","D11","R01","R02","R03","R04","R05","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "9",
                DicValue = new List<string>() { "B09","B10","B11","B12","R01","R02","R03","R04","R05","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "10",
                DicValue = new List<string>() { "D04","D05","D11","D13","R01","R02","R03","R04","R06","R08" }
            },
            new DictionaryData
            {
                DicKey = "11",
                DicValue = new List<string>() { "C01" }
            },
            new DictionaryData
            {
                DicKey = "13",
                DicValue = new List<string>() { "E01","R01","R02","R03","R04","R05","R06","R09" }
            },
            new DictionaryData
            {
                DicKey = "15",
                DicValue = new List<string>() { "C01","C02","R01","R02","R03","R04","R05","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "16",
                DicValue = new List<string>() { "C11","R01","R02","R03","R06" }
            },
            new DictionaryData
            {
                DicKey = "17",
                DicValue = new List<string>() { "D01","D11","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "18",
                DicValue = new List<string>() { "D02","R01","R06","R07","R08" }
            },
            new DictionaryData
            {
                DicKey = "20",
                DicValue = new List<string>() { "E01","E02","E03","R01","R02","R03","R04","R06","R07","R08","R09" }
            }
        };


    }

    /// <summary>
    /// 产部看板-请求参数
    /// </summary>
    [Serializable]
    public class RequestPlantData
    {       
        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName { get; set; }


        /// <summary>
        /// 工厂编码（库存组织代码）
        /// </summary>
        public List<string> FactoryCodeList { get; set; }

    }



    #region 安灯相关  
      
    /// <summary>
    /// 安灯异常报表-厂部。响应参数
    /// </summary>
    public class AndonReportDataOfType
    {
        /// <summary>
        /// 安灯次数（按类型）
        /// </summary>
        public List<CountOfType> CountOfType { get; set; }       

        /// <summary>
        /// 安灯消息
        /// </summary>
        public List<BaseAdonAndonManage>  AndonMsg { get; set; }

        /// <summary>
        /// 所有总次数（包含所有类型）
        /// </summary>
        public decimal AllTotalCount { get; set; } = 0;
    }

    /// <summary>
    /// 安灯次数（按类型）
    /// </summary>
    public class CountOfType
    {
        /// <summary>
        /// 安灯类型
        /// </summary>
        public string AndonType { get; set; }
        /// <summary>
        /// 总次数（按类型）
        /// </summary>
        public decimal TotalCount { get; set; } = 0;
        /// <summary>
        /// 超30分钟的数量
        /// </summary>
        public decimal Count1 { get; set; } = 0;
        /// <summary>
        /// 超60分钟的数量
        /// </summary>
        public decimal Count2 { get; set; } = 0;
        /// <summary>
        /// 超120分钟的数量
        /// </summary>
        public decimal Count3 { get; set; } = 0;
        /// <summary>
        /// 超240分钟的数量
        /// </summary>
        public decimal Count4 { get; set; } = 0;

    }
     
    /// <summary>
    /// 安灯管理消息
    /// </summary>
    public class BaseAdonAndonManage
    {
        /// <summary>
        /// 安灯类型编码
        /// </summary>
        public string AndonTypeCode { get; set; }

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName { get; set; }

        /// <summary>
        /// 安灯编码
        /// </summary>
        public string AdonManageCode { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string WipResourceName { get; set; }
        /// <summary>
        /// 安灯区域描述
        /// </summary>
        public string AndonArea { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipAccountCode { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string ProblemDesc { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime TriggerTime { get; set; }

        /// <summary>
        /// 所属厂部编码
        /// </summary>
        public string PlantCode { get; set; }

        /// <summary>
        ///  所属厂部名称
        /// </summary>
        public string PlantName { get; set; }


    }

    /// <summary>
    /// 所有的安灯类型
    /// </summary>
    public class AllAndonStype
    {
        /// <summary>
        /// 安灯类型编码
        /// </summary>
        public string AndonTypeCode { get; set; }

        /// <summary>
        /// 安灯类型名称
        /// </summary>
        public string AndonTypeName { get; set; }

    }

    #endregion

    #region 达成率相关  
    /// <summary>
    /// 最近7日达成率、当前自然月达成率
    /// </summary>
    [Serializable]
    public class ResponsePlanAchievedData
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime ThisDay { get; set; }

        /// <summary>
        /// 计划产量
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际产量
        /// </summary>
        public decimal ActualQty { get; set; }


        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty { get; set; }


        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 返工数
        /// </summary>
        public decimal ReworkQty { get; set; }




        /// <summary>
        /// 达成率
        /// </summary>
        public decimal DayAchievedRate { get; set; }

        /// <summary>
        /// 不良率
        /// </summary>
        public decimal NgQtyRate { get; set; }

    }



    /// <summary>
    /// 计划产量
    /// </summary>
    [Serializable]
    public class ResponseDispatchQtyData 
    {
        /// <summary>
        /// 计划开始日期
        /// </summary>
        public DateTime? PlanBeginDate { get; set; }

        /// <summary>
        /// 计划产量
        /// </summary>
        public decimal DispatchQTY { get; set; }

    }









    /// <summary>
    /// 月达计划达成率
    /// </summary>
    [Serializable]
    public class ResponseMouthAchievedRate
    {
        /// <summary>
        /// 月达计划达成率
        /// </summary>
        public decimal MouthAchievedRate { get; set; }
    }

    #endregion

    #region 报工记录相关 

    /// <summary>
    /// 报工记录请求参数
    /// </summary>
    [Serializable]
    public class RequestReportRecordData
    {
        /// <summary>
        ///报工时间范围
        /// </summary>
        public DateRange ReportTime { get; set; }

        /// <summary>
        /// 过滤的工序(只取的工序)
        /// </summary>
        public List<string> TargetProcesses { get; set; }
}


    /// <summary>
    /// 报工记录响应参数
    /// </summary>
    public class ReportRecordData
    {
        /// <summary>
        /// 报工时间
        /// </summary>
        public DateTime? ReportTime { get; set; }

        /// <summary>
        /// 报工数量
        /// </summary>
        public decimal ReportQty { get; set; }

        /// <summary>
        /// 合格数
        /// </summary>
        public decimal OkQty { get; set; }


        /// <summary>
        /// 不合格数
        /// </summary>
        public decimal NgQty { get; set; }

        /// <summary>
        /// 返工数
        /// </summary>
        public decimal ReworkQty { get; set; }


    }

    #endregion

    #region 可疑品相关 

    /// <summary>
    /// 可疑品标签,响应参数
    /// </summary>
    public class SuspectProdData
    {
        /// <summary>
        /// 日期（统计到天）
        /// </summary>
        public DateTime? UpdateDateDay { get; set; }
       

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 缺陷数量
        /// </summary>
        public decimal DefectQTY { get; set; }

        /// <summary>
        /// 缺陷比例
        /// </summary>
        public decimal DefectRate { get; set; }

    }

    






    /// <summary>
    /// 可疑品处理报表
    /// </summary>
    [Serializable]
    public class SuspectReportData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 部门(厂部)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 总产量(万)
        /// </summary>
        public decimal TotalQty { get; set; }

        /// <summary>
        /// 报废总量(万)
        /// </summary>
        public decimal TotalNgQty { get; set; }

        /// <summary>
        /// 可疑品总量(万)
        /// </summary>
        public decimal TotalSuspectQty { get; set; }

        /// <summary>
        /// 报废率
        /// </summary>
        public decimal NgQtyRate { get; set; }

        /// <summary>
        /// 可疑品率
        /// </summary>
        public decimal SuspectRate { get; set; }

        /// <summary>
        /// 一次下线合格率
        /// </summary>
        public decimal OkRate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class SuspectReportDataFactory
    {
        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 总产量(万)
        /// </summary>
        public decimal TotalQty { get; set; }

        /// <summary>
        /// 报废总量(万)
        /// </summary>
        public decimal TotalNgQty { get; set; }

        /// <summary>
        /// 可疑品总量(万)
        /// </summary>
        public decimal TotalSuspectQty { get; set; }
    }

    /// <summary>
    /// 可疑品缺陷
    /// </summary>
    [Serializable]
    public class SuspectDefectData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string DefectCode { get; set; }

        /// <summary>
        /// 缺陷名称
        /// </summary>
        public string DefectName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public decimal Rate { get; set; }
    }

    #endregion

    #region 产线设备状态安全生产天数相关 

    /// <summary>
    /// 产线或设备状态
    /// </summary>
    public class LineMachineStatusData
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 状态数量
        /// </summary>
        public decimal StatusQty { get; set; }
    }

    /// <summary>
    /// 安全生产天数 响应参数
    /// </summary>
    public class WorkSafetyDayData
    {
        /// <summary>
        /// 安全生产天数
        /// </summary>
        public decimal WorkSafetyDay { get; set; }
    }

    #endregion




    /// <summary>
    /// 派工任务表响应参数
    /// </summary>
    public class DispatchTaskData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public decimal InvOrgId { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController { get; set; }
        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode { get; set; }
        /// <summary>
        /// 任务单数量
        /// </summary>
        public decimal DispatchQty { get; set; }
        /// <summary>
        /// 已报工数量
        /// </summary>
        public decimal ReportQty { get; set; }
        /// <summary>
        /// 排程开始时间
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }     

    }







}
