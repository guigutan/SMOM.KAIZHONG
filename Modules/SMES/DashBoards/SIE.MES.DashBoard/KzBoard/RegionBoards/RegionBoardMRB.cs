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
    /// MRB控制者
    /// </summary>
    [ChildEntity, Serializable]
    [Label("MRP控制者")]
    public class RegionBoardMRB : DataEntity
    {
        #region 区域与产线关联关系 RegionBoard
        /// <summary>
        /// 区域与产线关联关系Id
        /// </summary>
        [Label("区域与产线关联关系")]
        public static readonly IRefIdProperty RegionBoardIdProperty =
            P<RegionBoardMRB>.RegisterRefId(e => e.RegionBoardId, ReferenceType.Parent);

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
            P<RegionBoardMRB>.RegisterRef(e => e.RegionBoard, RegionBoardIdProperty);

        /// <summary>
        /// 区域与产线关联关系
        /// </summary>
        public RegionBoard RegionBoard
        {
            get { return this.GetRefEntity(RegionBoardProperty); }
            set { this.SetRefEntity(RegionBoardProperty, value); }
        }
        #endregion

        #region MRB控制者 Code
        /// <summary>
        /// MRB控制者
        /// </summary>
        [Label("MRP控制者")]
        public static readonly Property<string> CodeProperty = P<RegionBoardMRB>.Register(e => e.Code);

        /// <summary>
        /// MRB控制者
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion
    }

    internal class RegionBoardMRBConfig : EntityConfig<RegionBoardMRB>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties = {
                RegionBoardMRB.RegionBoardIdProperty,
                RegionBoardMRB.CodeProperty
                },
                MessageBuilder = (e) =>
                {
                    return "已存在相同MRP控制者".L10N();
                }
            });
            base.AddValidations(rules);
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("REGION_BOARD_MRB").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
