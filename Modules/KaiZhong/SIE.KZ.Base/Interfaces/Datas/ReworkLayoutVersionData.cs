using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Rlvd
    {
        public List<ReworkLayoutVersionData> Data1 { get; set; } = new List<ReworkLayoutVersionData>();
    }

    /// <summary>
    /// 返工工艺路线版本
    /// </summary>
    [Serializable]
    public class ReworkLayoutVersionData
    {
        /// <summary>
        /// 生产版本
        /// </summary>
        public string VERID { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        public string TEXT1 { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 基本开始日期
        /// </summary>
        public string GSTRP { get; set; }

        /// <summary>
        /// 基本完成日期
        /// </summary>
        public string GLTRP { get; set; }

        /// <summary>
        /// 任务清单类型
        /// </summary>
        public string PLNTY { get; set; }

        /// <summary>
        /// 有效期截止日期
        /// </summary>
        public string BDATU { get; set; }

        /// <summary>
        /// 有效开始日期
        /// </summary>
        public string ADATU { get; set; }

        /// <summary>
        /// 任务清单组
        /// </summary>
        public string PLNNR { get; set; }

        /// <summary>
        /// 组计数器
        /// </summary>
        public string ALNAL { get; set; }

        public List<ReworkLayoutData> Data2 { get; set; } = new List<ReworkLayoutData>();
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ReworkLayoutData
    {
        /// <summary>
        /// 操作/活动编号
        /// </summary>
        public string VORNR { get; set; }

        /// <summary>
        /// 标准文本码/工序编码
        /// </summary>
        public string KTSCH { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public string ARBPL { get; set; }

        /// <summary>
        /// 控制码 （工序控制码）
        /// </summary>
        public string STEUS { get; set; }

        /// <summary>
        /// 工序工厂
        /// </summary>
        public string WERKS_D { get; set; }

        /// <summary>
        /// 工序数量
        /// </summary>
        public decimal MGVRG { get; set; }

        /// <summary>
        /// 分单数量
        /// </summary>
        public decimal ZCODE { get; set; }
    }
}
