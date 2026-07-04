using SIE.Common.Prints;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 任务管理
    /// </summary>
    [Serializable]
    [DisplayName("任务管理")]
    public class TaskManagementPrintable : BillPrintable<TaskManagement>
    {
        /// <summary>
        /// 根据实体类型获取属性
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>对应type的属性</returns>
        public override IEnumerable<string> GetPropertys(Type type = null)
        {
            var propertys = new HashSet<string>(base.GetPropertys(type).ToList());
            switch (type.Name)
            {
                case nameof(TaskManagement):
                    propertys.UnionWith(new List<string> {
                                                            "Ex_FromWarehouseCode",
                                                            "Ex_FromWarehouseName",
                                                            "Ex_FromAreaCode",
                                                            "Ex_ToWarehouseCode",
                                                            "Ex_ToWarehouseName",
                                                            "Ex_LotCode",
                                                            "Ex_ItemCode",
                                                            "Ex_ItemName",
                                                            "Ex_ItemSpecificationModel",
                                                            "Ex_ItemDesc",
                                                            "Ex_ItemUnitName",
                                                            "Ex_SuggestLotCode",
                                                            "Ex_SourceLocCode",
                                                            "Ex_SourceLocName",
                                                            "Ex_TargetLocCode",
                                                            "Ex_TargetLocName",
                                                            "Ex_ActualFromLocCode",
                                                            "Ex_ActualFromLocName",
                                                            "Ex_ActualToLocCode",
                                                            "Ex_ActualToLocName",
                                                            "Ex_TransactionName",
                                                            "Ex_TaskGroupNo",
                                                            "Ex_FromIsAllowManualGrounding",
                                                            "R_TableName",
                                                            "R_CreateBy",
                                                            "R_UpdateBy"
                                                        });
                    break;
                case nameof(Operator):
                case nameof(ActualOperator):
                    propertys.UnionWith(new List<string> {
                                                            "R_EmployeeCode",
                                                            "R_EmployeeName",
                                                            "R_EmployeeGroupName",
                                                            "R_WorkGroupName"
                                                        });
                    break;
            }
            return propertys.ToList();
        }

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data">实体对象</param>
        /// <returns>转换后的数据</returns>
        public override string ConverterData(object data)
        {
            var content = base.ConverterData(data);
            if (data is TaskManagement task)
            {
                var sb = new StringBuilder();

                sb.Append($"{task.FromWarehouse?.Code}{Separator}{task.FromWarehouse?.Name}{Separator}{task.FromArea?.Code}{Separator}{task.ToWarehouse?.Code}{Separator}");
                sb.Append($"{task.ToWarehouse?.Name}{Separator}{task.SuggestLot?.Code}{Separator}{task.Item?.Code}{Separator}{task.Item?.Name}{Separator}");
                sb.Append($"{task.Item?.SpecificationModel}{Separator}{task.Item?.Description}{Separator}{task.Item?.Unit?.Name}{Separator}");
                sb.Append($"{task.SuggestLot?.Code}{Separator}{task.SuggestFromLoc?.Code}{Separator}{task.SuggestFromLoc?.Name}{Separator}");
                sb.Append($"{task.SuggestToLoc?.Code}{Separator}{task.SuggestToLoc?.Name}{Separator}{task.ActualFromLoc?.Code}{Separator}");
                sb.Append($"{task.ActualFromLoc?.Name}{Separator}{task.ActualToLoc?.Code}{Separator}{task.ActualToLoc?.Name}{Separator}");
                sb.Append($"{task.Transaction?.Name}{Separator}{task.TaskGroup?.No}{Separator}{task.SuggestFromLoc?.Area?.IsAllowManualGrounding}{Separator}");

                var meta = RF.Find<TaskManagement>().EntityMeta;
                var tableName = meta.TableMeta.TableName;

                sb.Append($"{tableName}{Separator}{task.CreateBy}{Separator}{task.UpdateBy}{Separator}");

                content += sb.ToString();
            }
            if (data is Operator operat)
            {
                var sb = new StringBuilder();

                sb.Append($"{operat.Employee?.Code}{Separator}{operat.Employee?.Name}{Separator}");
                sb.Append($"{operat.Employee?.EmployeeGroup?.Name}{Separator}{operat.Employee?.WorkGroup?.Name}{Separator}");

                content += sb.ToString();
            }
            if (data is ActualOperator actualOperator)
            {
                var sb = new StringBuilder();

                sb.Append($"{actualOperator.Employee?.Code}{Separator}{actualOperator.Employee?.Name}{Separator}");
                sb.Append($"{actualOperator.Employee?.EmployeeGroup?.Name}{Separator}{actualOperator.Employee?.WorkGroup?.Name}{Separator}");

                content += sb.ToString();
            }
            return content;
        }
    }
}
