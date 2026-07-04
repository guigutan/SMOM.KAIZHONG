using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 承诺:以下全部数据都只来于同一个工厂，不会也不能夹杂其他工厂数据
    /// </summary>
    [Serializable]
    public class WorkOrderData
    {
        /// <summary>
        /// 工单信息
        /// </summary>
        public List<WorkOrderInf> workOrderInfs { get; set; } = new List<WorkOrderInf>();

        /// <summary>
        /// BOM信息
        /// </summary>
        public List<BomInf> bomInfs { get; set; } = new List<BomInf>();

        /// <summary>
        /// 工艺路线信息
        /// </summary>
        public List<LayoutInf> layoutInfs { get; set; } = new List<LayoutInf>();

        /// <summary>
        /// 父级旧料号信息
        /// </summary>
        public List<ParentItemInf> parentItemInfs { get; set; } = new List<ParentItemInf>();
    }

    /// <summary>
    /// 工单信息
    /// </summary>
    [Serializable]
    public class WorkOrderInf
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 订单数量（SAP目标数量）
        /// </summary>
        public decimal GAMNG { get; set; }

        /// <summary>
        /// 制卡数量
        /// </summary>
        public decimal ZTFL { get; set; }

        /// <summary>
        /// 工单类型
        /// </summary>
        /*
         KZ01	标准生产订单    对应MES量产
         KZ02	试制生产订单    对应MES试产
         KZ03	库存品返工生产订单     对应MES返工
         KZ04	改制生产订单                对应MES返工
         KZ05	在制返工生产订单         对应MES返工
         */
        public string DAUAT { get; set; }

        /// <summary>
        /// 计划开始时间（时分秒）
        /// </summary>
        public string GSTRP { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public string GLTRP { get; set; }

        /// <summary>
        /// 工厂（订单工厂）
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 状态(SAP已释放(I0002)对应MES发放，SAP技术性完成(TECO)对应MES关闭,REL关闭后再打开)
        /// </summary>
        public string STATU { get; set; }

        /// <summary>
        /// 计划批次
        /// </summary>
        public string CHARG { get; set; }

        /// <summary>
        /// 交货容差
        /// </summary>
        public string UEBTO { get; set; }

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string DISPO { get; set; }

        /// <summary>
        /// 生产管理员
        /// </summary>
        public string FEVOR { get; set; }

        /// <summary>
        /// 收货库位
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 备注（长文本）
        /// </summary>
        public string TEXT { get; set; }

        /// <summary>
        /// 内部订单
        /// </summary>
        public string AUFNR2 { get; set; }
    }

    /// <summary>
    /// 工单BOM信息
    /// </summary>
    [Serializable]
    public class BomInf
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string MEINS { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public decimal BDMNG { get; set; }

        /// <summary>
        /// 预留编号
        /// </summary>
        public string RSNUM { get; set; }

        /// <summary>
        /// 预留编号序号
        /// </summary>
        public string RSPOS { get; set; }

        /// <summary>
        /// 行项号
        /// </summary>
        public string POSNR { get; set; }

        /// <summary>
        /// 移动类型
        /// </summary>
        public string BWART { get; set; }

        /// <summary>
        /// 已发数量
        /// </summary>
        public decimal? ENMNG { get; set; }

        /// <summary>
        /// 是否虚实件
        /// </summary>
        public string DUMPS { get; set; }

        /// <summary>
        /// 发料库位
        /// </summary>
        public string LGORT { get; set; }

        /// <summary>
        /// 发料工厂
        /// </summary>
        public string WERKS { get; set; }
    }

    /// <summary>
    /// 工艺路线信息
    /// </summary>
    [Serializable]
    public class LayoutInf
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public string AUFNR { get; set; }

        /// <summary>
        ///     工序流水码
        /// </summary>
        public string VORNR { get; set; }

        /// <summary>
        /// 订单工艺路线号
        /// </summary>
        public string AUFPL { get; set; }

        /// <summary>
        /// 订单工艺路线序号
        /// </summary>
        public string APLZL { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string KTSCH { get; set; }

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string ARBPL { get; set; }

        /// <summary>
        /// 控制码 （工序控制码）
        /// </summary>
        public string STEUS { get; set; }

        /// <summary>
        /// 工厂（生产工厂）
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 工序数量
        /// </summary>
        public decimal MGVRG { get; set; }

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal ZCODE { get; set; }

        /// <summary>
        /// 直接人工-人工时间
        /// </summary>
        public decimal? VGW01 { get; set; }

        /// <summary>
        /// 间接人工-循环时间
        /// </summary>
        public decimal? VGW02 { get; set; }

        /// <summary>
        /// 动力-机器时间
        /// </summary>
        public decimal? VGW03 { get; set; }

    }

    /// <summary>
    /// 父级旧料号
    /// </summary>
    [Serializable]
    public class ParentItemInf
    {
        /// <summary>
        /// 物料
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 工厂(他们说工厂和主表的工厂一致的，所以跟随工单工厂就行)
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string MTART { get; set; }

        /// <summary>
        /// 上层物料号
        /// </summary>
        public string SMATNR { get; set; }

        /// <summary>
        /// 上层旧物料号
        /// </summary>
        public string BISMT { get; set; }
    }
}
