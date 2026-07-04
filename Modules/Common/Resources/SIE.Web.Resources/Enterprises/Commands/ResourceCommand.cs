using SIE.Domain;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.Resources.Enterprises.Commands
{
    /// <summary>
    /// 设置组织层级为资源
    /// </summary>
    [JsCommand("SIE.Web.Resources.Enterprises.Commands.EnableResourceCommand")]
    public class EnableResourceCommand : ViewCommand
    {
        /// <summary>
        /// 设置组织层级为资源
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var orgUserList = args.Data.ToJsonObject<List<EnterpriseLevel>>();
            Check.NotNullOrEmpty(orgUserList, nameof(orgUserList));
            if (null == orgUserList || orgUserList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(orgUserList)));
            }
            foreach (var item in orgUserList)
            {
                var ctl = RT.Service.Resolve<EnterpriseController>();
                var rtn = ctl.SetResource(item.Id, true);
                item.Clone(rtn, CloneOptions.ReadDbRow());    //克隆对象
                item.NotifyAllPropertiesChanged();
            }
            return true;
        }
    }

    /// <summary>
    /// 取消组织层级资源
    /// </summary>
    [JsCommand("SIE.Web.Resources.Enterprises.Commands.DisableResourceCommand")]
    public class DisableResourceCommand : ViewCommand
    {
        /// <summary>
        /// 取消组织层级资源
        /// </summary>
        /// <param name="args"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var orgUserList = args.Data.ToJsonObject<List<EnterpriseLevel>>();
            Check.NotNullOrEmpty(orgUserList, nameof(orgUserList));
            if (null == orgUserList || orgUserList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(orgUserList)));
            }
            var ctl = RT.Service.Resolve<EnterpriseController>();

            foreach (EnterpriseLevel resourceLevel in orgUserList)
            {
                var rtn = ctl.SetResource(resourceLevel.Id, false);
                RT.Service.Resolve<WipResourceController>().StopSchResourse(resourceLevel.Id, SyncSourceType.Enterprise);
                resourceLevel.Clone(rtn, CloneOptions.ReadDbRow());    //克隆对象
                resourceLevel.NotifyAllPropertiesChanged();
                resourceLevel.MarkSaved();
            }
            return true;
        }
    }
}
