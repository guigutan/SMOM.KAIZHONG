using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.ViceTransfers.Rules
{
    /// <summary>
    /// 工治具编码非重复验证规则
    /// </summary>
    [DisplayName("工治具编码非重复验证规则")]
    [Description("工治具编码非重复验证规则")]
    public class ViceTransfersFixtureNotDuplicateRule : NotDuplicateRule<ViceTransferFixture>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViceTransfersFixtureNotDuplicateRule()
        {
            Properties.Add(ViceTransferFixture.ViceTransferIdProperty);
            Properties.Add(ViceTransferFixture.FixtureQualityStateProperty);
            Properties.Add(ViceTransferFixture.FixtureEncodeIdProperty);
            MessageBuilder = (e) => { return "工治具编码不能重复".L10N(); };
        }
    }


    /// <summary>
    /// 备件编码非重复验证规则
    /// </summary>
    [DisplayName("备件编码非重复验证规则")]
    [Description("备件编码非重复验证规则")]
    public class ViceTransferSparePartNotDuplicateRule : NotDuplicateRule<ViceTransferSparePart>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViceTransferSparePartNotDuplicateRule()
        {
            Properties.Add(ViceTransferSparePart.ViceTransferIdProperty);
            Properties.Add(ViceTransferSparePart.QualityStatusProperty);
            Properties.Add(ViceTransferSparePart.SparePartIdProperty);
            MessageBuilder = (e) => { return "备件编码不能重复".L10N(); };
        }
    }


    /// <summary>
    /// 备件编码非重复验证规则
    /// </summary>
    [DisplayName("备件执行明细保存非重复验证规则")]
    [Description("备件执行明细保存非重复验证规则")]
    public class ViceTransferSparePartDetailNotDuplicateRule : NotDuplicateRule<ViceTransferSparePartDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViceTransferSparePartDetailNotDuplicateRule()
        {
            Properties.Add(ViceTransferSparePartDetail.ViceTransferIdProperty);
            Properties.Add(ViceTransferSparePartDetail.ViceTransferSparePartIdProperty);
            Properties.Add(ViceTransferSparePartDetail.StoreSummaryLotIdProperty);
            Properties.Add(ViceTransferSparePartDetail.StoreSummaryDetailIdProperty);
            Properties.Add(ViceTransferSparePartDetail.QualityStatusProperty);
            Properties.Add(ViceTransferSparePartDetail.StorageLocationIdProperty);
            MessageBuilder = (e) => { return "【需求行号+批次号+序列号+质量状态+来源库位】需唯一".L10N(); };
        }
    }

    /// <summary>
    /// 工治具执行明细保存非重复验证规则
    /// </summary>
    [DisplayName("工治具执行明细保存非重复验证规则")]
    [Description("工治具执行明细保存非重复验证规则")]
    public class ViceTransferFixtureDetailNotDuplicateRule : NotDuplicateRule<ViceTransferFixtureDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ViceTransferFixtureDetailNotDuplicateRule()
        {
            //校验保存的数据中【需求行号+序列号+质量状态+来源库位】的数据不能重复
            Properties.Add(ViceTransferFixtureDetail.ViceTransferIdProperty);
            Properties.Add(ViceTransferFixtureDetail.ViceTransferFixtureIdProperty);
            Properties.Add(ViceTransferFixtureDetail.FixtureIDAccountIdProperty);
            Properties.Add(ViceTransferFixtureDetail.FixtureQualityStateProperty);
            Properties.Add(ViceTransferFixtureDetail.StorageLocationIdProperty);
            MessageBuilder = (e) => { return "【需求行号+序列号+质量状态+来源库位】需唯一".L10N(); };
        }
    }
}
