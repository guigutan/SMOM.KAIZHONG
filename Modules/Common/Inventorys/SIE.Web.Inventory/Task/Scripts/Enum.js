Ext.define('SIE.Inventory.Task.TaskState', {
    statics: {
        Create: { value: 0, text: 'Create', label: '创建' },
        Release: { value: 1, text: 'Release', label: '释放' },
        Appoint: { value: 2, text: 'Appoint', label: '指定' },
        Frozen: { value: 3, text: 'Frozen', label: '冻结' },
        HangUp: { value: 4, text: 'HangUp', label: '挂起' },
        Finish: { value: 5, text: 'Finish', label: '完工' },
        Close: { value: 6, text: 'Close', label: '关闭' },
        Executing: { value: 7, text: 'Executing', label: '执行中' },
        Abnormal: { value: 8, text: 'Abnormal', label: '异常' },
        AutoFinish: { value: 9, text: 'AutoFinish', label: '自动完工' }
    }
});
//SIE:classEnd
Ext.define('SIE.Inventory.Task.SourceType', {
    statics: {
        PC: { value: 0, text: 'PC', label: 'PC端自建' },
        APP: { value: 1, text: 'APP', label: '移动端自建' },
        TASK: { value: 2, text: 'TASK', label: '系统任务创建' },
        ALLOCATEOUT: { value: 3, text: 'ALLOCATEOUT', label: '调拨出库单' },
        EXTINTERFACE: { value: 4, text: 'EXTINTERFACE', label: '外部接口' },
        STORCKCOUNT: { value: 5, text: 'STORCKCOUNT', label: '盘点单' }
    }
});
