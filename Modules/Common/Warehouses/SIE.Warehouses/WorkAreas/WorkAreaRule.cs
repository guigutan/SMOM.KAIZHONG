using SIE.Domain.Validation;
using System;

namespace SIE.Warehouses.WorkAreas
{

    #region 同一仓库中工作区编码非重验证规则
    /// <summary>
    /// 同一仓库中工作区编码非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("同一仓库中工作区编码非重验证规则")]
    [System.ComponentModel.Description("同一仓库中工作区编码非重验证规则")]
    public class NotDuplicateWorkArea : NotDuplicateRule<WorkArea>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateWorkArea()
        {
            Properties.Add(WorkArea.WarehouseIdProperty);
            Properties.Add(WorkArea.CodeProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as WorkArea;
                return "同一仓库已经存在相同【{0}】工作区区编码".L10nFormat(entity.Code);
            };
        }
    }
    #endregion
}
