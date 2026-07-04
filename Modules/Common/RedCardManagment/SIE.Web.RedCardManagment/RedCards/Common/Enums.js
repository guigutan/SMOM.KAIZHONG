
/**
 * 处理状态
 */
Ext.define('SIE.AbnormalInfo.AbnormalInfos.AbnormalStatus', {
    statics: {
        /**
         * 待处理
         */
        ToProcess: 0,
        /**
        * 处理中
        */
        Processing: 1,
        /**
        * 关闭
        */
        Close: 2,
    }
});

/**
 * 红牌状态
 */
Ext.define('SIE.RedCardManagment.RedCards.RedCardState', {
    statics: {
        /**
         * 禁用
         */
        Disable: 0,
        /**
        * 启用
        */
        Enable: 1,
        /**
        * 部分启用
        */
        PartialEnable: 2,

    }
});