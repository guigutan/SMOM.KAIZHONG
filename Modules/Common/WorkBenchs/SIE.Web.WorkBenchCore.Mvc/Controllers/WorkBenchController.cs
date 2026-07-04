using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.WorkBenchCommon.Workbench.Base;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SIE.Web.WorkBenchCore.Mvc.Controllers
{
    //[Authorize]
    public class WorkBenchController : Controller
    {
        WorkBenchCommon.Workbench.Base.WorkbenchController workcontroller = RT.Service.Resolve<WorkBenchCommon.Workbench.Base.WorkbenchController>();
        // GET: /<controller>/
        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult demo()
        {
            return View("/Views/Demo/Index.cshtml");
        }

        /// <summary>
        /// 布局预览
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public object LayoutPreview(string content)
        {
            if (content.IsNullOrEmpty())
            {
                ViewBag.Content = "";
            }
            else
            {
                byte[] tmplByte = Convert.FromBase64String(content.Substring(content.IndexOf(',') + 1));
                string jscontent = System.Text.Encoding.UTF8.GetString(tmplByte);
                ViewBag.Content = jscontent;
            }
            return View("/Views/WorkBench/LayoutPreview.cshtml");
        }

        /// <summary>
        /// 组件运行预览
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public object RunComponentPreview(string content)
        {
            if (content.IsNullOrEmpty())
            {
                ViewBag.Content = "";
            }
            else
            {
                byte[] tmplByte = Convert.FromBase64String(content.Substring(content.IndexOf(',') + 1));
                string jscontent = System.Text.Encoding.UTF8.GetString(tmplByte);
                ViewBag.Content = jscontent;
            }
            return View("/Views/WorkBench/RunComponentPreview.cshtml");
        }

        /// <summary>
        /// 组件预览
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public object ComponentPreview(string code)
        {

            var componentInfo = workcontroller.GetComponentInfo(code);
            if (componentInfo == null)
                throw new Exception("获取组件信息失败".L10N());


            ViewBag.Code = componentInfo.Code;
            ViewBag.Content = componentInfo.Content;
            return View("/Views/WorkBench/ComponentPreview.cshtml");
        }

        /// <summary>
        /// 添加工作台设计
        /// </summary>
        /// <param name="code">工作台编码</param>
        /// <param name="name">工作台名称</param>
        /// <param name="desc">工作台描述</param>
        /// <param name="layoutCode">布局编码</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public object AddWorkBench(string code,string name,string desc,string layoutCode,string token)
        {

            WorkbenchViewModel model = new WorkbenchViewModel();
            model.Code = code;
            model.Name = name;
            model.Description = desc;
            model.LayoutCode = layoutCode;
            model.Token = token;

            //ViewBag.WorkbenchCode = code;

            //ViewBag.WorkbenchName = name;

            //ViewBag.WorkbenchDesc = desc;
            
            //ViewBag.WorkbenchLayout = layoutCode;

            //ViewBag.Token = token;
            var layoutInfo=workcontroller.GetLayoutByCode(layoutCode);
            ViewBag.Content = layoutInfo.Content;
            return View("/Views/WorkBench/AddWorkBench.cshtml", model);
        }

        public object DesignWorkBench(string code,string layoutCode,string token)
        {
            var workbenchInfo = workcontroller.GetWorkbenchByCode(code);
            LayoutInfo layoutInfo = null;
            if(workbenchInfo.LayoutCode==layoutCode)
            {
                layoutInfo = workcontroller.GetLayoutByCode(workbenchInfo.LayoutCode); 
            }
            else
            {
                layoutInfo = workcontroller.GetLayoutByCode(layoutCode);
            }
            if (layoutInfo == null)
                throw new ValidationException("[布局管理]未查询到布局编码[{0}],".L10nFormat(layoutCode));
            string layoutContent = layoutInfo.Content;

            WorkbenchViewModel model = new WorkbenchViewModel();
            model.Code = workbenchInfo.Code;
            model.Name = workbenchInfo.Name;
            model.Description = workbenchInfo.Description;
            model.LayoutCode = layoutCode;
            model.ComponentContent = workbenchInfo.ComponentContent;
            model.Token = token;
            //反序列化组件绑定JSON
            var panelBindData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ComponentBindViewModel>>(workbenchInfo.ComponentContent);
            var componentCodes = panelBindData.Values.Select(p => p.ComponentCode).ToList();

            EntityList<ComponentInfo> componentInfoList = workcontroller.GetComponentInfoList(componentCodes);

            //组织前台组件JS脚本
            StringBuilder scriptBulider = new StringBuilder();
            foreach (var component in componentInfoList)
            {
                scriptBulider.Append(component.Content);
                scriptBulider.Append(Environment.NewLine);
            }
            model.ComponentScript = scriptBulider.ToString();


            ViewBag.Content = ProcessLayoutContent(layoutContent, panelBindData);
           
            return View("/Views/WorkBench/DesignWorkBench.cshtml", model);

        }

        public object ViewWorkBench(string code)
        {
            var workbenchInfo = workcontroller.GetWorkbenchByCode(code);
            var layoutInfo = workcontroller.GetLayoutByCode(workbenchInfo.LayoutCode);
            string layoutContent = layoutInfo.Content;

            WorkbenchViewModel model = new WorkbenchViewModel();
            model.Code = workbenchInfo.Code;
            model.Name = workbenchInfo.Name;
            model.Description = workbenchInfo.Description;
            model.LayoutCode = workbenchInfo.LayoutCode;
            model.ComponentContent = workbenchInfo.ComponentContent;

            //反序列化组件绑定JSON
            var panelBindData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ComponentBindViewModel>>(workbenchInfo.ComponentContent);
            var componentCodes = panelBindData.Values.Select(p => p.ComponentCode).ToList();

            EntityList<ComponentInfo> componentInfoList= workcontroller.GetComponentInfoList(componentCodes);

            //组织前台组件JS脚本
            StringBuilder scriptBulider = new StringBuilder();
            foreach(var component in componentInfoList)
            {
                scriptBulider.Append(component.Content);
                scriptBulider.Append(Environment.NewLine);
            }
            model.ComponentScript = scriptBulider.ToString();


            ViewBag.Content = ProcessLayoutContent(layoutContent, panelBindData);
            return View("/Views/WorkBench/ViewWorkBench.cshtml", model);
        }

        /// <summary>
        /// 处理布局内容
        /// </summary>
        /// <param name="content">布局</param>
        /// <param name="panelBindData">面板绑定数据</param>
        /// <returns></returns>
        private string ProcessLayoutContent(string content, Dictionary<string, ComponentBindViewModel> panelBindData)
        {
            string layoutContent = content;
            //匹配布局内容用于替换布局人容
            Regex partRegex = new Regex(@"{(\s*(\S+\s*:\s*['""]?.+?['""]?\s*)+\s*)},?", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex titleRegex = new Regex(@"title\s*:\s*['""](.+?)['""]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex xtypeRegex = new Regex(@"xtype\s*:\s*['""](.+?)['""]", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            MatchCollection matchCollection = partRegex.Matches(layoutContent);
            foreach (Match match in matchCollection)
            {
                string part = match.Groups[1].Value.ToString();
                Match titleMath = titleRegex.Match(part);
                string title = "";
                string titleValue = "";
                if (titleMath.Success)
                {
                    title = titleMath.Groups[0].Value.ToString();
                    titleValue = titleMath.Groups[1].Value.ToString();
                }

                Match xtypeMatch = xtypeRegex.Match(part);
                string xtype = "";
                string xtypeValue = "";
                if (xtypeMatch.Success)
                {
                    xtype = xtypeMatch.Groups[0].Value.ToString();
                    xtypeValue = xtypeMatch.Groups[1].Value.ToString();
                }

                string newpart = "";
                if (panelBindData.ContainsKey(titleValue))
                {
                    ComponentBindViewModel bindVM = panelBindData[titleValue];

                    newpart = part.Replace(xtype, xtype.Replace(xtypeValue, bindVM.ComponentCode));
                    newpart = newpart.TrimEnd(',');

                    //处理绑定参数
                    StringBuilder bindOutPutparam = new StringBuilder();
                    bindOutPutparam.Append(",bindOutPutParam: new Map([");
                    int cnt = 0;
                    foreach (ParameterBindViewModel param in bindVM.BindOutputparam)
                    {
                        if (cnt != 0)
                        {
                            bindOutPutparam.Append(",");
                        }
                        cnt++;
                        bindOutPutparam.Append(string.Format("[\"{0}\",\"{1}\"]", param.OutputParam, param.InputParam));
                        bindOutPutparam.Append(Environment.NewLine);
                    }
                    bindOutPutparam.Append("])");
                    bindOutPutparam.Append(Environment.NewLine);
                    bindOutPutparam.Append(",refreshInterval:{0}".FormatArgs(bindVM.RefreshInterval));

                    newpart += bindOutPutparam.ToString();
                }
                if (!newpart.IsNullOrWhiteSpace())
                    layoutContent = layoutContent.Replace(part, newpart);
            }

            return layoutContent;
        }
    }
}
