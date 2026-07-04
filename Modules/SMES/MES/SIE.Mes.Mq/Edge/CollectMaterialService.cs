using Newtonsoft.Json;
using SIE.MES.Edge;
using SIE.MES.Edge.Models;
using System;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 物料采集消息处理服务
    /// </summary>
    public class CollectMaterialService: ICollectMaterialService
    {
        private readonly ICollectDataDao collectDataDao;
        //private readonly ICollectMaterialDao materialDao;
        private readonly IEdgeErrorMessageDao errorMessageDao;
        //private readonly IEdgeWipDao wipDao;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectDataDao"></param>
        /// <param name="materialDao"></param>
        /// <param name="errorMessageDao"></param>
        /// <param name="wipDao"></param>
        public CollectMaterialService(ICollectDataDao collectDataDao, ICollectMaterialDao materialDao, IEdgeErrorMessageDao errorMessageDao, IEdgeWipDao wipDao)
        {
            this.collectDataDao = collectDataDao;
            this.errorMessageDao = errorMessageDao;
        }

        /// <summary>
        /// 采集数据处理
        /// </summary>
        /// <param name="em"></param>
        public void CollectData(EdgeMessage em)
        {
            if (em == null)
            {
                RT.Logger.Error(("物料采集处理收到非法的消息：NULL，传送的消息请按照EdgeMessage类结构进行序列化").L10N());
                return;
            }

            MaterialCollectData data;
            RT.InvOrg = int.Parse(em.InvOrg);
            string body = em.Body.ToString();
            data = JsonConvert.DeserializeObject<MaterialCollectData>(body);

            double employeeId = double.Parse(data.EmployeeId);
            double? userId = collectDataDao.GetUserId(employeeId);
            if (RT.IdentityId != employeeId && userId != null)
            {
                RT.Principal = new DataPortal.DataPortalPrincipal(employeeId, userId.Value, "");
            }

            SaveMaterialData(data);

        }

        /// <summary>
        /// 保存异常消息
        /// </summary>
        /// <param name="em"></param>
        /// <param name="ex"></param>
        public void SaveErrorMessage(EdgeMessage em, Exception ex)
        {
            string body = em.Body?.ToString();
            errorMessageDao.CreateErrorMessage(em.Id, ex.Message, em.Name, body);
        }

        private void SaveMaterialData(MaterialCollectData data)
        {
            ////switch(data.DataType)
            ////{
            ////    case MaterialDataType.Used:
            ////        break;
            ////    default:
            ////        break;
            ////}
            //materialDao.SaveMaterialCollectData(data);
        }
    }
}
