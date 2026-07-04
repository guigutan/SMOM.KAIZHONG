using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Tech.Processs;
using SIE.Tech.Routings;
using SIE.Tech.Routings.Technologys;
using SIE.Tech.Routings.ViewModels;
using SIE.Tech.VictoryStandards;
using SIE.Utils;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SIE.Web.Tech.Routings
{
    /// <summary>
    /// 工艺路线相关 DataQueryer
    /// </summary>
    public class TechDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取工序参数列表
        /// DesignCanvas.js调用
        /// </summary>
        /// <param name="processIds">工序ID集合</param>
        /// <returns>工序参数列表</returns>
        public object GetProcessInfo(List<double> processIds)
        {
            var processParamList = RT.Service.Resolve<ProcessController>().GetProcessParameterByProcessId(processIds.ToArray());
            return new { ProcessParameter = processParamList };
        }

        /// <summary>
        /// 获取物料集合
        /// ProcessBomControl.js调用
        /// </summary>
        /// <param name="itemIds">物料ID集合</param>
        /// <returns>物料集合</returns>
        public object GetBoms(List<double> itemIds)
        {
            Check.NotNull(itemIds, nameof(itemIds));
            var itemList = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
            var itemArray = itemList.Select(p => new
            {
                ItemId = p.Id,
                Code = p.Code,
                Name = p.Name
            });

            return itemArray;
        }

        /// <summary>
        /// 根据分类id获取物料集合
        /// SelectCategoryCommand.js调用
        /// </summary>
        /// <param name="smallCategoryIds">分类ID集合</param>
        /// <returns>物料集合</returns>
        public object GetItemsBySmallCategoryId(List<double> smallCategoryIds)
        {
            Check.NotNull(smallCategoryIds, nameof(smallCategoryIds));

            var itemList = RT.Service.Resolve<ItemController>().GetItemFromCategoryIds(smallCategoryIds);
            var itemArray = itemList.Select(p => new
            {
                ItemId = p.Id,
                Code = p.Code,
                Name = p.Name
            });

            return itemArray;
        }

        /// <summary>
        /// 获取工序信息
        /// ProcessTreeControl.js调用
        /// </summary>
        /// <returns>产品族分类信息</returns>
        public List<FamilyCategoryInfo> GetProcessTreeInfos()
        {
            var categoryList = RT.Service.Resolve<ItemController>().GetProductFamilyCategories();
            var dicFamily = RT.Service.Resolve<ItemController>().GetProductFamilies().GroupBy(p => p.CategoryId).ToDictionary(p => p.Key, f => f.ToList());
            var dicProcess = RT.Service.Resolve<ProcessController>().GetProcess().Where(p => p.ProductFamilyId != null).GroupBy(p => p.ProductFamilyId).ToDictionary(p => p.Key, f => f.ToList());
            List<FamilyCategoryInfo> categories = new List<FamilyCategoryInfo>();
            categoryList.ForEach(category =>
            {
                var info = new FamilyCategoryInfo()
                {
                    Id = category.Id,
                    Code = category.Code,
                    Name = category.Name,
                };
                if (dicFamily.ContainsKey(category.Id))
                {
                    var families = dicFamily[category.Id];
                    families.ForEach(family =>
                    {
                        var familyInfo = new FamilyInfo()
                        {
                            Id = family.Id,
                            Code = family.Code,
                            Name = family.Name
                        };
                        if (dicProcess.ContainsKey(family.Id))
                        {
                            var processList = dicProcess[family.Id];
                            processList.OrderBy(p => p.Name).ForEach(process =>
                            {
                                ProcessInfo processInfo = new ProcessInfo()
                                {
                                    Id = process.Id,
                                    Name = process.Name,
                                    Type = (int)process.Type,
                                    TypeDisplay = (int)process.Type >= (int)ProcessType.BatchAssembly ? process.Type.ToLabel().Replace("批次", "") : process.Type.ToLabel().L10N(),
    
                                    IsBatch = (int)process.Type >= (int)ProcessType.BatchAssembly ? "批".L10N() : "单".L10N(),
                                    ReferenceTime = process.ReferenceTimes,
                                    IsOutsourcing = process.IsOutsourcing,
                                    EnableMoveInControl = process.EnableMoveInControl,
                                    TransferType = (int?)process.TransferType,

                                    //获取工序属性数据
                                    CanChoose = process.CanChoose.HasValue ? process.CanChoose : false,
                                    IsRepeat = process.IsRepeat.HasValue ? process.IsRepeat : false,
                                    IsCreateSku = process.IsCreateSku.HasValue ? process.IsCreateSku : false,
                                    IsCalculate = process.IsCalculate.HasValue ? process.IsCalculate : false,

                                    IsGenerateTask = process.IsGenerateTask.HasValue ? process.IsGenerateTask : false,
                                    IsRequirementTask = process.IsRequirementTask.HasValue ? process.IsRequirementTask : false,

                                    IsBuckleMaterial = process.IsBuckleMaterial.HasValue ? process.IsBuckleMaterial : false,
                                    VictoryStandardId = process.VictoryStandardId,
                                    VictoryStandard_Display = process.VictoryStandardId.HasValue ? $"{process.VictoryStandard?.Name}-{process.VictoryStandard.MaxTestQty}" : "",

                                    RepairVictoryId = process.RepairVictoryId,
                                    RepairVictory_Display = process.RepairVictoryId.HasValue ? $"{process.RepairVictory?.Name}-{process.RepairVictory.MaxTestQty}" : "",
                                    IsStricter = process.IsStricter.HasValue ? process.IsStricter : false,
                                    Overtime = process.Overtime,

                                    IsPassRate = process.IsPassRate.HasValue ? process.IsPassRate : false,
                                    IsBinding = process.IsBinding.HasValue ? process.IsBinding : false,
                                    IsUnBinding = process.IsUnBinding.HasValue ? process.IsUnBinding : false,
                                    MaxPassNum = process.MaxPassNum,
                                    IsNextMoveIn = process.IsNextMoveIn.HasValue ? process.IsNextMoveIn : false,
                                };
                                processInfo.TypeDisplay = processInfo.TypeDisplay.L10N();
                                familyInfo.ProcessList.Add(processInfo);
                            });
                        }
                        info.FamilyList.Add(familyInfo);
                    });
                }
                categories.Add(info);
            });
            return categories;
        }

        /// <summary>
        /// 获取工艺路线信息
        /// RoutingTreeControl.js调用
        /// </summary>
        /// <returns></returns>
        public List<FamilyCategoryInfo> GetRoutingTreeInfos()
        {
            return RT.Service.Resolve<RoutingController>().GetRoutingTreeInfos();
        }

        /// <summary>
        /// 获取树型工艺路线信息
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>树型工艺路线信息</returns>
        public List<FamilyCategoryInfo> GetRoutingTreeInfosByKeyword(string keyword)
        {
            return RT.Service.Resolve<RoutingController>().GetRoutingTreeInfosByKeyword(keyword);
        }

        /// <summary>
        /// 获取工艺路线版本布局
        /// RoutingDesignControl.js调用
        /// </summary>
        /// <param name="layoutId">工艺路线布局id</param>
        /// <returns>布局xml</returns>
        public string GetRoutingLayout(double layoutId)
        {
            var layout = RF.GetById<RoutingLayout>(layoutId)?.Layout;
            return layout;
        }

        /// <summary>
        /// 设置默认工艺路线版本
        /// RoutingTreeControl.js调用
        /// </summary>
        /// <param name="routingId">工艺路线ID</param>
        /// <param name="versionId">默认工艺路线版本id</param>
        public void SetDefault(double routingId, double versionId)
        {
            RT.Service.Resolve<RoutingController>().SetDefaultVersion(routingId, versionId);
        }

        /// <summary>
        /// 粘贴后新增的工艺路线版本
        /// </summary>
        /// <param name="copyInfo">被复制数据</param>
        /// <param name="maxVersionNum">最大版本数</param>
        /// <param name="xml">被复制的xml</param>
        public object PraseRoutingVersion(CopyVersionInfo copyInfo, int maxVersionNum, string xml)
        {
            if (copyInfo.VersionId > 0)
            {
                var version = RF.GetById<RoutingVersion>(copyInfo.VersionId);
                if (version != null)
                {
                    var layout = version.Layout?.Layout;
                    if (!layout.IsNullOrWhiteSpace())
                        CheckProcessChanged(layout);
                    var containerModel = LoadFromXmlString(layout, true);
                    ProcessCopyRoutingVersion(containerModel, copyInfo);
                    containerModel.RoutingId = copyInfo.RoutingId;
                    var newLayout = containerModel.Serialize();
                    var routVersionInfo = RT.Service.Resolve<RoutingController>().GetPraseRoutingVersionInfo(copyInfo.RoutingId, maxVersionNum, xml);
                    return new
                    {
                        layout = newLayout,
                        Version = new
                        {
                            Id = 0,
                            text = routVersionInfo.Text,
                            leaf = routVersionInfo.Leaf,
                            nodetype = routVersionInfo.Nodetype,
                            routingId = routVersionInfo.RoutingId,
                            state = routVersionInfo.State,
                            isDefault = routVersionInfo.IsDefault,
                            versionName = routVersionInfo.VersionName,
                            targetRoutingId = routVersionInfo.TargetRoutingId,
                            isCopy = routVersionInfo.IsCopy,
                        },
                    };
                }
            }
            return new
            {
                layout = string.Empty,
                Version = string.Empty,
            };
        }

        /// <summary>
        /// 校验工序是否发生变更
        /// </summary>
        /// <param name="xml">工艺路线版本</param>
        void CheckProcessChanged(string xml)
        {
            var containerModel = new ContainerModel();
            containerModel.Deserialize(xml);
            foreach (var activity in containerModel.Activitys.Where(p => p.ProcessId > 0))
            {
                var process = RF.GetById<Process>(activity.ProcessId);
                if (process == null)
                {
                    throw new ValidationException("{0} 工序已删除，不能复制".L10nFormat(activity.Text));
                }

                if (process.Name != activity.Text)
                {
                    throw new ValidationException("{0} 工序已改名为：{1}，不能复制".L10nFormat(activity.Text, process.Name));
                }

                var rules = containerModel.Rules.Where(p => p.SourceActivityId == activity.Id);
                foreach (var rule in rules)
                {
                    var parameter = process.ParameterList.FirstOrDefault(p => p.Id == rule.ParameterId);
                    if (parameter != null && (parameter.Type.ToLabel() == rule.Text || parameter.Description == rule.Text))
                    {
                        continue;
                    }

                    throw new ValidationException("{0} 工序参数已发生变化，不能复制".L10nFormat(process.Name));
                }
            }
        }

        /// <summary>
        /// 加载xml到容器
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="isCopying">复制命令触发时为true</param>
        public ContainerModel LoadFromXmlString(string xml, bool isCopying = false)
        {
            var containerModel = new ContainerModel();
            if (xml.IsNullOrWhiteSpace())
            {
                CreateModel(containerModel);
                return containerModel;
            }

            try
            {
                containerModel.Deserialize(xml, isCopying);
                containerModel.RoutingVersionId = 0;
            }
            catch (Exception exc)
            {
                throw new ValidationException("打开流程失败".L10N() + exc.Message);
            }
            finally
            {
                // InitGridLines();
            }
            return containerModel;
        }

        /// <summary>
        /// 创建新的版本
        /// </summary>
        /// <param name="model">设计容器</param>
        private void CreateModel(ContainerModel model)
        {
            var beginActivityModel = new ActivityModel();
            beginActivityModel.Type = ActivityType.Initial;
            beginActivityModel.Text = "开始".L10N();

            var rule = new RuleModel();
            rule.SourceActivityId = beginActivityModel.Id;
            beginActivityModel.Rules.Add(rule);
            beginActivityModel.SetPoint(new SIE.Tech.Routings.Technologys.Point(400, 126));

            model.Rules.Add(rule);
            model.AddChild(beginActivityModel);

            var endActivityModel = new ActivityModel();
            endActivityModel.Type = ActivityType.Completion;
            endActivityModel.Text = "结束".L10N();

            endActivityModel.SetPoint(new Point(400, 500));
            model.AddChild(endActivityModel);
        }

        /// <summary>
        /// 根据复制选项处理复制的工艺路线版本
        /// </summary>      
        private void ProcessCopyRoutingVersion(ContainerModel model, CopyVersionInfo copyInfo)
        {
            if (model == null) return;
            var activitys = model.Activitys;
            if (activitys != null && activitys.Count > 0)
            {
                foreach (var activityModel in activitys)
                {
                    if (!copyInfo.IsCopyBom)
                        activityModel.Bom.Clear();
                    if (!copyInfo.IsCopyActivityProperty)
                    {
                        activityModel.IsOptional = false;
                        activityModel.IsRepeat = false;
                        activityModel.CreateSku = false;
                        activityModel.IsCalculate = false;
                        activityModel.IsGenerateTask = false;
                        activityModel.IsRequirementTask = false;
                        
                        activityModel.IsBuckleMaterial = false;
                        activityModel.IsPassRate = false;
                        activityModel.IsBinding = false;
                        activityModel.IsUnBinding = false;
                        activityModel.StartProcess = null;
                        activityModel.NormalVictory = null;
                        activityModel.RepairVictory = null;
                        activityModel.IsStricter = false;
                        activityModel.Overtime = null;
                        activityModel.MaxPassNum = null;
                        activityModel.IsNextMoveIn = false;
                    }
                    if (!copyInfo.IsCopyFixture)
                        activityModel.Fixtures.Clear();
                }
            }
        }

        /// <summary>
        /// 获取工序
        /// </summary>
        /// <param name="processId">工序Id</param>
        /// <returns>工序</returns>
        public Process GetProcessById(double processId)
        {
            return RF.GetById<Process>(processId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取新增工艺路线信息
        /// </summary>
        /// <param name="maxVersionNum">当前最大版本数</param>
        /// <returns>新增工艺路线信息</returns>
        public object GetAddRoutingVersionInfo(int maxVersionNum)
        {
            var model = new ContainerModel();
            var beginActivityModel = new ActivityModel();
            beginActivityModel.Type = ActivityType.Initial;
            beginActivityModel.Text = "开始".L10N();

            var rule = new RuleModel();
            rule.SourceActivityId = beginActivityModel.Id;
            beginActivityModel.Rules.Add(rule);
            beginActivityModel.SetPoint(new Point(400, 126));

            model.Rules.Add(rule);
            model.AddChild(beginActivityModel);

            var endActivityModel = new ActivityModel();
            endActivityModel.Type = ActivityType.Completion;
            endActivityModel.Text = "结束".L10N();

            endActivityModel.SetPoint(new Point(400, 500));
            model.AddChild(endActivityModel);

            var routVersionInfo = RT.Service.Resolve<RoutingController>().GetAddRoutingVersionInfo(maxVersionNum);
            return new
            {
                Layout = model.Serialize(),
                Version = new
                {
                    Id = 0,
                    text = routVersionInfo.Text,
                    leaf = routVersionInfo.Leaf,
                    nodetype = routVersionInfo.Nodetype,
                    state = routVersionInfo.State,
                    isNew = routVersionInfo.IsNew,
                    versionName = routVersionInfo.VersionName,
                },
            };
        }

        /// <summary>
        /// 获取胜制方案
        /// </summary>
        /// <returns>胜制方案列表</returns>
        public object GetVictoryStandards()
        {
            var victories = RT.Service.Resolve<VictoryStandardController>().GetEnableVictoryStandards();
            return victories.Select(p => new
            {
                Id = p.Id,
                Display = $"{p.Name}-{p.MaxTestQty}"
            });
        }

        /// <summary>
        /// 获取工序交接类型
        /// </summary>
        /// <returns>工序交接类型</returns>
        public object GetTransferTypes()
        {
            var list = EnumViewModel.GetByEnumType(typeof(TransferType));
            var transferTypeArray = new object[list.Count + 1];
            transferTypeArray[0] = new
            {
                Id = -1,
                Display = "\u3000"
            };
            for (var i = 1; i <= list.Count; i++)
            {
                transferTypeArray[i] = new
                {
                    Id = Convert.ToInt32(list[i - 1].EnumValue),
                    Display = list[i - 1].Label
                };
            }
            return transferTypeArray;
        }
    }
}