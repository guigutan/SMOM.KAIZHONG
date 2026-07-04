using SIE.Domain;
using SIE.Packages.Boxs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 周转箱保存后保存垛表数据
    /// </summary>
    [System.ComponentModel.DisplayName("周转箱保存后保存垛表数据")]
    [System.ComponentModel.Description("周转箱保存后保存垛表数据")]
    internal class TurnoverBoxSubmmited : OnSubmitted<TurnoverBox>
    {
        /// <summary>
        ///  周转箱保存后保存垛表数据
        /// </summary>
        /// <param name="entity">物料实体</param>
        /// <param name="e">由该事件生成的事件数据的类型</param>
        protected override void Invoke(TurnoverBox entity, EntitySubmittedEventArgs e)
        {
            if (entity != null && entity.Type == "物流周转箱".L10N())
            {
                RT.Service.Resolve<SaveTurnoverBoxToPile>().SaveTurnoverBoxs(new List<TurnoverBox>() { entity });
            }
        }
    }
}
