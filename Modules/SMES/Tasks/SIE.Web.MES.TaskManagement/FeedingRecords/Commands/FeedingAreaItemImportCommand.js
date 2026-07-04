SIE.defineCommand('SIE.Web.MES.TaskManagement.FeedingRecords.Commands.FeedingAreaItemImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    canExecute: function (view) {
        if (view == null || view.getParent() == null || view.getParent().getCurrent() == null)
            return false;
        return true;
    },

    buttonChange: function (field, newValue, oldValue) {
        var me = this;
        var file = field.fileInputEl.dom.files.item(0);
        var fileSize = file.size;
        var fileName = file.name;
        var limitSize = me.limitFileSize * 1000;
        var size = fileSize / 1024;
        if (size > limitSize) {
            Ext.MessageBox.alert("提示", "文件不能大于".t() + me.limitFileSize + "M".t());
            return false;
        }
        var fileExt = fileName.substring(fileName.lastIndexOf(".")).toLowerCase();
        var fileReader = new FileReader('file://' + newValue);
        fileReader.readAsDataURL(file);
        fileReader.onload = function (e) {
            Ext.MessageBox.show({
                msg: '导入数据中, 请稍等...'.t(),
                progressText: '导入中...'.t(),
                width: 300,
                closable: false,
                //modal: true,
                wait: {
                    interval: 200
                }
            });
            me.view.execute({
                data: {
                    BehaviorName: 'ImportData',
                    Type: me.view.model,
                    Data: e.target.result,
                    ViewGroup: me.view.viewGroup,
                    SelectedParentId: me.view.getParent().getCurrent().getId()
                },
                success: function (res) {
                    me.onSuccessImported(me, res);
                },
                error: function (res) {
                    Ext.MessageBox.close();
                }
            });
        }
    },

});