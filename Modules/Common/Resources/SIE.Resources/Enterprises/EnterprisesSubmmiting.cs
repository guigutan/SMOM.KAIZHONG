using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Resources.Enterprises
{

    /// <summary>
    /// 企业模型保存前事件
    /// </summary>
    [System.ComponentModel.DisplayName("企业模型保存前修改父节点值")]
    [System.ComponentModel.Description("企业模型保存前修改父节点值")]
    public class EnterprisesSubmmiting : OnSubmitting<Enterprise>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Invoke(Enterprise entity, EntitySubmittingEventArgs e)
        {
            if (e.Action == SubmitAction.Update)
            {
                var enterpriseModel = RT.Service.Resolve<EnterpriseController>().GetEnterpriseById(entity.Id);
                if (enterpriseModel != null)
                {
                    entity.SetTreePId(enterpriseModel.TreePId);
                }

            }
        }
    }
}
