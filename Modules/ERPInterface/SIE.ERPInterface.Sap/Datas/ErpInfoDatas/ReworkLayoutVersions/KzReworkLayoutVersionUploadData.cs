using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Sap.Datas.ErpInfoDatas.ReworkLayoutVersions
{
    /// <summary>
    /// 上传数据
    /// </summary>
    [Serializable]
    public class KzReworkLayoutVersionUploadData
    {
        /// <summary>
        /// 标记(1、新增 2、修改3、关闭)
        /// </summary>
        public string ZJSBS { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MATNR { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal GAMNG { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string DAUAT { get; set; }

        /// <summary>
        /// 基本开始日期
        /// </summary>
        public string GSTRP { get; set; }

        /// <summary>
        /// 基本完成日期
        /// </summary>
        public string GLTRP { get; set; }

        /// <summary>
        /// 生产版本
        /// </summary>
        public string VERID { get; set; }

        /// <summary>
        /// 卸货点
        /// </summary>
        public string ABLAD { get; set; }

        /// <summary>
        /// 收货方
        /// </summary>
        public string WEMPF { get; set; }
    }
}
