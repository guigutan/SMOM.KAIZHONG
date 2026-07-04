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
    /// 总控与NC接口日志
    /// </summary>
    public class InfNcDataLogGroupViewConfig : WebViewConfig<InfNcDataLogGroup>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.ClearCommands();

        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(InfNcDataLogGroupReUploadCommand).FullName, typeof(LogGroupSyncOtherFactoryCommand).FullName);
            View.Property(p => p.InvOrg);
            View.Property(p => p.FactoryName);
            View.Property(p => p.InfType);
            View.Property(p => p.CallResult);
            View.Property(p => p.BatchNo).HasLabel("GUID");
            View.Property(p => p.IsDistribute).Show().Readonly();
            //View.Property(p => p.KeyMsgone);
            //View.Property(p => p.KeyMsgtwo);
            //View.Property(p => p.KeyMsgthree);
            //View.Property(p => p.KeyMsgfour);
            //View.Property(p => p.KeyMsgfive);
            View.Property(p => p.DataJsons).ShowInList(width: 180);
            View.Property(p => p.ResponseContent).ShowInList(width: 180);
            View.Property(p => p.SuccessJson);
            View.Property(p => p.SysncResult);
            View.Property(p => p.SendState);
            View.Property(p => p.ErrorMsg);
            View.Property(p => p.FactoryErrorMsg);
            View.Property(p => p.BeginDate).ShowInList(width: 150).FixColumn();
            View.Property(p => p.EndDate).ShowInList(width: 150).FixColumn();
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.BatchNo).HasLabel("GUID");
            View.Property(p => p.InfType).ShowInList(width: 150);
            View.Property(p => p.InvOrg).ShowInList(width: 150);
            View.Property(p => p.FactoryName).ShowInList(width: 150);
            View.Property(p => p.CallResult);
            View.Property(p => p.KeyMsgone);
            View.Property(p => p.KeyMsgtwo);
            View.Property(p => p.KeyMsgthree);
            View.Property(p => p.BeginDate).UseDateRangeEditor();
            View.Property(p => p.SendState);
            View.Property(p => p.DataJsons);
            View.Property(p => p.SuccessJson);
            View.Property(p => p.ResponseContent);
            View.Property(p => p.ErrorMsg);
            View.Property(p => p.FactoryErrorMsg);
        }
    }
}
