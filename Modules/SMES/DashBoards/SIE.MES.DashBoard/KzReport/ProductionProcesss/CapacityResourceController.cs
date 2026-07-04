using DocumentFormat.OpenXml.Drawing;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.MES.DashBoard.KzReport.ProductionProcesss.Enums;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.Rbac.InvOrgs;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.ProductionProcesss
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CapacityResourceController:DomainController
    {
        public virtual Dictionary<string,decimal> GetStandardCapacity(CapacityDataType dataType, List<DictionaryData> dicpProcessCodes)
        {
            Dictionary<string, decimal> dispatchTaskList = new Dictionary<string, decimal>();
            var invCurr = RT.InvOrg;
            foreach (var item in dicpProcessCodes)
            {
                var invOrg = Query<InvOrg>().Where(p => p.ExternalId == item.DicKey).FirstOrDefault();
                if (invOrg == null)
                    continue;
                RT.InvOrg = invOrg.Code;

                var query = Query<CapacityResource>()
                    .LeftJoin<Process>((d, p) => d.ProcessId == p.Id)
                    .Where<Process>((d, p) => item.DicValue.Contains(p.Code));
                var entityList = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                decimal number = 0M;
                foreach (var value in item.DicValue)
                {
                    var capacityResourceList = entityList.Where(p=>p.ProcessCode == value).ToList();
                    foreach (var capacityResource in capacityResourceList)
                    {
                        number += GetUph(dataType, capacityResource.Uph);
                    }
                    dispatchTaskList.Add(item.DicKey+value, number);
                }
            }
            RT.InvOrg = invCurr;
            return dispatchTaskList;
        }

        /// <summary>
        /// 获取工序标准产能（单产线）
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="uph"></param>
        /// <returns></returns>
        public virtual decimal GetUph(CapacityDataType dataType,decimal uph)
        {
            var standardCapacity = 0M;
            switch (dataType)
            {
                case CapacityDataType.Moon:
                    standardCapacity = 26 * 20 * uph;
                    break;
                case CapacityDataType.Week:
                    standardCapacity = 20 * 6 * uph;
                    break;
                case CapacityDataType.Day:
                    standardCapacity = 20 * 1 * uph;
                    break;
                default:
                    standardCapacity = 20 * 1 * uph;
                    break;
            }
            return standardCapacity;
        }
    }
}
