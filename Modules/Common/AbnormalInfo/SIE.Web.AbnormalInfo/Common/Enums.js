
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
 * 处理状态
 */
Ext.define('SIE.AbnormalInfo.Common.TaskStateEnum', {
    statics: {
        /**
         * 未开始
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
        Cancel:3,
    }
});

/**
 * 任务状态
 */
Ext.define('SIE.AbnormalInfo.Common.TaskType', {
    statics: {
        /**
         * 自动
         */
        Auto: 0,
        /**
        * 手工
        */
        Manual: 1,
    }
});