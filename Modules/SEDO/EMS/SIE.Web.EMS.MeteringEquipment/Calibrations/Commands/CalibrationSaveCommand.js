SIE.defineCommand('SIE.Web.EMS.MeteringEquipment.Calibrations.Commands.CalibrationSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
   * 是否可执行 
   * @method canExecute 
   * @param {ListLogicalView} view 列表逻辑视图
   * @return {Boolean} 能执行返回true，否则返回false
   */
    canExecute: function (view) {
        var order = view.getCurrent();
        if (!order) {
            return false;
        }
        var data = order.data;
        if (data == null) {
            return false;
        }
        return data.InspectionRuleId > 0;
    },

    execute: function (view) {
        var me = this;
        me.saveCalibration(view);
    },

    saveCalibration: function (view) {
        var me = this;
        var data = view.getCurrent().data;
        var children = view.getChildren();
        if (!this.onValidation(view)) {
            SIE.MessageBox.showError("信息填写不完整！".L10N());
            return;
        }

        var calibrationEquipmentChild = children.first(function (p) {
            return p.model == "SIE.EMS.MeteringEquipment.Calibrations.CalibrationEquipment";
        });
        var calibrationItemChild = children.first(function (p) {
            return p.model == "SIE.EMS.MeteringEquipment.Calibrations.CalibrationItem";
        });

        var calibrationEquipmentdetails = [];
        var calibrationItemdetails = [];

        if (calibrationEquipmentChild && calibrationEquipmentChild.getData() && calibrationEquipmentChild.getData().data.items && calibrationEquipmentChild.getData().data.items.length > 0) {
            calibrationEquipmentdetails = calibrationEquipmentChild.getData().data.items.select(function (p) {
                return p.data;
            });
        }

        if (calibrationItemChild && calibrationItemChild.getData() && calibrationItemChild.getData().data.items && calibrationItemChild.getData().data.items.length > 0) {
            calibrationItemdetails = calibrationItemChild.getData().data.items.select(function (p) {
                return p.data;
            });
        }

        data.CalibrationEquipmentList = calibrationEquipmentdetails;
        data.CalibrationItemList = calibrationItemdetails;

        me.view.getData().dirty = true;//设置保存
        Ext.MessageBox.show({
            msg: '正在保存数据'.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });

        data.OrderNo = data.No;
        view.execute({
            //withChildren: withChildren,
            data: data,
            success: function (res) {
                Ext.MessageBox.hide();
                view._current.IsSavedMain = true;
                view._current.markSaved();
                view.syncCmdState(view, false);
                me.onSaved(view, res);
            }
        });
    },
    //保存后方法
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showToast('保存成功'.t(), '完成');
        window.setTimeout(function () {
            CRT.Event.fire("SIE.EMS.MeteringEquipment.Calibrations.Calibration_refresh");
            CRT.Workbench.closeCurrentTab();
        }, 1000);
    },
    //判断添加的属性值是否重复
    isRepeat: function (ary) {
        var me = this;
        var nary = [];
        SIE.each(ary, function (item) {
            Ext.Array.push(nary, item.data);
        })
        nary = nary.sort(me.compare('DefinitionId'));
        for (var i = 0; i < ary.length - 1; i++) {
            if (nary[i].DefinitionId == nary[i + 1].DefinitionId) {
                return true;
            }
        }
        return false;
    },
    compare: function (property) {
        return function (a, b) {
            var value1 = a[property];
            var value2 = b[property];
            return value1 - value2;
        }
    }
});