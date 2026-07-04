using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages.Boxs;
using System;

namespace SIE.Inventory.Piles
{

    /// <summary>
    /// 保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("垛表保存前事件，更新周转箱状态")]
    [System.ComponentModel.Description("垛表保存前事件，更新周转箱状态")]
    public class PileSubmmitting : OnSubmitting<Pile>
    {
        /// <summary>
        /// 保存前数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Invoke(Pile entity, EntitySubmittingEventArgs e)
        {
            if (e != null && entity != null && entity.TurnoverContainer)
            {
                if (e.Action == SubmitAction.Delete)
                {
                    throw new ValidationException("当前垛是周转容器[物流周转箱]，不能删除".L10N());
                }
                else if (e.Action == SubmitAction.Update)
                {
                    var oldPile = RF.GetById<Pile>(entity.Id);
                    if (entity.PileState != oldPile.PileState)
                    {
                        RT.Service.Resolve<BoxController>().UpdateTurnoverBox(entity.Code, entity.PileState);
                    }
                }
            }
        }
    }

}
