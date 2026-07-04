using System.Xml.Serialization;

namespace SIE.Common.Prints
{
    [XmlRoot("XtraReportsLayoutSerializer")]
    public class XtraReportsLayoutSerializer
    {
        // Token: 0x1700001D RID: 29
        // (get) Token: 0x060000B1 RID: 177 RVA: 0x0000741C File Offset: 0x0000561C
        // (set) Token: 0x060000B2 RID: 178 RVA: 0x00007424 File Offset: 0x00005624
        [XmlAttribute("Ref")]
        public string Ref { get; set; }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x060000B3 RID: 179 RVA: 0x0000742D File Offset: 0x0000562D
        // (set) Token: 0x060000B4 RID: 180 RVA: 0x00007435 File Offset: 0x00005635
        [XmlAttribute("ControlType")]
        public string ControlType { get; set; }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x060000B5 RID: 181 RVA: 0x0000743E File Offset: 0x0000563E
        // (set) Token: 0x060000B6 RID: 182 RVA: 0x00007446 File Offset: 0x00005646
        [XmlAttribute("PageWidth")]
        public string PageWidth { get; set; }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x060000B7 RID: 183 RVA: 0x0000744F File Offset: 0x0000564F
        // (set) Token: 0x060000B8 RID: 184 RVA: 0x00007457 File Offset: 0x00005657
        [XmlAttribute("PageHeight")]
        public string PageHeight { get; set; }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x060000B9 RID: 185 RVA: 0x00007460 File Offset: 0x00005660
        // (set) Token: 0x060000BA RID: 186 RVA: 0x00007468 File Offset: 0x00005668
        [XmlAttribute("Version")]
        public string Version { get; set; }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x060000BB RID: 187 RVA: 0x00007471 File Offset: 0x00005671
        // (set) Token: 0x060000BC RID: 188 RVA: 0x00007479 File Offset: 0x00005679
        [XmlAttribute("ForeColor")]
        public string ForeColor { get; set; }

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x060000BD RID: 189 RVA: 0x00007482 File Offset: 0x00005682
        // (set) Token: 0x060000BE RID: 190 RVA: 0x0000748A File Offset: 0x0000568A
        [XmlAttribute("DataSource")]
        public string DataSource { get; set; }

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x060000BF RID: 191 RVA: 0x00007493 File Offset: 0x00005693
        // (set) Token: 0x060000C0 RID: 192 RVA: 0x0000749B File Offset: 0x0000569B
        [XmlAttribute("DataMember")]
        public string DataMember { get; set; }

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x060000C1 RID: 193 RVA: 0x000074A4 File Offset: 0x000056A4
        // (set) Token: 0x060000C2 RID: 194 RVA: 0x000074AC File Offset: 0x000056AC
        [XmlElement("ComponentStorage")]
        public ComponentStorage ComponentStorage { get; set; }
    }

    [XmlType("ComponentStorage")]
    public class ComponentStorage
    {
        // Token: 0x17000026 RID: 38
        // (get) Token: 0x060000C4 RID: 196 RVA: 0x000074BD File Offset: 0x000056BD
        // (set) Token: 0x060000C5 RID: 197 RVA: 0x000074C5 File Offset: 0x000056C5
        [XmlElement("Item1")]
        public Item1 Item1 { get; set; }
    }
    [XmlType("Item1")]
    public class Item1
    {
        // Token: 0x17000027 RID: 39
        // (get) Token: 0x060000C7 RID: 199 RVA: 0x000074D6 File Offset: 0x000056D6
        // (set) Token: 0x060000C8 RID: 200 RVA: 0x000074DE File Offset: 0x000056DE
        [XmlAttribute("Ref")]
        public string Ref { get; set; }

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x060000C9 RID: 201 RVA: 0x000074E7 File Offset: 0x000056E7
        // (set) Token: 0x060000CA RID: 202 RVA: 0x000074EF File Offset: 0x000056EF
        [XmlAttribute("ObjectType")]
        public string ObjectType { get; set; }

