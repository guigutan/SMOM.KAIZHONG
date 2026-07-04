Ext.define('SIE.Web.EMS.Equipments.AlarmStates.Scripts.AlarmRecordValueBehavior', {
    _view: null,
    /**
    * view生命周期函数--view生成前
    * @param {*} meta 实体视图元数据
    * @param {*} curEntity 当前操作实体(可空)
    */
    beforeCreate: function (meta, curEntity) {
        var me = this;

        if (!meta) {
            return;
        }

        var cbDateTimeType = new Ext.form.ComboBox({
            name: 'cbResumeState',
            xtype: 'combobox',
            displayField: 'name',
            valueField: 'value',
            store: [
                { value: '0', name: '报警前后10Min'.t() },
                { value: '1', name: '报警前后20Min'.t() },
                { value: '2', name: '报警前后30Min'.t() },
                { value: '3', name: '自定义'.t() },
            ],
            queryMode: 'local',// 数据模式，local代表本地数据                
            editable: false,// 是否允许输入
            forceSelection: true,// 必须选择一个选项
            fieldLabel: '时间段'.t(),
            labelWidth: 50,
            width: 200,
            listeners: {
                select: function (combo, records, eOpts) {
                    //时间段类型变化时，修改时间值
                    me.setDateTime();
                }
            }
        });

        //默认时间段为【报警前后10Min】
        cbDateTimeType.setValue('0');

        var dpBeginDateTime = new SIE.control.DateTimeField({
            name: 'dpBeginDateTime',
            xtype: 'datetimefield',
        });

        var dpEndDateTime = new SIE.control.DateTimeField({
            name: 'dpEndDateTime',
            xtype: 'datetimefield',
        });

        var toolbar = Ext.create('Ext.toolbar.Toolbar', {
            id: "toolbarDateTime",
            dock: 'top',
            width: '100%',
            margin: '5 0 0 0',
            items: [cbDateTimeType, dpBeginDateTime, dpEndDateTime]
        });

        meta.gridConfig.dockedItems.push(toolbar);
    },

    onDataLoaded: function (view) {
        this._view = view;
        this.setDateTime();
    },

    /*
     * 设置根据报警时间和时间类型，设备查看曲线的时间范围
     */
    setDateTime: function () {
        if (this._view == null) {
            return;
        }

        var parentEntity = this._view.getParent().getCurrent();
        var curTime = parentEntity.getAlarmTime();

        var cbResumeState = this._view.getControl().dockedItems.items.first(function (p) {
            return p.xtype == "toolbar" && p.id =="toolbarDateTime"
        }).items.items.first(function (p) {
            return p.name == "cbResumeState"
        });

        var dpBeginDateTime = this._view.getControl().dockedItems.items.first(function (p) {
            return p.xtype == "toolbar" && p.id == "toolbarDateTime"
        }).items.items.first(function (p) {
            return p.name == "dpBeginDateTime"
        });

        var dpEndDateTime = this._view.getControl().dockedItems.items.first(function (p) {
            return p.xtype == "toolbar" && p.id == "toolbarDateTime"
        }).items.items.first(function (p) {
            return p.name == "dpEndDateTime"
        });

        //{ value: '0', name: '报警前后10Min' },
        //{ value: '1', name: '报警前后20Min' },
        //{ value: '2', name: '报警前后30Min' },
        //{ value: '3', name: '自定义' },

        switch (cbResumeState.value) {
            case "0":
                dpBeginDateTime.readOnly = true;
                dpEndDateTime.readOnly = true;
                dpBeginDateTime.setValue(new Date(curTime.getTime() - 10 * 60000));
                dpEndDateTime.setValue(new Date(curTime.getTime() + 10 * 60000));
                break;
            case "1":
                dpBeginDateTime.readOnly = true;
                dpEndDateTime.readOnly = true;
                dpBeginDateTime.setValue(new Date(curTime.getTime() - 20 * 60000));
                dpEndDateTime.setValue(new Date(curTime.getTime() + 20 * 60000));
                break;
            case "2":
                dpBeginDateTime.readOnly = true;
                dpEndDateTime.readOnly = true;
                dpBeginDateTime.setValue(new Date(curTime.getTime() - 30 * 60000));
                dpEndDateTime.setValue(new Date(curTime.getTime() + 30 * 60000));
                break;
            case "3":
                dpBeginDateTime.readOnly = false;
                dpEndDateTime.readOnly = false;
                dpBeginDateTime.setValue(new Date(curTime.getTime() - 10 * 60000));
                dpEndDateTime.setValue(new Date(curTime.getTime() + 10 * 60000));
                break;
            default:
                dpBeginDateTime.readOnly = false;
                dpEndDateTime.readOnly = false;
                break;
        }
    }
});