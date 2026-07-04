using SIE.Common.Schdules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.SmomControl;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ListAtts
{
    /// <summary>
    /// 从外部接口获取考勤数据并推送至各工厂
    /// </summary>
    [Job("从外部接口获取考勤数据并推送至各工厂", typeof(SendGroupListAttDataToFactoryJobParameter))]
    public class SendGroupListAttDataToFactoryJob:JobBase
    {       
        /// <summary>
        /// 是否运行中,用来防止并发问题
        /// </summary>
        public static bool IsRun = false;

        /// <summary>
        /// 执行调度
        /// </summary>
        /// <param name="param"></param>
        /// <exception cref="ValidationException"></exception>
        protected override void ExecuteJob(object param)
        {
            if (SendGroupListAttDataToFactoryJob.IsRun == true)
                throw new ValidationException("任务正在运行中,不允许并发执行".L10N());
            SendGroupListAttDataToFactoryJob.IsRun = true;

            try
            {
                AddLog($"当前组织[{RT.InvOrg}],当前身份[{RT.IdentityId}]\r\n");
                var listAttController = RT.Service.Resolve<ListAttController>();
                string curMsg = listAttController.ExecJob(param).GetAwaiter().GetResult();
                AddLog(curMsg); 
            }
            catch (Exception exMsg)
            {
                AddLog($"执行失败，错误信息: {exMsg.Message}");
            }
            finally
            {
                SendGroupListAttDataToFactoryJob.IsRun = false;
            }
        }     
    }








    /// <summary>
    /// 从外部接口获取考勤数据并推送至各工厂的调度参数
    /// </summary>
    [RootEntity, Serializable]
    public class SendGroupListAttDataToFactoryJobParameter : JobParameter
    {
        /// <summary>
        /// 调度参数    
        /// </summary>
        public SendGroupListAttDataToFactoryJobParameter()
        {
            RequestServer = "https://172.17.12.136:8098/api/transaction/listAttTransaction";
            AccessToken = "D0FC26A87AC5A5422281224CF296AD3A95D2F9E748F9075EED5584A27863CD58";
            Days = 1;
        }

        /// <summary>
        /// 考勤数据的调用服务网址
        /// </summary>
        [Label("考勤数据的调用服务网址")]
        public static readonly Property<string> RequestServerTokenProperty = P<SendGroupListAttDataToFactoryJobParameter>.Register(e => e.RequestServer);

        /// <summary>
        /// 考勤数据的调用服务网址
        /// </summary>
        public string RequestServer
        {
            get { return this.GetProperty(RequestServerTokenProperty); }
            set { this.SetProperty(RequestServerTokenProperty, value); }
        }

        /// <summary>
        /// 考勤数据的调用Token
        /// </summary>
        [Label("考勤数据的调用Token")]
        public static readonly Property<string> AccessTokenProperty = P<SendGroupListAttDataToFactoryJobParameter>.Register(e => e.AccessToken);

        /// <summary>
        /// 考勤数据的调用Token
        /// </summary>
        public string AccessToken
        {
            get { return this.GetProperty(AccessTokenProperty); }
            set { this.SetProperty(AccessTokenProperty, value); }
        }

        /// <summary>
        /// 要获取几天的考勤数据（1为当天）
        /// </summary>
        [Label("要获取几天的考勤数据（1为当天）")]
        public static readonly Property<int> DaysProperty = P<SendGroupListAttDataToFactoryJobParameter>.Register(e => e.Days);

        /// <summary>
        /// 要获取前几天考勤数据
        /// </summary>
        public int Days
        {
            get { return this.GetProperty(DaysProperty); }
            set { this.SetProperty(DaysProperty, value); }
        }       
    }

    /// <summary>
    /// 从外部接口获取考勤数据并推送至各工厂的调度参数-视图配置
    /// </summary>
    public class SendGroupListAttDataToFactoryJobParameterWebViewConfig : WebViewConfig<SendGroupListAttDataToFactoryJobParameter>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.RequestServer).Show();
                View.Property(p => p.AccessToken).Show();
                View.Property(p => p.Days).Show();
            }
        }
    }

























}
