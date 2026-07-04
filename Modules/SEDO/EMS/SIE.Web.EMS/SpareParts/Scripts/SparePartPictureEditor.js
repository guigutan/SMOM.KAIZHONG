Ext.define('SIE.Web.EMS.SpareParts.Scripts.SparePartPictureEditor', {
    extend: 'Ext.form.FieldContainer',
    id: 'sparePartPictureFieldContainer',
    alias: 'widget.sparePartPictureEditor',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'image',
            id: 'sparePartPicture',
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