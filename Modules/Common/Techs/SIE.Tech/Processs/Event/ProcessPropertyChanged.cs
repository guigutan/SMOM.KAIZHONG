using SIE.Utils;
using System;

namespace SIE.Tech.Processs.Event
{
    /// <summary>
    /// 工序属性变更事件
    /// </summary>
    public class ProcessPropertyChanged
    {
        /// <summary>
        /// 监听工序类型改变事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">变更事件参数</param>
        public void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Process.TypeProperty.Name)
            {
                SaveProcessTypePropertyChanged(sender as Process);
            }
        }

        /// <summary>
        /// 工序添加时type变更事件
        /// </summary>
        /// <param name="process">工序</param>
        protected virtual void SaveProcessTypePropertyChanged(Process process)
        {
            process.ParameterList.Clear();
            process.CollectStepList.Clear();
            process.DefectList.Clear();
            switch (process.Type)
            {
                case ProcessType.Pqc:
                //case ProcessType.Fqc:
                //    process.ParameterList.Add(CreateProcessParameter(ResultTypeForDesign.Pass));
                //    process.ParameterList.Add(CreateProcessParameter(ResultTypeForDesign.Fail));
                //    break;
                case ProcessType.Fix:
                case ProcessType.BatchFix:
                    process.ParameterList.Add(CreateProcessParameter(ResultTypeForDesign.Pass));
                    break;
                case ProcessType.Assembly:
                case ProcessType.BatchAssembly:
                case ProcessType.Rework:
                    process.ParameterList.Add(CreateProcessParameter(ResultTypeForDesign.Any));
                    break;
                case ProcessType.Packing:
                case ProcessType.BatchPacking:
                    process.ParameterList.Add(CreateProcessParameter(ResultTypeForDesign.Pass));
                    break;
                case ProcessType.BatchPqc:
                    process.ParameterList.Add(CreateProcessParameter(ResultTypeForDesign.Custom));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 创建工序参数数据
        /// </summary>
        /// <param name="resultTypeForDesign">采集结果</param>  
        /// <returns>返回工序参数对象</returns>
        ProcessParameter CreateProcessParameter(ResultTypeForDesign resultTypeForDesign)
        {
            return new ProcessParameter()
            {
                Type = resultTypeForDesign,
                Description = resultTypeForDesign == ResultTypeForDesign.Custom ? string.Empty : EnumViewModel.EnumToLabel(resultTypeForDesign).L10N(),
            };
        }
    }
}