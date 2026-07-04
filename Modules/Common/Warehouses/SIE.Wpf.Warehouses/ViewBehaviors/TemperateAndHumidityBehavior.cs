using SIE.Domain.Validation;
using SIE.Warehouses;
using System;

namespace SIE.Wpf.Warehouses.ViewBehaviors
{
    /// <summary>
    /// 库位仓储资料温度变更行为
    /// </summary>
    public class TemperateAndHumidityBehavior : ViewBehavior
    {
        /// <summary>
        /// 是否正在变更
        /// </summary>
        private bool isChangeing;

        /// <summary>
        /// 仓储资料对象
        /// </summary>
        private StorageLocationLayinInfo layinInfo;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as DetailLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= StorageLocationLayinInfo_CurrentChanged;
                view.CurrentChanged += StorageLocationLayinInfo_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前库位仓储资料对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void StorageLocationLayinInfo_CurrentChanged(object sender, System.EventArgs e)
        {
            DetailLogicalView logicalView = sender as DetailLogicalView;
            layinInfo = logicalView.Current as StorageLocationLayinInfo;
            if (layinInfo != null)
            {
                layinInfo.PropertyChanged -= LayinInfo_PropertyChanged;
                layinInfo.PropertyChanged += LayinInfo_PropertyChanged;
            }
        }

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void LayinInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isChangeing)
            {
                try
                {
                    isChangeing = true;
                    TemperateChange(sender, e);
                    HumidityChange(sender, e);
                }
                catch(Exception ex) 
                {
                    throw new ValidationException(ex.Message);
                }
                finally
                {
                    isChangeing = false;
                }
            }
        }

        /// <summary>
        /// 温度变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void TemperateChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StorageLocationLayinInfo.TemperatureTypeProperty.Name)
            {
                switch (layinInfo.TemperatureType)
                {
                    case TemperatureType.Normal:
                        layinInfo.TemperatureLower = 0M;
                        layinInfo.TemperatureUpper = 30M;
                        break;
                    case TemperatureType.Low:
                        layinInfo.TemperatureLower = 15M;
                        layinInfo.TemperatureUpper = 25M;
                        break;
                    case TemperatureType.Cold:
                        layinInfo.TemperatureLower = 0M;
                        layinInfo.TemperatureUpper = 10M;
                        break;
                    case TemperatureType.Freezing:
                        layinInfo.TemperatureLower = -24M;
                        layinInfo.TemperatureUpper = -4M;
                        break;
                }
            }
            else if (e.PropertyName == StorageLocationLayinInfo.TemperatureLowerProperty.Name ||
                e.PropertyName == StorageLocationLayinInfo.TemperatureUpperProperty.Name)
            {
                layinInfo.TemperatureType = TemperatureType.Custom;
            }
        }

        /// <summary>
        /// 湿度变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void HumidityChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StorageLocationLayinInfo.HumidityTypeProperty.Name)
            {
                switch (layinInfo.HumidityType)
                {
                    case HumidityType.Normal:
                        layinInfo.HumidityLower = 0M;
                        layinInfo.HumidityUpper = 95M;
                        break;
                    case HumidityType.Low:
                        layinInfo.HumidityLower = 40M;
                        layinInfo.HumidityUpper = 60M;
                        break;
                    case HumidityType.Dry:
                        layinInfo.HumidityLower = 0M;
                        layinInfo.HumidityUpper = 10M;
                        break;
                }
            }
            else if (e.PropertyName == StorageLocationLayinInfo.HumidityLowerProperty.Name ||
                e.PropertyName == StorageLocationLayinInfo.HumidityUpperProperty.Name)
            {
                layinInfo.HumidityType = HumidityType.Custom;
            }
        }
    }
}
