"use strict";

var ToolVm = new Vue({
  el: "#CommonTool",
  data: {
    TableData: [],
    TableCols: [],
    DrawerCols: [{
      field: "AttrName",
      label: "属性"
    }, {
      field: "Operator",
      label: "运算符"
    }, {
      field: "Content",
      label: "条件"
    }],
    FilterData: [{
      AttrName: '',
      Operator: '',
      Content: ''
    }],
    FilterWhere: "",
    Operator: [{
      filter: "不为NULL",
      value: "is not null"
    }, {
      filter: "不包含模式",
      value: "不包含模式"
    }, {
      filter: "不匹配",
      value: "不匹配"
    }, {
      filter: "不等于",
      value: "!="
    }, {
      filter: "为NULL",
      value: "is null"
    }, {
      filter: "包含模式",
      value: "包含模式"
    }, {
      filter: "匹配",
      value: "匹配"
    }, {
      filter: "大于",
      value: ">"
    }, {
      filter: "大于或等于",
      value: ">="
    }, {
      filter: "小于",
      value: "<"
    }, {
      filter: "小于或等于",
      value: "<="
    }, {
      filter: "开头为",
      value: "开头为"
    }, {
      filter: "等于",
      value: "="
    }],
    TableHeight: 'calc(100vh - 178px)',
    EntityColsTabLoading: false,
    ModelSelectOptions: [],
    EntitySelectOptions: [],
    AllEntityOptions: [],
    AttrsSelectOptions: [],
    AllAttrSelectOptions: [],
    ErrorLogs: [],
    ErrorLogsCols: [{
      field: "filename",
      title: "文件名"
    }, {
      field: "errorMessages",
      title: "错误消息"
    }, {
      field: "state",
      title: "状态"
    }],
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
      allNum: 0
    },
    pagination2: {
      currentpage: 1,
      pagesizes: [5, 10, 15, 20],
      pagesize: 5,
      allNum: 0
    },
    UploadExtra: {
      entityname: ''
    },
    EditRowValue: false,
    linkTableData: [],
    DrawerDialog: false,
    ColumnFixed: ''
  },
  created: function created() {
    var urlParms = window.location.hash.split("/");

    if (/^[0-9]*$/g.test(urlParms[2])) {
      this.InitViewTables();
    }

    this.InitModelSelectOptions();
    this.InitEntitySelectOptions();
  },
  mounted: function mounted() {},
  methods: {
    headerClass: function headerClass() {
      return 'text-align: center;background:#eef1f6;';
    },
    InitViewTables: function InitViewTables() {
      var that = this;
      $.post("/MasterData_Maintain_Manage/InitEntityTable", {
        Entity: window.Entity,
        where: that.FilterWhere,
        page: that.pagination.currentpage,
        rows: that.pagination.pagesize
      }, function (RES) {
        that.EntityColsTabLoading = false; //console.log(RES);

        if (that.DrawerDialog) {
          that.DrawerDialog = false;
        }

        that.TableCols = [];
        that.TableData = [];

        if (RES.data.length == 0) {
          that.pagination.allNum = RES.total;
          that.EntityName = RES.entityName;
          $.each(RES.tableCols, function (index, value) {
            var obj = {};
            obj.field = value.Name;
            obj.Label = value.DisplayName;
            that.TableCols.push(obj);
          });
          that.ColumnFixed = "right";
        } else {
          that.EntityName = RES.entityName;
          that.pagination.allNum = RES.total;
          that.linkTableData = RES.linkTableData;
          $.each(RES.tableCols, function (index, value) {
            var obj = {};
            obj.field = value.Name;
            obj.Label = value.DisplayName;
            that.TableCols.push(obj);
          });
          $.each(RES.data, function (index, value) {
            $.each(that.TableCols, function (idx, val) {
              value[val.field] = that.ValueFormatter(val.field, value[val.field]);
            });
          });
          setTimeout(function () {
            that.ColumnFixed = "right";
          }, 500);
          that.TableData = RES.data;
          setTimeout(function () {
            var inputs = $('table .el-input__inner');
            $.each(inputs, function (index, value) {
              value.style.border = "1px solid #DCDFE6";
            });
          }, 100); //console.log(RES.data);
        }

        that.ShowTable = true;
      });
    },
    ValueFormatter: function ValueFormatter(columnName, cellValue) {
      if (this.linkTableData[columnName]) {
        var obj = this.linkTableData[columnName].find(function (f) {
          return f.Code == cellValue;
        });

        if (obj) {
          cellValue = obj.Name;
        }

        return cellValue;
      } else if (columnName == "validity" || columnName == "Validity" || columnName == "IsValid") {
        if (cellValue == 1) {
          return "是";
        } else {
          return "否";
        }
      } else {
        return cellValue;
      }
    },
    InitErrorLogs: function InitErrorLogs(file) {
      var that = this;
      var batchid = that.BatchIDs.join(",");
      $.post("/Xlsx/GetErrorLogs", {
        BatchID: batchid,
        page: that.pagination2.currentpage,
        rows: that.pagination2.pagesize
      }, function (RES) {
        that.pagination2.allNum = RES.total;

        if (RES.data) {
          $.each(RES.data, function (index, value) {
            value.filename = file.name;
            value.file = file.raw;
          });
          that.ErrorLogs = RES.data;
        }

        that.ShowLogsTable = true;
      });
    },
    InitModelSelectOptions: function InitModelSelectOptions() {
      var that = this;
      $.post("/system/Attributes_ModelsGet", function (RES) {
        //console.log(RES);
        if (RES.success) {
          that.ModelSelectOptions = RES.data;
        } else {
          that.ModelSelectOptions = [];
        }
      });
    },
    InitEntitySelectOptions: function InitEntitySelectOptions() {
      var that = this;
      $.post("/system/Attributes_EntitysGet", function (RES) {
        //console.log(RES);
        if (RES.success) {
          that.EntitySelectOptions = RES.data;
          that.AllEntityOptions = RES.data;
        } else {
          that.EntitySelectOptions = [];
        }
      });
    },
    InitAttrSelectOptions: function InitAttrSelectOptions() {
      var that = this;
      $.post("/system/GetAll_Attributes", {
        EntityID: that.EntityID
      }, function (RES) {
        if (RES.success) {
          that.AttrsSelectOptions = RES.data;
          that.AllAttrSelectOptions = RES.data;
        } else {
          that.AttrsSelectOptions = [];
          that.AllAttrSelectOptions = [];
        }
      });
    },
    ModelHasChanged: function ModelHasChanged() {
      var _this = this;

      //console.log(this.ModelID);
      this.EntityID = "";
      this.EntitySelectOptions = this.AllEntityOptions.filter(function (o) {
        return o.modelId == _this.ModelID;
      });
    },
    EntityHasChanged: function EntityHasChanged() {
      var _this2 = this;

      //console.log(this.EntitySelectOptions);
      if (!this.ModelID) {
        this.$message.error("请先选择模型");
        this.EntityID = '';
        return;
      }

      this.EntityName = this.EntitySelectOptions.find(function (e) {
        return e.id == _this2.EntityID;
      }).name;
      this.FilterWhere = "";
      this.ColumnFixed = "";
      this.EntityColsTabLoading = true;
      window.Entity = this.EntityID;
      this.InitViewTables();
    },
    DownExcelTemp: function DownExcelTemp() {
      if (!this.EntityName) {
        this.$message.error("请先选择实体");
        return;
      }

      window.open("/Xlsx/OutputExcel?entityID=" + window.Entity);
    },
    UploadExcel: function UploadExcel() {
      if (!this.EntityName) {
        this.$message.error("请先选择实体");
        return;
      }

      $(".el-upload__input").click();
    },
    BeforeUpload: function BeforeUpload() {
      this.EntityColsTabLoading = true;
      this.UploadExtra.entityname = this.EntityName;
    },
    UploadSuccess: function UploadSuccess(response, file, fileList) {
      //console.log(response);
      //this.BatchIDs.push(response.batchid);
      //this.SelectedFile = file;
      if (response.success) {
        this.$message({
          message: response.message,
          type: 'success'
        });
        this.InitViewTables();
      } else {
        var that = this;
        that.EntityColsTabLoading = false; //that.$message.error(response.message);

        that.$alert("<strong>".concat(response.message, ",<a href=\"/Xlsx/DownError?batchid=").concat(response.batchid, "&EID=").concat(window.Entity, "\" target=\"_blank\">\u70B9\u6B64\u4E0B\u8F7D\u9519\u8BEF\u6587\u4EF6</a></strong>"), '错误提示', {
          dangerouslyUseHTMLString: true,
          confirmButtonText: '取消'
        }); //that.TableHeight = 400;
        //that.InitErrorLogs(file);
      }
    },
    DownErrorFile: function DownErrorFile(file) {
      //console.log(file);
      var blob = new Blob([file], {
        type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
      });
      var link = document.createElement('a');
      link.href = window.URL.createObjectURL(blob);
      link.download = file.name;
      link.click();
      window.URL.revokeObjectURL(link.href);
    },
    handleDelete: function handleDelete(index, row) {
      //console.log(row);
      var that = this;
      var id = row.id; //that.SelectedRow = row;

      that.$confirm('此操作将将删除该数据, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(function () {//$.post("/system/DelStageData",
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
      })["catch"](function () {
        return;
      });
    },
    handleEdit: function handleEdit(index, row, event) {
      var that = this;

      if (that.EditRowValue) {
        that.EditValue(row, event, index);
        return;
      } else {
        that.EditRowValue = true;
        var i = $(event.target).children()[0];

        if (i) {
          $(i).attr("class", "el-icon-check");
        } else {
          $(event.target).attr("class", "el-icon-check");
        }

        var tr = $('tr')[index + 1];
        var rowInputs = $(tr).find('input');
        $.each(rowInputs, function (index, value) {
          $(value).removeAttr("readonly");
          $(value)[0].style.border = "1px solid #409EFF";
        });
      }
    },
    EditValue: function EditValue(row, event, index) {
      var that = this;
      var tr = $('tr')[index + 1];
      var inputs = $(tr).find('input');
      $.each(that.TableCols, function (index, value) {
        if (value.field == "Validity") {
          row[value.field] = $(inputs[index]).val() == "是" ? 1 : 0;
        } else if (that.linkTableData[value.field]) {
          var obj = that.linkTableData[value.field].find(function (c) {
            return c.Name == $(inputs[index]).val();
          });

          if (obj) {
            row[value.field] = obj.Code;
          } else {
            row[value.field] = $(inputs[index]).val();
          }
        } else {
          row[value.field] = $(inputs[index]).val();
        }
      }); //console.log(row);

      row.batch_id = "";
      row.ValidateStatus = 1;
      row.BissnessRuleStatus = 1;
      row.WorkFlowStatus = 0;
      var postData = {};
      postData.Data = new Array(row);
      postData.EntityName = that.EntityName; //that.SelectedRow = row;

      that.$confirm('此操作将将修改数据, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(function () {
        that.EntityColsTabLoading = true;
        $.post("/system/DataSaveEntityMember", {
          body: JSON.stringify(postData)
        }, function (RES) {
          if (RES.success) {
            that.$message({
              message: RES.message,
              type: 'success'
            });
          } else {
            that.$message.error(RES.message);
          }

          var i = $(event.target).children()[0];

          if (i) {
            $(i).attr("class", "el-icon-edit");
          } else {
            $(event.target).attr("class", "el-icon-edit");
          }

          that.EntityColsTabLoading = false;
          that.EditRowValue = false;
          that.InitViewTables(); //console.log(RES);
        });
      })["catch"](function () {
        var tr = $('tr')[index + 1];
        var rowInputs = $(tr).find('input');
        $.each(rowInputs, function (index, value) {
          $(value).attr("readonly");
          $(value)[0].style.border = "1px solid #DCDFE6";
        });
        var i = $(event.target).children()[0];

        if (i) {
          $(i).attr("class", "el-icon-edit");
        } else {
          $(event.target).attr("class", "el-icon-edit");
        }

        that.EditRowValue = false;
        return;
      });
    },
    handleSizeChange: function handleSizeChange(val) {
      this.pagination.pagesize = val;
      this.EntityColsTabLoading = true;
      this.InitViewTables();
    },
    handleCurrentChange: function handleCurrentChange(val) {
      this.pagination.currentpage = val;
      this.EntityColsTabLoading = true;
      this.InitViewTables();
    },
    handleSizeChange2: function handleSizeChange2(val) {
      this.pagination2.pagesize = val; //this.ErrorLogs = [];

      this.InitErrorLogs(this.SelectedFile);
    },
    handleCurrentChange2: function handleCurrentChange2(val) {
      this.pagination2.currentpage = val;
      this.ErrorLogs = [];
      this.InitErrorLogs(this.SelectedFile);
    },
    DataFilter: function DataFilter() {
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
    DrawerhandleClose: function DrawerhandleClose() {
      this.FilterData = [];
    },
    AddFilter: function AddFilter() {
      this.FilterData.push({
        AttrName: '',
        Operator: '',
        Content: ''
      });
    },
    RefreshFilter: function RefreshFilter() {
      this.FilterData = [];
    },
    DrawerhandleDelete: function DrawerhandleDelete(row) {
      //console.log(row);
      this.FilterData.pop(row);
    },
    SearchFilter: function SearchFilter() {
      this.FilterWhere = JSON.stringify(this.FilterData);
      this.EntityColsTabLoading = true;
      this.InitViewTables();
    }
  }
});