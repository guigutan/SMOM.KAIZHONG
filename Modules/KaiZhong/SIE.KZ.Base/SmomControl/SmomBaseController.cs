using Org.BouncyCastle.Asn1.Ocsp;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Caching;
using SIE.Domain.Validation;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.SmomControl
{
    public partial class SmomBaseController : DomainController
    {

        #region 是否启用制卡数量维护

        /// <summary>
        /// 通过接口获取集团【是否启用制卡数量维护】数据
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<MtartZtflRelation> GetMtartZtflRelations(List<string> mtarts)
        {
            EntityList<MtartZtflRelation> relations = mtarts.SplitContains(ms =>
            {
                return Query<MtartZtflRelation>().Where(p => ms.Contains(p.Mtart)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            #region 

            //var invOrg = Query<InvOrg>().Where(p => p.Code == RT.InvOrg).FirstOrDefault();
            //var Url = RT.Config.Get("Group.URL");
            //if (Url.IsNullOrEmpty())
            //{
            //    throw new ValidationException("请在配置文件中维护总控集团的URL接口地址!".L10N());
            //}
            //try
            //{
            //    var smomParam = new List<SmomParam>()
            //        {
            //        new SmomParam { Value =invOrg.ExternalId },
            //        new SmomParam { Value =mtarts },
            //                        new SmomParam { Value =1 }
            //                     }.ToArray();
            //    var response = SmomControlHepler.SmomPost<EntityList<MtartZtflRelation>>("SmomBaseController", "GetMtartZtflRelations", Url, smomParam);
            //    if (response == null)
            //    {
            //        relations = new EntityList<MtartZtflRelation>();
            //    }
            //    else
            //    {
            //        relations = response;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    //throw new ValidationException(ex.GetBaseException().Message);
            //}
            #endregion

            return relations;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public virtual string JtSyncSmomControlSetting()
        {
            string msg = "";
            //获取全部数据
            var settings = Query<SmomControlSetting>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var smomControlSetting in settings)
            {
                try
                {
                    if (smomControlSetting == null || smomControlSetting.FactoryUrl.IsNullOrEmpty())
                    {
                        throw new ValidationException("未配置Url地址;");
                    }

                    var smomParam = new List<SmomParam>()
                    {
                    new SmomParam { Value =settings },
                                    new SmomParam { Value =smomControlSetting.FactoryCode }
                                 }.ToArray();
                    var response = SmomControlHepler.SmomPost<object>("SmomBaseController", "SyncSmomControlSetting", smomControlSetting.FactoryUrl, smomParam);
                }
                catch (Exception ex)
                {
                    msg += "工厂{0}:".L10nFormat(smomControlSetting.FactoryCode) + ex.GetBaseException()?.Message;
                }
            }
            if (msg == "")
            {
                msg = "调度执行结束;";
            }
            return msg;
        }

        /// <summary>
        /// 获取总控配置
        /// </summary>
        /// <returns></returns>
        public virtual EntityList<SmomControlSetting> GetSmomControlSettings()
        {
            EntityList<SmomControlSetting> settings = new EntityList<SmomControlSetting>();
            var list = Query<SmomControlSetting>().Where(p => p.IsMain == YesNo.Yes).ToList().ToLookup(p => p.FactoryCode);
            foreach (var setting in list)
            {
                settings.Add(setting.First());
            }
            return settings;
        }

        /// <summary>
        /// 根据工厂编码获取总控
        /// </summary>
        /// <param name="FactoryCode"></param>
        /// <returns></returns>
        public virtual SmomControlSetting GetSmomControlSettingByFactoryCode(string FactoryCode)
        {
            var first = Query<SmomControlSetting>().Where(p => p.FactoryCode == FactoryCode).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            return first;
        }    

        /// <summary>
        /// 根据工厂获取数据
        /// </summary>
        /// <param name="factoryCodes"></param>
        /// <returns></returns>
        public virtual EntityList<SmomControlSetting> GetSmomControlSettingByFactoryCodes(List<string> factoryCodes)
        {
            return Query<SmomControlSetting>().Where(p => factoryCodes.Contains(p.FactoryCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取所有基地的Url
        /// </summary>
        /// <param name="FactoryCode"></param>
        /// <returns></returns>
        public virtual List<string> GetSmomControlSettingUrl()
        {
            return Query<SmomControlSetting>().Select(p => p.FactoryUrl).Distinct().ToList<string>().ToList();
        }
    }
}
