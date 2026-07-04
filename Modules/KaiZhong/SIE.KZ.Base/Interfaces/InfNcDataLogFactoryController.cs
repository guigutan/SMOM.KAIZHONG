using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Configs;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces
{
    public class InfNcDataLogFactoryController : DomainController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<InfNcDataLogFactory> CriteriaInfNcDataLogFactory(InfNcDataLogFactoryCriteria criteria)
        {
            var q = DB.Query<InfNcDataLogFactory>("log");
            if (!criteria.InvOrg.IsNullOrEmpty())
                q.Where(p => p.InvOrg.Contains(criteria.InvOrg));
            if (criteria.InfType != null)
                q.Where(p => p.InfType == criteria.InfType);
            if (!criteria.FactoryName.IsNullOrEmpty())
                q.Where(p => p.FactoryName.Contains(criteria.FactoryName));
            if (criteria.SendState != null)
                q.Where(p => p.SendState == criteria.SendState);
            if (!criteria.GroupGuid.IsNullOrEmpty())
                q.Where(p => p.GroupGuid.Contains(criteria.GroupGuid));
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            if (!criteria.DataJsons.IsNullOrEmpty())
            {
                if (!criteria.DataJsons.Contains('%'))
                    criteria.DataJsons = $"%{criteria.DataJsons}%";
                q.Where(p => p.SQL<bool>($"log.Data_Jsons like '{criteria.DataJsons}'"));
            }
            if (!criteria.ErrorMsg.IsNullOrEmpty())
            {
                if (!criteria.ErrorMsg.Contains('%'))
                    criteria.ErrorMsg = $"%{criteria.ErrorMsg}%";
                q.Where(p => p.SQL<bool>($"log.Error_Msg like '{criteria.ErrorMsg}'"));
            }
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }

        /// <summary>
        /// 根据接口类型获取需要传输的数据
        /// </summary>
        /// <param name="infType"></param>
        /// <returns></returns>
        public virtual EntityList<InfNcDataLogFactory> GetWaitSendInfNcDataLogFactorys(InfType infType, int failCount)
        {
            return Query<InfNcDataLogFactory>().Where(p => p.InfType == infType && (p.SendState == SendState.NoSend || (p.SendState == SendState.SendFail && p.FailCount < failCount))).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 5000));
        }

        /// <summary>
        /// 根据工单接口类型获取需要传输的数据
        /// </summary>
        /// <param name="infType"></param>
        /// <returns></returns>
        public virtual EntityList<InfNcDataLogFactory> GetWaitSendInfNcDataSOLogFactorys(InfType infType, string BatchNo)
        {
            var batchNos = BatchNo?.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).Distinct().ToList() ?? new List<string>();

            if (batchNos.Count == 0)
            {
                // 如果没有有效的BatchNo，返回空列表或根据需求处理
                return null;
            }
            return Query<InfNcDataLogFactory>().Where(p => p.InfType == infType && batchNos.Contains(p.GroupGuid)).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));

            //return Query<InfNcDataLogFactory>().Where(p => p.InfType == infType && batchNos.Contains(p.BatchNo)/*&& p.SuccessJson != null*/).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));
            //return Query<InfNcDataLogGroup>().Where(p => p.InfType == infType && p.BatchNo == BatchNo).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 5000));
            //return Query<InfNcDataLogFactory>().Where(p => p.InfType == infType && p.BatchNo == BatchNo).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));

        }

        /// <summary>
        /// 重新同步
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual StringBuilder ResyncExecuteInterface(IList<double> ids)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int failCount = 5;
            var config = ConfigService.GetConfig(new InfLogFacFailConfig(), typeof(InfNcDataLogFactory));
            if (config != null && config.FailCount > 0)
            {
                failCount = config.FailCount;
            }
            //查找所有工厂
            var smomControlSettingDic = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettings().ToDictionary(p => p.FactoryCode);
            if (smomControlSettingDic == null || smomControlSettingDic.Count == 0)
            {
                throw new ValidationException("没有维护SMOM总控配置数据!".L10N());
            }
            //var infMappings = Query<InfMapping>().ToList();
            //if (infMappings == null)
            //{
            //    throw new ValidationException("没有维护菜单基础数据接口信息!".L10N());
            //}
            var factoryList = Query<InfNcDataLogFactory>().Where(p => ids.Contains(p.Id)).OrderBy(p => p.CreateDate).ToList().Where(p => p.SendState != SendState.SendSuccess);
            if (factoryList == null || factoryList.Count() == 0)
            {
                throw new ValidationException("请选择需要传输的未推送成功的数据!".L10N());
            }
            var infGroupData = factoryList.GroupBy(p => p.InfType);
            foreach (var inftype in infGroupData)
            {
                var infGroupType = inftype.Key;
                //var infMapping = infMappings.FirstOrDefault(p => p.InfType == (InfType)infGroupType);
                //if (infMapping.ApiType.IsNullOrEmpty() || infMapping.Method.IsNullOrEmpty())
                //{
                //    stringBuilder.Append("接口类型{0}没有维护控制器或者方法名信息!".L10nFormat(infGroupType.ToLabel()));
                //    continue;
                //}
                var dataList = inftype.AsEntityList();
                //推送工厂日志
                stringBuilder = RT.Service.Resolve<GroupFactoryJobController>().PushFactoryDatas(dataList, smomControlSettingDic, infGroupType.Value, failCount, stringBuilder);
            }

            return stringBuilder;

        }
    }
}
