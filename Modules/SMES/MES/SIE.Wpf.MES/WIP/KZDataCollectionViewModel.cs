using DocumentFormat.OpenXml.Bibliography;
using SIE.Common.Configs;
using SIE.Domain.Validation;
using SIE.MES.WIP.Configs;
using SIE.Tech.Processs;
using System;

namespace SIE.Wpf.MES.WIP
{

    /// <summary>
    /// KZ数据采集基类
    /// </summary>
    [RootEntity, Serializable]
    public class KZDataCollectionViewModel : DataCollectionViewModel
    {

        /// <summary>
        /// 获取端口类型
        /// </summary>
        /// <returns></returns>
        public override DevicePortConfigValue GetDevicePort()
        {
            if (Workstation.Resource == null)
                return null;
            var devicePort = ConfigService.GetConfig(new DevicePortConfig(), GetType(), ResourceStation.Find(Workstation.Resource));
            return devicePort;
        }
        /// <summary>
        /// 获取端口配置信息
        /// </summary>
        /// <returns></returns>
        public override SerialPortsConfigValue GetSerialPortsConfig()
        {
            if (Workstation.Resource == null)
                return null;
            var serialPortsConfig = ConfigService.GetConfig(new SerialPortsConfig(), GetType(), ResourceStation.Find(Workstation.Resource));
            return serialPortsConfig;
        }

        /// <summary>
        /// 工作站信息
        /// </summary>
        public KZWorkstation _kzworkstation;

        /// <summary>
        /// 工作站
        /// </summary>
        public KZWorkstation KZWorkstation
        {
            get
            {
                if (_kzworkstation == null)
                {
                    _kzworkstation = new KZWorkstation(this);
                }

                return _kzworkstation;
            }
        }

        public void InitWorkstationNotWindow(params ProcessType[] processTypes)
        {
            Workstation.PropertyChanged += OnWorkstationPropertyChanged;

            Workstation.ProcessTypes.AddRange(processTypes); //设置工作站工序类型
            Workstation.EmployeeId = CRT.IdentityId;

            KZWorkstation.ProcessTypes.AddRange(processTypes); //设置工作站工序类型
            KZWorkstation.EmployeeId = CRT.IdentityId;

            var broken = KZWorkstation.Validate(ValidatorActions.None);

            if (broken.Count > 0)
            {
                ShowError(broken.ToString());
            }
        }

        /// <summary>
        /// 初始化工作站信息
        /// </summary>
        /// <param name="processTypes">工序类型数值</param>
        public override void InitWorkstation(params ProcessType[] processTypes)
        {
            Workstation.PropertyChanged += OnWorkstationPropertyChanged;

            Workstation.ProcessTypes.AddRange(processTypes); //设置工作站工序类型
            Workstation.EmployeeId = CRT.IdentityId;

            KZWorkstation.ProcessTypes.AddRange(processTypes); //设置工作站工序类型
            KZWorkstation.EmployeeId = CRT.IdentityId;

            //if (!LoadWorkstation() //如果工作站信息不存在，或者与上次登录用户的资源工序工位分配不一样，重新选择
            //    && WorkstationSelector.SelectOperation(Workstation))
            //{
            //    //有切换工作单元，则将工作单元信息保存在本地配置文件中
            //    SaveWorkstation();
            //}         
            KZWorkstationSelector.SelectOperation(KZWorkstation, Workstation);

            var broken = KZWorkstation.Validate(ValidatorActions.None);

            if (broken.Count > 0)
            {
                ShowError(broken.ToString());
            }
        }
        /// <summary>
        /// 工作单元属性变更
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void OnWorkstationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(Workstation.Employee))
            //{
            //    EmployeeChanged(Workstation.Employee);
            //}
              

            if (e.PropertyName == nameof(Workstation.Resource))
            {
                ResourceChanged(Workstation.Resource);
            }

        }
    }

}