        // Token: 0x17000029 RID: 41
        // (get) Token: 0x060000CB RID: 203 RVA: 0x000074F8 File Offset: 0x000056F8
        // (set) Token: 0x060000CC RID: 204 RVA: 0x00007500 File Offset: 0x00005700
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x1700002A RID: 42
        // (get) Token: 0x060000CD RID: 205 RVA: 0x00007509 File Offset: 0x00005709
        // (set) Token: 0x060000CE RID: 206 RVA: 0x00007511 File Offset: 0x00005711
        [XmlAttribute("Base64")]
        public string Base64 { get; set; }
    }
    [XmlType("Query")]
    public class Query
    {
        // Token: 0x17000055 RID: 85
        // (get) Token: 0x06000131 RID: 305 RVA: 0x00007854 File Offset: 0x00005A54
        // (set) Token: 0x06000132 RID: 306 RVA: 0x0000785C File Offset: 0x00005A5C
        [XmlAttribute("Type")]
        public string Type { get; set; }

        // Token: 0x17000056 RID: 86
        // (get) Token: 0x06000133 RID: 307 RVA: 0x00007865 File Offset: 0x00005A65
        // (set) Token: 0x06000134 RID: 308 RVA: 0x0000786D File Offset: 0x00005A6D
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x06000135 RID: 309 RVA: 0x00007876 File Offset: 0x00005A76
        // (set) Token: 0x06000136 RID: 310 RVA: 0x0000787E File Offset: 0x00005A7E
        [XmlElement("Tables")]
        public Tables Tables { get; set; }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x06000137 RID: 311 RVA: 0x00007887 File Offset: 0x00005A87
        // (set) Token: 0x06000138 RID: 312 RVA: 0x0000788F File Offset: 0x00005A8F
        [XmlElement("Columns")]
        public Columns Columns { get; set; }
    }
    [XmlType("Relation")]
    public class Relation
    {
        // Token: 0x1700005E RID: 94
        // (get) Token: 0x06000148 RID: 328 RVA: 0x00007915 File Offset: 0x00005B15
        // (set) Token: 0x06000149 RID: 329 RVA: 0x0000791D File Offset: 0x00005B1D
        [XmlAttribute("Master")]
        public string Master { get; set; }

        // Token: 0x1700005F RID: 95
        // (get) Token: 0x0600014A RID: 330 RVA: 0x00007926 File Offset: 0x00005B26
        // (set) Token: 0x0600014B RID: 331 RVA: 0x0000792E File Offset: 0x00005B2E
        [XmlAttribute("Detail")]
        public string Detail { get; set; }

        // Token: 0x17000060 RID: 96
        // (get) Token: 0x0600014C RID: 332 RVA: 0x00007937 File Offset: 0x00005B37
        // (set) Token: 0x0600014D RID: 333 RVA: 0x0000793F File Offset: 0x00005B3F
        [XmlElement("KeyColumn")]
        public KeyColumn KeyColumn { get; set; }
    }
    [XmlType("KeyColumn")]
    public class KeyColumn
    {
        // Token: 0x17000061 RID: 97
        // (get) Token: 0x0600014F RID: 335 RVA: 0x00007950 File Offset: 0x00005B50
        // (set) Token: 0x06000150 RID: 336 RVA: 0x00007958 File Offset: 0x00005B58
        [XmlAttribute("Master")]
        public string Master { get; set; }

        // Token: 0x17000062 RID: 98
        // (get) Token: 0x06000151 RID: 337 RVA: 0x00007961 File Offset: 0x00005B61
        // (set) Token: 0x06000152 RID: 338 RVA: 0x00007969 File Offset: 0x00005B69
        [XmlAttribute("Detail")]
        public string Detail { get; set; }
    }
    [XmlType("Tables")]
    public class Tables
    {
        // Token: 0x17000059 RID: 89
        // (get) Token: 0x0600013A RID: 314 RVA: 0x000078A0 File Offset: 0x00005AA0
        // (set) Token: 0x0600013B RID: 315 RVA: 0x000078A8 File Offset: 0x00005AA8
        [XmlElement("Table")]
        public List<Table> TableList { get; set; }
    }
    [XmlType("Table")]
    public class Table
    {
        // Token: 0x1700005A RID: 90
        // (get) Token: 0x0600013D RID: 317 RVA: 0x000078B9 File Offset: 0x00005AB9
        // (set) Token: 0x0600013E RID: 318 RVA: 0x000078C1 File Offset: 0x00005AC1
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
    [XmlType("Columns")]
    public class Columns
    {
        // Token: 0x1700005B RID: 91
        // (get) Token: 0x06000140 RID: 320 RVA: 0x000078D2 File Offset: 0x00005AD2
        // (set) Token: 0x06000141 RID: 321 RVA: 0x000078DA File Offset: 0x00005ADA
        [XmlElement("Column")]
        public List<Column> ColumnList { get; set; }
    }
    [XmlType("Column")]
    public class Column
    {
        // Token: 0x1700005C RID: 92
        // (get) Token: 0x06000143 RID: 323 RVA: 0x000078EB File Offset: 0x00005AEB
        // (set) Token: 0x06000144 RID: 324 RVA: 0x000078F3 File Offset: 0x00005AF3
        [XmlAttribute("Table")]
        public string Table { get; set; }

