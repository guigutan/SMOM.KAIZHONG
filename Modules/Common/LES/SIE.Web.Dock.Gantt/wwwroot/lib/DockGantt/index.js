var orderTypeData = new bryntum.schedulerpro.AjaxStore({
    readUrl: "/Dock/LoadGantt/GetOrderTypeData",
    autoLoad: true,
});

var parkData = new bryntum.schedulerpro.AjaxStore({
    readUrl: "/Dock/LoadGantt/GetParks",
    autoLoad: true,
});

var warehouseData = new bryntum.schedulerpro.AjaxStore({
    readUrl: "/Dock/LoadGantt/GetWarehouses",
    autoLoad: true,
});

let park = "", warehouse = "", states = 1, startDate = new Date(), endDate = new Date().setDate(new Date().getDate() + 6), taskEditEvent;
let appointNo = "", orderNo = "";
const scheduler = new bryntum.schedulerpro.SchedulerPro({
    enableDeleteKey: false, //防止键盘按[Delete]和[Backspace]钮删除计划
    appendTo: 'container',
    eventStyle: 'border',
    zoomOnTimeAxisDoubleClick: false,
    zoomOnMouseWheel: false,
    features: {
        cellMenu: false,
        timeRanges: {
            showCurrentTimeLine: {
                name: ''
            }
        },
        resourceTimeRanges: {
            enableMouseEvents: true
        },
        dependencies: false,
        taskEdit: {
            //编辑器配置
            editorConfig: {
                bbar: {
                    items: {
                        deleteButton: false,
                    },
                },
            },
            items: {
                generalTab: {
                    title: '基础数据',
                    items: {
                        percentDoneField: false,
                        endDateField: null,
                        startDateField: null,
                        resourcesField: null,
                        nameField: false,
                        orderNo: {
                            type: 'textfield',
                            name: 'orderNo',
                            label: '预约号',
                            labelWidth: 50,
                            weight: 110,
                            flex: '1 0 50%',
                            disabled: true,
                            readOnly: true,
                        },
                        orderType: {
                            type: 'combo',
                            name: 'orderType',
                            displayField: 'text',
                            valueField: 'value',
                            label: '预约类型',
                            labelWidth: 50,
                            weight: 110,
                            flex: '1 0 50%',
                            listeners: {
                                change: (event) => {
                                    if (event.value != undefined && event.oldValue != null) {
                                        taskEditEvent.orderType = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }

                        },
                        billNo: {
                            type: 'textfield',
                            name: 'billNo',
                            label: '单据号',
                            labelWidth: 50,
                            required: true,
                            flex: '1 0 50%',
                            weight: 110,
                            listeners: {
                                change: (event) => {
                                    if (event.value != "" && event.oldValue != "") {
                                        taskEditEvent.billNo = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }

                        },
                        orderPlace: {
                            type: 'text',
                            name: 'orderPlace',
                            label: '预约地点',
                            labelWidth: 50,
                            disabled: true,
                            flex: '1 0 50%',
                            weight: 110,
                            listeners: {
                                change: ({ source: field }) => {
                                    field.parent.widgetMap.orderPlace.disabled = true;
                                }
                            }
                        },

                        corporate: {
                            type: 'text',
                            name: 'name',
                            label: '公司名称',
                            value: '',
                            labelWidth: 50,
                            flex: '1 0 50%',
                            weight: 110,
                            listeners: {
                                change: (event) => {
                                    if (event.value == "New") {
                                        event.source.parent.widgetMap.corporate.value = "";
                                    }
                                    if (event.value != "" && event.oldValue != "") {
                                        taskEditEvent.corporate = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }
                        },
                        carNumber: {
                            type: 'textfield',
                            name: 'carNumber',
                            label: '车牌号',
                            labelWidth: 50,
                            flex: '1 0 50%',
                            weight: 110,
                            required: true,
                            listeners: {
                                change: (event) => {
                                    if (event.value != "" && event.oldValue != "") {
                                        taskEditEvent.carNumber = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }
                        },
                        contacts: {
                            type: 'textfield',
                            name: 'contacts',
                            label: '联系人',
                            labelWidth: 50,
                            flex: '1 0 50%',
                            labelWidth: 50,
                            weight: 110,
                            required: true,
                            listeners: {
                                change: (event) => {
                                    if (event.value != "" && event.oldValue != "") {
                                        taskEditEvent.contacts = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }
                        },
                        phone: {
                            type: 'textfield',
                            name: 'phone',
                            label: '联系电话',
                            labelWidth: 50,
                            flex: '1 0 50%',
                            weight: 110,
                            required: true,
                            listeners: {
                                change: (event) => {
                                    if (event.value != "" && event.oldValue != "") {
                                        taskEditEvent.phone = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }
                        },
                        identity: {
                            type: 'text',
                            name: 'identity',
                            label: '身份证',
                            labelWidth: 50,
                            weight: 110,
                            required: true,
                            cls: 'b-inline',
                            listeners: {
                                change: (event) => {

                                    if (event.value != "" && event.oldValue != "") {
                                        taskEditEvent.identity = event.value;
                                        taskEditEvent.operateType = "modify";
                                    }
                                }
                            }
                        },
                        orderDate: {
                            type: 'datefield',
                            name: 'orderDate',
                            label: '预约时间',
                            flex: '1 0 50%',
                            labelWidth: 50,
                            weight: 110,
                            format: "YYYY/MM/DD",
                            disabled: true,
                            listeners: {
                                change: ({ source: field }) => {
                                    field.parent.widgetMap.orderDate.disabled = field.value;
                                }
                            }

                        },
                        orderTime: {
                            type: 'text',
                            name: 'orderTime',
                            label: '预约时段',
                            flex: '1 0 50%',
                            labelWidth: 50,
                            weight: 110,
                            listeners: {
                                change: ({ source: field }) => {
                                    field.parent.widgetMap.orderTime.disabled = field.value;
                                }
                            }
                        },
                        durationField: {
                            label: "预计占用",
                            labelWidth: 50,
                            flex: '1 0 50%',
                            durationUnit: "hour",
                            listeners: {
                                change: ({ source: field }) => {
                                    field.parent.widgetMap.durationField.disabled = field.value;
                                }
                            }
                        }

                    }
                },
                notesTab: false,
                predecessorsTab: false,
                successorsTab: false,
                advancedTab: false
            },
        },
        //菜单
        eventMenu: {
            items: {
                deleteEvent: false,
                unassignEvent: false,
                copyEvent: false,
                cutEvent: false,
                delete: {
                    text: '取消预约',
                    icon: 'b-fa b-fa-fw b-fa-trash',
                    onItem({ eventRecord, source }) {
                        if (eventRecord.isScheduling) {
                            eventRecord.startDate = "0001-01-01 00:00:00";
                            eventRecord.endDate = "0001-01-01 00:00:00";
                            eventRecord.operateType = DBOperateType.Delete;
                        } else {
                            eventRecord.resourceNewId = -1;
                            eventRecord.operateType = "delete";
                            eventRecord.unassign()
                        }
                    }
                },
            }
        },

        //排产器菜单
        scheduleMenu: {
            items: {
                addEvent: false
            }
        },
        cellMenu: {
            items: {
                removeRow: false
            },

        },
        resourceNonWorkingTime: true,//显示
        eventDrag: {
            validatorFn({ eventRecords, newResource }) {
                const task = eventRecords[0];
                let valid = true;
                task.message = "注意：";
                if (task.orderType == 0 && newResource.type != "in" && newResource.type != "all") {
                    valid = false;
                }
                if (task.orderType == 1 && newResource.type != "out" && newResource.type != "all") {
                    valid = false;
                }
                if (!valid) {
                    task.message += "<div class='messageDrag'>不同月台不可以移动</div>";
                }
                if (new Date(task.data.startDate) < new Date()) {
                    task.message += "<div class='messageDrag'>已经开始的预约不可移动</div>";
                    valid = false;
                }
                if (newResource.state != "ON") {
                    task.message += "<div class='messageDrag'>月台已被禁用</div>";
                    valid = false;
                }
                return {
                    valid: valid,
                    message: !valid ? task.message : ''
                };
            }
        }
    },

    columns: [
        { text: '分组', field: 'groupName', editor: false, mergeCells: true },
        {
            text: '月台', field: 'name', editor: false, width: 150,
            renderer({ cellElement, record, row }) {
                var backgroundColor = "#fff";
                var color = '#fff';
                var html = '<div class="color-box-type">' + record.name + '</div>';
                switch (record.type) {
                    case "all":
                        backgroundColor = "#61dda9";
                        html += '<i class="b-fa b-fa-arrow-up" role="presentation" style="margin-right: 0.5em;"></i><i class="b-fa b-fa-arrow-down" role="presentation" style="margin-right: 0.5em;"></i>';
                        break;
                    case "in":
                        backgroundColor = "#6fbef4";
                        html += '<i class="b-fa b-fa-arrow-up" role="presentation" style="margin-right: 0.5em;"></i>';
                        break;

                    case "out":
                        backgroundColor = "#eacb5e";
                        html += '<i class="b-fa b-fa-arrow-down" role="presentation" style="margin-right: 0.5em;"></i>';
                        break;
                    default:
                        color = "#000";
                        break;
                }
                cellElement.style.color = color;
                cellElement.style.backgroundColor = backgroundColor;
                cellElement.innerHTML = html;
            }
        },
        {
            editor: false,
            text: '状态',
            field: 'state',
            width: 60,
            cls: 'color',
            flex: 1,
            htmlEncode: false,
            alwaysClearCell: false,            
            renderer: ({ value, record, cellElement }) => {
                cellElement.innerHTML = '<div class="color-box"></div><div>'+value+'</div>';
                if (value == "ON") {
                    cellElement.firstElementChild.style.backgroundColor = "#3bb18e";
                    cellElement.style.color = "#606263";
                } else {
                    cellElement.firstElementChild.style.backgroundColor = "#d45139";
                    cellElement.style.color = "Red";
                }

            }
        }
    ],

    tbar: [{
        label: '园区',
        type: 'combo',
        editable: false,
        items: parkData,
        onSelect({ record }) {
            park = record.data.value;
        }
    },
    {
        label: '仓库',
        type: 'combo',
        editable: false,
        items: warehouseData,
        onSelect({ record }) {
            warehouse = record.data.value;
        }
    },
    {
        label: '状态',
        type: 'combo',
        editable: false,
        items: [
            ['null', ''],
            ['1', '可用'],
            ['0', '禁用']
        ],
        value: '可用',
        onSelect({ record }) {
            states = record.data.value;
        }
    },
    {
        label: '日期',
        type: 'date',
        format: "YYYY-MM-DD",
        value: new Date(),
        onChange({ value }) {
            startDate = value;
        }

    },
    {
        label: '至',
        type: 'date',
        format: "YYYY-MM-DD",
        value: new Date().setDate(new Date().getDate() + 6),
        onChange({ value }) {
            endDate = value;
        }
    },
    {
        label: '预约号',
        type: 'text',
        keyStrokeChangeDelay: 100,
        onChange({ value }) {
            value = value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
            appointNo = value;
        }
    },
    {
        label: '单据号',
        type: 'text',
        keyStrokeChangeDelay: 100,
        onChange({ value }) {
            value = value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
            orderNo = value;
        }
    },
    {
        type: 'button',
        ref: 'search',
        icon: 'b-fa-search',
        text: '查找',
        onClick: ({ value }) => {
            var state = compareDate(startDate, endDate);
            if (state) {
                scheduler.project.transport.load.url = '/Dock/LoadGantt/LoadData'
                scheduler.project.transport.load.params = {
                    yardMaintainId: park,
                    warehouseId: warehouse,
                    state: states,
                    beginDate: bryntum.schedulerpro.DateHelper.format(startDate, 'YYYY-MM-DD 00:00:00'),
                    endDate: bryntum.schedulerpro.DateHelper.format(new Date(endDate), 'YYYY-MM-DD 23:59:59'),
                    appointNo: appointNo,
                    billNo: orderNo,
                }
                scheduler.startDate = bryntum.schedulerpro.DateHelper.format(startDate, 'YYYY-MM-DD 00:00:00'),
                    scheduler.endDate = bryntum.schedulerpro.DateHelper.format(new Date(endDate), 'YYYY-MM-DD 23:59:59'),
                    scheduler.project.load()
            }
        },
    },
    {
        type: 'button',
        ref: 'save',
        icon: 'b-fa-save',
        cls: 'b-blue',
        text: '保存',
        onClick() {
            var eventData = scheduler.eventStore.chain(x => x.operateType != "UnChanged" && !(x.operateType == "delete" && !x.id.indexOf("_generatedClassDefEx")));
            const formData = new FormData();
            if (eventData.count <= 0) {
                bryntum.schedulerpro.Mask.mask({
                    text: "没有可以保存的数据!",
                });
                setTimeout(() => {
                    bryntum.schedulerpro.Mask.unmask();
                }, 3000);
            } else {
                var params = getParamsData(eventData);
                var data = JSON.stringify(params);
                formData.append('contents', data);
                bryntum.schedulerpro.AjaxHelper.post('../SaveGantt/SaveOrderGanttData', formData, { parseJson: true }).then(
                    response => {
                        if (response.parsedJson.Success) {
                            scheduler.project.load();
                        }
                        bryntum.schedulerpro.Mask.mask({
                            text: response.parsedJson.Message,
                        });
                        setTimeout(() => {
                            bryntum.schedulerpro.Mask.unmask();
                        }, 3000);
                    }
                );
            }
        },

    }],

    project: {
        autoLoad: true,
        eventStore: {
            fields: [
                { name: 'durationUnit', defaultValue: 'h' }
            ] //计划占用时间以小时计算
        },
        transport: {
            load: {
                url: "/Dock/LoadGantt/LoadData",
                params: {
                    yardMaintainId: park,
                    warehouseId: warehouse,
                    state: states,
                    beginDate: bryntum.schedulerpro.DateHelper.format(new Date(startDate), 'YYYY-MM-DD 00:00:00'),
                    endDate: bryntum.schedulerpro.DateHelper.format(new Date(endDate), 'YYYY-MM-DD 23:59:59'),
                    appointNo: appointNo,
                    billNo: orderNo,
                },
            },

        },

    },

    startDate: bryntum.schedulerpro.DateHelper.format(new Date(startDate), 'YYYY-MM-DD 00:00:00'),
    endDate: bryntum.schedulerpro.DateHelper.format(new Date(endDate), 'YYYY-MM-DD 23:59:59'),
    viewPreset: 'minuteAndHour',  //日历显示模式
});
scheduler.localeManager.locale = 'Zh'; //本地化

//新增与修改计划
scheduler.on('beforeTaskEdit', (event) => {
    const widgetMap = event.taskEdit.editor.widgetMap;
    if (event.taskRecord.locked) {
        widgetMap.saveButton.hidden = true;
    } else {
        widgetMap.saveButton.hidden = false;
    }
    if (event.taskRecord.durationUnit == "minute") {
        event.taskRecord.durationUnit = "hour";
        event.taskRecord.duration = 0.5;
        widgetMap.orderDate.readOnly = true;
        widgetMap.orderTime.readOnly = true;
        widgetMap.orderPlace.readOnly = true;
        widgetMap.durationField.readOnly = true;
    }
    if (event.taskRecord.orderNo == undefined) {
        widgetMap.orderNo.hidden = true;
        widgetMap.orderDate.hidden = true;
        widgetMap.orderTime.hidden = true;
        widgetMap.orderPlace.hidden = true;
    } else {
        widgetMap.orderNo.hidden = false;
        widgetMap.orderDate.hidden = false;
        widgetMap.orderTime.hidden = false;
        widgetMap.orderPlace.hidden = false;
    }
});




scheduler.on('beforeTaskEditShow', ({ editor, taskRecord }) => {
    taskEditEvent = taskRecord;
    const disabledAdvancedFields = ['orderNo', 'orderDate', 'orderTime', 'orderPlace']
    disabledAdvancedFields.forEach(field => {
        editor.widgetMap[field].disabled = true;
    });
    
    editor.widgetMap.orderType.items = orderTypeData;
    switch (taskRecord.resource.type) {
        case "in":
            editor.widgetMap.orderType.disabled = true;
            editor.widgetMap.orderType.readOnly = true;
            editor.widgetMap.orderType.hidden = true;
            break;
        case "out":
            editor.widgetMap.orderType.disabled = true;
            editor.widgetMap.orderType.readOnly = true;
            editor.widgetMap.orderType.hidden = true;
            break;
        default:
            editor.widgetMap.orderType.disabled = false;
            editor.widgetMap.orderType.readOnly = false;
            editor.widgetMap.orderType.hidden = false;
    }
    
}),

    scheduler.on('eventdrop', (event) => {
        //拖动后自动前拖后推
        var draggedTask = event.eventRecords[0];//当前的计划
        var resource = draggedTask.resourceStore._data.filter(x => x.id == draggedTask.resourceId)[0].calendar;//获取当前计划使用的日历
        var calendar = draggedTask.calendarManagerStore._data.filter(x => x.id == resource)[0].intervals;//获取日历详细中那一段
        calendar.forEach(e => {
            e.recurrentStartDate = e.recurrentStartDate.replace("at ", "");
            e.recurrentEndDate = e.recurrentEndDate.replace("at ", "");
        });
        //当前计划的时间
        const newStartDateTime = bryntum.schedulerpro.DateHelper.format(draggedTask.startDate, 'HH:mm:ss');
        //当前计划的日期 
        const newStartDate = bryntum.schedulerpro.DateHelper.format(draggedTask.startDate, 'YYYY-MM-DD');

        var oDate1 = new Date();
        var oDate2 = new Date(draggedTask.startDate);

        //获取当前计划所在的时间段
        const CalendarRight = calendar.filter((ev) => ev.recurrentStartDate <= newStartDateTime && ev.recurrentEndDate >= newStartDateTime);    // 降序
        if (CalendarRight.length > 0 && oDate1.getTime() <= oDate2.getTime()) {
            const newrecurrentStartDate = newStartDate + " " + CalendarRight[0].recurrentStartDate + ":00";  //设置时间段范围的开始时间
            const newrecurrentEndDate = newStartDate + " " + CalendarRight[0].recurrentEndDate + ":00"; //设置时间段范围的结束时间
            const futureEvents = event.targetResourceRecord.events;//当前资源所有计划
            //获取当前资源中所时间段计划，不包括当前移动的计划
            const filterNew = futureEvents.filter((ev) => ev.startDateRange == newrecurrentStartDate && ev.endDateRange == newrecurrentEndDate && ev.id != draggedTask.id).sort(function (a, b) { return a.startDate - b.startDate })  // 按照日期 升序
            var eventDate = newrecurrentStartDate;//当前资源按程的最后结束时间，可以做为当前移动计划的开始时间

            if (filterNew.length > 0) { //当前时间段是否其他计划
                filterNew.forEach((ev, i = 0) => {
                    if (i == 0)
                        ev.startDate = newrecurrentStartDate;
                    else
                        ev.startDate = filterNew[i - 1].endDate;  //当前计划前
                    ev.endDate = bryntum.schedulerpro.DateHelper.add(ev.startDate, ev.duration, 'hour');
                    ev.operateType = "modify";
                    if (bryntum.schedulerpro.DateHelper.format(ev.endDate, "YYYY-MM-DD HH:mm:ss") > eventDate)  //判断当前计划的结束时间是否大于时间段的结束时间，如果大于结束时间，当前计划的结束时间=时间段的结束时间
                        eventDate = bryntum.schedulerpro.DateHelper.format(ev.endDate, "YYYY-MM-DD HH:mm:ss");
                });
                if (newrecurrentEndDate > eventDate) {    //判断排程后的计划是否有空间排程当前的计划。
                    var endDate1 = bryntum.schedulerpro.DateHelper.format(bryntum.schedulerpro.DateHelper.add(eventDate, draggedTask.duration, 'hour'));
                    if (endDate1 > newrecurrentEndDate) {
                        var datefield = GetDateDiff(eventDate, newrecurrentEndDate, "minute");
                        event.context.valid = false;
                        bryntum.schedulerpro.MessageDialog.alert({
                            title: '提示',
                            message: '当前区域，只能放置' + datefield + "分钟"
                        });
                        //如果没有空间，返回计划。   
                        const startDate = event.eventRecords[0].originalData.startDate;
                        const duration = event.eventRecords[0].originalData.duration;
                        const resourceId = event.eventRecords[0].originalData.resourceId;
                        const endDate = bryntum.schedulerpro.DateHelper.add(startDate, duration, 'hour');
                        draggedTask.startDate = startDate;
                        draggedTask.endDate = endDate;
                        draggedTask.resourceId = resourceId;
                        event.context.valid = false;
                    } else {
                        draggedTask.startDate = eventDate;
                        draggedTask.endDate = endDate1;
                        draggedTask.startDateRange = newrecurrentStartDate;
                        draggedTask.endDateRange = newrecurrentEndDate;
                        draggedTask.operateType = "modify";
                    }
                } else {
                    //如果没有空间，返回计划。
                    const startDate = event.eventRecords[0].originalData.startDate;
                    const duration = event.eventRecords[0].originalData.duration;
                    const resourceId = event.eventRecords[0].originalData.resourceId;
                    const endDate = bryntum.schedulerpro.DateHelper.add(startDate, duration, 'hour');
                    draggedTask.startDate = startDate;
                    draggedTask.endDate = endDate;
                    draggedTask.resourceId = resourceId;
                    event.context.valid = false;
                }
            } else {
                //当前时间段没有其他计划
                draggedTask.startDate = newrecurrentStartDate;
                draggedTask.endDate = bryntum.schedulerpro.DateHelper.add(newrecurrentStartDate, draggedTask.duration, 'hour');
                draggedTask.startDateRange = newrecurrentStartDate;
                draggedTask.endDateRange = newrecurrentEndDate;
                draggedTask.operateType = "modify";
                //draggedTask.resourceId = event.eventRecords[0].resourceId;
            }
        } else {
            console.log("false", draggedTask.startDate, draggedTask.endDate);
            var startDate = event.eventRecords[0].originalData.startDate;
            var duration = event.eventRecords[0].originalData.duration;
            var resourceId = event.eventRecords[0].originalData.resourceId;
            var endDate = bryntum.schedulerpro.DateHelper.add(startDate, duration, 'hour');
            draggedTask.startDate = startDate;
            draggedTask.endDate = endDate;
            draggedTask.resourceId = resourceId;
            event.context.valid = false;
        }
    });

//调整计划大小
scheduler.on('eventResizeEnd', ({ source, eventRecord }) => {
    const startDateRange = eventRecord.startDateRange || bryntum.schedulerpro.DateHelper.format(eventRecord.startDate, "YYYY-MM-DD HH:mm:ss");
    const endDateRange = eventRecord.endDateRange || bryntum.schedulerpro.DateHelper.format(eventRecord.endDate, "YYYY-MM-DD HH:mm:ss");;
    const cutEventEndDate = bryntum.schedulerpro.DateHelper.format(eventRecord.endDate, "YYYY-MM-DD HH:mm:ss");
    const oldDuration = eventRecord.originalData.duration || eventRecord._data.duration;
    const oldEndDate = bryntum.schedulerpro.DateHelper.add(eventRecord.startDate, oldDuration, 'hour');
    const oldEndDateFormat = bryntum.schedulerpro.DateHelper.format(oldEndDate, "YYYY-MM-DD HH:mm:ss");
    var datefield = GetDateDiff(startDateRange, endDateRange, "minute");
    if (endDateRange < cutEventEndDate) {
        bryntum.schedulerpro.MessageDialog.alert({
            title: '提示',
            message: '当前区域，只能放置' + Math.abs(datefield) + "分钟"
        });
        eventRecord.operateType = "UnChanged";
        eventRecord.endDate = oldEndDate;
    } else {
        var resourceId = eventRecord.resource ? eventRecord.resource.id : eventRecord.originalData.resourceId;
        const futureEvents = source.events;//当前资源所有计划
        //获取当前资源中所时间段计划，不包括当前移动的计划
        const filterNew = futureEvents.filter((ev) => startDate > eventRecord.startDate && ev.startDateRange == startDateRange && ev.endDateRange == endDateRange).sort((a, b) => a.startDate > b.startDate ? 1 : -1);//当前计划后面的计划(包含当前计划)
        var maxEventDate;

        if (filterNew.length > 0) { //当前时间段是否其他计划
            filterNew.forEach((ev, i = 0) => {
                var startDate;
                if (i == 0)
                    startDate = eventRecord.endDate;
                else
                    startDate = filterNew[i - 1].endDate;  //当前计划前
                maxEventDate = bryntum.schedulerpro.DateHelper.add(startDate, ev.duration, 'hour');
            });

            var maxEventDates = bryntum.schedulerpro.DateHelper.format(maxEventDate, "YYYY-MM-DD HH:mm:ss");
            if (maxEventDates > endDateRange) {
                var datefield = oldDuration * 60;
                var datefield1 = GetDateDiff(endDateRange, maxEventDates, "minute");

                bryntum.schedulerpro.MessageDialog.alert({
                    title: '提示',
                    message: '当前区域，只能放置' + (datefield - datefield1) + '分钟'
                });
                //如果没有空间，返回计划。   
                const startDate = eventRecord.originalData.startDate;
                const duration = eventRecord.originalData.duration;
                const endDate = bryntum.schedulerpro.DateHelper.add(startDate, duration, 'hour');
                eventRecord.startDate = startDate;
                eventRecord.endDate = endDate;
                eventRecord.operateType = "UnChanged";
            }

        }

    }
    eventRecord.operateType = "modify";
});

function compareDate(sDate1, sDate2) {    //sDate1和sDate2是2006-12-18格式

    var oDate1 = new Date(sDate1);
    var oDate2 = new Date(sDate2);
    if (oDate1.getTime() > oDate2.getTime()) {
        bryntum.schedulerpro.MessageDialog.alert({
            title: '提示',
            message: '开始时间小于结束时间!'
        });
        return false;
    }
    var aDate, oDate1, oDate2, iDays
    aDate = getFormatDate(sDate1).split("-");
    oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])    //转换为12-18-2006格式  
    aDate = getFormatDate(sDate2).split("-");
    oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])
    iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24)    //把相差的毫秒数转换为天数  
    // 当间隔日期大于 规定的7天
    if (iDays > 7 || iDays < 1) {
        bryntum.schedulerpro.MessageDialog.alert({
            title: '提示',
            message: '数据范围必须[1-7]天的数据!'
        });
        return false;
    }
    return true;
}

function GetDateDiff(startTime, endTime, diffType) {
    //将xxxx-xx-xx的时间格式，转换为 xxxx/xx/xx的格式 
    startTime = startTime.replace(/\-/g, "/");
    endTime = endTime.replace(/\-/g, "/");
    //将计算间隔类性字符转换为小写
    diffType = diffType.toLowerCase();
    var sTime = new Date(startTime); //开始时间
    var eTime = new Date(endTime); //结束时间
    //作为除数的数字
    var timeType = 1;
    switch (diffType) {
        case "second":
            timeType = 1000;
            break;
        case "minute":
            timeType = 1000 * 60;
            break;
        case "hour":
            timeType = 1000 * 3600;
            break;
        case "day":
            timeType = 1000 * 3600 * 24;
            break;
        default:
            break;
    }
    return parseInt((eTime.getTime() - sTime.getTime()) / parseInt(timeType));
}

function getFormatDate(dateData) {
    let date = new Date(dateData);
    let year = date.getFullYear();
    let month = date.getMonth() + 1;
    let day = date.getDate();
    let hour = date.getHours();
    let minutes = date.getMinutes();
    let seconds = date.getSeconds();
    month = (month < 10) ? '0' + month : month;
    day = (day < 10) ? '0' + day : day;
    hour = (hour < 10) ? '0' + hour : hour;
    minutes = (minutes < 10) ? '0' + minutes : minutes;
    seconds = (seconds < 10) ? '0' + seconds : seconds;
    let currentDate = year + "-" + month + "-" + day;
    return currentDate;
}

function getParamsData(eventData) {
    var params = [];
    eventData._data.forEach(function (item) {
        var paramsItem = {
            id: item.id,
            name: item.name,
            startDate: item.startDate,
            endDate: item.endDate,
            durationUnit: item.durationUnit,
            duration: item.duration,
            startDateRange: item.startDateRange,
            endDateRange: item.endDateRange,
            eventColor: item.eventColor,
            resourceId: item.resource ? item.resource.id : item.originalData.resourceId,
            orderType: item.orderType,
            billNo: item.billNo,
            orderPlace: item.orderPlace,
            orderNo: item.orderNo,
            carNumber: item.carNumber,
            contacts: item.contacts,
            phone: item.phone,
            identity: item.identity,
            orderDate: item.orderDate,
            orderTime: item.orderTime,
            operateType: item.operateType,
            locked: item.locked
        }
        params.push(paramsItem);
    });
    return params;
}