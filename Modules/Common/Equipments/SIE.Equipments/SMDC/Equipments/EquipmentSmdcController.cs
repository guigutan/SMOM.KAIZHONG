using Newtonsoft.Json;
using SIE.Common.Configs;
using SIE.Core.ApiModels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.SMDC.Equipments.Infos;
using SIE.Equipments.DeviceControls;
using SIE.Equipments.DeviceIOTParas;
using SIE.Equipments.DeviceIOTParas.ConfigValues;
using SIE.Equipments.DeviceIOTParas.ViewModles;
using SIE.Equipments.SMDC.Equipments.Infos;
using SIE.EventMessages.EAP.Equipments;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace SIE.SMDC
{
    /// <summary>
    /// 设备台账EAP实现
    /// </summary>
    public class EquipmentSmdcController : DomainController, IEquipmentEap
    {
        private const string CONTENT_TYPE_JSON = "application/json";
        private const string EXCEPTION_API = "接口异常：{0}";
        private const string API_FAILED = "调用接口失败：{0}";

        /// <summary>
        /// 获取EAP实时值
        /// </summary>
        /// <returns></returns>
        public virtual EquipEapRTValueInfo GetEquipEapRTValueInfo(EquipEapRTValuePara para)
        {
            string url =RT.Service.Resolve< DeviceControlController>().GetSmdcApiUrl("/Eap/GetRTRecipeValue");// http://10.10.22.193:7666/dataService/webapi/Eap/GetRTRecipeValue

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                string jsonPara = SerializeObjectPara(para);
                byte[] postData = encoding.GetBytes(jsonPara);

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

                var mdcReturn = JsonConvert.DeserializeObject<EquipEapRTValueInfo>(retString);

                return mdcReturn;
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 按照接口样式格式化参数
        /// </summary>
        /// <returns></returns>
        private string SerializeObjectPara(EquipEapRTValuePara para)
        {
            var newPara = new List<Object>();
            newPara.Add(para.EquipmentCode);
            newPara.Add(para.Paras);

            return JsonConvert.SerializeObject(newPara);
        }


        /// <summary>
        /// 根据设备型号编码，在MDC接口中查找明细数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MDCDetailViewModle> GetMDCDetailByEquipModel(MDCDetailViewModleCriteria criteria)
        {
            var config = ConfigService.GetConfig(new EquipMDCConfig(), typeof(DeviceIOTPara));
            if (config == null || config.Url.IsNullOrEmpty())
                throw new ValidationException("未找到设备WebApi地址,请检查【设备台账WebApi地址配置项】".L10N());

            try
            {
                //请求信息
                Object[] equipTypeNames = new object[1];
                equipTypeNames[0] = criteria.MesModel;
                String sendJson = JsonConvert.SerializeObject(equipTypeNames);
                //请求接口
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://10.10.22.101:7666/dataService/webapi/Eap/GetEquipmentbyModel");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(config.Url));
                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(sendJson);
                request.ContentLength = postData.Length;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(postData, 0, postData.Length);
                myRequestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);

                //返回信息
                string retString = myStreamReader.ReadToEnd();
                var mdcReturn = JsonConvert.DeserializeObject<MdcReturn<List<EquipParaInfo>>>(retString);

                //关闭流
                myStreamReader.Close();
                myResponseStream.Close();
                if (!mdcReturn.IsSuccess)
                {
                    throw new ValidationException(API_FAILED.L10nFormat(mdcReturn.Error?.Message));
                }

                if (mdcReturn.Error != null)
                    throw new ValidationException(API_FAILED.L10nFormat(mdcReturn.Error.Message));

                EntityList<MDCDetailViewModle> list = new EntityList<MDCDetailViewModle>();
                foreach (var item in mdcReturn.Data)
                {
                    MDCDetailViewModle mDCDetail = new MDCDetailViewModle() { EquipmentCode = item.AssetsCode, MesDeviceName = item.MesDeviceName, MesModel = item.MesModel };
                    list.Add(mDCDetail);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }


        /// <summary>
        /// 根据资产编码,在MDC接口中获取设备Tags
        /// </summary>
        /// <param name="assetsCode"></param>
        /// <returns></returns>
        public virtual String GetEquipmentTags(string assetsCode)
        {
            //http://10.10.22.101:7666/dataService/webapi/Eap/GetEquipmentTags
            string url = RT.Service.Resolve<DeviceControlController>().GetSmdcApiUrl("/Eap/GetEquipmentTags");

            Object[] assetsCodes = new object[1];
            assetsCodes[0] = assetsCode;
            String sendJson = JsonConvert.SerializeObject(assetsCodes);

            //请求接口            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.ContentType = CONTENT_TYPE_JSON;
            Encoding encoding = Encoding.UTF8;
            byte[] postData = encoding.GetBytes(sendJson);
            request.ContentLength = postData.Length;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(postData, 0, postData.Length);
            myRequestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);

            //返回信息
            string retString = myStreamReader.ReadToEnd();
            var mdcReturn = JsonConvert.DeserializeObject<MdcReturn<List<EquipmentTagInfo>>>(retString);


            //关闭流
            myStreamReader.Close();
            myResponseStream.Close();
            if (!mdcReturn.IsSuccess)
            {
                throw new ValidationException(API_FAILED.L10nFormat(mdcReturn.Error?.Message));
            }

            if (mdcReturn.Error != null)
            {
                throw new ValidationException(API_FAILED.L10nFormat(mdcReturn.Error.Message));
            }

            String JsonReturn = JsonConvert.SerializeObject(mdcReturn.Data);
            return JsonReturn;
        }

        /// <summary>
        /// 通过资产编码获取设备的运行状态
        /// </summary>
        /// <param name="equipAccountCodeArr">设备编码数组</param>
        /// <returns></returns>
        public virtual Dictionary<string, int> GetDeviceRunStateByAssetCode(string[] equipAccountCodeArr)
        {
            string jsonString = JsonConvert.SerializeObject(new[] { equipAccountCodeArr });
            string url = RT.Service.Resolve<DeviceControlController>().GetSmdcApiUrl("/IO/GetDeviceRunStateByAssetCode");// http://10.10.22.193:7666/dataService/webapi/IO/GetDeviceRunStateByAssetCode

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(jsonString);

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

                var mdcReturn = JsonConvert.DeserializeObject<Dictionary<string, int>>(retString);

                return mdcReturn;
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 通过资产编码获取设备的在线状态
        /// </summary>
        /// <param name="equipAccountCodeArr">设备编码数组</param>
        /// <returns></returns>
        public virtual Dictionary<string, bool> GetDeviceIsOnLineByAssetCode(string[] equipAccountCodeArr)
        {
            string jsonString = JsonConvert.SerializeObject(new[] { equipAccountCodeArr });
            string url = RT.Service.Resolve<DeviceControlController>().GetSmdcApiUrl("/IO/GetDeviceIsOnLineByAssetCode");// http://10.10.22.193:7666/dataService/webapi/IO/GetDeviceIsOnLineByAssetCode

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(jsonString);

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

                var mdcReturn = JsonConvert.DeserializeObject<Dictionary<string, bool>>(retString);

                return mdcReturn;
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 获取报警历史数据
        /// </summary>
        /// <param name="startTime">取数开始时间</param>
        /// <param name="endTime">取数结束时间</param>
        /// <param name="maxCount">取数数量</param>
        /// <param name="isDesc">是否倒序</param>
        /// <returns></returns>
        public virtual AlarmRecordInfo[] GetAlarmRecords(DateTime startTime, DateTime endTime, int maxCount, bool isDesc)
        {
            string jsonString = JsonConvert.SerializeObject(new ArrayList() { startTime, endTime, maxCount, isDesc });
            string url = RT.Service.Resolve<DeviceControlController>().GetSmdcApiUrl("/Alarm/QueryRecord");// http://10.10.22.193:7666/dataService/webapi/Alarm/QueryRecord

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(jsonString);

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

                return JsonConvert.DeserializeObject<AlarmRecordInfo[]>(retString);
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 获取设备的所有tag全路径
        /// </summary>
        /// <param name="equipmentCode">设备编码</param>
        /// <returns></returns>
        public virtual MdcReturn<List<EquipmentTagFullNameInfo>> GetEquipmentTagFullNames(string equipmentCode)
        {
            string jsonString = JsonConvert.SerializeObject(new[] { equipmentCode });
            string url = RT.Service.Resolve<DeviceControlController>().GetSmdcApiUrl("/Eap/GetEquipmentTagFullNames");// http://10.10.22.193:7666/dataService/webapi/Eap/GetEquipmentTagFullNames

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                byte[] postData = encoding.GetBytes(jsonString);

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

                var mdcReturn = JsonConvert.DeserializeObject<MdcReturn<List<EquipmentTagFullNameInfo>>>(retString);

                return mdcReturn;
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }

        /// <summary>
        /// 查询MDC Tag的历史值
        /// </summary>
        /// <param name="tags">标签列表</param>
        /// <param name="dateTimeBegin">开始时间</param>
        /// <param name="dateTimeEnd">结束时间</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>

        public virtual List<TagHistoryValue> GetHistoryTagValue(List<string> tags, DateTime dateTimeBegin, DateTime dateTimeEnd)
        {
            //http://10.10.22.193:7666/dataService/webapi/HDA/QueryToSTR
            string url = RT.Service.Resolve<DeviceControlController>().GetSmdcApiUrl("/HDA/QueryToDt");

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));

                request.Method = "POST";
                request.ContentType = CONTENT_TYPE_JSON;
                Encoding encoding = Encoding.UTF8;
                var newPara = new List<Object>();
                newPara.Add(tags);
                newPara.Add(dateTimeBegin);
                newPara.Add(dateTimeEnd);
                newPara.Add(int.MaxValue);
                newPara.Add(false);
                string jsonPara = JsonConvert.SerializeObject(newPara);
                byte[] postData = encoding.GetBytes(jsonPara);

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

                var mdcReturn = JsonConvert.DeserializeObject<MdcReturn<TagHistoryValueInfo>>(retString);

                if (!mdcReturn.IsSuccess)
                {
                    if (mdcReturn.Error != null)
                    {
                        throw new ValidationException("调用MDC的【查询MDC Tag的历史值】API出错，错误信息:{0}".L10nFormat(mdcReturn.Error.Message));
                    }
                    else
                    {
                        throw new ValidationException("调用MDC的【查询MDC Tag的历史值】API出错，错误信息:无".L10N());
                    }
                }

                return mdcReturn.Data.TagRecord_INT;
            }
            catch (Exception ex)
            {
                throw new ValidationException(EXCEPTION_API.L10nFormat(ex.Message));
            }
        }
    }
}
