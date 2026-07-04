using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 产品直通率
    /// </summary>
    [Serializable]
    public class ProductFirstPassYieldData
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
        /// 产品
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal FirstPassYield { get; set; }
    }

    [Serializable]
    public class ProductFirstPassYieldFactoryData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Inv_Org_Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string Product { get; set; }

        public List<ProductFirstPassYieldDtlData> datas { get; set; } = new List<ProductFirstPassYieldDtlData>();
    }

    /// <summary>
    /// 产品直通率明细
    /// </summary>
    [Serializable]
    public class ProductFirstPassYieldDtlData
    { 
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 投料量
        /// </summary>
        public decimal FeedingQty { get; set; }

        /// <summary>
        /// 可疑品数量
        /// </summary>
        public decimal SuspectQty { get; set; }

        /// <summary>
        /// 直通率
        /// </summary>
        public decimal FirstPassYield { get; set; }
    }
}
