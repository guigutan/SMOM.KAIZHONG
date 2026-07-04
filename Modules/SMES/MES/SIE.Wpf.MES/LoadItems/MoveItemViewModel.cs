using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 工位挪料
    /// </summary>
    [RootEntity, Serializable]
    [Label("工位挪料")]
    public class MoveItemViewModel : ViewModel
    {
        #region Qty 配送数量
        /// <summary>
        /// 配送数量
        /// </summary>
        [MinValue(0)]
        [Label("配送数量")]
        public static readonly Property<int> QtyProperty = P<MoveItemViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 配送数量
        /// </summary>
        public int Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region Resource 目标产线
        /// <summary>
        /// 目标产线ID
        /// </summary>
        [Label("目标产线")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<MoveItemViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 目标产线ID
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 目标产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<MoveItemViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 目标产线
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region Process 目标工序
        /// <summary>
        /// 目标工序ID
        /// </summary>
        [Label("目标工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<MoveItemViewModel>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 目标工序ID
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 目标工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<MoveItemViewModel>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 目标工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region Station 目标工位
        /// <summary>
        /// 目标工位ID
        /// </summary>
        [Label("目标工位")]
        public static readonly IRefIdProperty StationIdProperty =
            P<MoveItemViewModel>.RegisterRefId(e => e.StationId, ReferenceType.Normal);

        /// <summary>
        /// 目标工位ID
        /// </summary>
        public double StationId
        {
            get { return (double)this.GetRefId(StationIdProperty); }
            set { this.SetRefId(StationIdProperty, value); }
        }

        /// <summary>
        /// 目标工位
        /// </summary>
        public static readonly RefEntityProperty<Station> StationProperty =
            P<MoveItemViewModel>.RegisterRef(e => e.Station, StationIdProperty);

        /// <summary>
        /// 目标工位
        /// </summary>
        public Station Station
        {
            get { return this.GetRefEntity(StationProperty); }
            set { this.SetRefEntity(StationProperty, value); }
        }
        #endregion

        #region 来源工位 FromStation
        /// <summary>
        /// 来源工位Id
        /// </summary>
        [Label("来源工位")]
        public static readonly IRefIdProperty FromStationIdProperty =
            P<MoveItemViewModel>.RegisterRefId(e => e.FromStationId, ReferenceType.Normal);

        /// <summary>
        /// 来源工位Id
        /// </summary>
        public double FromStationId
        {
            get { return (double)this.GetRefId(FromStationIdProperty); }
            set { this.SetRefId(FromStationIdProperty, value); }
        }

        /// <summary>
        /// 来源工位
        /// </summary>
        public static readonly RefEntityProperty<Station> FromStationProperty =
            P<MoveItemViewModel>.RegisterRef(e => e.FromStation, FromStationIdProperty);

        /// <summary>
        /// 来源工位
        /// </summary>
        public Station FromStation
        {
            get { return this.GetRefEntity(FromStationProperty); }
            set { this.SetRefEntity(FromStationProperty, value); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (e.Property == ResourceProperty)
            {
                this.Station = null;
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// 获取工作单元信息
        /// </summary>
        /// <returns>工作单元</returns>
        public Workcell GetWorkcell()
        {
            Workcell workcell = new Workcell();
            workcell.EmployeeId = RT.IdentityId;
            workcell.ResourceId = ResourceId;
            workcell.ProcessId = ProcessId;
            workcell.StationId = StationId;
            return workcell;
        }
    }

    /// <summary>
    /// 工位挪料实体配置
    /// </summary>
    class MoveItemViewModelConfig : EntityConfig<MoveItemViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add((s, e) =>
            {
                var m = s as MoveItemViewModel;
                if (m.Qty <= 0)
                    e.BrokenDescription = "挪料数量必须大于0".L10N();
                if (m.FromStationId == m.StationId)
                    e.BrokenDescription = "目标工位不能是原工位，请更换工位".L10N();

                if (!e.IsBroken)
                {
                    Workcell workcell = m.GetWorkcell();
                    ////获取目标工位在生产工单
                    var wipLineWorkOrder = RT.Service.Resolve<AssemblyController>().GetWipResourceWorkOrder(workcell);
                    if (wipLineWorkOrder == null || wipLineWorkOrder.WorkOrder == null)
                        e.BrokenDescription = "产线工单不存在".L10N();
                }
            });
        }
    }
}
