using SIE.Items;
using System;

namespace SIE.MES.WIP.LoadMateriales.ApiModels
{
    /// <summary>
    /// 替代料
    /// </summary>
    [Serializable]
    public class AltItemViewModel
    {
        /// <summary>
        /// 装配清单Id
        /// </summary>
        public double AssemblyDetailId
        {
            get;
            set;
        }

        /// <summary>
        /// 装配清单
        /// </summary>
        public AssemblyDetailViewModel AssemblyDetail
        {
            get;
            set;
        }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get;
            set;
        }
        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get;
            set;
        }
        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get;
            set;
        }
    }
}