        // Token: 0x1700005D RID: 93
        // (get) Token: 0x06000145 RID: 325 RVA: 0x000078FC File Offset: 0x00005AFC
        // (set) Token: 0x06000146 RID: 326 RVA: 0x00007904 File Offset: 0x00005B04
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
    [XmlType("ResultView")]
    public class ResultView
    {
        // Token: 0x17000045 RID: 69
        // (get) Token: 0x0600010B RID: 267 RVA: 0x00007714 File Offset: 0x00005914
        // (set) Token: 0x0600010C RID: 268 RVA: 0x0000771C File Offset: 0x0000591C
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x0600010D RID: 269 RVA: 0x00007725 File Offset: 0x00005925
        // (set) Token: 0x0600010E RID: 270 RVA: 0x0000772D File Offset: 0x0000592D
        [XmlElement("Field")]
        public List<Field> FieldList { get; set; }
    }
    [XmlType("Field")]
    public class Field
    {
        // Token: 0x17000047 RID: 71
        // (get) Token: 0x06000110 RID: 272 RVA: 0x0000773E File Offset: 0x0000593E
        // (set) Token: 0x06000111 RID: 273 RVA: 0x00007746 File Offset: 0x00005946
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x06000112 RID: 274 RVA: 0x0000774F File Offset: 0x0000594F
        // (set) Token: 0x06000113 RID: 275 RVA: 0x00007757 File Offset: 0x00005957
        [XmlAttribute("Type")]
        public string Type { get; set; }
    }

    [XmlType("Parameter")]
    public class Parameter
    {
        // Token: 0x17000053 RID: 83
        // (get) Token: 0x0600012C RID: 300 RVA: 0x0000782A File Offset: 0x00005A2A
        // (set) Token: 0x0600012D RID: 301 RVA: 0x00007832 File Offset: 0x00005A32
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x0600012E RID: 302 RVA: 0x0000783B File Offset: 0x00005A3B
        // (set) Token: 0x0600012F RID: 303 RVA: 0x00007843 File Offset: 0x00005A43
        [XmlAttribute("Value")]
        public string Value { get; set; }
    }
    [XmlType("Parameters")]
    public class Parameters
    {
        // Token: 0x17000052 RID: 82
        // (get) Token: 0x06000129 RID: 297 RVA: 0x00007811 File Offset: 0x00005A11
        // (set) Token: 0x0600012A RID: 298 RVA: 0x00007819 File Offset: 0x00005A19
        [XmlElement("Parameter")]
        public List<Parameter> ParameterList { get; set; }
    }
    [XmlType("Connection")]
    public class Connection
    {
        // Token: 0x1700004F RID: 79
        // (get) Token: 0x06000122 RID: 290 RVA: 0x000077D6 File Offset: 0x000059D6
        // (set) Token: 0x06000123 RID: 291 RVA: 0x000077DE File Offset: 0x000059DE
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x06000124 RID: 292 RVA: 0x000077E7 File Offset: 0x000059E7
        // (set) Token: 0x06000125 RID: 293 RVA: 0x000077EF File Offset: 0x000059EF
        [XmlAttribute("ProviderKey")]
        public string ProviderKey { get; set; }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x06000126 RID: 294 RVA: 0x000077F8 File Offset: 0x000059F8
        // (set) Token: 0x06000127 RID: 295 RVA: 0x00007800 File Offset: 0x00005A00
        [XmlElement("Parameters")]
        public Parameters Parameters { get; set; }
    }

