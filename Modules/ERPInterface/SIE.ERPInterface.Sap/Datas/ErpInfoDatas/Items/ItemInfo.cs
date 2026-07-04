using SapNwRfc;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// 物料信息
    /// </summary>
    public class ItemInfo
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        [SapName("MATNR")]
        public string MATNR { get; set; }
        /// <summary>
        /// 物料描述
        /// </summary>
        [SapName("MAKTX")]
        public string MAKTX { get; set; }
        /// <summary>
        /// 基本计量单位
        /// </summary>
        [SapName("MEINS")]
        public string MEINS { get; set; }
        /// <summary>
        /// 度量单位文本
        /// </summary>
        [SapName("MSEHL")]
        public string MSEHL { get; set; }
        /// <summary>
        /// 采购分类
        /// </summary>
        [SapName("ZCGFL")]
        public string ZCGFL { get; set; }
        /// <summary>
        /// 采购分类描述
        /// </summary>
        [SapName("VTEXT")]
        public string VTEXT { get; set; }
        /// <summary>
        /// 物料组
        /// </summary>
        [SapName("MATKL")]
        public string MATKL { get; set; }
        /// <summary>
        /// 物料组描述
        /// </summary>
        [SapName("WGBEZ")]
        public string WGBEZ { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        [SapName("MTART")]
        public string MTART { get; set; }
        /// <summary>
        /// 机型分类
        /// </summary>
        [SapName("ZJXFL")]
        public string ZJXFL { get; set; }
        /// <summary>
        /// 产品线
        /// </summary>
        [SapName("ZCPX")]
        public string ZCPX { get; set; }
        /// <summary>
        /// 散件标识
        /// </summary>
        [SapName("ZSJBS")]
        public string ZSJBS { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SapName("ZDEBZ")]
        public string ZDEBZ { get; set; }
        /// <summary>
        /// 成品条码规则
        /// </summary>
        [SapName("ZCPTMGZ")]
        public string ZCPTMGZ { get; set; }
        /// <summary>
        /// 产品型号
        /// </summary>
        [SapName("ZCPXH")]
        public string ZCPXH { get; set; }
        /// <summary>
        /// 电源线插头 
        /// </summary>
        [SapName("ZDYXCT")]
        public string ZDYXCT { get; set; }
        /// <summary>
        /// 电压
        /// </summary>
        [SapName("ZDY")]
        public string ZDY { get; set; }
        /// <summary>
        /// ZLMMC
        /// </summary>
        [SapName("ZLMMC")]
        public string ZLMMC { get; set; }
        /// <summary>
        /// 颜色码
        /// </summary>
        [SapName("ZYSM")]
        public string ZYSM { get; set; }
        /// <summary>
        /// 识别码
        /// </summary>
        [SapName("ZSBM")]
        public string ZSBM { get; set; }
        /// <summary>
        /// 扩展颜色
        /// </summary>
        [SapName("ZKZYS")]
        public string ZKZYS { get; set; }
        /// <summary>
        /// 客户代码
        /// </summary>
        [SapName("ZKHDM")]
        public string ZKHDM { get; set; }
        /// <summary>
        /// 压缩机
        /// </summary>
        [SapName("ZYSJ")]
        public string ZYSJ { get; set; }
        /// <summary>
        /// 容积
        /// </summary>
        [SapName("ZRJ")]
        public string ZRJ { get; set; }
        /// <summary>
        /// 冷媒码
        /// </summary>
        [SapName("ZLMM")]
        public string ZLMM { get; set; }
        /// <summary>
        /// 特征码
        /// </summary>
        [SapName("ZTZM")]
        public string ZTZM { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        [SapName("ZZTM")]
        public string ZZTM { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        [SapName("WERKS")]
        public string WERKS { get; set; }
        /// <summary>
        /// 物料需求计划控制员
        /// </summary>
        [SapName("DISPO")]
        public string DISPO { get; set; }
        /// <summary>
        /// 采购类型
        /// </summary>
        [SapName("BESKZ")]
        public string BESKZ { get; set; }
        /// <summary>
        /// 特殊采购类
        /// </summary>
        [SapName("SOBSL")]
        public string SOBSL { get; set; }
        /// <summary>
        /// 反冲
        /// </summary>
        [SapName("RGEKZ")]
        public string RGEKZ { get; set; }

        /// <summary>
        /// 内外销标识
        /// </summary>
        [SapName("ZNWXBS")]
        public string ZNWXBS { get; set; }

        /// <summary>
        /// 客户型号
        /// </summary>
        [SapName("ZKHXH")]
        public string ZKHXH { get; set; }
    }
}
