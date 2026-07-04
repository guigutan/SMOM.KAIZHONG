SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonGroupDtlImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", hierarchy: "导入".t(), group: "business", iconCls: "icon-Upload icon-green" },
    parentId: null,
    execute: function (view, source) {
        this.mainview = view;
        this.parentId = view.getParent().getCurrent().getId();
        var btnFile = Ext.create('Ext.form.field.FileButton', { renderTo: Ext.getBody(), hidden: true, accept: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel' });
        btnFile.on("change", this.buttonChange, this);
        btnFile.fileInputEl.dom.click();
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
                    SelectedParentId: me.parentId
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