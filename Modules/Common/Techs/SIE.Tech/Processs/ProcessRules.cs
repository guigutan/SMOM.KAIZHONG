using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MetaModel;
using SIE.Resources.Employees;
using SIE.Resources.ProcessSegments;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Tech.Processs
{
    #region 工序验证
    /// <summary>
    /// 工序引用验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序引用验证规则")]
    [System.ComponentModel.Description("工序有引用不能删除")]
    public class ProcessDeleteRule : EntityRule<Process>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessDeleteRule()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Delete;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var process = entity as Process;
            if (process.ReferenceTimes > 0)
            {
                e.BrokenDescription = "工序[{0}]有{1}个引用，不能删除！".L10nFormat(process.Name, process.ReferenceTimes);
            }
        }
    }

    /// <summary>
    /// 工序参数验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序参数验证规则")]
    [System.ComponentModel.Description("参数中存在任意的结果，请删除其它结果")]
    public class ProcessResultTypeRule : EntityRule<Process>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessResultTypeRule()
        {
            Property = Process.TypeProperty;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update | EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            List<ProcessParameter> processParameters = new List<ProcessParameter>();
            var process = entity as Process;
            processParameters.AddRange(RT.Service.Resolve<ProcessController>().GetProcessParameters(process.Id, process.ParameterList));
            processParameters = processParameters.Where(p => p.Type != ResultTypeForDesign.Optional).ToList();
            if (processParameters.Count == 0)
                e.BrokenDescription = "工序参数不能为空".L10N();
            if (processParameters.Any(f => f.Type == ResultTypeForDesign.Any) && processParameters.Count > 1)
                e.BrokenDescription = "参数中存在任意的结果，请删除其它结果".L10N();
            var parameterGroup = processParameters.GroupBy(p => p.Type);
            parameterGroup.ForEach(f =>
            {
                if (f.Count() > 1 && f.Key != ResultTypeForDesign.Custom && f.Key != ResultTypeForDesign.Optional)
                    e.BrokenDescription = "结果类型非[{0}]，不能重复".L10nFormat(EnumViewModel.EnumToLabel(ResultTypeForDesign.Custom).L10N());
            });
        }
    }

    /// <summary>
    /// 工序采集步骤参数验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序采集步骤参数验证规则")]
    [System.ComponentModel.Description("采集步骤参数不能为空")]
    public class ProcessCollectStepRule : EntityRule<Process>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessCollectStepRule()
        {
            Property = Process.CollectStepListProperty;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update | EntityStatusScopes.Delete;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            EntityList<ProcessCollectStep> processCollectSteps = new EntityList<ProcessCollectStep>();
            var process = entity as Process;
            processCollectSteps.AddRange(RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(process.Id, process.CollectStepList));
            if (processCollectSteps.Count == 0)
                e.BrokenDescription = "采集步骤参数不能为空".L10N();
            if (process.Type == ProcessType.Rework)
            {
                if (processCollectSteps.Any(p => p.BarcodeType != BarcodeType.SN))
                    e.BrokenDescription = "工序类型为[{0}]，采集步骤必须为[{1}]，且最多只能有两个步骤".L10nFormat(ProcessType.Rework.ToLabel(), BarcodeType.SN.ToLabel());
                return;
            }

            var stepGroup = processCollectSteps.GroupBy(f => f.BarcodeType);
            stepGroup.ForEach(f =>
            {
                if (f.Count() > 1 && f.Key != BarcodeType.BatchBarocde && f.Key != BarcodeType.ContainerNo)
                    e.BrokenDescription = "条码类型[{0}]不能重复".L10nFormat(f.Key.ToLabel());
            });
        }
    }


    /// <summary>
    /// 工序采集步骤参数条码类型验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序采集步骤参数验证规则")]
    [System.ComponentModel.Description("工序采集步骤中不能存在条码类型为空的采集步骤")]
    public class ProcessCollectStepTypeRule : EntityRule<Process>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessCollectStepTypeRule()
        {
            Property = Process.CollectStepListProperty;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            EntityList<ProcessCollectStep> processCollectSteps = new EntityList<ProcessCollectStep>();
            var process = entity as Process;
            processCollectSteps.AddRange(RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(process.Id, process.CollectStepList));
            if (processCollectSteps.Count > 0 && processCollectSteps.Any(p => p.BarcodeType == 0))
            {
                e.BrokenDescription = "采集步骤条码类型不能为空".L10N();
            }
        }
    }

    /// <summary>
    /// 工序参数验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序参数验证规则")]
    [System.ComponentModel.Description("工序参数中存在任意类型的采集结果，请删除其它类型的结果")]
    public class ProcessParameterRule : EntityRule<Process>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessParameterRule()
        {
            Property = Process.ProcessPackingUnitListProperty;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            EntityList<ProcessCollectStep> processCollectSteps = new EntityList<ProcessCollectStep>();
            var process = entity as Process;
            processCollectSteps.AddRange(RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(process.Id, process.CollectStepList));
            if (processCollectSteps.Count > 0 && processCollectSteps.Any(p => p.BarcodeType == 0))
            {
                e.BrokenDescription = "工序参数中存在任意类型的采集结果，请删除其它类型的结果".L10N();
            }
        }
    }

    /// <summary>
    /// 工序采集步骤出入类型验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序采集步骤出入站类型验证")]
    [System.ComponentModel.Description("批次工序的工序采集步骤必须存在入站类型")]
    public class ProcessNeedPlugTypeRule : EntityRule<Process>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessNeedPlugTypeRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
            ConnectToDataSource = true;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            EntityList<ProcessCollectStep> processCollectSteps = new EntityList<ProcessCollectStep>();
            var process = entity as Process;
            if (process == null || e == null)
            {
                return;
            }

            processCollectSteps.AddRange(RT.Service.Resolve<ProcessController>().GetProcessCollectSteps(process.Id, process.CollectStepList));
            if ((process.Type == ProcessType.BatchAssembly || process.Type == ProcessType.BatchFix || process.Type == ProcessType.BatchPacking || process.Type == ProcessType.BatchPqc) && !processCollectSteps.Any(p => p.PlugType == PlugType.In))
            {
                e.BrokenDescription = "[{0}]工序必须存在入站步骤".L10nFormat(EnumViewModel.EnumToLabel(process.Type).L10N());
            }

            if ((process.Type == ProcessType.BatchAssembly || process.Type == ProcessType.BatchPqc) && !processCollectSteps.Any(p => p.PlugType == PlugType.Out))
            {
                e.BrokenDescription = "[{0}]工序必须存在出站步骤".L10nFormat(EnumViewModel.EnumToLabel(process.Type).L10N());
            }

            if (processCollectSteps.Count(p => p.PlugType == PlugType.In) > 1)
            {
                e.BrokenDescription = "出入类型为[{0}]只能有一个".L10nFormat(EnumViewModel.EnumToLabel(PlugType.In).L10N());
            }
            if (processCollectSteps.Count(p => p.PlugType == PlugType.Out) > 1)
            {
                e.BrokenDescription = "出入类型为[{0}]只能有一个".L10nFormat(EnumViewModel.EnumToLabel(PlugType.Out).L10N());
            }
        }
    }
    #endregion

    #region 缺陷代码验证
    /// <summary>
    /// 关联工序不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("缺陷删除规则")]
    [System.ComponentModel.Description("关联工序不能删除")]

    public class DefectReferenceRoutingRule : NoReferencedRule<Defect>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public DefectReferenceRoutingRule()
        {
            Properties.Add(ProcessDefect.DefectIdProperty);
        }
    }
    #endregion

    #region 工序参数验证  
    /// <summary>
    /// 工序参数结果验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序参数验证规则")]
    [System.ComponentModel.Description("结果不能为空")]
    public class ResultTypeForDesignRequireRule : EntityRule<ProcessParameter>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ResultTypeForDesignRequireRule()
        {
            ConnectToDataSource = true;
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var parameter = entity as ProcessParameter;
            if (parameter.Type == 0)
            {
                e.BrokenDescription = "结果不能为空".L10N();
            }

            if (parameter.Type == ResultTypeForDesign.Custom && parameter.Script.IsNullOrEmpty())
            {
                e.BrokenDescription = "结果类型为[{0}]，脚本不能为空".L10nFormat(EnumViewModel.EnumToLabel(parameter.Type).L10N());
            }

            if (parameter.Process.Type != ProcessType.BatchPqc && parameter.Type == ResultTypeForDesign.Custom) ////工序类型为非批次检验，工序参数采集结果不能维护自定义
            {
                e.BrokenDescription = "工序类型为[{0}]，采集结果不能添加[{1}]"
                    .L10nFormat(parameter.Process.Type.ToLabel(), parameter.Type.ToLabel());
            }

            if (parameter.Process.Type == ProcessType.BatchPqc && CheckProcessParameterCustomPassResult(parameter)) ////工序类型为批次检验，工序采集结果不能同时维护自定义和通过
            {
                e.BrokenDescription = "工序类型为[{0}]，采集结果不能同时存在[{1}]和[{2}]"
                    .L10nFormat(ProcessType.BatchPqc.ToLabel(), ResultTypeForDesign.Custom.ToLabel(), ResultTypeForDesign.Pass.ToLabel());
            }
        }

        /// <summary>
        /// 检查工序参数采集结果是否同时维护自定义和通过
        /// </summary>
        /// <param name="parameter">当前工序参数</param>
        /// <returns>bool</returns>
        private bool CheckProcessParameterCustomPassResult(ProcessParameter parameter)
        {
            bool result = false;
            EntityList<ProcessParameter> processParameters = new EntityList<ProcessParameter>();
            processParameters.AddRange(RT.Service.Resolve<ProcessController>().GetProcessParameters(parameter.ProcessId, parameter.Process.ParameterList));
            if (parameter.Type == ResultTypeForDesign.Pass)
            {
                result = processParameters.Any(x => x.Type == ResultTypeForDesign.Custom && x.Id != parameter.Id);
            }
            else if (parameter.Type == ResultTypeForDesign.Custom)
            {
                result = processParameters.Any(x => x.Type == ResultTypeForDesign.Pass && x.Id != parameter.Id);
            }
            else 
            {
                //
            }

            return result;
        }
    }
    #endregion

    #region 采集步骤验证
    ///// <summary>
    ///// 工序采集步骤条码类型验证规则
    ///// </summary>
    ////[System.ComponentModel.DisplayName("工序条码验证规则")]
    ////[System.ComponentModel.Description("条码不能重复")]
    ////public class ProcessParameterBarcodeTypeRule : NotDuplicateRule<ProcessCollectStep>
    ////{
    ////    /// <summary>
    ////    /// 初始化需要验证的属性、影响范围、规则
    ////    /// </summary>
    ////    public ProcessParameterBarcodeTypeRule()
    ////    {
    ////        Properties.Add(ProcessCollectStep.BarcodeTypeProperty);
    ////        Properties.Add(ProcessCollectStep.ProcessIdProperty);
    ////        MessageBuilder = ((o) =>
    ////          {
    ////              var step = o as ProcessCollectStep;
    ////              return "已经存在[条码类型]是{0}条码的采集步骤".L10nFormat(step.BarcodeType);
    ////          });
    ////    }
    ////}

    /// <summary>
    /// 工序采集步骤条码类型验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序采集步骤是否解绑验证")]
    [System.ComponentModel.Description("工序采集步骤只有周转箱条码可以是解绑状态")]
    public class ProcessCollectStepIsUnboundRule : EntityRule<ProcessCollectStep>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessCollectStepIsUnboundRule()
        {
            Scope = EntityStatusScopes.Add | EntityStatusScopes.Update;
        }

        /// <summary>
        /// 添加动态验证规则
        /// </summary>
        /// <param name="entity">验证实体</param>
        /// <param name="e">为业务规则验证方法提供一些必要的参数。</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            var processCollectStep = entity as ProcessCollectStep;
            if (processCollectStep.IsUnbound && processCollectStep.BarcodeType != BarcodeType.TurnoverBox && e != null)
            {
                e.BrokenDescription = "不允许解绑，条码类型是：{0} 。只有：{1} 才可以是解绑状态".L10nFormat(processCollectStep.BarcodeType.ToLabel(), BarcodeType.TurnoverBox.ToLabel());
            }
        }
    }
    #endregion

    #region 工序缺陷验证
    /// <summary>
    /// 工序缺陷验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序缺陷验证规则")]
    [System.ComponentModel.Description("该工序已存在该缺陷代码")]
    public class ProcessDefectNotDuplicateRule : NotDuplicateRule<ProcessDefect>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessDefectNotDuplicateRule()
        {
            Properties.Add(ProcessDefect.ProcessIdProperty);
            Properties.Add(ProcessDefect.DefectIdProperty);
            MessageBuilder = (e) =>
            {
                var model = e as ProcessDefect;
                return "工序【{0}】已存在缺陷代码【{1}】".L10nFormat(model.Process.Name, model.Defect.Code);
            };
        }
    }
    #endregion

    #region 工序员工验证
    /// <summary>
    /// 工序员工验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("工序员工验证规则")]
    [System.ComponentModel.Description("该工序已存在该员工")]
    public class ProcessEmployeeNotDuplicateRule : NotDuplicateRule<ProcessEmployee>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public ProcessEmployeeNotDuplicateRule()
        {
            Properties.Add(ProcessEmployee.ProcessIdProperty);
            Properties.Add(ProcessEmployee.EmployeeIdProperty);
            MessageBuilder = (e) => { return "已存在一条相同的记录，不能重复添加！".L10N(); };
        }
    }
    #endregion

    #region 产品族验证
    /// <summary>
    /// 关联工序不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("产品族验证规则")]
    [System.ComponentModel.Description("关联工序不能删除")]
    public class UndeleteInvolveProductFamily : NoReferencedRule<ProductFamily>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public UndeleteInvolveProductFamily()
        {
            Properties.Add(Process.ProductFamilyIdProperty);
        }
    }
    #endregion

    #region 工段验证
    /// <summary>
    /// 关联工段不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("产品族验证规则")]
    [System.ComponentModel.Description("关联工段不能删除")]
    public class UndeleteInvolveProcessSegment : NoReferencedRule<ProcessSegment>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public UndeleteInvolveProcessSegment()
        {
            Properties.Add(Process.SegmentIdProperty);
        }
    }
    #endregion

    #region 工序员工验证
    /// <summary>
    /// 员工关联工序不能删除
    /// </summary>
    [System.ComponentModel.DisplayName("员工删除校验")]
    [System.ComponentModel.Description("员工关联工序不允许删除")]
    public class EmployeeInvolveProcess : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 初始化需要验证的属性、影响范围、规则
        /// </summary>
        public EmployeeInvolveProcess()
        {
            Properties.Add(ProcessEmployee.EmployeeIdProperty);
            MessageBuilder = (o, e) =>
            {
                var employee = o as Employee;
                return "员工[{0}]已关联工序，不允许删除".L10nFormat(employee.Name);
            };
        }
    }
    #endregion
}