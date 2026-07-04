using SIE.Domain;
using System.Collections.Generic;

namespace SIE.EMS.SpecialEquipment.Models
{
    /// <summary>
    /// 设备型号维护扩展控制器
    /// </summary>
    public class EquipModelExtensionController : DomainController
    {
        /// <summary>
        /// 根据设备型号维护Id获取检验规程
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="orderInfos"></param>
        /// <returns></returns>
        public virtual EntityList<EquipModelRegularInspection> GetEquipModelRegularInspectionList(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelRegularInspection>()
                .Where(r => r.EquipModelId == modelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 保存选择的检验规程数据
        /// </summary>
        /// <param name="equipModelRegularInspectionInfos"></param>
        public virtual void SaveSelModelInspection(List<EquipModelRegularInspection> equipModelRegularInspectionInfos)
        {
            if (equipModelRegularInspectionInfos != null)
            {
                EntityList<EquipModelRegularInspection> savedData = new EntityList<EquipModelRegularInspection>();
                foreach (var item in equipModelRegularInspectionInfos)
                {
                    var checkProject = new EquipModelRegularInspection();
                    checkProject.EquipModelId = item.EquipModelId;
                    checkProject.InspectionRuleId = item.InspectionRuleId;
                    savedData.Add(checkProject);
                }
                RF.Save(savedData);
            }
        }

       
    }
}
