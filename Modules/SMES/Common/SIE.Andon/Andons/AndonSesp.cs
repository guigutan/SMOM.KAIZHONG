using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.Andon;
using SIE.MES.EmpWork;
using SIE.MES.Fixture;
using SIE.MES.ItemChecker;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons
{
    /// <summary>
    /// 安灯责任清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("安灯责任清单")]
    public class AndonSesp:DataEntity
    {
        #region 快码
        /// <summary>
        /// 级别
        /// </summary>
        public const string LevelCatalogType = "ANDONSESP_LEVEL";

        #endregion

        #region 安灯维护 Andon
        /// <summary>
        /// 安灯维护Id
        /// </summary>
        [Label("安灯维护")]
        public static readonly IRefIdProperty AndonIdProperty =
            P<AndonSesp>.RegisterRefId(e => e.AndonId, ReferenceType.Parent);

        /// <summary>
        /// 安灯维护Id
        /// </summary>
        public double AndonId
        {
            get { return (double)this.GetRefId(AndonIdProperty); }
            set { this.SetRefId(AndonIdProperty, value); }
        }

        /// <summary>
        /// 安灯维护
        /// </summary>
        public static readonly RefEntityProperty<Andon> AndonProperty =
            P<AndonSesp>.RegisterRef(e => e.Andon, AndonIdProperty);

        /// <summary>
        /// 安灯维护
        /// </summary>
        public Andon Andon
        {
            get { return this.GetRefEntity(AndonProperty); }
            set { this.SetRefEntity(AndonProperty, value); }
        }
        #endregion

        #region 安灯区域 AndonUphold
        /// <summary>
        /// 安灯区域Id
        /// </summary>
        [Label("安灯区域")]
        public static readonly IRefIdProperty AndonUpholdIdProperty = P<AndonSesp>.RegisterRefId(e => e.AndonUpholdId, ReferenceType.Normal);

        /// <summary>
        /// 安灯区域Id
        /// </summary>
        public double? AndonUpholdId
        {
            get { return (double?)GetRefNullableId(AndonUpholdIdProperty); }
            set { SetRefNullableId(AndonUpholdIdProperty, value); }
        }

        /// <summary>
        /// 安灯区域
        /// </summary>
        public static readonly RefEntityProperty<AndonUphold> AndonUpholdProperty = P<AndonSesp>.RegisterRef(e => e.AndonUphold, AndonUpholdIdProperty);

        /// <summary>
        /// 安灯区域
        /// </summary>
        public AndonUphold AndonUphold
        {
            get { return GetRefEntity(AndonUpholdProperty); }
            set { SetRefEntity(AndonUpholdProperty, value); }
        }
        #endregion

        #region 责任人 Employee
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Label("责任人")]
        public static readonly IRefIdProperty EmployeeIdProperty = P<AndonSesp>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 责任人Id
        /// </summary>
        public double EmployeeId
        {
            get { return (double)GetRefNullableId(EmployeeIdProperty); }
            set { SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty = P<AndonSesp>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 责任人
        /// </summary>
        public Employee Employee
        {
            get { return GetRefEntity(EmployeeProperty); }
            set { SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 级别 AndonLevel
        /// <summary>
        /// 级别
        /// </summary>
        [Required]
        [Label("级别")]
        public static readonly Property<string> AndonLevelProperty = P<AndonSesp>.Register(e => e.AndonLevel);

        /// <summary>
        /// 级别
        /// </summary>
        public string AndonLevel
        {
            get { return this.GetProperty(AndonLevelProperty); }
            set { this.SetProperty(AndonLevelProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 责任清单
    /// </summary>
    public class QTPushObjectConfig : EntityConfig<AndonSesp>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(new NotDuplicateRule()
            {
                Properties =
                {
                    AndonSesp.EmployeeIdProperty,
                    AndonSesp.AndonUpholdIdProperty,
                    AndonSesp.AndonIdProperty
                },
                MessageBuilder = (e) =>
                {
                    return "数据已存在!".L10N();
                }
            });
            base.AddValidations(rules);
        }
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ANDON_SESP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 员工被安灯清单引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("员工被安灯清单引用不允许删除")]
    [System.ComponentModel.Description("员工被安灯清单引用不允许删除")]
    public partial class UndeleteAndonSespEmployee : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteAndonSespEmployee()
        {
            Properties.Add(AndonSesp.EmployeeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var process = o as Process;
                return "员工[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.Code, "安灯清单".L10N());
            };
        }
    }

    /// <summary>
    /// 安灯区域被安灯清单引用不允许删除
    /// </summary>
    [System.ComponentModel.DisplayName("安灯区域被安灯清单引用不允许删除")]
    [System.ComponentModel.Description("安灯区域被安灯清单引用不允许删除")]
    public partial class UndeleteAndonSespAndonUphold : NoReferencedRule<AndonUphold>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UndeleteAndonSespAndonUphold()
        {
            Properties.Add(AndonSesp.AndonUpholdIdProperty);
            MessageBuilder = (o, e) =>
            {
                var process = o as AndonUphold;
                return "安灯区域[{0}]已经被[{1}]引用,不能删除".L10nFormat(process.AndonDesc, "安灯清单".L10N());
            };
        }
    }
}
