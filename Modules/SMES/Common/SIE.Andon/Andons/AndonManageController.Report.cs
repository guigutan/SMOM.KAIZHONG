using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Org.BouncyCastle.Crypto;
using SIE.Andon.Andons.APIModel;
using SIE.Andon.Andons.ForWinform.ApiModels;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.KZ.Base.Interfaces;
using SIE.Rbac.InvOrgs;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    public partial class AndonManageController:DomainController
    {
        /// <summary>
        /// 获取安灯管理信息
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, EntityList<AndonManage>> GetAndonManages(List<DictionaryData> dicWids, DateTime? startTime, DateTime? endTime, ref List<double> andonManageIds)
        {
            Dictionary<string, EntityList<AndonManage>> andonManageList = new Dictionary<string, EntityList<AndonManage>>();
            var invCurr = RT.InvOrg;
            foreach (var item in dicWids)
            {
                var invOrg = Query<InvOrg>().Where(p => p.ExternalId == item.DicKey).FirstOrDefault();
                if (invOrg == null)
                    continue;
                RT.InvOrg = invOrg.Code;

                var andonManageLists = item.DicValue.SplitContains(codes =>
                {
                    var query = Query<AndonManage>().LeftJoin<Enterprise>((x, y) => x.WorkShopId == y.Id)
                    .Where<Enterprise>((x, y) => codes.Contains(y.Code))
                    .Where(p => p.TriggerTime >= startTime)
                    .Where(p => p.TriggerTime <= endTime)
                    .Where(m => m.State != Andons.Enum.AndonManageState.Cancel);
                    return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
                andonManageIds.AddRange(andonManageLists.Select(p => p.Id).ToList<double>().ToList());
                andonManageList.Add(item.DicKey, andonManageLists);
            }
            RT.InvOrg = invCurr;
            return andonManageList;
        }

        /// <summary>
        /// 生成安灯异常统计
        /// </summary>
        /// <param name="dicWids"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public virtual List<AndonManageGroupByData> GetAndonAnomaly(List<DictionaryData> dicWids, DateTime? startTime, DateTime? endTime)
        {
            List<AndonManageGroupByData> manageGroupByDatas = new List<AndonManageGroupByData>();
            EntityList<AndonManageOperateLog> logs = new EntityList<AndonManageOperateLog>();
            List<double> andonManageIds = new List<double>();
            //获取安灯管理信息
            var entityList = GetAndonManages(dicWids, startTime, endTime,ref andonManageIds);
            using (SIE.DataAuth.DataAuths.LoadAll())
            {
                logs = andonManageIds.SplitContains(ids =>
                {
                    return Query<AndonManageOperateLog>().Where(p => ids.Contains(p.AndonManageId)).ToList();
                });
            }
            foreach (var item in entityList)
            {
                var groupbyData = item.Value.GroupBy(p => new { p.AndonManageClass, p.WorkShopId }).ToDictionary(p => p.Key, p => p.ToList());
                if (groupbyData == null || groupbyData.Count == 0) continue;
                foreach (var gData in groupbyData)
                {
                    var andonTypeGroupBys = gData.Value.GroupBy(p => p.AndonTypeName).ToDictionary(p => p.Key, p => p.ToList());
                    foreach (var andonTypeGroupBy in andonTypeGroupBys)
                    {
                        //生成安灯统计信息
                        var entity = GenAndonManageGroupByData(andonTypeGroupBy.Value,logs);
                        entity.FactoryCode = item.Key;
                        manageGroupByDatas.Add(entity);
                    }
                }
            }
            return manageGroupByDatas;
        }

        /// <summary>
        /// 生成安灯统计信息
        /// </summary>
        /// <param name="andonTypeGroupByValue"></param>
        /// <param name="logs"></param>
        /// <returns></returns>
        private AndonManageGroupByData GenAndonManageGroupByData(List<AndonManage> andonTypeGroupByValue, EntityList<AndonManageOperateLog> logs)
        {
            var entity = new AndonManageGroupByData();
            var andonManageFirst = andonTypeGroupByValue.FirstOrDefault();
            entity.AndonNum = andonTypeGroupByValue.Count;
            entity.WorkShopId = andonManageFirst.WorkShopId;
            entity.WorkShopCode = andonManageFirst.WorkShopCode;
            entity.AndonType = andonManageFirst.AndonTypeName;
            entity.AndonBigType = andonManageFirst.AndonManageClass.ToLabel();
            entity.OnTimeProcessCount = 0;
            entity.OnTimeResponseCount = 0;
            entity.ExceptionResponseTime = 0;
            entity.ExceptionProcessTime = 0;
            foreach (var andonManage in andonTypeGroupByValue)
            {
                //响应
                var andonManageOperateLogs = logs.Where(p => p.AndonManageId == andonManage.Id).ToList();
                var sResponse = andonManageOperateLogs.FirstOrDefault(p => p.OperateType == Enum.AndonManageOperateType.Response);
                if (sResponse != null)
                {
                    if (sResponse.LastOperate < 0.08)
                        entity.OnTimeResponseCount += 1;
                    else
                        entity.ExceptionResponseTime += Convert.ToDecimal(sResponse.LastOperate);
                }
                else if (andonManageOperateLogs.Count > 0)
                    entity.ExceptionResponseTime += Convert.ToDecimal(Math.Round((DateTime.Now - andonManageOperateLogs.FirstOrDefault().OperateTime).TotalHours, 2));
                //处理
                var sHandle = andonManageOperateLogs.FirstOrDefault(p => p.OperateType == Enum.AndonManageOperateType.Handle);
                if (sHandle != null)
                {
                    if (sResponse.LastOperate > 0.5)
                        entity.OnTimeProcessCount += 1;
                    else
                        entity.ExceptionProcessTime += Convert.ToDecimal(sResponse.LastOperate);
                }
                else if (andonManageOperateLogs.Count > 1)
                    entity.ExceptionProcessTime += Convert.ToDecimal(Math.Round((DateTime.Now - sResponse.OperateTime).TotalHours, 2));
            }
            entity.OnTimeResponseRate = (entity.OnTimeResponseCount == 0 || entity.AndonNum == 0) ? 0 : (entity.OnTimeResponseCount / entity.AndonNum);
            entity.OnTimeProcessRate = (entity.OnTimeProcessCount == 0 || entity.AndonNum == 0) ? 0 : (entity.OnTimeProcessCount / entity.AndonNum);

            return entity;
        }

    }
}
