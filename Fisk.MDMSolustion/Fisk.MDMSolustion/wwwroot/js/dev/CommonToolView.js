var ToolVm = new Vue({
    el: "#CommonTool",
    data: {
        TableData: [],
        TableCols: [],
        DrawerCols: [
            { field: "AttrName", label: "属性" },
            { field: "Operator", label: "运算符" },
            { field: "Content", label: "条件" },
        ],
        FilterData: [
            { AttrName: '', Operator: '', Content: '' }
        ],
        FilterWhere: "",
        Operator: [
            { filter: "不为NULL", value: "is not null" },
            { filter: "不包含模式", value: "不包含模式" },
            { filter: "不匹配", value: "不匹配" },
            { filter: "不等于", value: "!=" },
            { filter: "为NULL", value: "is null" },
            { filter: "包含模式", value: "包含模式" },
            { filter: "匹配", value: "匹配" },
            { filter: "大于", value: ">" },
            { filter: "大于或等于", value: ">=" },
            { filter: "小于", value: "<" },
            { filter: "小于或等于", value: "<=" },
            { filter: "开头为", value: "开头为" },
            { filter: "等于", value: "=" },
        ],
        TableHeight: 'calc(100vh - 178px)',
        EntityColsTabLoading: false,
        ModelSelectOptions: [],
        EntitySelectOptions: [],
        AllEntityOptions: [],
        AttrsSelectOptions: [],
        AllAttrSelectOptions: [],
        ErrorLogs: [],
        ErrorLogsCols: [
            { field: "filename", title: "文件名" },
            { field: "errorMessages", title: "错误消息" },
            { field: "state", title: "状态" }
        ],
        BatchIDs: [],
        SelectedFile: '',
        ShowTable: false,
        ShowLogsTable: false,
        ModelID: '',
        EntityID: '',
        AttrName: '',
        EntityName: '',
        pagination: {
            currentpage: 1,
            pagesizes: [10, 20, 30, 40],
            pagesize: 10,
            allNum: 0,
        },
        pagination2: {
            currentpage: 1,
            pagesizes: [5, 10, 15, 20],
            pagesize: 5,
            allNum: 0,
        },
        UploadExtra: {
            entityname: '',
        },
        EditRowValue: false,
        linkTableData: [],
        DrawerDialog: false,
        ColumnFixed: ''
    },
    created: function () {
        var urlParms = window.location.hash.split("/");
        if (/^[0-9]*$/g.test(urlParms[2])) {
            this.InitViewTables()
        }
        this.InitModelSelectOptions();
        this.InitEntitySelectOptions();
    },
    mounted: function () {
    },
    methods: {
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        InitViewTables: function () {
            let that = this;
            $.post("/MasterData_Maintain_Manage/InitEntityTable",
                {
                    Entity: window.Entity,
                    where: that.FilterWhere,
                    page: that.pagination.currentpage,
                    rows: that.pagination.pagesize
                }, RES => {
                    that.EntityColsTabLoading = false;
                    //console.log(RES);
                    if (that.DrawerDialog) {
                        that.DrawerDialog = false;
                    }
                    that.TableCols = [];
                    that.TableData = [];
                    if (RES.data.length == 0) {
                        that.pagination.allNum = RES.total;
                        that.EntityName = RES.entityName;
                        $.each(RES.tableCols, function (index, value) {
                            let obj = {};
                            obj.field = value.Name;
                            obj.Label = value.DisplayName;
                            that.TableCols.push(obj);
                        })
                        that.ColumnFixed = "right";
                    }
                    else {
                        that.EntityName = RES.entityName;
                        that.pagination.allNum = RES.total;
                        that.linkTableData = RES.linkTableData;
                        $.each(RES.tableCols, function (index, value) {
                            let obj = {};
                            obj.field = value.Name;
                            obj.Label = value.DisplayName;
                            that.TableCols.push(obj);
                        })
                        $.each(RES.data, function (index, value) {
                            $.each(that.TableCols, function (idx, val) {
                                value[val.field] = that.ValueFormatter(val.field, value[val.field]);
                            })
                        })
                        setTimeout(function () {
                            that.ColumnFixed = "right";
                        }, 500)
                        that.TableData = RES.data;
                        setTimeout(function () {
                            let inputs = $('table .el-input__inner');
                            $.each(inputs, function (index, value) {
                                value.style.border = "1px solid #DCDFE6";
                            })
                        }, 100)
                        //console.log(RES.data);
                    }
                    that.ShowTable = true;
                })
        },
        ValueFormatter: function (columnName, cellValue) {
            if (this.linkTableData[columnName]) {
                let obj = this.linkTableData[columnName].find(f => {
                    return f.Code == cellValue;
                });
                if (obj) {
                    cellValue = obj.Name;
                }
                return cellValue;
            }
            else if (columnName == "validity" || columnName == "Validity" || columnName == "IsValid") {
                if (cellValue == 1) {
                    return "是";
                }
                else {
                    return "否";
                }
            }
            else {
                return cellValue;
            }
        },
        InitErrorLogs: function (file) {
            let that = this;
            var batchid = that.BatchIDs.join(",");
            $.post("/Xlsx/GetErrorLogs",
                {
                    BatchID: batchid,
                    page: that.pagination2.currentpage,
                    rows: that.pagination2.pagesize
                },
                RES => {
                    that.pagination2.allNum = RES.total;
                    if (RES.data) {
                        $.each(RES.data, function (index, value) {
                            value.filename = file.name;
                            value.file = file.raw;
                        })
                        that.ErrorLogs = RES.data;
                    }
                    that.ShowLogsTable = true;
                })
        },
        InitModelSelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_ModelsGet", RES => {
                //console.log(RES);
                if (RES.success) {
                    that.ModelSelectOptions = RES.data;
                }
                else {
                    that.ModelSelectOptions = [];
                }
            })
        },
        InitEntitySelectOptions: function () {
            let that = this;
            $.post("/system/Attributes_EntitysGet", RES => {
                //console.log(RES);
                if (RES.success) {
                    that.EntitySelectOptions = RES.data;
                    that.AllEntityOptions = RES.data;
                }
                else {
                    that.EntitySelectOptions = [];
                }
            })
        },
        InitAttrSelectOptions: function () {
            let that = this;
            $.post("/system/GetAll_Attributes", {
                EntityID: that.EntityID
            }, RES => {
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
        ModelHasChanged: function () {
            //console.log(this.ModelID);
            this.EntityID = "";
            this.EntitySelectOptions = this.AllEntityOptions.filter(o => {
                return o.modelId == this.ModelID;
            })
        },
        EntityHasChanged: function () {
            //console.log(this.EntitySelectOptions);
            if (!this.ModelID) {
                this.$message.error("请先选择模型");
                this.EntityID = '';
                return;
            }
            this.EntityName = this.EntitySelectOptions.find(e => {
                return e.id == this.EntityID;
            }).name;
            this.FilterWhere = "";
            this.ColumnFixed = "";
            this.EntityColsTabLoading = true;
            window.Entity = this.EntityID;
            this.InitViewTables();
        },
        DownExcelTemp: function () {
            if (!this.EntityName) {
                this.$message.error("请先选择实体");
                return;
            }
            window.open("/Xlsx/OutputExcel?entityID=" + window.Entity)
        },
        UploadExcel: function () {
            if (!this.EntityName) {
                this.$message.error("请先选择实体");
                return;
            }
            $(".el-upload__input").click();
        },
        BeforeUpload: function () {
            this.EntityColsTabLoading = true;
            this.UploadExtra.entityname = this.EntityName;
        },
        UploadSuccess: function (response, file, fileList) {
            //console.log(response);
            //this.BatchIDs.push(response.batchid);
            //this.SelectedFile = file;
            if (response.success) {
                this.$message({
                    message: response.message,
                    type: 'success'
                });
                this.InitViewTables();
            }
            else {
                let that = this;
                that.EntityColsTabLoading = false;
                //that.$message.error(response.message);
                that.$alert(`<strong>${response.message},<a href="/Xlsx/DownError?batchid=${response.batchid}&EID=${window.Entity}" target="_blank">点此下载错误文件</a></strong>`, '错误提示', {
                    dangerouslyUseHTMLString: true,
                    confirmButtonText: '取消'
                });
                //that.TableHeight = 400;
                //that.InitErrorLogs(file);
            }
        },
        DownErrorFile: function (file) {
            //console.log(file);
            var blob = new Blob([file], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = file.name;
            link.click();
            window.URL.revokeObjectURL(link.href);
        },
        handleDelete: function (index, row) {
            //console.log(row);
            let that = this;
            var id = row.id;
            //that.SelectedRow = row;
            that.$confirm('此操作将将删除该数据, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                //$.post("/system/DelStageData",
                //    {
                //        EntityID: window.Entity,
                //        Id: id
                //    },
                //    RES => {
                //        if (RES.success) {
                //            that.$message({
                //                message: RES.message,
                //                type: 'success'
                //            });
                //        }
                //        else {
                //            this.$message.error(RES.message);
                //        }
                //        that.InitViewTables()
                //        //console.log(RES);
                //    })
            }).catch(() => {
                return;
            });
        },
        handleEdit: function (index, row, event) {
            let that = this;
            if (that.EditRowValue) {
                that.EditValue(row, event, index);
                return;
            }
            else {
                that.EditRowValue = true;
                var i = $(event.target).children()[0];
                if (i) {
                    $(i).attr("class", "el-icon-check");

                }
                else {
                    $(event.target).attr("class", "el-icon-check");
                }
                var tr = $('tr')[index + 1];
                var rowInputs = $(tr).find('input');
                $.each(rowInputs, function (index, value) {
                    $(value).removeAttr("readonly");
                    $(value)[0].style.border = "1px solid #409EFF"
                })
            }
        },
        EditValue: function (row, event, index) {
            let that = this;
            let tr = $('tr')[index + 1];
            let inputs = $(tr).find('input');
            $.each(that.TableCols, function (index, value) {
                if (value.field == "Validity") {
                    row[value.field] = $(inputs[index]).val() == "是" ? 1 : 0;
                }
                else if (that.linkTableData[value.field]) {
                    let obj = that.linkTableData[value.field].find(c => {
                        return c.Name == $(inputs[index]).val();
                    });
                    if (obj) {
                        row[value.field] = obj.Code;
                    }
                    else {
                        row[value.field] = $(inputs[index]).val();
                    }
                }
                else {
                    row[value.field] = $(inputs[index]).val();
                }
            })
            //console.log(row);
            row.batch_id = "";
            row.ValidateStatus = 1;
            row.BissnessRuleStatus = 1;
            row.WorkFlowStatus = 0;
            let postData = {};
            postData.Data = new Array(row);
            postData.EntityName = that.EntityName;
            //that.SelectedRow = row;
            that.$confirm('此操作将将修改数据, 是否继续?', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                that.EntityColsTabLoading = true;
                $.post("/system/DataSaveEntityMember",
                    {
                        body: JSON.stringify(postData)
                    },
                    RES => {
                        if (RES.success) {
                            that.$message({
                                message: RES.message,
                                type: 'success'
                            });
                        }
                        else {
                            that.$message.error(RES.message);
                        }
                        let i = $(event.target).children()[0];
                        if (i) {
                            $(i).attr("class", "el-icon-edit");

                        }
                        else {
                            $(event.target).attr("class", "el-icon-edit");
                        }
                        that.EntityColsTabLoading = false;
                        that.EditRowValue = false;
                        that.InitViewTables()
                        //console.log(RES);
                    })
            }).catch(() => {
                let tr = $('tr')[index + 1];
                let rowInputs = $(tr).find('input');
                $.each(rowInputs, function (index, value) {
                    $(value).attr("readonly");
                    $(value)[0].style.border = "1px solid #DCDFE6";
                })
                let i = $(event.target).children()[0];
                if (i) {
                    $(i).attr("class", "el-icon-edit");

                }
                else {
                    $(event.target).attr("class", "el-icon-edit");
                }
                that.EditRowValue = false;
                return;
            });
        },
        handleSizeChange: function (val) {
            this.pagination.pagesize = val;
            this.EntityColsTabLoading = true;
            this.InitViewTables()
        },
        handleCurrentChange: function (val) {
            this.pagination.currentpage = val;
            this.EntityColsTabLoading = true;
            this.InitViewTables()
        },
        handleSizeChange2: function (val) {
            this.pagination2.pagesize = val;
            //this.ErrorLogs = [];
            this.InitErrorLogs(this.SelectedFile);
        },
        handleCurrentChange2: function (val) {
            this.pagination2.currentpage = val;
            this.ErrorLogs = [];
            this.InitErrorLogs(this.SelectedFile);
        },
        DataFilter: function () {
            //var obj = {
            //    "EntityName": "Distributor",
            //    "Data":
            //        [
            //            { "ValidateStatus": 0, "BissnessRuleStatus": 0, "WorkFlowStatus": 0, "batch_id": "", "SalesOrgCode": "4543", "Type": "1", "Name": "测试2", "DeliveryAddress": "撒点", "OrderAddress": "上海市嘉定区南翔镇沪宜公路1097号南翔智地·越界产业园", "ShipToCode": "1", "Contact": "12", "Phone": "12", "Address": "12", "Code": 130222, "CreateUser": "admin", "CreateTime": "2020-06-08 13:35:09", "UpdateUser": "admin", "UpdateTime": "2020-06-08 13:35:09", "Validity": 1 }
            //        ]
            //}
            //$.ajax({
            //    type: "get",
            //    url: "http://192.168.11.201:5004/OAuth/token?name=admin&pwd=123",
            //    dataType: "json",
            //    success: function (data) {
            //        debugger
            //        var xhr = new XMLHttpRequest();
            //        xhr.open("post", "http://192.168.11.201:5004/mdmapi/DataSaveEntityMember", true)
            //        xhr.setRequestHeader("authorization", data.authorization);
            //        xhr.responseType = 'json';
            //        xhr.send(obj);
            //        xhr.onreadystatechange = function () {
            //            if (xhr.readyState == 4 && xhr.status == 200) {
            //                console.log(xhr.response)
            //            }
            //        }
            //    }, error: function (error) {
            //        console.log(error);
            //    }
            //});
            if (!this.ModelID) {
                this.$message.error("请先选择模型");
                this.EntityID = '';
                return;
            }
            if (!this.EntityName) {
                this.$message.error("请先选择实体");
                return;
            }
            this.InitAttrSelectOptions();
            this.DrawerDialog = true;
        },
        DrawerhandleClose: function () {
            this.FilterData = [];
        },
        AddFilter: function () {
            this.FilterData.push({ AttrName: '', Operator: '', Content: '' });
        },
        RefreshFilter: function () {
            this.FilterData = [];
        },
        DrawerhandleDelete: function (row) {
            //console.log(row);
            this.FilterData.pop(row);
        },
        SearchFilter: function () {
            this.FilterWhere = JSON.stringify(this.FilterData);
            this.EntityColsTabLoading = true;
            this.InitViewTables();
        }
    }
})