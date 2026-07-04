using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP
{
    [Serializable]
    public class CollectBarcode
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CollectBarcode()
        {
            Context = new CollectionContext();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">条码号</param>
        /// <param name="type">条码类型</param>
        public CollectBarcode(string code, BarcodeType type)
        {
            Code = code;
            Type = type;
            AssemblyItems = new Dictionary<string, LoadItemBarcodeInfo>();
            Context = new CollectionContext();
        }

        /// <summary>
        /// 条码号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 拼板码
        /// </summary>
        public string PanelCode { get; set; }

        /// <summary>
        /// 条码过站类型
        /// </summary>
        public BarcodeType Type { get; set; }


        /// <summary>
        /// 装配项 上料ID、换料数量
        /// </summary>
        public Dictionary<string, LoadItemBarcodeInfo> AssemblyItems { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; }

        /// <summary>
        /// 采集条码ToString()
        /// </summary>
        /// <returns>格式化字符串</returns>
        //public override string ToString()
        //{
        //    //return "{0}:{1}".FormatArgs(EnumViewModel.EnumToLabel(Type).L10N(), Code);
        //}
    }
}