    [XmlType("ResultSchema")]
    public class ResultSchema
    {
        // Token: 0x17000043 RID: 67
        // (get) Token: 0x06000106 RID: 262 RVA: 0x000076EA File Offset: 0x000058EA
        // (set) Token: 0x06000107 RID: 263 RVA: 0x000076F2 File Offset: 0x000058F2
        [XmlElement("DataSet")]
        public SqlDataSet DataSet { get; set; }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x06000108 RID: 264 RVA: 0x000076FB File Offset: 0x000058FB
        // (set) Token: 0x06000109 RID: 265 RVA: 0x00007703 File Offset: 0x00005903
        [XmlElement("View")]
        public ResultView View { get; set; }
    }
    [XmlType("SqlDataSet")]
    public class SqlDataSet
    {
        // Token: 0x17000063 RID: 99
        // (get) Token: 0x06000154 RID: 340 RVA: 0x0000797A File Offset: 0x00005B7A
        // (set) Token: 0x06000155 RID: 341 RVA: 0x00007982 File Offset: 0x00005B82
        [XmlAttribute("Name")]
        public string Name { get; set; }

        // Token: 0x17000064 RID: 100
        // (get) Token: 0x06000156 RID: 342 RVA: 0x0000798B File Offset: 0x00005B8B
        // (set) Token: 0x06000157 RID: 343 RVA: 0x00007993 File Offset: 0x00005B93
        [XmlElement("View")]
        public List<ResultView> View { get; set; }

        // Token: 0x17000065 RID: 101
        // (get) Token: 0x06000158 RID: 344 RVA: 0x0000799C File Offset: 0x00005B9C
        // (set) Token: 0x06000159 RID: 345 RVA: 0x000079A4 File Offset: 0x00005BA4
        [XmlElement("Relation")]
        public List<Relation> Relation { get; set; }
    }
    [XmlType("SqlDataSource")]
    public class SqlDataSource
    {
        // Token: 0x17000049 RID: 73
        // (get) Token: 0x06000115 RID: 277 RVA: 0x00007768 File Offset: 0x00005968
        // (set) Token: 0x06000116 RID: 278 RVA: 0x00007770 File Offset: 0x00005970
        [XmlElement("Name")]
        public string Name { get; set; }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x06000117 RID: 279 RVA: 0x00007779 File Offset: 0x00005979
        // (set) Token: 0x06000118 RID: 280 RVA: 0x00007781 File Offset: 0x00005981
        [XmlElement("Connection")]
        public Connection Connection { get; set; }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x06000119 RID: 281 RVA: 0x0000778A File Offset: 0x0000598A
        // (set) Token: 0x0600011A RID: 282 RVA: 0x00007792 File Offset: 0x00005992
        [XmlElement("Query")]
        public List<Query> Query { get; set; }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x0600011B RID: 283 RVA: 0x0000779B File Offset: 0x0000599B
        // (set) Token: 0x0600011C RID: 284 RVA: 0x000077A3 File Offset: 0x000059A3
        [XmlElement("Relation")]
        public List<Relation> Relation { get; set; }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x0600011D RID: 285 RVA: 0x000077AC File Offset: 0x000059AC
        // (set) Token: 0x0600011E RID: 286 RVA: 0x000077B4 File Offset: 0x000059B4
        [XmlElement("ResultSchema")]
        public ResultSchema ResultSchema { get; set; }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x0600011F RID: 287 RVA: 0x000077BD File Offset: 0x000059BD
        // (set) Token: 0x06000120 RID: 288 RVA: 0x000077C5 File Offset: 0x000059C5
        [XmlElement("ConnectionOptions")]
        public ConnectionOptions ConnectionOptions { get; set; }
    }
    [XmlType("ConnectionOptions")]
    public class ConnectionOptions
    {
        // Token: 0x17000066 RID: 102
        // (get) Token: 0x0600015B RID: 347 RVA: 0x000079B5 File Offset: 0x00005BB5
        // (set) Token: 0x0600015C RID: 348 RVA: 0x000079BD File Offset: 0x00005BBD
        [XmlAttribute("CloseConnection")]
        public string CloseConnection { get; set; }
    }
}
