using DocumentFormat.OpenXml.Drawing;
using Newtonsoft.Json;
using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipStatus.Datas;
using SIE.Equipments.EquipStatus.Enums;
using SIE.KZ.Base.Interfaces;
using SIE.Resources.LineAndons;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus
{
    public partial class EquipStatusController : DomainController
    {
        /// <summary>
        /// 获取设备状态数据
        /// </summary>
        /// <param name="data"></param>
        [ApiService("获取设备状态数据")]
        [AllowAnonymous]
        public virtual void GetEquipStatusData(EquipStatusRequestData data)
        {
            if (data == null || data.EqupNo.IsNullOrEmpty())
                return;

            AndonLine andonLine = null;
            using (SIE.Common.InvOrg.InvOrgs.WithAll())
            {
                andonLine = Query<AndonLine>().Where(p => p.AndonEntity == data.EqupNo).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                if (andonLine == null || andonLine.FactoryCode.IsNullOrEmpty())
                    return;

                var invOrg = Query<Rbac.InvOrgs.InvOrg>().Where(p => p.ExternalId == andonLine.FactoryCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
                RT.InvOrg = invOrg.Code;
            }

            var err = string.Empty;
            var beginTime = RF.Find<EquipStatus>().GetDbTime();

            try
            {
                if (andonLine.EquipmentCode.IsNullOrEmpty())
                    throw new ValidationException("产线与安灯区域，设备编码不存在".L10N());

                EquipStatusDetailStatus status = EquipStatusDetailStatus.Standby;
                switch (data.Status)
                {
                    case 0:
                        status = EquipStatusDetailStatus.OffLine;
                        break;
                    case 1:
                        status = EquipStatusDetailStatus.Standby;
                        break;
                    case 2:
                        status = EquipStatusDetailStatus.Running;
                        break;
                }

                EquipStatus equipStatus = GetEquipStatusByEquipCode(andonLine.EquipmentCode);
                if (equipStatus == null)
                {
                    equipStatus = new EquipStatus();
                    equipStatus.PersistenceStatus = PersistenceStatus.New;
                    equipStatus.EquipAccountId = andonLine.EquipmentId.Value;
                }
                equipStatus.Status = status;
                equipStatus.Factory = andonLine.FactoryCode;

                EquipStatusDetail lastDtl = null;
                if (equipStatus.EquipStatusDetailList != null && equipStatus.EquipStatusDetailList.Count > 0)
                {
                    //获取上一条明细
                    lastDtl = equipStatus.EquipStatusDetailList.OrderByDescending(p => p.EndTime).FirstOrDefault();
                }

                var newDtl = new EquipStatusDetail();
                newDtl.Status = status;
                newDtl.BeginTime = lastDtl == null ? null : lastDtl.EndTime;
                newDtl.EndTime = data.Time;
                if (newDtl.BeginTime != null)
                    newDtl.Minute = Math.Round((decimal)(newDtl.EndTime - newDtl.BeginTime.Value).TotalMinutes, 4);
                equipStatus.EquipStatusDetailList.Add(newDtl);

                RF.Save(equipStatus);
            }
            catch (Exception ex)
            {
                err = ex.GetBaseException()?.Message;
            }
            finally
            {
                //保存日志
                KZ.Base.Interfaces.Enums.CallResult callResult = KZ.Base.Interfaces.Enums.CallResult.Success;
                if (!err.IsNullOrEmpty())
                    callResult = KZ.Base.Interfaces.Enums.CallResult.Fail;
                var log = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(KZ.Base.Interfaces.Enums.InfType.IotEquipStatus, JsonConvert.SerializeObject(data), beginTime, KZ.Base.Interfaces.Enums.CallDirection.IotToMes, callResult, 1);
                log.EndDate = RF.Find<EquipStatus>().GetDbTime();
                log.ResponseContent = err;
                log.ErrorMsg = err;
                RF.Save(log);
            }
        }
    }
}
