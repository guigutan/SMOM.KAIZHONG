using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Enums
{
    /// <summary>
    /// 接口方向
    /// </summary>
    public enum CallDirection
    {
        /// <summary>
        /// NC到MOM
        /// </summary>
        [Label("NC到MOM")]
        NcToMom = 0,

        /// <summary>
        /// MOM到NC
        /// </summary>
        [Label("MOM到NC")]
        MomToNc = 1,

        /// <summary>
        /// SAP到MOM
        /// </summary>
        [Label("SAP到MOM")]
        SAPToMom = 2,

        ///// <summary>
        ///// 飞书到MOM
        ///// </summary>
        //[Label("飞书到MOM")]
        //LarkToMom = 2,
        ///// <summary>
        ///// MOM到飞书
        ///// </summary>
        //[Label("MOM到飞书")]
        //MomToLark = 3,
        ///// <summary>
        ///// MOM到飞书
        ///// </summary>
        //[Label("MES到ERP")]
        //MesToErp = 4,
        ///// <summary>
        ///// QMS到飞书
        ///// </summary>
        //[Label("QMS到飞书")]
        //QMSToLark = 5,
        ///// <summary>
        ///// 飞书到QMS
        ///// </summary>
        //[Label("飞书到QMS")]
        //LarkToQMS = 6,
        ///// <summary>
        ///// WMS到QMS
        ///// </summary>
        //[Label("WMS到QMS")]
        //WmsToQMS = 7,
        ///// <summary>
        ///// QMS到WMS
        ///// </summary>
        //[Label("QMS到WMS")]
        //QMSToWms = 8,
        ///// <summary>
        ///// MES到WMS
        ///// </summary>
        //[Label("MES到WMS")]
        //MESToWms = 9,
        ///// <summary>
        ///// WMS到MES
        ///// </summary>
        //[Label("WMS到MES")]
        //WmsToMES = 10,
        /// <summary>
        /// MES到SCADA
        /// </summary>
        [Label("MES到SCADA")]
        MESToScada = 11,
        /// <summary>
        /// SCADA到MES
        /// </summary>
        [Label("SCADA到MES")]
        ScadaToMES = 12,
        ///// <summary>
        ///// MES到条码管理系统
        ///// </summary>
        //[Label("MES到Barcode")]
        //MesToBarcode = 13,
        ///// <summary>
        ///// EDO到NC
        ///// </summary>
        //[Label("EDO到NC")]
        //EdoToNc = 14,
        ///// <summary>
        ///// MES到其它
        ///// </summary>
        //[Label("MesToOther")]
        //MesToOther = 15,

        ///// <summary>
        ///// SRM到QMS
        ///// </summary>
        //[Label("SRM到QMS")]
        //SrmToQms = 16,

        ///// <summary>
        ///// MES到飞书
        ///// </summary>
        //[Label("MES到飞书")]
        //MesToLark = 17,
        ///// <summary>
        ///// EDO到飞书
        ///// </summary>
        //[Label("EDO到飞书")]
        //EdoToLark = 18,

        ///// <summary>
        ///// QMS到SRM
        ///// </summary>
        //[Label("QMS到SRM")]
        //QmsToSrm = 19,

        ///// <summary>
        ///// MES到TC
        ///// </summary>
        //[Label("MES到TC")]
        //MesToTc = 20,

        ///// <summary>
        ///// BOP到MOM
        ///// </summary>
        //[Label("BOP到MOM")]
        //BopToMom = 21,

        ///// <summary>
        ///// MOM到BOP
        ///// </summary>
        //[Label("MOM到BOP")]
        //MomToBop = 22,

        //[Label("QMS到OCR")]
        //QmsToOCR = 23,

        //[Label("MOM到外围设备")]
        //MomToPeripheral = 24,

        ///// <summary>
        ///// QMS到PDF生成平台
        ///// </summary>
        //[Label("QMS到PDF生成平台")]
        //QmsToPdf = 25,

        ///// <summary>
        ///// QMS到CRM
        ///// </summary>
        //[Label("QMS到CRM")]
        //QmsToCrm = 26,

        ///// <summary>
        ///// QAD erp到MOM
        ///// </summary>
        //[Label("QAD到MOM")]
        //QadToMom = 27,

        ///// <summary>
        ///// MOM到QAD erp
        ///// </summary>
        //[Label("MOM到QAD")]
        //MomToQad = 28,
        /// <summary>
        /// 子工厂到总控
        /// </summary>
        [Label("子工厂到总控")]
        FactoryToGroup = 29,
        ///// <summary>
        ///// MES到SRM
        ///// </summary>
        //[Label("MES到SRM")]
        //MesToSrm = 30,
        ///// <summary>
        ///// MOM到主机厂
        ///// </summary>
        //[Label("MOM到主机厂")]
        //MomToMain = 31,

        ///// <summary>
        ///// QMS到TC
        ///// </summary>
        //[Label("QMS到TC")]
        //QmsToTc = 32,

        ///// <summary>
        ///// MES到EMS
        ///// </summary>
        //[Label("MES到EMS")]
        //MmsToEms = 33,

        ///// <summary>
        ///// EDO到AI设备守卫
        ///// </summary>
        //[Label("EDO到AI设备守卫")]
        //EdoToAI = 34,

        ///// <summary>
        ///// AI设备守卫到EDO
        ///// </summary>
        //[Label("AI设备守卫到EDO")]
        //AIToEDO = 35,

        ///// <summary>
        ///// QMS到NC
        ///// </summary>
        //[Label("QMS到NC")]
        //QMSToNC = 36,

        ///// <summary>
        ///// 数据埋点
        ///// </summary>
        //[Label("数据埋点")]
        //DataTrack = 37,

        ///// <summary>
        ///// QMS到FMEA
        ///// </summary>
        //[Label("QMS到FMEA")]
        //QMSToFMEA = 38,

        ///// <summary>
        ///// MOM到数字孪生
        ///// </summary>
        //[Label("MOM到数字孪生")]
        //MOMTDigitalTwinData = 39,

        //[Label("MOM到飞书多维表格")]
        //MOMToMultiTable = 40,

        //[Label("飞书Ai到MOM")]
        //AiToMOM = 41,

        //[Label("MOM到电检互联")]
        //MOMToMQTT = 42,

        ///// <summary>
        ///// TC到QMS
        ///// </summary>
        //[Label("TC到QMS")]
        //TcToQMS = 43,

        ///// <summary>
        ///// MES到CRM
        ///// </summary>
        //[Label("MES到CRM")]
        //MESToCrm = 44,

        /// <summary>
        /// MES到SAP
        /// </summary>
        [Label("MES到SAP")]
        MesToSap = 45,

        /// <summary>
        /// MES到IOT
        /// </summary>
        [Label("MES到IOT")]
        MesToIot = 46,

        /// <summary>
        /// MES到Qv
        /// </summary>
        [Label("MES到Qv")]
        MesToQv = 47,

        /// <summary>
        /// 工厂到工厂
        /// </summary>
        [Label("工厂到工厂")]
        FactoryToFactory = 48,

        /// <summary>
        /// IOT到MES
        /// </summary>
        [Label("IOT到MES")]
        IotToMes = 49,
    }

}
