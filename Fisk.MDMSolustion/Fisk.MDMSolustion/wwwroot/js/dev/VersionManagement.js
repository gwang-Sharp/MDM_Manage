﻿var connection = null;
var VersionVM = new Vue({
    el: '#Version',
    data: {
        ModelID: '',
        EntityID: '',
        ModelID2: '',
        EntityID2: '',
        AttrID: '',
        TableHeight: 'calc(100vh - 210px)',
        ColsTabLoading: false,
        ColsTabLoading2: false,
        dialogTitle: '添加快照',
        dialogVisible: false,
        dialogVisible2: false,
        VersionTableCols: [
            { field: "name", title: "版本名称" },
            { field: "remark", title: "版本说明" },
            { field: "createUser", title: "创建人" },
            { field: "createTime", title: "创建时间" },
            { field: "updateUser", title: "修改人" },
            { field: "updateTime", title: "修改时间" }
        ],
        TableCols: [
            { field: "name", title: "实体" },
            { field: "AttributeName", title: "属性" },
            { field: "Value", title: "值" },
            { field: "CreateTime", title: "开始时间" },
        ],
        tags: [],
        VsesionForm: {
            Name: '',
            Remark: ''
        },
        VersionFromrules: {
            Remark: [{

                required: true,
                message: '请输入说明',
                trigger: 'change'
            }],
            Name: [{
                required: true,
                message: '请输入名称',
                trigger: 'blur'
            }]
        },
        TableData: [],
        TableData2: [],
        pagination: {
            currentpage: 1,
            pagesizes: [10, 20, 30, 40],
            pagesize: 10,
            allNum: 0,
        },
        pagination2: {
            currentpage: 1,
            pagesizes: [10, 20, 30, 40],
            pagesize: 10,
            allNum: 0,
        },
        ModelSelectOptions: [],
        EntitySelectOptions: [],
        ModelSelectOptions2: [],
        EntitySelectOptions2: [],
        AttrsSelectOptions: [],
        AllAttrSelectOptions: [],
        AllEntityOptions: [],
        activities: [],
        linkTables: [],
        activeName: 'first',
        IsBackUp: false,
        ShowClose: false,
        startBackUp: false
    },
    created: function () {
        this.InitModelSelectOptions();
        this.InitEntitySelectOptions();
        this.InitAttrSelectOptions();
    },
    mounted: function () { },
    methods: {
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        InitVersionTable: function () {
            let that = this;
            that.ColsTabLoading = true;
            $.post("/MasterData_Version_Manage/InitVersionTable",
                {
                    EntityID: that.EntityID,
                    page: that.pagination.currentpage,
                    rows: that.pagination.pagesize
                },
                RES => {
                    //console.log(RES);
                    if (RES.data) {
                        that.ColsTabLoading = false;
                        that.tags = [];
                        that.TableData = RES.data;
                        $.each(RES.extraData, function (index, value) {
                            if (value.EntityTable) {
                                let obj = {};
                                obj.name = value.EntityTable;
                                that.tags.push(obj);
                            }
                        })
                        // console.log(that.tags);
                    }
                    else {
                        that.TableData = [];
                    }
                    that.pagination.allNum = RES.total;
                })
        },
        InitAttrStraceTable: function () {
            let that = this;
            $.post("/MasterData_Version_Manage/InitAttrStraceTable", {
                EntityID: that.EntityID2,
                AttrID: that.AttrID,
                page: that.pagination2.currentpage,
                rows: that.pagination2.pagesize
            }, RES => {
                that.TableData2 = RES.data;
                that.pagination2.allNum = RES.total;
                that.AllEntityStraceAttrs = RES.data;
                console.log(RES);
            })
        },
        AddVersion: function () {
            if (!this.ModelID) {
                this.$message.error("请选择模型");
                return;
            }
            if (!this.EntityID) {
                this.$message.error("请选择实体");
                return;
            }
            this.IsBackUp = false;
            this.BeginConnect();
            this.dialogVisible = true;
        },
        InitModelSelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_ModelsGet", RES => {
                //console.log(RES);
                if (RES.success) {
                    that.ModelSelectOptions = RES.data;
                    that.ModelSelectOptions2 = RES.data;
                }
                else {
                    that.ModelSelectOptions = [];
                    that.ModelSelectOptions2 = [];
                }
            })
        },
        InitEntitySelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_EntitysGet", RES => {
                //console.log(RES);
                if (RES.success) {
                    that.EntitySelectOptions = RES.data;
                    that.EntitySelectOptions2 = RES.data;
                    that.AllEntityOptions = RES.data;
                }
                else {
                    that.EntitySelectOptions2 = [];
                    that.EntitySelectOptions = [];
                }
            })
        },
        InitAttrSelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_GetAll", RES => {
                if (RES.success) {
                    that.AttrsSelectOptions = RES.data;
                    that.AllAttrSelectOptions = RES.data;
                }
                else {
                    that.AttrsSelectOptions = [];
                    that.AllAttrSelectOptions = [];
                }
            })
        },
        handleDelete: function (row) {
            //console.log(row)
            let that = this;
            that.SelectedRow = row;
            that.$confirm('此操作将将删除该版本快照, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                that.ColsTabLoading = true;
                $.post("/MasterData_Version_Manage/VersionDel",
                    {
                        entityID: row.entityID,
                        versionName: row.name
                    },
                    RES => {
                        // console.log(RES);
                        that.ColsTabLoading = false;
                        if (RES.success) {
                            that.$message({
                                message: RES.message,
                                type: 'success'
                            });
                            that.InitVersionTable();
                        }
                        else {
                            that.$message.error(RES.message);
                        }
                    })
            }).catch(() => {
                return;
            });
        },
        ModelHasChanged: function () {
            //console.log(this.ModelID);
            this.EntityID = "";
            this.EntitySelectOptions = this.AllEntityOptions.filter(o => {
                return o.modelId == this.ModelID;
            })
        },
        EntityHasChanged: function () {
            this.InitVersionTable();
        },
        ModelHasChanged2: function () {
            //console.log(this.ModelID);
            this.EntityID = "";
            this.EntitySelectOptions2 = this.AllEntityOptions.filter(o => {
                return o.modelId == this.ModelID2;
            })
        },
        EntityHasChanged2: function () {
            this.InitAttrStraceTable();
            this.AttrID = "";
            this.AttrsSelectOptions = this.AllAttrSelectOptions.filter(a => {
                return a.entityID == this.EntityID2;
            })
        },
        AttrHasChanged: function () {
            // console.log(this.AttrsSelectOptions)
            this.pagination2.currentpage = 1;
            this.InitAttrStraceTable();
        },
        handleSizeChange: function (val) {
            this.pagination.pagesize = val;
            this.InitVersionTable()
        },
        handleCurrentChange: function (val) {
            this.pagination.currentpage = val;
            this.InitVersionTable()
        },
        handleSizeChange2: function (val) {
            this.pagination2.pagesize = val;
        },
        handleCurrentChange2: function (val) {
            this.pagination2.currentpage = val;
        },
        resetFormModel: function () {
            this.$refs.VsesionForm.resetFields();
        },
        handleClose: function () {
            if (!this.IsBackUp) {
                connection.invoke("FinishConnect");
            }
            this.IsBackUp = true;
            this.resetFormModel();
        },
        handleClose2: function () {
            this.IsBackUp = true;
            this.activities = [];
            this.linkTables = [];
            this.ShowClose = false;
        },
        BeginConnect: function () {
            let that = this;
            let setupConnection = () => {
                connection = new signalR.HubConnectionBuilder()
                    .withUrl("/VersionHub")
                    .build();
                connection.on("onconnect", (response) => {
                    console.log(response.msg);
                });
                connection.on("onclosedialog", () => {
                    that.dialogVisible2 = true;
                    that.dialogVisible = false;
                    let obj = {};
                    obj.content = "正在备份" + that.linkTables[0];
                    obj.timestamp = that.GettTimestamp();
                    obj.size = 'large';
                    obj.type = 'primary';
                    obj.icon = 'el-icon-loading';
                    that.activities.push(obj);
                });
                connection.on("onshowdialog", () => {
                    that.dialogVisible = false;
                    setTimeout(function () {
                        that.InitVersionTable();
                    }, 1500)
                    connection.stop();
                    that.$message({
                        message: "备份成功",
                        type: 'success'
                    });
                })
                connection.on("onfailed", (response) => {
                    that.linkTables = [];
                    that.$message.error(response.msg);
                });
                connection.on("onsuccess", (response) => {
                    let obj = {};
                    obj.content = that.linkTables[response.next - 1] + "备份成功";
                    obj.timestamp = that.GettTimestamp();
                    obj.type = 'success';
                    obj.icon = 'el-icon-check';
                    obj.color = '#0bbd87';
                    that.activities.push(obj);
                    if (that.linkTables[response.next]) {
                        let obj = {};
                        obj.content = "正在备份" + that.linkTables[response.next];
                        obj.timestamp = that.GettTimestamp();
                        obj.size = 'large';
                        obj.type = 'primary';
                        obj.icon = 'el-icon-loading';
                        that.activities.push(obj);
                    }
                    else {
                        let obj = {};
                        obj.content = "备份结束";
                        obj.type = 'success';
                        obj.timestamp = that.GettTimestamp();
                        that.activities.push(obj);
                        that.IsBackUp = true;
                        that.ShowClose = true;
                        connection.invoke("FinishConnect");
                    }
                });
                connection.on("onfinished", (response) => {
                    if (that.startBackUp) {
                        setTimeout(function () {
                            that.InitVersionTable();
                        }, 1000)
                        console.log("结束备份");
                    } else {
                        console.log("已取消备份");
                    }
                    setTimeout(function () {
                        connection.stop();
                    }, 1000);
                });
            };
            setupConnection();
            connection.start()
                .catch(err => console.error(err.toString()));
        },
        addOrUpdate: function () {
            VersionVM.$refs.VsesionForm.validate((valid) => {
                if (valid) {
                    window.onbeforeunload = function (event) {
                        event.returnValue = false;
                        if (!that.IsBackUp) {
                            return event.returnValue;
                        }
                    };
                    let that = this;
                    that.startBackUp = true;
                    $.each(that.tags, function (index, value) {
                        that.linkTables.push(value.name);
                    })
                    var linktables = that.linkTables.join(",");
                    connection.invoke("CreateVersion", linktables, that.EntityID, JSON.stringify(that.VsesionForm));
                }
                else {
                    return false;
                }
            })
        },
        CancelAddOrUpdate: function () {
            this.resetFormModel();
            this.dialogVisible = false;
        },
        GettTimestamp: function () {
            var time = new Date();
            var datetime1 = time.getFullYear() + '-' + (time.getMonth() + 1) + '-' + time.getDate() + ' ' + time.getHours() + ':' + time.getMinutes() + ':' + time.getSeconds()
            return datetime1;
        },
        handleClick: function (tab, event) {
            console.log(tab, event);
        }
    }
})