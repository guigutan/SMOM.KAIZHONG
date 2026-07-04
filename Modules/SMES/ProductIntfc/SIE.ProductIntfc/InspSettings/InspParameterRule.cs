using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ProductIntfc.InspRecords;
using SIE.Utils;
using System;
using System.ComponentModel;

namespace SIE.ProductIntfc.InspSettings
{
    /// <summary>
    /// 报检参数属性验证规则
    /// </summary>
    [DisplayName("报检参数属性验证规则")]
    [Description("报检参数必须大于0")]
    public class InspParmRule : PropertyRule<InspParameter>
    {
        /// <summary>
        /// 验证属性
        /// </summary>
        protected override IManagedProperty Property
        {
            get
            {
                return InspParameter.InspParmProperty;
            }
        }

        /// <summary>
        /// 验证属性
        /// </summary>
        /// <param name="entity">报检参数</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var parameter = entity as InspParameter;
            if (parameter.InspParm <= 0)
                e.BrokenDescription = "报检参数必须大于0".L10N();
            if (parameter.ProcessType == InspProcess.Custom && parameter.InspProcessId == null)
                e.BrokenDescription = "工序类型为自定义工序时，报检工序不能为空".L10N();
        }
    }

    /// <summary>
    /// 报检参数：【产品编码+报检类型+工序类型+报检工序】非重复验证
    /// </summary>
    [System.ComponentModel.DisplayName("报检参数非重复验证")]
    [System.ComponentModel.Description("报检参数：【产品编码+报检类型+工序类型+报检工序】非重复验证")]
    public class InspParameterGeneralRules : EntityRule<InspParameter>
    {
        /// <summary>
        /// 重写实体规则验证方法
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">规则参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var param = entity as InspParameter;
            if (param.InspType == InspType.FirstProduct)
            {
                var isExist = RT.Service.Resolve<InspParameterController>().ExistSameInspParameter(param);
                if (isExist)
                    e.BrokenDescription = "报检参数：产品编码[{0}]、报检类型[{1}]、工序类型[{2}]、报检工序[{3}]已存在，不能重复添加".L10nFormat(param.Product?.Code, param.InspType.ToLabel().L10N(), param.ProcessType.ToLabel().L10N(), param.InspProcess?.Name);
            }
            else 
            {
                if (param.ProductId == null)//通用产品
                {
                    var isExist = RT.Service.Resolve<InspParameterController>().ExistGeneralParam(param.Id, param.InspType);
                    if (isExist)
                        e.BrokenDescription = "报检参数：【通用产品】和【报检类型】不能重复".L10N();
                }
                else//非通用产品
                {
                    var isExist = RT.Service.Resolve<InspParameterController>().ExistOtherParam(param.InspType, param.ProductId.Value, param.Id);
                    if (isExist)
                        e.BrokenDescription = "已存在产品编号[{0}]、报检类型[{1}]的报检参数".L10nFormat(param.Product?.Code, param.InspType.ToLabel().L10N());
                }
            }
            
        }
    }

    /// <summary>
    /// 报检参数被报检记录引用不能删除
    /// </summary>
    [DisplayName("报检参数被报检记录引用不能删除")]
    [Description("报检参数被报检记录引用不能删除")]
    public class InspParameterReferenceRule : EntityRule<InspParameter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InspParameterReferenceRule()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="entity">报检参数</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var count = RT.Service.Resolve<InspRecordController>().GetInspRecordCount((entity as InspParameter).Id);
            if (count > 0)
                e.BrokenDescription = "不允许删除，报检参数被报检记录引用{0}次".L10nFormat(count);
        }
    }
}