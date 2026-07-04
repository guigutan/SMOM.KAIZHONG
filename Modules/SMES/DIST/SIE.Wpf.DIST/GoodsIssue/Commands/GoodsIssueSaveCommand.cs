using SIE.DIST;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items.ViewModels;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存数据", Gestures = "Ctrl+S", GroupType = 10)]
    public class GoodsIssueSaveCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="view">当前明细逻辑视图</param>
        protected override void OnSaving(DetailLogicalView view)
        {
            GoodsIssue goodsIssue = view.Data as GoodsIssue;

            SetPropertyValues(goodsIssue);

            base.OnSaving(view);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="goodsIssue">工单发料信息</param>
        private static void SetPropertyValues(GoodsIssue goodsIssue)
        {
            var goodsIssueValues = goodsIssue.LocalContext.GetPropertyOrDefault<EntityList<PropertyValueViewModel>>("PropertyValueViewModel");
            if (goodsIssueValues != null)
            {
                goodsIssue.PropertyValueList.Clear();
                foreach (var goodsIssueValue in goodsIssueValues)  //工单发料单属性值
                {
                    if (goodsIssueValue.Values.Count == 0)
                    {
                        throw new ValidationException("物料属性值为空".L10N());
                    }

                    foreach (var value in goodsIssueValue.Values)
                    {
                        GoodsIssuePropertyValue item = new GoodsIssuePropertyValue()
                        {
                            Definition = goodsIssueValue.Definition,
                            Value = value,
                            GoodsIssueId = goodsIssue.Id
                        };
                        if (goodsIssue.PropertyValueList.FirstOrDefault(p => p.Definition == item.Definition && p.Value == item.Value) != null)
                            throw new ValidationException("该发料属性的属性值已经存在".L10N());
                        goodsIssue.PropertyValueList.Add(item);
                    }
                }
            }

            if (goodsIssue.Qty.HasValue)
            {
                goodsIssue.RemainderQty = goodsIssue.Qty.Value;
            }
        }
    }
}