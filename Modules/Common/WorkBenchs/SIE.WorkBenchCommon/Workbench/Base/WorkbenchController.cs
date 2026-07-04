using SIE.Common.Platform;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Mda.Metadatas.Modules;
using SIE.Rbac.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 工作台控制器
    /// </summary>
    public class WorkbenchController : DomainController
    {
        /// <summary>
        /// 工作台定义
        /// </summary>
        public static readonly string ModuleKeyLabel = "#工作台定义.";
        /// <summary>
        /// 根据编码获取工作台
        /// </summary>
        /// <param name="code">工作台编码</param>
        /// <returns></returns>
        public virtual WorkbenchDefinition GetWorkbenchByCode(string code)
        {
            Check.NotNullOrWhiteSpace(code,nameof(code));

            return Query<WorkbenchDefinition>().Where(p => p.Code == code).FirstOrDefault();
        }

        /// <summary>
        /// 根据布局编码获取布局实体
        /// </summary>
        /// <param name="code">布局编码</param>
        /// <returns></returns>
        public virtual LayoutInfo GetLayoutByCode(string code)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));

            var q = Query<LayoutInfo>().Where(p => p.Code == code);

            return q.FirstOrDefault();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="queryKey">查询关键字</param>
        /// <param name="pagingInfo">翻页对像</param>
        /// <returns></returns>
        public virtual EntityList<LayoutInfo> GetLayoutList(string queryKey,PagingInfo pagingInfo)
        {
            var q = Query<LayoutInfo>();

            if (!queryKey.IsNullOrWhiteSpace())
                q.Where(p => p.Code == queryKey || p.Description == queryKey);

            return q.ToList(pagingInfo);
        }

        /// <summary>
        /// 添加工作台定义
        /// </summary>
        /// <param name="vmodel"></param>
        public virtual void InsertWorkbenchDefinition(WorkbenchViewModel vmodel)
        {

            WorkbenchDefinition workbenchDefinition = new WorkbenchDefinition();
            workbenchDefinition.GenerateId();
            workbenchDefinition.PersistenceStatus = PersistenceStatus.New;

            workbenchDefinition.Code = vmodel.Code;
            workbenchDefinition.Name = vmodel.Name;
            workbenchDefinition.Description = vmodel.Description;
            workbenchDefinition.LayoutCode = vmodel.LayoutCode;
            workbenchDefinition.ComponentContent = vmodel.ComponentContent;

            RF.Save(workbenchDefinition);
        }

        /// <summary>
        /// 添加界面更新工作台定义
        /// </summary>
        /// <param name="vmodel"></param>
        public virtual void AddUpdateWorkbenchDefinition(WorkbenchViewModel vmodel)
        {
            WorkbenchDefinition workbenchDefinition = null;
            workbenchDefinition = Query<WorkbenchDefinition>().Where(p => p.Code == vmodel.Code).FirstOrDefault();
            workbenchDefinition.ComponentContent = vmodel.ComponentContent;
            workbenchDefinition.PersistenceStatus = PersistenceStatus.Modified;
            RF.Save(workbenchDefinition);
        }

        /// <summary>
        /// 更新工作台定义
        /// </summary>
        /// <param name="vmodel"></param>
        public virtual void UpdateWorkbenchDefinition(WorkbenchViewModel vmodel)
        {
            WorkbenchDefinition workbenchDefinition = null;
            workbenchDefinition = Query<WorkbenchDefinition>().Where(p => p.Code == vmodel.Code).FirstOrDefault();
            workbenchDefinition.LayoutCode = vmodel.LayoutCode;
            workbenchDefinition.ComponentContent = vmodel.ComponentContent;
            workbenchDefinition.PersistenceStatus = PersistenceStatus.Modified;
            RF.Save(workbenchDefinition);
        }

        /// <summary>
        /// 根据组件编码集合获取组件信息
        /// </summary>
        /// <param name="codes">组件编码集合</param>
        /// <returns></returns>
        public virtual EntityList<ComponentInfo> GetComponentInfoList(List<string> codes)
        {
            Check.NotNull(codes, nameof(codes));

            if (codes.Count == 0)
                return new EntityList<ComponentInfo>();


            return Query<ComponentInfo>().Where(p => codes.Contains(p.Code)).ToList();
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public virtual ComponentInfo GetComponentInfo(string code)
        {
            Check.NotNullOrWhiteSpace(code, nameof(code));

            return Query<ComponentInfo>().Where(p => p.Code== code).FirstOrDefault();
        }

        /// <summary>
        /// 工作台同步到模块定义
        /// </summary>
        /// <param name="workbenchCode">工作台编码</param>
        public virtual void PublishWorkbench(string workbenchCode)
        {

            var workbench = GetWorkbenchByCode(workbenchCode);
            if (workbench == null)
            {
                throw new ValidationException("不存在编码为[{0}]的工作台".L10nFormat(workbenchCode));
            }

            var moduleKey = "{0}{1}".FormatArgs(WorkbenchController.ModuleKeyLabel, workbench.Code);

            var moduleConfig = RT.Service.Resolve<ModuleConfigController>().GetModuleConfig(moduleKey) ?? CreateModuleConfig(moduleKey);

                moduleConfig.Platform = Platform.Web;
                moduleConfig.IsInside = false;
                moduleConfig.Label = workbench.Name;
             
                var webViewUrl = "/WorkBench/ViewWorkBench?code={0}".FormatArgs(workbench.Code);
                moduleConfig.CustomUrl = "http://#domainName#{0}".FormatArgs(webViewUrl);

                 RF.Save(moduleConfig); //一个模块提交一次。


        }

        private ModuleConfig CreateModuleConfig(string moduleKey)
        {
            var m = new ModuleConfig
            {
                Key = moduleKey,
            };
            m.GenerateId();
            return m;
        }

        /// <summary>
        /// 取消发布
        /// </summary>
        /// <param name="workbenchCode">工作台编码</param>
        public virtual void UnPublishWorkbench(string workbenchCode)
        {
            var workbench = GetWorkbenchByCode(workbenchCode);
            if (workbench == null)
                throw new ValidationException("不存在编码为[{0}]的工作台".L10nFormat(workbenchCode));

            var moduleKey = "{0}{1}".FormatArgs(WorkbenchController.ModuleKeyLabel, workbench.Code);
            var menu = Query<Menu>().Where(p => p.ModuleKey == moduleKey).FirstOrDefault();
            var moduleConfig = RT.Service.Resolve<ModuleConfigController>().GetModuleConfig(moduleKey);
            if(moduleConfig!=null)
            {
                moduleConfig.PersistenceStatus = PersistenceStatus.Deleted;

                RF.Save(moduleConfig);
            }

            if(menu!=null)
            {
                menu.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(menu);
            }
        }

        /// <summary>
        /// 布局是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual LayoutInfo ExitLayout(string code)
        {
            return Query<LayoutInfo>().Where(p => p.Code == code).FirstOrDefault();
        }
    }

}
