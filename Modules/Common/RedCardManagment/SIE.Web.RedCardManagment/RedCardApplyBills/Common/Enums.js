
/**
 * 申请类型
 */
Ext.define('SIE.RedCardManagment.RedCardApplyBills.ApplyType', {
    statics: {
        /**
         * 手工创建
         */
        Manual: 0,
        /**
        * 自动创建
        */
        Auto: 1,
    }
});

/**
 * 申请单状态
 */
Ext.define('SIE.RedCardManagment.RedCardApplyBills.BillStatus', {
    statics: {
        /**
         * 待发起
         */
        ToDo: 0,
        /**
        * 进行中
        */
        Doing: 1,
        /**
         * 完成
         */
        Done: 2,
        /**
        * 取消
        */
        Cancel: 3,
        /**
        * 驳回
        */
        Reject: 4,
    }
});