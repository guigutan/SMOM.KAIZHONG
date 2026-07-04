/**
 * 校验状态
 */
Ext.define('SIE.EMS.Calibration.Records.CalibrationState', {
    statics: {
        /**
        * 待审核
        */
        ToAudit: 0,
        /**
        * 审核中
        */
        Auditing: 1,
        /**
        * 驳回
        */
        Reject: 2,
        /**
        * 校验中
        */
        Calirating: 3,
        /**
         * 待校验
         */
        ToCalirate: 4,
        /**
        * 已校验
        */
        Calirated: 5
    }
});

/**
 * 结果
 */
Ext.define('SIE.EMS.InspectionResult', {
    statics: {
        /**
         * 合格
         */
        Pass: 1,
        /**
        * 不合格
        */
        Fail: 2
    }
});

/**
 * 结果
 */
Ext.define('SIE.EMS.MainenanceProjects.ProjectType', {
    statics: {
        /**
         * 点检
         */
        Check: 0,
        /**
        * 保养
        */
        Maintain: 5,
        /**
        * 校验
        */
        Verify: 10
    }
});
