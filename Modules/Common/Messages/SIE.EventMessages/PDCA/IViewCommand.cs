using System;
using System.Collections.Generic;

namespace SIE.EventMessages.PDCA
{
    /// <summary>
    /// 查看文档帮助类
    /// </summary>
    public class ViewDocsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        protected ViewDocsHelper() { }

        //单据/文档查看命令帮助类集合
        static List<IViewCommandHelper> _helper = new List<IViewCommandHelper>();

        /// <summary>
        /// 将每一个PDCA查看单据/文档的接口实现加入到单据/文档查看命令帮助类的集合中
        /// </summary>
        /// <param name="helper"></param>
        public static void AddHelper(IViewCommandHelper helper)
        {
            _helper.Add(helper);
        }

        /// <summary>
        /// 查看检验单据
        /// </summary>
        /// <param name="event">PDCA查看单据/文档参数</param>
        public static void ViewBillDocs(ViewDocEvent @event)
        {
            // 遍历每个接口的实现，确保每个接口的实现都能调用到查看单据的方法
            foreach (var item in _helper)
            {
                item.ViewBillMethod(@event);
            }
        }

        /// <summary>
        /// 查看作业指导书
        /// </summary>
        /// <param name="event">PDCA查看单据/文档参数</param>
        public static void ViewSOPDocs(ViewDocEvent @event)
        {
            // 遍历每个接口的实现，确保每个接口的实现都能调用到查看单据的方法
            foreach (var item in _helper)
            {
                item.VieSOPMethod(@event);
            }
        }

        /// <summary>
        /// 查看图档
        /// </summary>
        /// <param name="event">PDCA查看单据/文档参数</param>
        public static void ViewDrawingDocs(ViewDocEvent @event)
        {
            // 遍历每个接口的实现，确保每个接口的实现都能调用到查看单据的方法
            foreach (var item in _helper)
            {
                item.ViewDrawingMethod(@event);
            }
        }

        /// <summary>
        /// 查看规格书
        /// </summary>
        /// <param name="event">PDCA查看单据/文档参数</param>
        public static void VieSpecDocs(ViewDocEvent @event)
        {
            foreach (var item in _helper)
            {
                item.ViewSpecMethod(@event);
            }
        }
    }

    /// <summary>
    /// 单据/文档查看命令帮助类
    /// </summary>
    public interface IViewCommandHelper
    {
        /// <summary>
        /// 查看检验单
        /// </summary>
        /// <param name="event">发起PDCA改善参数</param>
        void ViewBillMethod(ViewDocEvent @event);

        /// <summary>
        /// 查看作业指导书
        /// </summary>
        /// <param name="event">发起PDCA改善参数</param>
        void VieSOPMethod(ViewDocEvent @event);

        /// <summary>
        /// 查看规格书
        /// </summary>
        /// <param name="event">发起PDCA改善参数</param>
        void ViewSpecMethod(ViewDocEvent @event);

        /// <summary>
        /// 查看图档
        /// </summary>
        /// <param name="event">发起PDCA改善参数</param>
        void ViewDrawingMethod(ViewDocEvent @event);
    }

    /// <summary>
    /// PDCA查看单据/文档参数
    /// </summary>
    [Serializable]
    public class ViewDocEvent
    {
        /// <summary>
        /// 检验单Id
        /// </summary>
        public double BillId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public string InspectionType { get; set; }

        /// <summary>
        /// 命令对应的参数视图元数据显示的标题
        /// </summary>
        public string ViewMetaLabel { get; set; }

        /// <summary>
        /// 命令对应的元数据显示的标题
        /// </summary>
        public string MetaLabel { get; set; }
    }
}