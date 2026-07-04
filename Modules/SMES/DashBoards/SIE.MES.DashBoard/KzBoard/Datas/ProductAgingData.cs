using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.Datas
{
    /// <summary>
    /// 产品老化数据
    /// </summary>
    [Serializable]
    public class ProductAgingData
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductModel { get; set; }

        /// <summary>
        /// 当前在制数
        /// </summary>
        public decimal CurrentInProcessNum { get; set; }

        /// <summary>
        /// 最大在制数
        /// </summary>
        public decimal MaxInProcessNum { get; set; }

        /// <summary>
        /// 最小在制数
        /// </summary>
        public decimal MinInProcessNum { get; set; }

        /// <summary>
        /// 待老化
        /// </summary>
        public decimal WaitAging { get;set; }

        /// <summary>
        /// 老化中
        /// </summary>
        public decimal Aging { get; set; }

        /// <summary>
        /// 可用数量
        /// </summary>
        public decimal Available { get; set; }

        /// <summary>
        /// 当前在制数是否大于安全在制数
        /// true:大于，false：小于
        /// </summary>
        public bool? IsInProcessNumMax { get; set; }
        /// <summary>
        /// 老化明细集合
        /// </summary>
        public List<ProductAgingDtl> Data { get; set; }
    }

    /// <summary>
    /// 老化明细
    /// </summary>
    [Serializable]
    public class ProductAgingDtl
    {
        /// <summary>
        /// 产品编码
        /// </summary>
        public string AgingNum { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string AgingOutTime { get; set; }

        /// <summary>
        /// 当前在制数
        /// </summary>
        public decimal InFurnaceNum { get; set; }

    }
}
