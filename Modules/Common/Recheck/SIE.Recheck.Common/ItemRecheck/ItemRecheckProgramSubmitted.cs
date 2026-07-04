using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Recheck.Common.ItemRecheck;
using System;

namespace SIE.Recheck.Common.ItemRecheck
{
    /// <summary>
    /// 复检方案保存复检次数
    /// </summary>
    [System.ComponentModel.DisplayName("复检方案保存复检次数")]
    [System.ComponentModel.Description("复检方案保存复检次数")]
    public class ItemRecheckProgramSubmitted : OnSubmitted<ItemRecheckProgram>
    {
        /// <summary>
        ///  复检方案保存复检次数
        /// </summary>
        /// <param name="entity">物料复检方案</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(ItemRecheckProgram entity, EntitySubmittedEventArgs e)
        {
            if (e.Action == SubmitAction.Update || e.Action == SubmitAction.Insert)
            {
                var item = RF.GetById<ItemRecheckProgram>(entity.Id);
                var detailCount = item.ItemRecheckProgramDetailList.Count;
                if (detailCount == 0)
                    throw new ValidationException("保存失败：复检方案[{0}]没有明细".L10nFormat(entity.Code));
                if (item.MaxRecheckCount != detailCount)
                {
                    item.MaxRecheckCount = detailCount;
                    RF.Save(item);
                }
            }
        }
    }

    /// <summary>
    /// 复检方案保存复检次数
    /// </summary>
    [System.ComponentModel.DisplayName("复检方案保存复检次数")]
    [System.ComponentModel.Description("复检方案保存复检次数")]
    public class ItemRecheckProgramSubmitting : OnSubmitting<ItemRecheckProgram>
    {
        /// <summary>
        /// 复检方案保存复检次数
        /// </summary>
        /// <param name="entity">物料复检方案</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(ItemRecheckProgram entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Insert)
            {
                var detail = entity.ItemRecheckProgramDetailList;
                if (detail != null && detail.Count != entity.MaxRecheckCount)
                    entity.MaxRecheckCount = detail.Count;
            }
        }
    }
}
