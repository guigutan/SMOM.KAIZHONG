/**
 * EMS共用帮助类
 * @class SIE.Web.EMS.Common.Script.EmsCommonHelper
 * @constructs
 */
Ext.define('SIE.Web.EMS.Common.Script.EmsCommonHelper', {
    /**
     * Excel导出
     * @method excelDownload
     * @param  title 标题
    * @param data 数据源
    * @param mask 遮罩
     */
    excelDownload: function (title, data, mask) {
        var jsonData = JSON.parse(data.rows);
        if (jsonData.length == 0) {
            SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
            return false;
        }
        var view = null;
        SIE.AutoUI.getMeta({
            async: false,
            model: data.type,
            callback: function (meta) {
                view = SIE.AutoUI.createListView(meta);
            }
        });

        view.getData().setData(jsonData);
        view.getData().commitChanges();
        this.excelDownloadCommon(view, false, title, mask);
    },

    /**
     * Excel导出共用类
     * @method excelDownloadCommon
     * @param  view 视图
    * @param onlySels 是否是选中数据源
    * @param title 标题
    * @param mask 遮罩
     */
    excelDownloadCommon: function (view, onlySels, title, mask) {
        mask = mask || this.showMask(Ext.getBody().component, '下载中...');
        var task = new Ext.util.DelayedTask(function () {
            var style = Ext.create('Ext.exporter.file.Style', {
                alignment: 'Automatic',//Center
                font: {
                    color: '#aabbcc',
                    family: 'Consolas, Arial'
                }
            });
            var columns = Ext.create('Ext.exporter.data.Column', { style: style });
            view.gridConfig.columns.forEach(function (col) {
                if (col.xtype === 'rownumberer') return true;
                columns.addColumn({ text: col.header, width: col.width || 140 });
            });

            var rows = [];
            var datas = [];
            if (onlySels)
                datas = view.getSelection();
            else
                datas = view.getData().getData().items;
            datas.forEach(function (item) {
                var row = Ext.create('Ext.exporter.data.Row');
                view.gridConfig.columns.forEach(function (col) {
                    var colIdx = col.dataIndex;
                    if (!!!colIdx) return true; // rownumberer没有dataIndex
                    var val = null;
                    if (colIdx.indexOf('Id') > 0 && item.data[colIdx + '_Display'] !== undefined) {
                        val = item.data[colIdx + '_Display'];
                    }
                    else {
                        val = item.data[colIdx];
                        if (col.xtype === 'comboboxcolumn') {
                            if (col.editor.store.data[0] instanceof Array) {//全局快码
                                var arr = col.editor.store.data.filter(function (d) { return d[0] === val });
                                if (arr.length > 0)
                                    val = arr[0][1];
                            } else {//枚举
                                var arr = col.editor.store.data.filter(function (d) { return d[col.editor.valueField] === val });
                                if (arr.length > 0)
                                    val = arr[0][col.editor.displayField];
                            }
                        }
                        if (col.xtype === 'comboColumn') {//快码
                            var arr = JSON.parse(col.editor.store.data).filter(function (d) { return d.Code === val });
                            if (arr.length > 0)
                                val = arr[0].Name;
                        }
                        if (col.xtype === 'checkboxcolumn') {//布尔
                            val = val ? '是' : '否';
                        }
                    }
                    if (val instanceof Date) {
                        var fmt = 'Y-m-d';
                        if (Ext.Date.format(val, 'H:i:s') !== "00:00:00")
                            fmt = 'Y-m-d H:i:s';
                        val = Ext.Date.format(val, fmt);
                    }
                    row.addCell({ value: val });
                });
                rows.push(row);
            });

            var excelData = Ext.create('Ext.exporter.data.Table', {
                columns: columns,
                rows: rows
            });
            title = title || view.label;
            var excel = Ext.create('Ext.exporter.excel.Xlsx', {
                fileName: title + Ext.Date.format(new Date(), 'YmdHis') + '.xlsx',
                title: title,
                author: 'SIE',
                data: excelData
            });
            excel.saveAs();
            SIE.Msg.showInstantMessage('成功导出【{0}】笔数据！'.format(rows.length));
            mask.hide();
        });
        task.delay(50);
    },

    /**
     * 显示遮罩
     * @method showMask
     * @param  target 目标控件
    * @param msg 显示信息
     */
    showMask: function (target, msg) {
        msg = msg || '读取中...';
        var mask = new Ext.LoadMask({
            target: target,
            msg: msg,
        });
        mask.show();
        return mask;
    },

    /**
    * 获取view的控制器
    * @param {SIE.view.View} view实例
    */
    getViewController: function (view, controller) {
        var ctl = null;
        if (view) {
            ctl = view.getController();
            if (!ctl) {
                ctl = new controller();
                view.setController(ctl);
            }
            var childrens = view.getChildren();
            if (childrens && childrens.length > 0) {
                for (var i = 0, length = childrens.length; i < length; i++) {
                    var children = childrens[i];
                    if (children.isView) {
                        children.setController(ctl);
                    }
                }
            }
        }
        return ctl;
    },
});