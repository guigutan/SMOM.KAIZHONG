using System;

namespace SIE.Andon.Andons.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AndonManageModel
    {

        public string AndonManageCode { get; set; }
        public string AndonManageClass { get; set; }
        public string AndonTypeName { get; set; }
        public string AndonName { get; set; }
        public string Solution { get; set; }
        public string ProblemDesc { get; set; }
        public string Priority { get; set; }
        public string Defect { get; set; }
        public string Department { get; set; }
        public string State { get; set; }
        public string FaultTime { get; set; }
        public string TriggerName { get; set; }
        public string TriggerTime { get; set; }
        public string HandlerName { get; set; }
        public string CloseTime { get; set; }
        public string LastTime { get; set; }
        public string ActualTime { get; set; }
        public string FactoryName { get; set; }
        public string WorkShopName { get; set; }
        public string WipResourceName { get; set; }
        public string StationName { get; set; }
        public string EquipAccountCode { get; set; }
        public string EquipAccountName { get; set; }
        public string WorkGroup { get; set; }
        public string WoNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProcessName { get; set; }
        public string BarCode { get; set; }
        public string LineStop { get; set; }
        public string AskMaterial { get; set; }
    }
}
