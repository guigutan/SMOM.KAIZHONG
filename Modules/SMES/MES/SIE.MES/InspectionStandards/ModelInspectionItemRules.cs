using SIE.Domain.Validation;

namespace SIE.MES.InspectionStandards
{
    /// <summary>
    /// 机型检验项目非重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("同个机型、工序、检验项目不能重复")]
    [System.ComponentModel.Description("同个机型、工序、检验项目不能重复")]
    public class NotDuplicateItem : NotDuplicateRule<ModelInspectionItem>
    {
        /// <summary>
        /// 机型检验项目非重复验证
        /// </summary>
        public NotDuplicateItem()
        {
            Properties.Add(ModelInspectionItem.ModelIdProperty);
            Properties.Add(ModelInspectionItem.ProcessIdProperty);
            Properties.Add(ModelInspectionItem.NameProperty);
            MessageBuilder = (e) =>
             {
                 return "同个机型、工序、检验项目不能重复";
             };
        }
    }

    /// <summary>
    /// 机型检验项目非重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("同个产品、工序、检验项目不能重复")]
    [System.ComponentModel.Description("同个产品、工序、检验项目不能重复")]
    public class NotDuplicateProductItem : NotDuplicateRule<ModelInspectionItem>
    {
        /// <summary>
        /// 机型检验项目非重复验证
        /// </summary>
        public NotDuplicateProductItem()
        {
            Properties.Add(ModelInspectionItem.ProductItemIdProperty);
            Properties.Add(ModelInspectionItem.ProcessIdProperty);
            Properties.Add(ModelInspectionItem.NameProperty);
            MessageBuilder = (e) =>
            {
                return "同个产品、工序、检验项目不能重复";
            };
        }
    }
}