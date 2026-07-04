using SIE.Defects.InspectionItems;
using SIE.xUnit.Defects.Defects;
using System;

namespace SIE.xUnit.Elec.IQC.IqcBills.Fixtures
{
    /// <summary>
    /// 电子-来料检验固件(退料)
    /// </summary>
    public class ElecIqcBillReturnFixture : ElecIqcBillFixture
    {
        /// <summary>
        /// 获取检验方式
        /// </summary>
        /// <returns></returns>
        protected override InspectionMode GetInspectionMode()
        {
            var mode = RT.Service.Resolve<InspectionItemController>().GetInspectionMode("退料".L10N());
            if (mode == null)
                return RT.Service.Resolve<DefectTestController>().CreateInspectionMode(FixPropInspectionType, "退料".L10N());
            else
                return mode;
        }
    }
}
