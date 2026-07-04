Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Scripts.HandoverConfirmPictureEditor', {
    extend: 'Ext.form.FieldContainer',
    id: 'handoverPictureFieldContainer',
    alias: 'widget.handoverPictureEditor',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'image',
            id: 'handoverPicture',
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