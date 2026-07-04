SIE.defineCommand('SIE.Web.MES.TeamManagement.RatedItems.Commands.RatedItemSystemCommand', {
    meta: { text: "系统评分项目", group: "edit", iconCls: "icon-Repair icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        //创建表格数据
        var datas = [
            ['G0001', '考勤异常', '依据系统考勤时间作为员工正常到岗的判定标准'.t()],
            ['G0002', '标准产能未达标', '依据产成品总数及持续作业时间判定班组员工产能是否达标'.t()]
        ];

        var sysdatas = [];
        var seldatas = [];
        var finaldatas = [];
        var index = 0;
        var store = view.getData().data.items;
        for (var i = 0; i < datas.length; i++) {
            var isOk = true;
            for (var j = 0; j < store.length; j++) {
                if (store[j].getCode() == datas[i][0]) { isOk = false; break; }
            }
            if (isOk) {
                sysdatas[index] = datas[i];
                index = index + 1;
            }
        }

        //创建Grid表格组件
        var ui = Ext.create('Ext.grid.Panel', {
            viewConfig: {
                forceFit: true,
                stripeRows: true//在表格中显示斑马线
            },
            store: {//配置数据源
                fields: ['code', 'name', 'txt'],//定义字段
                proxy: {
                    type: 'memory',//Ext.data.proxy.Memory内存代理
                    data: sysdatas,//读取内嵌数据
                    reader: 'array'//Ext.data.reader.Array解析器
                },
                autoLoad: true//自动加载
            },
            xtype: 'rownumberer',
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                injectCheckbox: 1,//checkbox位于哪一列，默认值为0
                mode: 'multi',//multi,simple,single；默认为多选multi
                checkOnly: true,//如果值为true，则只用点击checkbox列才能选中此条记录
                allowDeselect: true,//如果值true，并且mode值为单选（single）时，可以通过点击checkbox取消对其的选择
                enableKeyNav: false,
                listeners: {
                    deselect: function (model, record, index) {//取消选中时产生的事件
                        var dindex = seldatas.indexOf(index);
                        if (dindex > -1) {
                            seldatas.splice(dindex, 1);
                        }
                    },
                    select: function (model, record, index) {//record被选中时产生的事件
                        seldatas.push(index)
                    },
                    selectionchange: function (model, selected) {//选择有改变时产生的事件
                    }
                }
            }),
            columns: [//配置表格列
                { xtype: 'rownumberer' },
                { header: "编码".t(), width: 80, dataIndex: 'code', sortable: true },
                { header: "名称".t(), width: 200, dataIndex: 'name', sortable: true },
                { header: "备注".t(), width: 500, dataIndex: 'txt', sortable: true }
            ]
        });

        //显示出来
        var win = SIE.Window.show({
            title: '系统评分项目'.t(),
            items: ui,
            width: 800,
            height: 500,
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var selection = seldatas;
                    if (selection.length <= 0) {
                        Ext.Msg.show({
                            title: '错误'.t(),
                            message: '请至少选择一行'.t()
                        });
                        return false;
                    }

                    for (var i = 0; i < selection.length; i++) {
                        finaldatas[i] = sysdatas[selection[i]];
                    }

                    for (var i = 0; i < finaldatas.length; i++) {
                        var item = view.addNew();
                        item.setCode(finaldatas[i][0]);
                        item.setName(finaldatas[i][1]);
                        item.setIsSystem(true);
                        view.getData().data.add(item);
                    }
                }
            },
        });
    },
});