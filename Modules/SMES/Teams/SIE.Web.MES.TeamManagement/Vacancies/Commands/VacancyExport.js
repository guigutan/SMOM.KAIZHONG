SIE.defineCommand('SIE.Web.MES.TeamManagement.Vacancies.Commands.VacancyExport', {
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    ctYear: null,
    ctMonth: null,
    ctDay: null,
    execute: function (view) {
        var me = this;
        var criter = view._relations[0]._target.getCurrent();
        delete criter.data['CriteriaModuleKey'];
        delete criter.data['CriteriaType'];
        delete criter.data["CriteriaString"];
        var token = view.getToken();

        var win = Ext.create("Ext.window.Window", {
            title: "选择导出范围".t(), //标题            
            draggable: false,
            bodyStyle: 'padding:10px 30px 10px 30px',
            height: 200, //高度
            width: 300, //宽度
            modal: true, //是否模态窗口，默认为false
            resizable: false,
            labelWidth: 40,
            closeAction: 'close',
            autoDestroy: true,
            items: [
                {
                    xtype: 'combobox',
                    name: 'attentCbYear',
                    fieldLabel: '年'.t(),
                    labelStyle: 'width:40px;',
                    emptyText: '',
                    readOnly: true
                }
                , {
                    xtype: 'combobox',
                    name: 'attentCbMonth',
                    fieldLabel: '月'.t(),
                    labelStyle: 'width:40px;',
                    valueField: 'month',
                    displayField: 'month',
                    allowBlank: false,
                    typeAhead: true,
                    listeners: {
                        select: function (combo, record, opts) {
                            var data = combo.getValue();
                            SIE.invokeDataQuery({
                                method: 'GetDayByMonth',
                                action: 'queryer',
                                params: [data],
                                type: 'SIE.Web.MES.TeamManagement.ClockingIns.EmployeeAttentDataQueryer',
                                token: token,
                                success: function (res) {
                                    var day = res.Result;
                                    var dayStore = Ext.create('Ext.data.Store', {
                                        data: day,
                                        sorters: { property: 'day', direction: 'ASC' },
                                        groupField: 'day'
                                    });
                                    me.ctDay.setStore(dayStore);
                                }
                            });
                        }
                    }
                }
                , {
                    xtype: 'combobox',
                    name: 'attentCbDay',
                    fieldLabel: '日'.t(),
                    name: 'attentCbDay',
                    labelStyle: 'width:40px;',
                    valueField: 'day',
                    displayField: 'day',
                    typeAhead: true,
                    emptyText: '请选择'.t(),
                    listeners: {
                        afterRender: function () {
                            me.ctYear = Ext.ComponentQuery.query('combobox[name=attentCbYear]');
                            me.ctMonth = Ext.ComponentQuery.query('combobox[name=attentCbMonth]');
                            me.ctDay = Ext.ComponentQuery.query('combobox[name=attentCbDay]');
                            me.ctYear = me.ctYear[me.ctYear.length - 1];
                            me.ctMonth = me.ctMonth[me.ctMonth.length - 1];
                            me.ctDay = me.ctDay[me.ctDay.length - 1];
                            document.getElementById(me.ctYear.id).children[0].children[0].style.width = 'auto';
                            document.getElementById(me.ctMonth.id).children[0].children[0].style.width = 'auto';
                            document.getElementById(me.ctDay.id).children[0].children[0].style.width = 'auto';
                        }
                    }
                }
            ],
            buttons: [{
                text: '保存'.t(),
                handler: function () {
                    var y = me.ctYear.getValue();
                    var m = me.ctMonth.getValue();
                    var d = me.ctDay.getValue();
                    SIE.invokeDataQuery({
                        method: 'GetVacancyData',
                        params: [y, m, d, criter.data],
                        action: 'queryer',
                        type: 'SIE.Web.MES.TeamManagement.Vacancies.VacancyDataQueryer',
                        token: token,
                        success: function (res) {
                            if (res.Success) {
                                var exportData = res.Result;
                                if (exportData && exportData.Tables && exportData.Tables.length === 0) {
                                    me.timer = Ext.defer(function () {
                                        me.timer = null;
                                        Ext.MessageBox.hide();
                                        win.close();
                                    }, 1000);
                                    SIE.Msg.showMessage("没有可导出的数据".L10N());
                                }
                                else {
                                    me.generateExcel(view, exportData);
                                    me.timer = Ext.defer(function () {
                                        me.timer = null;
                                        Ext.MessageBox.hide();
                                        win.close();
                                    }, 1000);
                                }
                            }
                        }
                    });
                }
            }, {
                text: '取消'.t(),
                handler: function () {
                    win.close();
                }
            }],
            autoScroll: true,
            listeners: {
                afterrender: function () {
                    me.getstoreData(token);
                },
                beforeclose: function () {
                }
            }
        });
        win.show();
    },

    generateExcel: function (view, exportData) {
        SIE.Signature.otherCheckIsNeedToSign("导出".t(), view, function () {
            SIE.Web.MES.Common.Scripts.Helpers.ExportExcelHelper.tablesToMultiSheetExcel(exportData, '班组缺编统计'.L10N() + Ext.util.Format.date(new Date(), 'Ymdhis'), false);
        });

    },

    /**
    * 获取数据
    * @param {票据} token 
    * @returns {} 
    */
    getstoreData: function (token) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetYearAndMonth',
            action: 'queryer',
            type: 'SIE.Web.MES.TeamManagement.ClockingIns.EmployeeAttentDataQueryer',
            token: token,
            success: function (res) {
                var month = res.Result[0]['Month'];
                var monthStore = Ext.create('Ext.data.Store', {
                    data: month,
                    sorters: { property: 'month', direction: 'ASC' },
                    groupField: 'month'
                });

                me.ctYear.setValue(res.Result[0]['Year']);
                me.ctMonth.setStore(monthStore);
                me.ctMonth.setValue(month[month.length - 1].month);
                me.ctMonth.fireEvent('select', me.ctMonth);
            }
        });
    },

});

