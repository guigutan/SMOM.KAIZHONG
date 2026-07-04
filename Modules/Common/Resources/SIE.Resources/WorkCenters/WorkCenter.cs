using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.WorkCenters
{
    /// <summary>
    /// 工作中心实体
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(Code))]
    [Label("工作中心")]
    public class WorkCenter : DataEntity, IStateEntity
    {

        #region 工作中心类别 Category
        /// <summary>
        /// 工作中心类别
        /// </summary>
        [Label("工作中心类别")]
        public static readonly Property<string> CategoryProperty = P<WorkCenter>.Register(e => e.Category);

        /// <summary>
        /// 工作中心类别
        /// </summary>
        public string Category
        {
            get { return this.GetProperty(CategoryProperty); }
            set { this.SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 工作中心编码 Code
        /// <summary>
        /// 工作中心编码
        /// </summary>
        [Label("工作中心编码")]
        public static readonly Property<string> CodeProperty = P<WorkCenter>.Register(e => e.Code);

        /// <summary>
        /// 工作中心编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 工作中心名称 Name
        /// <summary>
        /// 工作中心名称
        /// </summary>
        [Label("工作中心名称")]
        public static readonly Property<string> NameProperty = P<WorkCenter>.Register(e => e.Name);

        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 负责人 Person
        /// <summary>
        /// 负责人
        /// </summary>
        [Label("负责人")]
        public static readonly Property<string> PersonProperty = P<WorkCenter>.Register(e => e.Person);

        /// <summary>
        /// 负责人
        /// </summary>
        public string Person
        {
            get { return this.GetProperty(PersonProperty); }
            set { this.SetProperty(PersonProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<WorkCenter>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

    }

    internal class WorkCenterConfig : EntityConfig<WorkCenter>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(WorkCenter.CodeProperty, new NotDuplicateRule());
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("WORK_CENTRT").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
