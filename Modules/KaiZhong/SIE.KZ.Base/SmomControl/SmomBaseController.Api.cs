using Org.BouncyCastle.Asn1.Ocsp;
using SIE.Api;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.SmomControl
{
    public partial class SmomBaseController : DomainController
    {
        /// <summary>
        /// 获取是否启用制卡数量维护数据
        /// </summary>
        /// <param name="datas"></param>
        [ApiService("获取是否启用制卡数量维护数据")]
        [AllowAnonymous]
        public virtual EntityList<MtartZtflRelation> GetMtartZtflRelations(int facotry, List<string> mtarts, int invOrg)
        {
            RT.InvOrg = invOrg;
            RT.Service.Resolve<LoginController>().Login();

            var relations = mtarts.SplitContains(s => {
                return Query<MtartZtflRelation>().Where(p => p.Factory == facotry.ToString() && s.Contains(p.Mtart)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return relations;
        }

        /// <summary>
        /// 基地传输GUID到集团
        /// </summary>
        /// <param name="datas"></param>
        [ApiService("基地传输GUID到集团")]
        [AllowAnonymous]
        public virtual void GuidFactory(List<GuidFactoryRelastion> datas)
        {
            RT.InvOrg = 1;
            RT.Service.Resolve<LoginController>().Login();

            foreach (var data in datas)
            {
                try
                {
                    GuidFactoryRelastion relastion = new GuidFactoryRelastion();
                    relastion.PersistenceStatus = PersistenceStatus.New;
                    relastion.Guid = data.Guid;
                    relastion.InfType = data.InfType;
                    relastion.SourceFactory = data.SourceFactory;
                    RF.Save(relastion);
                }
                catch (Exception ex)
                { 
                    
                }
            }
        }

        /// <summary>
        /// 从集团获取总控配置，然后同步到各个工厂中
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="invOrg"></param>
        [ApiService("同步集团的总控配置")]
        [AllowAnonymous]
        public virtual void SyncSmomControlSetting(List<SmomControlSetting> datas,string invOrg)
        {
            RT.Service.Resolve<LoginController>().Login(invOrg);
            var factoryCodes = datas.Select(p => p.FactoryCode).Distinct().ToList();
            EntityList<SmomControlSetting> settings = Query<SmomControlSetting>().Where(p => factoryCodes.Contains(p.FactoryCode)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var data in datas)
            {
                var setting = settings.FirstOrDefault(p => p.FactoryCode == data.FactoryCode);
                if (setting == null)
                {
                    setting = new SmomControlSetting();
                    setting.Clone(data, new CloneOptions(CloneActions.NormalProperties));
                    setting.GenerateId();
                    setting.PersistenceStatus = PersistenceStatus.New;
                    RF.Save(setting);
                    settings.Add(setting);
                    continue;
                }

                if (data.FactoryName != setting.FactoryName)
                {
                    setting.FactoryName = data.FactoryName;
                    setting.PersistenceStatus = PersistenceStatus.Modified;
                }
                if (data.FactoryUrl != setting.FactoryUrl)
                {
                    setting.FactoryUrl = data.FactoryUrl;
                    setting.PersistenceStatus = PersistenceStatus.Modified;
                }
                if (setting.PersistenceStatus == PersistenceStatus.Modified || setting.PersistenceStatus == PersistenceStatus.New)
                {
                    RF.Save(setting);
                }
            }
        }
    }
}
