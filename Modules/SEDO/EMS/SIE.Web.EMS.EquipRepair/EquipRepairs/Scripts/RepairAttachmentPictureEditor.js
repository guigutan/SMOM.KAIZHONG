Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Scripts.RepairAttachmentPictureEditor', {
    extend: 'Ext.form.FieldContainer',
    id: 'repairPictureFieldContainer',
    alias: 'widget.repairPictureEditor',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'image',
            id: 'repairPicture',
            beforeRender: function () {
                this.src = "";
                this.width = 300;
                this.height = 300;
            },
            listeners: {
                el: {
                    click: 'onClick'
                }
            },
            onClick: function () {
            }
        }
    ],
});