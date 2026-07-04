window.bryntum = window.bryntum || {};
window.bryntum.locales = window.bryntum.locales || {};

// put the locale under window.bryntum.locales to make sure it is discovered automatically
window.bryntum.locales.Zh = {
    localeName: 'Zh',
    localeDesc: 'Chinese',
    Object: {
        Yes: 'Yes',
        No: 'No',
        Cancel: '取消',
        Ok: 'OK',
        newEvent: 'New'
    },
    //#region  Core

    Combo: {
        noResults: 'No results',
        recordNotCommitted: 'Record could not be addded',
        addNewValue: value => `Add ${value}`
    },
    FilePicker: {
        file: 'File'
    },
    Field: {
        // native input ValidityState statuses
        badInput: 'Invalid field value',
        patternMismatch: 'Value should match a specific pattern',
        rangeOverflow: value => `Value must be less than or equal to ${value.max}`,
        rangeUnderflow: value => `Value must be greater than or equal to ${value.min}`,
        stepMismatch: 'Value should fit the step',
        tooLong: 'Value should be shorter',
        tooShort: 'Value should be longer',
        typeMismatch: 'Value is required to be in a special format',
        valueMissing: 'This field is required',

        invalidValue: 'Invalid field value',
        minimumValueViolation: 'Minimum value violation',
        maximumValueViolation: 'Maximum value violation',
        fieldRequired: 'This field is required',
        validateFilter: 'Value must be selected from the list'
    },
    DateField: {
        invalidDate: 'Invalid date input'
    },
    NumberFormat: {
        locale: 'zh-US',
        currency: 'CHN'
    },
    DurationField: {
        invalidUnit: 'Invalid unit'
    },
    TimeField: {
        invalidTime: 'Invalid time input'
    },
    List: {
        loading: 'Loading...'
    },
    GridBase: {
        loadMask: 'Loading...',
        syncMask: 'Saving changes, please wait...'
    },
    PagingToolbar: {
        firstPage: 'Go to first page',
        prevPage: 'Go to previous page',
        page: 'Page',
        nextPage: 'Go to next page',
        lastPage: 'Go to last page',
        reload: 'Reload current page',
        noRecords: '无数据可显示',
        pageCountTemplate: data => `of ${data.lastPage}`,
        summaryTemplate: data => `Displaying records ${data.start} - ${data.end} of ${data.allCount}`
    },
    PanelCollapser: {
        Collapse: 'Collapse',
        Expand: 'Expand'
    },
    UndoRedo: {
        Undo: 'Undo',
        Redo: 'Redo',
        UndoLastAction: '前进',
        RedoLastAction: '后退'
    },
    DateHelper: {
        locale: 'zh-US',
        weekStartDay: 0,
        defaultFormat: 'YYYY-MM-DD HH:mm:ss',
        defaultParseFormat: 'YYYY-MM-DD HH:mm:ss',
        // Non-working days which match weekends by default, but can be changed according to schedule needs
        nonWorkingDays: {
            0: true,
            6: true
        },
        // Days considered as weekends by the selected country, but could be working days in the schedule
        weekends: {
            0: true,
            6: true
        },
        unitNames: [
            { single: 'millisecond', plural: 'ms', abbrev: 'ms' },
            { single: 'second', plural: 'seconds', abbrev: 's' },
            { single: 'minute', plural: 'minutes', abbrev: 'min' },
            { single: 'hour', plural: 'hours', abbrev: 'h' },
            { single: 'day', plural: 'days', abbrev: 'd' },
            { single: 'week', plural: 'weeks', abbrev: 'w' },
            { single: 'month', plural: 'months', abbrev: 'mon' },
            { single: 'quarter', plural: 'quarters', abbrev: 'q' },
            { single: 'year', plural: 'years', abbrev: 'yr' },
            { single: 'decade', plural: 'decades', abbrev: 'dec' }
        ],
        // Used to build a RegExp for parsing time units.
        // The full names from above are added into the generated Regexp.
        // So you may type "2 w" or "2 wk" or "2 week" or "2 weeks" into a DurationField.
        // When generating its display value though, it uses the full localized names above.
        unitAbbreviations: [
            ['mil'],
            ['s', 'sec'],
            ['m', 'min'],
            ['h', 'hr'],
            ['d'],
            ['w', 'wk'],
            ['mo', 'mon', 'mnt'],
            ['q', 'quar', 'qrt'],
            ['y', 'yr'],
            ['dec']
        ],
        parsers: {
            L: 'YYYY/MM/DD',
            LT: 'HH:mm A',
            LLL: 'YYYY/MM/DD HH:mm'

        },
        ordinalSuffix: number => {
            const hasSpecialCase = ['11', '12', '13'].find((n) => number.endsWith(n));

            let suffix = 'th';

            if (!hasSpecialCase) {
                const lastDigit = number[number.length - 1];
                suffix = { 1: 'st', 2: 'nd', 3: 'rd' }[lastDigit] || 'th';
            }

            return number + suffix;
        }
    },
    TrialButton: {
        downloadTrial: 'Download trial'
    },
    TrialPanel: {
        title: 'Please complete fields',
        name: 'Name',
        email: 'Email',
        company: 'Company',
        product: 'Product',
        cancel: 'Cancel',
        submit: 'Submit',
        downloadStarting: 'Download starting, please wait...'
    },
    //#endregion Core

    //#region grid
    ColumnPicker: {
        column: '列',
        columnsMenu: '列菜单',
        hideColumn: '隐藏列',
        hideColumnShort: '隐藏',
        newColumns: '新增列'
    },
    Filter: {
        applyFilter: 'Apply filter',
        filter: 'Filter',
        editFilter: 'Edit filter',
        on: 'On',
        before: 'Before',
        after: 'After',
        equals: 'Equals',
        lessThan: 'Less than',
        moreThan: 'More than',
        removeFilter: 'Remove filter'
    },
    FilterBar: {
        enableFilterBar: 'Show filter bar',
        disableFilterBar: 'Hide filter bar'
    },
    Group: {
        group: 'Group',
        groupAscending: 'Group ascending',
        groupDescending: 'Group descending',
        groupAscendingShort: 'Ascending',
        groupDescendingShort: 'Descending',
        stopGrouping: 'Stop grouping',
        stopGroupingShort: 'Stop'
    },
    Search: {
        searchForValue: 'Search for value'
    },
    Sort: {
        sort: '排序',
        sortAscending: '升序',
        sortDescending: '降序',
        multiSort: 'Multi sort',
        removeSorter: 'Remove sorter',
        addSortAscending: 'Add ascending sorter',
        addSortDescending: 'Add descending sorter',
        toggleSortAscending: 'Change to ascending',
        toggleSortDescending: 'Change to descending',
        sortAscendingShort: 'Ascending',
        sortDescendingShort: 'Descending',
        removeSorterShort: 'Remove',
        addSortAscendingShort: '+ Ascending',
        addSortDescendingShort: '+ Descending'
    },
    GridBase: {
        loadFailedMessage: '数据加载失败!',
        syncFailedMessage: 'Data synchronization failed!',
        unspecifiedFailure: 'Unspecified failure',
        networkFailure: 'Network error',
        parseFailure: 'Failed to parse server response',
        noRows: '无数据!',
        moveColumnLeft: 'Move to left section',
        moveColumnRight: 'Move to right section',
        removeRow: 'Delete',
        moveColumnTo: region => `Move column to ${region}`
    },
    CellMenu: {
        removeRow: 'Delete'
    },
    RowCopyPaste: {
        copyRecord: 'Copy',
        cutRecord: 'Cut',
        pasteRecord: 'Paste'
    },
    PdfExport: {
        'Waiting for response from server': 'Waiting for response from server...',
        'Export failed': 'Export failed',
        'Server error': 'Server error',
        'Generating pages': 'Generating pages...'
    },
    ExportDialog: {
        width: '40em',
        labelWidth: '12em',
        exportSettings: 'Export settings',
        export: 'Export',
        exporterType: 'Control pagination',
        cancel: 'Cancel',
        fileFormat: 'File format',
        rows: 'Rows',
        alignRows: 'Align rows',
        columns: 'Columns',
        paperFormat: 'Paper format',
        orientation: 'Orientation',
        repeatHeader: 'Repeat header'
    },
    ExportRowsCombo: {
        all: 'All rows',
        visible: 'Visible rows'
    },
    ExportOrientationCombo: {
        portrait: 'Portrait',
        landscape: 'Landscape'
    },
    SinglePageExporter: {
        singlepage: 'Single page'
    },
    MultiPageExporter: {
        multipage: 'Multiple pages',
        exportingPage: ({ currentPage, totalPages }) => `Exporting page ${currentPage}/${totalPages}`
    },
    MultiPageVerticalExporter: {
        multipagevertical: 'Multiple pages (vertical)',
        exportingPage: ({ currentPage, totalPages }) => `Exporting page ${currentPage}/${totalPages}`
    },
    //#endregion grid    

    //#region Scheduler
    ResourceInfoColumn: {
        eventCountText: function (data) {
            return data + ' event' + (data !== 1 ? 's' : '');
        }
    },
    Dependencies: {
        from: 'From',
        to: 'To',
        valid: 'Valid',
        invalid: 'Invalid'
    },
    DependencyType: {
        SS: 'SS',
        SF: 'SF',
        FS: 'FS',
        FF: 'FF',
        StartToStart: 'Start-to-Start',
        StartToEnd: 'Start-to-Finish',
        EndToStart: 'Finish-to-Start',
        EndToEnd: 'Finish-to-Finish',
        short: [
            'SS',
            'SF',
            'FS',
            'FF'
        ],
        long: [
            'Start-to-Start',
            'Start-to-Finish',
            'Finish-to-Start',
            'Finish-to-Finish'
        ]
    },
    DependencyEdit: {
        From: 'From',
        To: 'To',
        Type: 'Type',
        Lag: 'Lag',
        'Edit dependency': 'Edit dependency',
        Save: '保存',
        Delete: 'Delete',
        Cancel: '删除',
        StartToStart: 'Start to Start',
        StartToEnd: 'Start to End',
        EndToStart: 'End to Start',
        EndToEnd: 'End to End'
    },
    EventEdit: {
        Name: 'Name',
        Resource: 'Resource',
        Start: 'Start',
        End: 'End',
        Save: '保存',
        Delete: '删除',
        Cancel: 'Cancel',
        'Edit event': 'Edit event',
        Repeat: 'Repeat'
    },
    EventDrag: {
        eventOverlapsExisting: '计划与该资源的现有计划相重叠',
        noDropOutsideTimeline: '计划不能在时间轴之外'
    },
    SchedulerBase: {
        'Add event': '添加计划',
        'Delete event': '删除计划',
        'Unassign event': '取消计划'
    },
    HeaderContextMenu: {
        pickZoomLevel: '缩放',
        activeDateRange: '日期范围',
        startText: '开始时间',
        endText: '结束时间',
        todayText: '当天'
    },
    TimeAxisHeaderMenu: {
        pickZoomLevel: '缩放',
        activeDateRange: '日期范围',
        startText: '开始时间',
        endText: '结束时间',
        todayText: '当天'
    },
    EventCopyPaste: {
        copyEvent: '复制计划',
        cutEvent: '剪切计划',
        pasteEvent: '粘贴计划'
    },
    EventFilter: {
        filterEvents: '查找计划',
        byName: '名称'
    },
    TimeRanges: {
        showCurrentTimeLine: '显示当前的时间线'
    },
    PresetManager: {
        secondAndMinute: {
            topDateFormat: 'MM月DD日 HH:mm'
        },
        minuteAndHour: {
            topDateFormat: 'MM月DD日 HH:mm'
        },
        hourAndDay: {
            topDateFormat: 'YYYY年MM月DD日'
        },
        weekAndDay: {
            middleDateFormat: 'YYYY年MM月',
            bottomDateFormat: 'DD',
        },
        weekAndMonth: { topDateFormat: 'YYYY年MM月', middleDateFormat: 'DD日' },
        weekDateAndMonth: { topDateFormat: 'YYYY年MM月' },
        weekAndDayLetter: { topDateFormat: 'YYYY年MM月DD日' },
        monthAndYear: { topDateFormat: 'YYYY年', middleDateFormat: 'MM月' },
        year: { topDateFormat: 'YYYY年', middleDateFormat: 'MM月' },
        manyYears: { middleDateFormat: 'YYYY年' }
    },
    RecurrenceConfirmationPopup: {
        'delete-title': '你正在删除一个计划',
        'delete-all-message': 'Do you want to delete all occurrences of this event?',
        'delete-further-message': 'Do you want to delete this and all future occurrences of this event, or only the selected occurrence?',
        'delete-further-btn-text': 'Delete All Future Events',
        'delete-only-this-btn-text': 'Delete Only This Event',

        'update-title': 'You are changing a repeating event',
        'update-all-message': 'Do you want to change all occurrences of this event?',
        'update-further-message': 'Do you want to change only this occurrence of the event, or this and all future occurrences?',
        'update-further-btn-text': 'All Future Events',
        'update-only-this-btn-text': 'Only This Event',

        Yes: 'Yes',
        Cancel: 'Cancel',

        width: 600
    },
    RecurrenceLegend: {
        ' and ': ' and ',
        // frequency patterns
        Daily: 'Daily',
        // Weekly on Sunday
        // Weekly on Sun, Mon and Tue
        'Weekly on {1}': ({ days }) => `Weekly on ${days}`,
        // Monthly on 16
        // Monthly on the last weekday
        'Monthly on {1}': ({ days }) => `Monthly on ${days}`,
        // Yearly on 16 of January
        // Yearly on the last weekday of January and February
        'Yearly on {1} of {2}': ({ days, months }) => `Yearly on ${days} of ${months}`,
        // Every 11 days
        'Every {0} days': ({ interval }) => `Every ${interval} days`,
        // Every 2 weeks on Sunday
        // Every 2 weeks on Sun, Mon and Tue
        'Every {0} weeks on {1}': ({ interval, days }) => `Every ${interval} weeks on ${days}`,
        // Every 2 months on 16
        // Every 2 months on the last weekday
        'Every {0} months on {1}': ({ interval, days }) => `Every ${interval} months on ${days}`,
        // Every 2 years on 16 of January
        // Every 2 years on the last weekday of January and February
        'Every {0} years on {1} of {2}': ({ interval, days, months }) => `Every ${interval} years on ${days} of ${months}`,
        // day position translations
        position1: 'the first',
        position2: 'the second',
        position3: 'the third',
        position4: 'the fourth',
        position5: 'the fifth',
        'position-1': 'the last',
        // day options
        day: 'day',
        weekday: 'weekday',
        'weekend day': 'weekend day',
        // {0} - day position info ("the last"/"the first"/...)
        // {1} - day info ("Sunday"/"Monday"/.../"day"/"weekday"/"weekend day")
        // For example:
        //  "the last Sunday"
        //  "the first weekday"
        //  "the second weekend day"
        daysFormat: ({ position, days }) => `${position} ${days}`
    },
    RecurrenceEditor: {
        'Repeat event': 'Repeat event',
        Cancel: '取消',
        Save: '保存',
        Frequency: 'Frequency',
        Every: 'Every',
        DAILYintervalUnit: 'day(s)',
        WEEKLYintervalUnit: 'week(s)',
        MONTHLYintervalUnit: 'month(s)',
        YEARLYintervalUnit: 'year(s)',
        Each: 'Each',
        'On the': 'On the',
        'End repeat': 'End repeat',
        'time(s)': 'time(s)'
    },
    RecurrenceDaysCombo: {
        day: 'day',
        weekday: 'weekday',
        'weekend day': 'weekend day'
    },
    RecurrencePositionsCombo: {
        position1: 'first',
        position2: 'second',
        position3: 'third',
        position4: 'fourth',
        position5: 'fifth',
        'position-1': 'last'
    },
    RecurrenceStopConditionCombo: {
        Never: 'Never',
        After: 'After',
        'On date': 'On date'
    },
    RecurrenceFrequencyCombo: {
        Daily: 'Daily',
        Weekly: 'Weekly',
        Monthly: 'Monthly',
        Yearly: 'Yearly'
    },
    RecurrenceCombo: {
        None: 'None',
        Custom: 'Custom...'
    },
    //region Features
    Summary: {
        'Summary for': date => `Summary for ${date}`
    },
    //endregion
    //region Export
    ScheduleRangeCombo: {
        completeview: 'Complete schedule',
        currentview: 'Visible schedule',
        daterange: 'Date range',
        completedata: 'Complete schedule (for all events)'
    },
    SchedulerExportDialog: {
        'Schedule range': 'Schedule range',
        'Export from': 'From',
        'Export to': 'To'
    },
    ExcelExporter: {
        'No resource assigned': 'No resource assigned'
    },
    //endregion
    CrudManagerView: {
        serverResponseLabel: 'Server response:'
    },
    //#endregion Scheduler

    //#region SchedulerPro
    ConstraintTypePicker: {
        none: 'None',
        muststarton: 'Must start on',
        mustfinishon: 'Must finish on',
        startnoearlierthan: 'Start no earlier than',
        startnolaterthan: 'Start no later than',
        finishnoearlierthan: 'Finish no earlier than',
        finishnolaterthan: 'Finish no later than'
    },
    CalendarField: {
        'Default calendar': '默认日历'
    },
    TaskEditorBase: {
        Information: '信息',
        Save: '保存',
        Cancel: '取消',
        Delete: '删除',
        calculateMask: '加载中...',
        saveError: '无法保存，请先纠正错误'
    },
    TaskEdit: {
        'Edit task': '编辑计划',
        ConfirmDeletionTitle: '确认删除',
        ConfirmDeletionMessage: '你确定要删除该计划吗？'
    },
    GanttTaskEditor: {
        editorWidth: '44em'
    },
    SchedulerTaskEditor: {
        editorWidth: '32em'
    },
    SchedulerGeneralTab: {
        labelWidth: '6em',
        General: '基本信息',
        Name: '名称',
        Resources: '生产资源',
        '% complete': '百分比(%)',
        Duration: '时长',
        Start: '开始时间',
        Finish: '结束时间'
    },
    GeneralTab: {
        labelWidth: '6.5em',
        General: 'General',
        Name: 'Name',
        '% complete': '% complete',
        Duration: 'Duration',
        Start: 'Start',
        Finish: 'Finish',
        Effort: 'Effort',
        Dates: 'Dates'
    },
    SchedulerAdvancedTab: {
        labelWidth: '13em',
        Calendar: 'Calendar',
        Advanced: 'Advanced',
        'Manually scheduled': 'Manually scheduled',
        'Constraint type': 'Constraint type',
        'Constraint date': 'Constraint date',
        Inactive: 'Inactive'
    },
    AdvancedTab: {
        labelWidth: '11.5em',
        Advanced: 'Advanced',
        Calendar: 'Calendar',
        'Scheduling mode': 'Scheduling mode',
        'Effort driven': 'Effort driven',
        'Manually scheduled': 'Manually scheduled',
        'Constraint type': 'Constraint type',
        'Constraint date': 'Constraint date',
        Constraint: 'Constraint',
        Rollup: 'Rollup',
        Inactive: 'Inactive'
    },
    DependencyTab: {
        Predecessors: 'Predecessors',
        Successors: 'Successors',
        ID: 'ID',
        Name: 'Name',
        Type: 'Type',
        Lag: 'Lag',
        cyclicDependency: 'Cyclic dependency',
        invalidDependency: 'Invalid dependency'
    },
    NotesTab: {
        Notes: 'Notes'
    },
    ResourcesTab: {
        unitsTpl: ({ value }) => `${value}%`,
        Resources: 'Resources',
        Resource: 'Resource',
        Units: 'Units'
    },
    SchedulingModePicker: {
        Normal: 'Normal',
        'Fixed Duration': 'Fixed Duration',
        'Fixed Units': 'Fixed Units',
        'Fixed Effort': 'Fixed Effort'
    },
    ResourceHistogram: {
        barTipInRange: '<b>{resource}</b> {startDate} - {endDate}<br><span class="{cls}">{allocated} of {available}</span> allocated',
        barTipOnDate: '<b>{resource}</b> on {startDate}<br><span class="{cls}">{allocated} of {available}</span> allocated',
        groupBarTipAssignment: '<b>{resource}</b> - <span class="{cls}">{allocated} of {available}</span>',
        groupBarTipInRange: '{startDate} - {endDate}<br><span class="{cls}">{allocated} of {available}</span> allocated:<br>{assignments}',
        groupBarTipOnDate: 'On {startDate}<br><span class="{cls}">{allocated} of {available}</span> allocated:<br>{assignments}',
        plusMore: '+{value} more'
    },
    DurationColumn: {
        Duration: 'Duration'
    }
    //#endregion SchedulerPro
}