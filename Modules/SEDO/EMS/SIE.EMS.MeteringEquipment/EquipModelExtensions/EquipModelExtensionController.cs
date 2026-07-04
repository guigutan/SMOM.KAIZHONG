using SIE.Domain;
using System.Collections.Generic;

namespace SIE.EMS.MeteringEquipment.EquipModelExtensions
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
        public virtual EntityList<EquipModelCalibration> GetEquipModelCalibrationList(double modelId,
            PagingInfo pagingInfo, IList<OrderInfo> orderInfos)
        {
            return Query<EquipModelCalibration>()
                .Where(r => r.EquipModelId == modelId)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 保存选择的检验规程数据
        /// </summary>
        /// <param name="equipModelCalibrationInfos"></param>
        public virtual void SaveSelModelCalibration(List<EquipModelCalibration> equipModelCalibrationInfos)
        {
            if (equipModelCalibrationInfos != null)
            {
                EntityList<EquipModelCalibration> savedData = new EntityList<EquipModelCalibration>();
                foreach (var item in equipModelCalibrationInfos)
                {
                    var checkProject = new EquipModelCalibration();
                    checkProject.EquipModelId = item.EquipModelId;
                    checkProject.InspectionRuleId = item.InspectionRuleId;
                    savedData.Add(checkProject);
                }
                RF.Save(savedData);
            }
        }
    }
}
