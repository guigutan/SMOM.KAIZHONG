using SIE.CrossPlatform.Collect.Models.Enums;
using SIE.CrossPlatform.Collect.Models.WIP;
using System;

namespace SIE.CrossPlatform.Collect.Models
{
    [Serializable]
    public class ProductAssemblyDetailViewModel
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public string Id { get; set; }

        public string TreePId { get; set; }
       
        public WipProductProcessKeyItem KeyItem { get; set; }

        /// <summary>
        /// 待换料条码
        /// </summary>
        public string Barcode
        {
            get; set;
        }

        /// <summary>
        /// 换料数量
        /// </summary>
        public decimal ChangeQty
        {
            get; set;
        }


        /// <summary>
        /// 是否换料
        /// </summary>
        public bool IsChangeSn
        {
            get; set;
        }

        /// <summary>
        /// 换料后标签
        /// </summary>
        public string ChangeBarcode
        {
            get; set;
        }

        /// <summary>
        /// 总换料数量
        /// </summary>
        public decimal TotalChangeQty
        {
            get; set;
        }

        /// <summary>
        /// 原标签
        /// </summary>
        public string SourceCode
        {
            get; set;
        }

        /// <summary>
        /// 关键件Id
        /// </summary>
        public double KeyItemId
        {
            get; set;
        }
        /// <summary>
        /// 物料标签
        /// </summary>
        public string KeyItemItemCode
        {
            get; set;
        }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string KeyItemItemName
        {
            get; set;
        }

        /// <summary>
        /// 置换后处理
        /// </summary>
        public ChangeItemHandleMethod HandleMethod
        {
            get; set;
        }

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get; set;
        }
    }
}
