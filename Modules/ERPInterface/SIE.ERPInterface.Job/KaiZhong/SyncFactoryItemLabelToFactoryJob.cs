using SIE.Common.Schdules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Parameters;
using SIE.KZ.Group.SmomControl.Controllers;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Job.KaiZhong
{
    /// <summary>
    /// 跨组织物料标签同步
    /// </summary>
    [Job("跨组织物料标签同步", typeof(JobParameter))]

    internal class SyncFactoryItemLabelToFactoryJob: JobBase
    {
        protected override void ExecuteJob(object param)
        {
            var redisKey = "SyncFactoryItemLabelToFactoryJob" + RT.InvOrg;
            string lockId = null;
            var locked = RT.Redis.Lock(redisKey, out lockId, 1800);
            if (!locked)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var result = RT.Service.Resolve<InfNcDataLogGroupController>().SyncFactoryItemLabelToFactory();
                if (result == "")
                    AddLog("执行结束");
                else
                    AddLog(result);
            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                RT.Redis.UnLock(redisKey, lockId);
            }
        }
    }


    //[RootEntity, Serializable]
    //public class SyncFactoryItemLabelToFactoryParameter : JobParameter
    //{
    //    #region 注释 Name
    //    /// <summary>
    //    /// 注释
    //    /// </summary>
    //    [Label("属性名")]
    //    public static readonly Property<string> NameProperty = P<SyncFactoryItemLabelToFactoryParameter>.Register(e => e.Name);

    //    /// <summary>
    //    /// 注释
    //    /// </summary>
    //    public string Name
    //    {
    //        get { return this.GetProperty(NameProperty); }
    //        set { this.SetProperty(NameProperty, value); }
    //    }
    //    #endregion

    //}

    //public class SyncFactoryItemLabelToFactoryParameterViewConfig : WebViewConfig<SyncFactoryItemLabelToFactoryParameter>
    //{
    //    protected override void ConfigView()
    //    {
    //        View.Property(p => p.Name).Show();
    //    }
    //}
}
