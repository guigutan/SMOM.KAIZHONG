using SIE.KZ.Base.Interfaces;
using SIE.Web.KZ.Base.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.KZ.Base.Interfaces
{
    /// <summary>
    /// 总控与工厂接口日志
    /// </summary>
    public class InfNcDataLogFactoryViewConfig : WebViewConfig<InfNcDataLogFactory>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {

        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(ResyncDataCommand).FullName);
            View.Property(p => p.InvOrg);
            View.Property(p => p.FactoryName);
            View.Property(p => p.InfType);
            View.Property(p => p.BatchNo).HasLabel("唯一码");
            View.Property(p => p.GroupGuid);
            View.Property(p => p.FailCount);
            View.Property(p => p.GroupGuid);
            View.Property(p => p.KeyMsgone);
            View.Property(p => p.KeyMsgtwo);
            View.Property(p => p.KeyMsgthree);
            View.Property(p => p.KeyMsgfour);
            View.Property(p => p.KeyMsgfive);
            View.Property(p => p.DataJsons).ShowInList(width: 180);
            View.Property(p => p.SendState);
            View.Property(p => p.ErrorMsg);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.InvOrg);
            View.Property(p => p.InfType).ShowInList(width: 150);
            View.Property(p => p.FactoryName).ShowInList(width: 150);
            View.Property(p => p.SendState);
            View.Property(p => p.FailCount);
            View.Property(p => p.KeyMsgone);
            View.Property(p => p.KeyMsgtwo);
            View.Property(p => p.KeyMsgthree);
            View.Property(p => p.GroupGuid);
            View.Property(p => p.BatchNo).HasLabel("唯一码");
            View.Property(p => p.CreateDate).UseDateRangeEditor();
            View.Property(p => p.DataJsons);
            View.Property(p => p.ErrorMsg);
        }
    }
}
