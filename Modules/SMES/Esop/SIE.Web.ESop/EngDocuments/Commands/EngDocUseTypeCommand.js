SIE.defineCommand('SIE.Web.ESop.EngDocuments.Commands.EngDocUseTypeCommand', {
    meta: { text: "使用类型维护", group: "edit", iconCls: "icon-DocumentLabel icon-blue" },
    execute: function (view, source) {
        CRT.Workbench.addPage({
            entityType: 'SIE.ESop.EngDocuments.FileUseDetail',
            title: '工程文件使用类型'.L10N(),
            module: "SIE.ESop.EngDocuments.FileUseDetail,SIE.ESop",
            isAggt:true
        });
    }
});
