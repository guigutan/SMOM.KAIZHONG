using DevExpress.Xpf.Editors;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.IOT;
using SIE.MES.TaskManagement.Reports.Datas;
using SIE.ObjectModel;
using SIE.Wpf.Controls.WaitProgress;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// 生产报工(多工位) 视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("生产报工(多工位)")]
    public class KZTaskReportMultiStationViewModel : KZTaskReportViewModelBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZTaskReportMultiStationViewModel()
        {
            IotMode = IotMode.MultiStation;

            //初始化8个工位
            ReportViewModelList.Clear();
            for (int i = 0; i < 8; i++)
            {
                ReportViewModelList.Add(new KZTaskReportViewModel() { Id = $"{i + 1}", ReportEmployee = ReportEmployee, IotMode = IotMode.MultiStation });
            }
            ReportViewModelList.MarkSaved();
        }

        /// <summary>
        /// 对象锁
        /// </summary>
        private readonly object lockObj = new object();

        #region 生产任务列表 ReportViewModelList
        /// <summary>
        /// 生产任务列表
        /// </summary>
        public static readonly ListProperty<EntityList<KZTaskReportViewModel>> ReportViewModelListProperty = P<KZTaskReportMultiStationViewModel>.RegisterList(e => e.ReportViewModelList, new ListPropertyMeta()
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = (DispatchTask) => { return new EntityList<KZTaskReportViewModel>(); }
        });

        /// <summary>
        /// 生产任务列表
        /// </summary>
        public EntityList<KZTaskReportViewModel> ReportViewModelList
        {
            get { return this.GetLazyList(ReportViewModelListProperty); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            if (kZReportHelper == null)
                kZReportHelper = new KZReportHelper(this);
            kZReportHelper.ShowReportMulitStation();
            Reset(ResetType.None);

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            //base.OnClose();
            StopIOTReportTimer();
            ReportTimer = null;
        }

        /// <summary>
        /// 加载首个任务
        /// </summary>
        public override void LoadFirstQueueTask()
        {
            //base.LoadFirstQueueTask();
        }
    }
}
