using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.LES.MaterialReturnApplys
{
    /// <summary>
    /// 选择退料申请明细
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MaterialReturnApplyDtlSelCriteria))]
    [Label("选择退料申请明细")]
    public class MaterialReturnApplyDetailSelect : MaterialReturnApplyDetailBase
    {
    }

    /// <summary>
    /// 退料申请明细实体配置
    /// </summary>
    public class MaterialReturnApplyDetailSelectConfig : EntityConfig<MaterialReturnApplyDetailSelect>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("LES_MRAPPLY_DETAIL").MapAllProperties();
            Meta.Property(MaterialReturnApplyDetailSelect.CtrlModeProperty).DontMapColumn();
            Meta.Property(MaterialReturnApplyDetailSelect.AvailableQtyProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
