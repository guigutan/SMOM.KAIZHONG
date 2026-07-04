using SIE.Api;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.EquipAccounts;
using SIE.KZ.Base.SmomControl;
using SIE.Security;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.ProcessProperty
{
    /// <summary>
    /// 工序参数数采控制器
    /// </summary>
    public class ProcessParamCollectController : DomainController
    {
        /// <summary>
        /// 查询工序参数数采
        /// </summary>
        /// <param name="criterial">工序参数数采查询</param>
        /// <returns>工序参数数采</returns>
        public virtual EntityList<ProcessParamCollect> CriterialProcessParamCollect(ProcessParamCollectCriteria criterial)
        {
            var q = Query<ProcessParamCollect>();
            if (!criterial.SN.IsNullOrEmpty())
            {
                q.Where(m => m.SN.Contains("%" + criterial.SN + "%"));
            }
            if (!criterial.ProcessCode.IsNullOrEmpty())
            {
                q.Where(m => m.ProcessCode.Contains("%" + criterial.ProcessCode + "%"));
            }
            if (!criterial.EquipmentName.IsNullOrEmpty())
            {
                q.Where(m => m.EquipmentName.Contains("%" + criterial.EquipmentName + "%"));
            }
            if (criterial.QualityStatus.HasValue)
            {
                q.Where(p => p.QualityStatus == criterial.QualityStatus);
            }
            if (criterial.CreateDate.BeginValue != null)
            {
                q.Where(p => p.CreateDate >= criterial.CreateDate.BeginValue.Value);
            }
            if (criterial.CreateDate.EndValue != null)
            {
                q.Where(p => p.CreateDate <= criterial.CreateDate.EndValue.Value);
            }
            return q.OrderBy(criterial.OrderInfoList).ToList(criterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 创建工序参数数采
        /// </summary>
        /// <param name="dataList">创建数据列表</param>
        [ApiService("创建工序参数数采")]
        [AllowAnonymous]
        public virtual void CreateProcessParamCollect(List<ProcessParamCollectData> dataList)
        {
            RT.InvOrg = 1;
            //校验数据
            ValidationProcessParamCollect(dataList);
            var factoryCodes = dataList.Select(p => p.Factory).Distinct().ToList();
            var settings = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCodes(factoryCodes);
            foreach (var g in settings.GroupBy(p => p.FactoryUrl))
            {
                var fCs = g.Select(p => p.FactoryCode).Distinct().ToList();
                var factoryList = dataList.Where(p => fCs.Contains(p.Factory)).ToList();
                var smomParam = new List<SmomParam>(){
                            new SmomParam { Value = factoryList }
                        }.ToArray();
                SmomControlHepler.SmomPost<object>("ProcessParamCollectController", "CreateProcessParamCollectFactory", g.Key, smomParam);
            }
        }

        /// <summary>
        /// 校验工序参数数采数据
        /// </summary>
        /// <param name="dataList">工序参数数采</param>
        private void ValidationProcessParamCollect(List<ProcessParamCollectData> dataList)
        {
            if (dataList.IsNullOrEmpty())
            {
                throw new ValidationException("工序参数数采创建数据不能为空！".L10N());
            }
            // 校验每条数据
            if (dataList.Any(p => p.SN.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("SN不能为空！".L10N());
            }
            if (dataList.Any(p => p.Time.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("时间不能为空！".L10N());
            }
            if (dataList.Any(p => p.ProcessCode.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("工序编码不能为空！".L10N());
            }
            if (dataList.Any(p => p.Factory.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("工厂不能为空！".L10N());
            }
            if (dataList.Any(p => p.EquipmentCode.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("设备编码不能为空！".L10N());
            }
            if (dataList.Any(p => p.QualityStatus.IsNullOrWhiteSpace()))
            {
                throw new ValidationException("质量状态不能为空！".L10N());
            }
        }

        /// <summary>
        /// 创建工序参数数采-工厂
        /// </summary>
        /// <param name="dataList">创建数据列表</param>
        [ApiService("创建工序参数数采-工厂")]
        [AllowAnonymous]
        public virtual void CreateProcessParamCollectFactory(List<ProcessParamCollectData> dataList)
        {
            //校验数据
            ValidationProcessParamCollect(dataList);
            // 批量查询工序和设备台账
            var processCodes = dataList.Select(d => d.ProcessCode).Distinct().ToList();
            var equipCodes = dataList.Select(d => d.EquipmentCode).Distinct().ToList();
            var processList = processCodes.SplitContains(c => Query<Process>().Where(p => c.Contains(p.Code)).ToList());
            var equipList = equipCodes.SplitContains(c => Query<EquipAccount>().Where(p => c.Contains(p.Code)).ToList());
            var entitys = new EntityList<ProcessParamCollect>();
            foreach (var data in dataList)
            {
                var entity = new ProcessParamCollect();
                entity.SN = data.SN;
                entity.Time = data.Time;
                entity.ProcessFlowCode = data.Vornr;
                entity.ProcessCode = data.ProcessCode;
                entity.ProcessName = processList.FirstOrDefault(p => p.Code == data.ProcessCode)?.Name;
                entity.Factory = data.Factory;
                entity.EquipmentCode = data.EquipmentCode;
                entity.EquipmentName = equipList.FirstOrDefault(p => p.Code == data.EquipmentCode)?.Name;
                entity.QualityStatus = data.QualityStatus.ToUpper() == "TRUE" ? InspResult.OK : InspResult.NG;

                // 工艺参数
                foreach (var param in data.ParamList)
                {
                    var detail = new ProcessParamCollectParam
                    {
                        ParamName = param.ParamName,
                        ParamValue = param.ParamValue,
                        Unit = param.Unit,
                    };
                    entity.ProcessParamCollectParamList.Add(detail);
                }
                // 子件
                foreach (var component in data.ComponentList)
                {
                    var detail = new ProcessParamCollectComponent
                    {
                        ComponentSN = component.ComponentSN,
                    };
                    entity.ProcessParamCollectComponentList.Add(detail);
                }
                entitys.Add(entity);
            }
            RF.Save(entitys);
        }

        /// <summary>
        /// 获取工序参数数采
        /// </summary>
        /// <param name="sn">SN</param>
        /// <returns>工序参数数采</returns>
        public virtual ProcessParamCollect GetProcessParamCollect(string sn)
        {
            return Query<ProcessParamCollect>().Where(p => p.SN == sn).OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }
    }
}
