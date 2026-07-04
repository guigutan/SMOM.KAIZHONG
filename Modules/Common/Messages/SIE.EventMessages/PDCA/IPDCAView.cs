using SIE.Domain;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EventMessages.PDCA
{
    /// <summary>
    /// 查看检验单接口实现
    /// </summary>
    public class DefaultViewInspBill
    {
        /// <summary>
        /// 查看检验单方法类集合
        /// </summary>
        public static Dictionary<int, IViewInspBillHelper> DicHelper { get; set; } = new Dictionary<int, IViewInspBillHelper>();


        /// <summary>
        /// 获取查看检验单URL/model+viewgroup字符串
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns>URL或ViewInspBill</returns>
        public object GetInspBillUrl(ViewInspBillEventArgs eventArgs)
        {
            var viewData = DicHelper.FirstOrDefault(p => p.Key == eventArgs.InspectionTypeInt).Value?.GetInspBillUrl(eventArgs);
            if (viewData != null)
                return viewData;
            throw new InvalidOperationException("无法获取检验单视图信息。".L10N());
        }

    }

    /// <summary>
    /// 查看检验单方法类接口定义
    /// </summary>
    public interface IViewInspBillHelper
    {
        /// <summary>
        /// 获取查看检验单URL/model+viewgroup字符串
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns>URL或ViewInspBill</returns>
        object GetInspBillUrl(ViewInspBillEventArgs eventArgs);
    }

    /// <summary>
    /// 查看检验单
    /// </summary>
    public class ViewInspBill
    {
        /// <summary>
        /// 对应不合格审核的ViewGroup
        /// </summary>
        public string ViewGroup { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }
    }

    /// <summary>
    /// 查看检验单实体
    /// </summary>
    [Serializable]
    public class ViewInspBillViewData
    {
        /// <summary>
        /// 对应不合格审核的ViewGroup
        /// </summary>
        public string ViewGroup { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// 单据实体
        /// </summary>
        public Entity Bill { get; set; }

        /// <summary>
        /// 视图元数据
        /// </summary>
        public EntityViewMeta EVM { get; set; }

        /// <summary>
        /// 实体额外添加的属性
        /// </summary>
        public Dictionary<string, object> DicExtensionProperty { get; set; }
    }
}
