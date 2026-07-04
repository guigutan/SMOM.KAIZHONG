using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.RegionBoards
{
    /// <summary>
    /// 产线明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产线明细")]
    public class RegionBoardDetail : DataEntity
    {
        #region 区域与产线关联关系 RegionBoard
        /// <summary>
        /// 区域与产线关联关系Id
        /// </summary>
        [Label("区域与产线关联关系")]
        public static readonly IRefIdProperty RegionBoardIdProperty =
            P<RegionBoardDetail>.RegisterRefId(e => e.RegionBoardId, ReferenceType.Parent);

        /// <summary>
        /// 区域与产线关联关系Id
        /// </summary>
        public double RegionBoardId
        {
            get { return (double)this.GetRefId(RegionBoardIdProperty); }
            set { this.SetRefId(RegionBoardIdProperty, value); }
        }

        /// <summary>
        /// 区域与产线关联关系
        /// </summary>
        public static readonly RefEntityProperty<RegionBoard> RegionBoardProperty =
            P<RegionBoardDetail>.RegisterRef(e => e.RegionBoard, RegionBoardIdProperty);

        /// <summary>
        /// 区域与产线关联关系
        /// </summary>
        public RegionBoard RegionBoard
        {
            get { return this.GetRefEntity(RegionBoardProperty); }
            set { this.SetRefEntity(RegionBoardProperty, value); }
        }
        #endregion

        #region 产线 WipResource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty WipResourceIdProperty =
            P<RegionBoardDetail>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double WipResourceId
        {
            get { return (double)this.GetRefId(WipResourceIdProperty); }
            set { this.SetRefId(WipResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> WipResourceProperty =
            P<RegionBoardDetail>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource WipResource
        {
            get { return this.GetRefEntity(WipResourceProperty); }
            set { this.SetRefEntity(WipResourceProperty, value); }
        }
        #endregion

        #region 排序 Sort
        /// <summary>
        /// 排序
        /// </summary>
        [Label("排序")]
        public static readonly Property<int> SortProperty = P<RegionBoardDetail>.Register(e => e.Sort);

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort
        {
            get { return this.GetProperty(SortProperty); }
            set { this.SetProperty(SortProperty, value); }
        }
        #endregion


        #region 限时报工正常（产线几小时内有报工记录的也视为正常状态）
        /// <summary>
        /// 限时报工正常
        /// </summary>
        [Label("限时报工正常")]
        public static readonly Property<decimal> ReportHoursProperty = P<RegionBoardDetail>.Register(e => e.ReportHours);

        /// <summary>
        /// 限时报工正常（产线几小时内有报工记录的也视为正常状态）
        /// </summary>
        public decimal ReportHours
        {
            get { return this.GetProperty(ReportHoursProperty); }
            set { this.SetProperty(ReportHoursProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 产线编码 ResourceCode
        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> ResourceCodeProperty = P<RegionBoardDetail>.RegisterView(e => e.ResourceCode, p => p.WipResource.Code);

        /// <summary>
        /// 产线编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<RegionBoardDetail>.RegisterView(e => e.ResourceName, p => p.WipResource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion


        #endregion
    }

    internal class RegionBoardDetailConfig : EntityConfig<RegionBoardDetail>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                RegionBoardDetail.RegionBoardIdProperty,
                RegionBoardDetail.SortProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同排序的序号".L10N();
                }
            });
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                    RegionBoardDetail.RegionBoardIdProperty,
                RegionBoardDetail.WipResourceIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同的产线".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("REGION_BOARD_DTL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }



}
