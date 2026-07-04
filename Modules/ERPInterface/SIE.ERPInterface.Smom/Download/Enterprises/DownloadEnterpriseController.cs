using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 企业模型下载控制器
    /// </summary>
    public class DownloadEnterpriseController : DomainController
    {
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="enterpriseDatas"></param>
        /// /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadEnterpriseToBusiness(List<EnterpriseData> enterpriseDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<EnterpriseData>(
                enterpriseDatas,
                p => this.SaveEnterprises(p.OrderByLastUpdateDate()),
                JobType.Enterprise,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载企业模型到业务表
        /// </summary>
        public virtual ProcessResult DownloadEnterpriseInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<EnterpriseInf>(
                () => ctl.GetUnprocessedDatas<EnterpriseInf>().OrderBy(p => p.EnterpriseLevelNum),         //企业模型中间表数据，按层次从小到大，父到子排序
                p =>
                {
                    var paras = this.GenerateEnterprisePara(p);
                    return this.SaveEnterprises(paras.OrderByLastUpdateDate());
                },
                JobType.Enterprise, isManual);
        }

        /// <summary>
        /// 生成企业模型实体
        /// </summary>
        /// <param name="enterpriseInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<EnterpriseData> GenerateEnterprisePara(IEnumerable<EnterpriseInf> enterpriseInfs)
        {
            var paras = new List<EnterpriseData>();

            enterpriseInfs.ForEach(p =>
            {
                var data = new EnterpriseData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.IsResource = p.IsResource;
                data.LevelCode = p.Level;
                data.ParentCode = p.ParentCode;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }


        /// <summary>
        /// ERP保存企业模型数据
        /// </summary>
        /// <param name="datas">通信数据类</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveEnterprises(List<EnterpriseData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            //获取企业层级数据
            var enterpriseLevels = RF.GetAll<EnterpriseLevel>(null, new EagerLoadOptions().LoadWith(Enterprise.LevelProperty));
            if (enterpriseLevels.Count == 0)
                throw new ValidationException("请先维护企业层级数据".L10N());
            var dicLevel = enterpriseLevels.ToDictionary(p => p.Code);

            //获取企业模型数据
            var enterprises = RF.GetAll<Enterprise>();
            var dicEnterprise = enterprises.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveEnterprise(p, dicEnterprise, dicLevel);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
                }
            }

            return errors;
        }


        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicLevel">数据字典</param>
        private void SaveEnterprise(EnterpriseData data, Dictionary<string, Enterprise> dic, Dictionary<string, EnterpriseLevel> dicLevel)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            Enterprise entity;
            var key = data.Code;
            if (key.IsNullOrEmpty())
                throw new ValidationException("企业模型编码为空".L10nFormat(key));
            //处理待删除数据
            if (dic.ContainsKey(key))
            {
                if (data.IsDelete)
                {
                    ctl.DeleteEntity(dic, key, dic[key]);
                }
                return;
            }
            if (!dic.ContainsKey(key))
                dic.Add(key, new Enterprise());
            entity = dic[key];
            if (data.LevelCode.IsNullOrEmpty() || !dicLevel.ContainsKey(data.LevelCode))
                throw new ValidationException("企业层级[{0}]不存在".L10nFormat(data.LevelCode));

            if (data.Code == data.ParentCode)
                throw new ValidationException("企业模型编码[{0}]和父编码不能一样".L10nFormat(data.Code));

            if (data.ParentCode.IsNullOrEmpty() || !dic.ContainsKey(data.ParentCode))
                throw new ValidationException("父编码[{0}]不存在".L10nFormat(data.ParentCode));

            entity.Code = data.Code;
            entity.Name = data.Name;
            entity.ErpOrgId = data.ErpOrgId;
            entity.IsResource = data.IsResource;
            entity.Level = dicLevel[data.LevelCode];
            entity.InvOrgId = RT.InvOrg.Value;
            entity.TreePId = dic[data.ParentCode].Id;

            RF.Save(entity);
        }
    }
}
