using Newtonsoft.Json;
using SIE.Api;
using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.Equipments.DeviceControls.ApiModels;
using SIE.Equipments.DeviceControls.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SIE.Equipments.DeviceControls
{
    /// <summary>
    /// 物料控制器
    /// </summary>
    public partial class DeviceControlController : DomainController
    {
        #region 获取设备开停机状态接口
        /// <summary>
        /// 获取设备开停机状态接口
        /// </summary>
        /// <param name="deviceCode">设备编码</param>
        /// <returns></returns>
        [ApiService("获取设备开停机状态接口")]
        [return: ApiReturn("获取设备开停机状态接口 GetEquipAccountState")]
        public virtual DeviceStateInfo GetEquipAccountState([ApiParameter("设备编码")] string deviceCode)
        {
            var result = new DeviceStateInfo();
            result.IsSuccess = true;
            result.Message = string.Empty;
            result.DeviceCode = deviceCode;
            result.IsStop = false;
            //验证设备编码是否存在
            var equipAccount = RT.Service.Resolve<CoreEquipController>().GetEquipAccountByCode(deviceCode);
            if (equipAccount == null)
            {
                result.IsSuccess = false;
                result.Message = "设备编码[{0}]不存在！".L10nFormat(deviceCode);
                return result;
            }

            //获取所有来源
            var sourceList = GetSourceControls("");

            //根据来源判断每个来源的最终状态
            DateTime lastTime = DateTime.MinValue;
            foreach (var source in sourceList)
            {
                var deviceControl = GetDeviceControlBySource(source.Source);
                if (deviceControl != null)
                {
                    if (result.Source.IsNullOrEmpty())
                    {
                        result.Source = source.Source;
                        result.IsStop = deviceControl.IsStop;
                        lastTime = deviceControl.CreateDate;
                    }
                    else if (deviceControl.IsStop && !result.IsStop)
                    {
                        result.Source = source.Source;
                        result.IsStop = deviceControl.IsStop;
                        lastTime = deviceControl.CreateDate;
                    }
                    else if (!deviceControl.IsStop && !result.IsStop && deviceControl.CreateDate > lastTime)
                    {
                        result.Source = source.Source;
                        result.IsStop = deviceControl.IsStop;
                        lastTime = deviceControl.CreateDate;
                    }
                }

            }
            return result;
        }
        #endregion

        #region 停开机控制接口
        /// <summary>
        /// 停开机控制接口
        /// </summary>
        /// <param name="deviceStopInfo">停开机控制信息</param>
        /// <returns></returns>
        [ApiService("停开机控制接口")]
        [return: ApiReturn("停开机控制接口 DeviceControl")]
        public virtual void DeviceControl([ApiParameter("停开机控制信息")] DeviceStopInfo deviceStopInfo)
        {
            //验证设备编码是否存在
            var equipAccount = RT.Service.Resolve<CoreEquipController>().GetEquipAccountByCode(deviceStopInfo.DeviceCode);
            if (equipAccount == null)
                throw new ValidationException("设备编码[{0}]不存在！".L10nFormat(deviceStopInfo.DeviceCode));
            //判断来源是否存在，不存在则新增
            ValidationSource(deviceStopInfo.Source);
            //获取所有来源
            var sourceList = GetSourceControls("");

            var isEffective = true;
            //根据来源判断每个来源的最终状态
            foreach (var source in sourceList)
            {
                if (source.Source == deviceStopInfo.Source)
                    continue;
                var deviceControl = GetDeviceControlBySource(source.Source);
                if (deviceControl != null && deviceControl.IsStop)
                {
                    isEffective = false;
                    break;
                }

            }

            var newDeviceControl = new DeviceControl();
            newDeviceControl.EquipAccountId = equipAccount.Id;
            newDeviceControl.Source = deviceStopInfo.Source;
            newDeviceControl.IsStop = deviceStopInfo.IsStop;
            newDeviceControl.OpearDateTime = DateTime.Now;
            newDeviceControl.IsEffective = isEffective;
            SaveDeviceControl(newDeviceControl);

            //调用设备开停机接口

            if (isEffective)
            {
                MdcDeviceControl(newDeviceControl.IsStop ? "StopElectronDevice" : "StartElectronDevice", deviceStopInfo.DeviceCode);
            }

        }
        #endregion

        /// <summary>
        /// MDC设备停开机接口
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="deviceCode"></param>
        private void MdcDeviceControl(string methodName, string deviceCode)
        {

            var config = ConfigService.GetConfig(new SmdcUrlConfig(), typeof(DeviceControl));
            if (config == null || config.Url.IsNullOrEmpty())
                throw new ValidationException("未找到设备WebApi地址,请检查规则配置".L10N());
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(config.Url)); //"http://192.168.18.148:7666/dataservice/webapi/RpcInvoke"
                var rpcPara = new RpcPara();
                rpcPara.MethodName = methodName;//// "StartElectronDevice";
                rpcPara.Paras = new List<string>();
                rpcPara.Paras.Add(deviceCode); ////"HS50-1"
                var rpcParaInfo = new RpcParaInfo() { RpcPara = rpcPara };
                request.Method = "POST";
                request.ContentType = "application/json";
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(JsonConvert.SerializeObject(rpcParaInfo));
                request.ContentLength = postData.Length;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(postData, 0, postData.Length);
                myRequestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                var mdcReturn =  JsonConvert.DeserializeObject<MdcReturn>(retString);

                if (mdcReturn.IsSuccess == false)
                    throw new ValidationException("调用接口失败：{0}".L10nFormat(mdcReturn.Data?.Message));
            }
            catch (Exception ex)
            {
                throw new ValidationException("接口异常：{0}".L10nFormat(ex.Message));
            }
        }
    }

}