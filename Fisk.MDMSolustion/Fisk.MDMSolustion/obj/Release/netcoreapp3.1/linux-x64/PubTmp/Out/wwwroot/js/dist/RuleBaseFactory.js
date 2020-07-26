"use strict";

var _data;

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var RuleBaseManage = new Vue({
  el: '#RuleBaseManage',
  data: (_data = {
    tableHeight: 300,
    //默认给300
    RuleBaseTableData: [],
    //角色表格
    total: 1,
    //总条数
    pagenum: 1,
    // 当前页
    pagesize: 10,
    // 每页条数
    pageMaxsize: '',
    //当前页可以存放最大条数
    Title: '添加角色',
    //弹框标题
    ModelName: "",
    TabNumber: 'first',
    activeName: 'first',
    ModelSelectOptions: [],
    EntityName: "",
    EntitySelectOptions: [],
    AttributeSelectOptions: [],
    AddRuleBasedialogVisible: false,
    //添加角色弹框是否显示
    //添加用户角色form表单
    RuleBaseForm: {
      ID: '',
      ModelID: '',
      EntityID: '',
      AttributeID: '',
      AttributeName: "",
      RuleName: '',
      Description: '',
      Required: true,
      Type: '',
      Expression: "",
      CustomTypeName: ""
    },
    //验证Roleform表单规则
    RuleBaseRules: {
      AttributeName: [{
        required: true,
        message: "请选择属性",
        trigger: 'blur'
      }],
      Description: [{
        required: true,
        message: "请输入规则描述",
        trigger: 'blur'
      }],
      RuleName: [{
        required: true,
        message: "请输入规则名称",
        trigger: 'blur'
      }],
      Type: [{
        required: true,
        message: "请选择验证规则",
        trigger: 'blur'
      }]
    },
    RuleBaseTabLoading: false,
    pagenumBottom: '',
    EditOrAdd: 'Add',
    CustomTypeIsShow: false,
    RegularIs: false,
    DataValidationTableData: [],
    DataValidationTabLoading: false,
    DataValidationForm: {
      Id: 0,
      Name: '',
      ModelID: 0,
      EntityID: 0,
      Description: '',
      FunctionName: ''
    },
    DataValidationFormRules: {
      Name: [{
        required: true,
        message: "请输入名称",
        trigger: 'blur'
      }],
      Description: [{
        required: true,
        message: "请输入说明",
        trigger: 'blur'
      }],
      FunctionName: [{
        required: true,
        message: "请输入存储过程名称",
        trigger: 'blur'
      }]
    },
    DatadialogTitle: '添加逻辑',
    DatadialogVisible: false,
    DataModelName: '',
    DataEntityName: '',
    DataModelSelectOptions: [],
    DataEntitySelectOptions: [],
    //-----------字段验证----
    FieldValidationTableData: [],
    //-----------数据维护------//
    EntityDataTableData: [],
    pagination: {
      currentpage: 1,
      pagesizes: [10, 20, 30, 40],
      pagesize: 10,
      allNum: 0
    },
    DataFormModel: {
      RuleName: '',
      RuleType: '',
      Apiaddress: '',
      RuleRemark: '',
      AttributeID: [],
      BpmName: '',
      ApplyTitle: ''
    },
    FieldEntityID: ''
  }, _defineProperty(_data, "DataValidationFormRules", {
    RuleName: [{
      required: true,
      message: "请输入名称",
      trigger: 'blur'
    }],
    RuleRemark: [{
      required: true,
      message: "请输入说明",
      trigger: 'blur'
    }],
    RuleType: [{
      required: true,
      message: "请选择维护类型",
      trigger: 'blur'
    }],
    Apiaddress: [{
      required: true,
      message: "请填写API地址",
      trigger: 'blur'
    }],
    AttributeID: [{
      required: true,
      message: "请选择属性",
      trigger: 'blur'
    }]
  }), _defineProperty(_data, "DataModelChangeSelectOptions", [{
    name: '添加',
    value: 'add'
  }, {
    name: '变更监控',
    value: 'update'
  }]), _defineProperty(_data, "DatadialogVisible2", false), _defineProperty(_data, "DatadialogTitle2", '添加数据维护规则'), _defineProperty(_data, "AttrsSelectOptions", []), _defineProperty(_data, "SelectedRow", ''), _defineProperty(_data, "HasError", false), _defineProperty(_data, "FieldValidationTabLoading", false), _defineProperty(_data, "EntityDataTabLoading", false), _defineProperty(_data, "FieldValidationForm", {
    Id: 0,
    Name: '',
    ModelID: 0,
    EntityID: 0,
    Description: '',
    FunctionName: ''
  }), _defineProperty(_data, "FieldValidationFormRules", {
    Name: [{
      required: true,
      message: "请输入名称",
      trigger: 'blur'
    }],
    Description: [{
      required: true,
      message: "请输入说明",
      trigger: 'blur'
    }],
    FunctionName: [{
      required: true,
      message: "请输入存储过程名称",
      trigger: 'blur'
    }]
  }), _defineProperty(_data, "FieldialogTitle", '添加逻辑'), _defineProperty(_data, "FieldialogVisible", false), _defineProperty(_data, "FieldModelName", ''), _defineProperty(_data, "FieldEntityName", ''), _defineProperty(_data, "FieldModelSelectOptions", []), _defineProperty(_data, "FieldEntitySelectOptions", []), _data),
  created: function created() {},
  mounted: function mounted() {
    var _this = this;

    this.GetModelSelectData();
    this.$nextTick(function () {
      _this.pagesize = parseInt((LayOutStaticWindowsHeight - 390) / 40);
      _this.tableHeight = parseInt(LayOutStaticWindowsHeight - 210);
      _this.pageMaxsize = _this.pagesize;

      _this.GetModelSelectData();

      _this.GetRuleBaseTableList();
    });
  },
  methods: {
    GetModelSelectData: function GetModelSelectData() {
      var _this2 = this;

      $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, function (res) {
        if (res.success) {
          _this2.ModelSelectOptions = res.data;

          if (res.data != null) {
            var ModelID = res.data[0].id;
            _this2.ModelName = res.data[0].name;
            _this2.RuleBaseForm.ModelID = ModelID;

            _this2.GetEntitySelectData(ModelID);
          } else {
            _this2.GetEntitySelectData(ModelID);
          }
        }
      });
    },
    GetEntitySelectData: function GetEntitySelectData(ModelID) {
      var _this3 = this;

      $.post('/MasterData_Quality_Manage/GetEntitySelectRule', {
        ModelID: ModelID
      }, function (res) {
        if (res.success) {
          if (res.data.length > 0) {
            _this3.EntitySelectOptions = res.data; // console.log(res.data);

            _this3.EntityName = res.data[0].name;
            _this3.RuleBaseForm.EntityID = res.data[0].id;

            _this3.GetAttributeSelectData();
          } else {
            _this3.EntityName = '';
            _this3.RuleBaseForm.EntityID = 0;
          }

          _this3.GetRuleBaseTableList();
        }
      });
    },
    GetAttributeSelectData: function GetAttributeSelectData() {
      var _this4 = this;

      $.post('/MasterData_Quality_Manage/GetAttributeSelectRule', {
        EntityID: this.RuleBaseForm.EntityID
      }, function (res) {
        if (res.success) {
          _this4.AttributeSelectOptions = res.data;
        }
      });
    },
    //进入页面渲染表格
    GetRuleBaseTableList: function GetRuleBaseTableList() {
      this.RuleBaseTabLoading = true;
      $.post('/MasterData_Quality_Manage/SearchRuleBase', {
        Where: this.RuleBaseForm.EntityID,
        Page: this.pagenum,
        limit: this.pagesize
      }, function (res) {
        RuleBaseManage.RuleBaseTableData = res.data;
        RuleBaseManage.total = res.total;
        RuleBaseManage.RuleBaseTabLoading = false;
      }, 'json');
    },
    // 只要每页条数变化了, 触发
    handleSizeChange: function handleSizeChange(val) {
      // 更新每页条数
      RuleBaseManage.pagesize = val; // 只要修改了每页条数, 数据所在的页码发生了变化了, 回到第一页

      RuleBaseManage.pagenum = 1; // 重新渲染

      RuleBaseManage.GetRoleTableList();
    },
    // 只要当前页变化时, 触发函数
    handleCurrentChange: function handleCurrentChange(val) {
      // 更新当前页
      RuleBaseManage.pagenum = val; // 重新渲染

      RuleBaseManage.GetRoleTableList();
    },
    //点击添加用户对话框
    OpenRuleBaseDialog: function OpenRuleBaseDialog() {
      RuleBaseManage.AddRuleBasedialogVisible = true;
      RuleBaseManage.Title = '添加业务规则';
      RuleBaseManage.EditOrAdd = "Add";
      this.CustomTypeIsShow = false;
    },
    //点击edit 弹框 ,修改弹框名称,内容回显
    OpenRuleBaseEditDialog: function OpenRuleBaseEditDialog(index, row) {
      var _this5 = this;

      RuleBaseManage.AddRuleBasedialogVisible = true;
      RuleBaseManage.$nextTick(function () {
        RuleBaseManage.Title = '修改业务规则';
        RuleBaseManage.EditOrAdd = "Edit";
        RuleBaseManage.RuleBaseForm.ID = row.id;
        RuleBaseManage.RuleBaseForm.RuleName = row.ruleName;
        RuleBaseManage.RuleBaseForm.Description = row.description;
        RuleBaseManage.RuleBaseForm.Required = row.required == "true" ? true : false;

        if (row.type != "手机号" && row.type != "邮箱") {
          _this5.CustomTypeIsShow = true;
          RuleBaseManage.RuleBaseForm.Expression = row.expression;
        } else {
          _this5.CustomTypeIsShow = false;
        }

        RuleBaseManage.RuleBaseForm.Type = row.type;
        RuleBaseManage.RuleBaseForm.AttributeName = row.attributeName;
        RuleBaseManage.RuleBaseForm.ModelID = row.modelID;
        RuleBaseManage.RuleBaseForm.AttributeID = row.attributeID;
        RuleBaseManage.RuleBaseForm.EntityID = row.entityID;
      });
    },
    //点击删除角色
    DelRuleBase: function DelRuleBase(row) {
      //console.log(row)
      RuleBaseManage.$confirm("是否删除", "业务规则删除", {
        cancelButtonClass: "btn-custom-cancel",
        cancelButtonText: "取消",
        confirmButtonText: "确定",
        type: 'warning',
        callback: function callback(action, instance) {
          if (action === 'confirm') {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/DeleteRuleBase', {
              RuleID: row.id
            }, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.GetRuleBaseTableList();
                RuleBaseManage.$message.success(res.message);
              } else {
                RuleBaseManage.$message.error(res.message);
              }
            }, 'json');
          }
        }
      });
    },
    //关闭添加用户对话框或者修改用户对话框
    ClosedAddRuleBasedialog: function ClosedAddRuleBasedialog() {
      var _this6 = this;

      RuleBaseManage.$nextTick(function () {
        RuleBaseManage.AddRuleBasedialogVisible = false;
        RuleBaseManage.$refs.RuleBaseForm.resetFields();
        _this6.RuleBaseForm.Expression = "";
      });
    },
    //关闭添加用户对话框或者修改用户对话框
    CloseRuleBasedialogVisible: function CloseRuleBasedialogVisible(RoleForm) {
      var _this7 = this;

      RuleBaseManage.$nextTick(function () {
        RuleBaseManage.AddRuleBasedialogVisible = false;
        RuleBaseManage.$refs.RuleBaseForm.resetFields(); //重置表单

        _this7.RuleBaseForm.Expression = "";
      });
    },
    SubmitRuleBaseForm: function SubmitRuleBaseForm() {
      var _this8 = this;

      //提交角色表单
      if (RuleBaseManage.EditOrAdd == 'Add') {
        RuleBaseManage.$refs.RuleBaseForm.validate(function (valid) {
          if (valid) {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/InsertRuleBase', _this8.RuleBaseForm, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.$nextTick(function () {
                  RuleBaseManage.$message.success(res.message);
                  RuleBaseManage.AddRuleBasedialogVisible = false;
                  RuleBaseManage.GetRuleBaseTableList();
                  RuleBaseManage.$refs.RuleBaseForm.resetFields(); //重置表单
                });
              } else {
                RuleBaseManage.$message.error(res.message);
                RuleBaseManage.AddRuleBasedialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      } else {
        RuleBaseManage.$refs.RuleBaseForm.validate(function (valid) {
          if (valid) {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/EiteRuleBase', _this8.RuleBaseForm, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.$nextTick(function () {
                  RuleBaseManage.$message.success(res.message);
                  RuleBaseManage.AddRuleBasedialogVisible = false;
                  RuleBaseManage.GetRuleBaseTableList();
                  RuleBaseManage.$refs.RuleBaseForm.resetFields(); //重置表单
                });
              } else {
                RuleBaseManage.$message.error(res.message);
                RuleBaseManage.AddRuleBasedialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      }
    },
    //  表头样式
    headerClass: function headerClass() {
      return 'text-align: center;background:#eef1f6;';
    },
    AddRole: function AddRole() {
      RuleBaseManage.TitleType = 'Add';
      RuleBaseManage.Add_Edit_Role();
    },
    TabHandleClick: function TabHandleClick(tab, event) {
      var TabNumber = tab.name;
      this.TabNumber = tab.name;
      this.pagenum = 1;

      switch (TabNumber) {
        case "second":
          this.GetDataValidationTableList();
          this.GetDataModelSelectData();
          break;

        case "first":
          this.GetRuleBaseTableList();
          break;

        case "Three":
          this.GetFieldModelSelectData();
          this.InitEntityDataTable();
          break;

        default:
          break;
      }
    },
    //绑定回车事件
    EnterSearch: function EnterSearch() {
      $('#RoleKeyStr').bind('keydown', function (event) {
        if (event.keyCode == "13") {
          RuleBaseManage.ReloadTable('Role');
        }
      });
    },
    ModelHasChanged: function ModelHasChanged(ModelID) {
      switch (this.TabNumber) {
        case "second":
          this.GetDataEntitySelectData(ModelID);

        case "first":
          this.GetEntitySelectData(ModelID);
          break;

        case "Three":
          this.GetFieldEntitySelectData(ModelID);
          this.FieldEntityID = "";
          break;
      }
    },
    EntityHasChanged: function EntityHasChanged(ID) {
      switch (this.TabNumber) {
        case "second":
          this.DataValidationForm.EntityID = ID;
          this.GetDataValidationTableList();

        case "first":
          this.RuleBaseForm.EntityID = ID;
          this.GetAttributeSelectData();
          this.GetRuleBaseTableList();
          break;

        case "Three":
          this.FieldValidationForm.EntityID = ID;
          break;
      }
    },
    AttributeNameDialogChange: function AttributeNameDialogChange(ID) {
      this.RuleBaseForm.AttributeID = ID;
    },
    TypeChange: function TypeChange(val) {
      switch (val) {
        case "手机号":
          this.CustomTypeIsShow = false;
          this.RuleBaseForm.Expression = "^[1][35678][0-9]{9}$";
          this.RegularIs = true;
          break;

        case "邮箱":
          this.CustomTypeIsShow = false;
          this.RuleBaseForm.Expression = "^[a-z0-9A-Z]+[-|a-z0-9A-Z._]+@([a-z0-9A-Z]+(-[a-z0-9A-Z]+)?\\.)+[a-z]{2,}$";
          this.RegularIs = true;
          break;

        case "自定义验证规则":
          this.CustomTypeIsShow = true;
          this.RuleBaseForm.Expression = "";
          this.RegularIs = false;
          break;

        default:
          break;
      }
    },
    ReleaseProc: function ReleaseProc() {
      var _this9 = this;

      var obj = {
        modelID: this.RuleBaseForm.ModelID,
        entityID: this.RuleBaseForm.EntityID
      };
      $.post('/MasterData_Quality_Manage/AttributeRuleRelease', obj, function (res) {
        if (res.success) {
          _this9.$message.success(res.message);

          _this9.GetRuleBaseTableList();
        } else _this9.$message.error(res.message);
      });
    },
    //发布存储过程
    //------------------------------------数据维护--------------------------------------------
    InitEntityDataTable: function InitEntityDataTable() {
      var that = this;
      $.post("/MasterData_Quality_Manage/GetDatamaintenance", {
        page: that.pagination.currentpage,
        rows: that.pagination.pagesize
      }, function (RES) {
        // console.log(RES);
        if (RES.data) {
          that.EntityDataTableData = RES.data;
          that.pagination.allNum = RES.total;
        }
      });
    },
    OpenDataTableDialog: function OpenDataTableDialog() {
      if (!this.FieldEntityID) {
        this.$message.error("请先选择实体");
        return;
      }

      RuleBaseManage.DatadialogVisible2 = true;
      RuleBaseManage.DatadialogTitle2 = '添加数据维护规则';
      RuleBaseManage.EditOrAdd = "Add";
    },
    resetFormModel: function resetFormModel() {
      RuleBaseManage.DataFormModel.RuleType = "";
      RuleBaseManage.DataFormModel.AttributeID = [];
      delete RuleBaseManage.DataFormModel.Id;
      RuleBaseManage.$refs.DataFormModel.resetFields();
    },
    DataCancelAddOrUpdate2: function DataCancelAddOrUpdate2() {
      this.resetFormModel();
      this.DatadialogVisible2 = false;
    },
    SubmitDataFrom: function SubmitDataFrom() {
      var that = this;
      that.$refs.DataFormModel.validate(function (valid) {
        if (valid) {
          var url = "";
          var postData = {};

          if (that.EditOrAdd == "Add") {
            if (that.HasError) {
              if (that.DataFormModel.AttributeID) {
                var attrArray = that.DataFormModel.AttributeID;
                that.DataFormModel.AttributeID = [];
                $.each(attrArray, function (index, value) {
                  var id = that.AttrsSelectOptions.find(function (a) {
                    return a.name == value;
                  }).id;
                  that.DataFormModel.AttributeID.push(id);
                });
                that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
              }
            } else {
              that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
            }

            postData.entityid = that.FieldEntityID;
            postData._Datamaintenance = JSON.stringify(that.DataFormModel);
            url = "/MasterData_Quality_Manage/AddDatamaintenance";
          }

          if (that.EditOrAdd == "update") {
            var _attrArray = that.DataFormModel.AttributeID;
            that.DataFormModel.AttributeID = [];
            $.each(_attrArray, function (index, value) {
              var attr = that.AttrsSelectOptions.find(function (a) {
                return a.name == value;
              });

              if (attr) {
                that.DataFormModel.AttributeID.push(attr.id);
              } else {
                that.DataFormModel.AttributeID.push(value);
              }
            });
            that.DataFormModel.AttributeID = that.DataFormModel.AttributeID.join(",");
            url = "/MasterData_Quality_Manage/UpdataDatamaintenance";
            postData.FormModel = JSON.stringify(that.DataFormModel);
          }

          $.post(url, postData, function (RES) {
            if (RES.success) {
              that.$message({
                message: RES.message,
                type: 'success'
              });
              that.InitEntityDataTable();
              that.DatadialogVisible2 = false;
              that.resetFormModel();
            } else {
              if (typeof that.DataFormModel.AttributeID == "string") {
                if (that.DataFormModel.AttributeID) {
                  var _attrArray2 = that.DataFormModel.AttributeID.split(",");

                  that.DataFormModel.AttributeID = [];
                  $.each(_attrArray2, function (index, value) {
                    var name = that.AttrsSelectOptions.find(function (a) {
                      return a.id == value;
                    }).name;
                    that.DataFormModel.AttributeID.push(name);
                  });
                }
              }

              that.HasError = true;
              that.$message.error(RES.message);
            }
          });
        } else {
          return false;
        }
      });
    },
    handleSizeChange3: function handleSizeChange3(val) {
      this.pagination.pagesize = val();
      this.InitEntityDataTable();
    },
    handleCurrentChange3: function handleCurrentChange3(val) {
      this.pagination.currentpage = val;
      this.InitEntityDataTable();
    },
    DatahandleClose2: function DatahandleClose2() {
      this.DataFormModel.AttributeID = [];
      this.resetFormModel();
    },
    DelDtValidation: function DelDtValidation(row) {
      var that = this; // console.log(row);

      that.$confirm('此操作将删除该规则, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(function () {
        $.post("/MasterData_Quality_Manage/DelEntityDataRule", {
          Id: row.id
        }, function (RES) {
          //console.log(RES);
          if (RES.success) {
            that.$message({
              message: RES.message,
              type: 'success'
            });
            that.InitEntityDataTable();
          } else {
            that.$message.error(RES.message);
          }
        });
      })["catch"](function () {
        return;
      });
    },
    EntityDataRuleEditDialog: function EntityDataRuleEditDialog(row) {
      var _this10 = this;

      //console.log(row);
      this.SelectedRow = row;
      this.FieldEntityID = row.entityID;
      this.DatadialogVisible2 = true;
      this.DatadialogTitle2 = "编辑实体数据维护规则";
      this.EditOrAdd = "update";
      this.$nextTick(function () {
        if (row.ruleType == "update") {
          _this10.AttrsSelectOptions = [];

          _this10.InitAttrSelectOptions();

          _this10.DataFormModel.AttributeID = [];
        }

        _this10.DataFormModel.RuleType = row.ruleType;
        _this10.DataFormModel.Id = row.id;
        _this10.DataFormModel.Apiaddress = row.apiaddress;
        _this10.DataFormModel.RuleName = row.ruleName;
        _this10.DataFormModel.RuleRemark = row.ruleRemark;
        _this10.DataFormModel.BpmName = row.bpmName;
        _this10.DataFormModel.ApplyTitle = row.applyTitle;
      });
    },
    InitAttrSelectOptions: function InitAttrSelectOptions() {
      var that = this;
      $.post("/MasterData_Quality_Manage/Attributes_GetAll_ByEntityID", {
        EntityID: that.FieldEntityID
      }, function (RES) {
        if (RES.success) {
          that.AttrsSelectOptions = RES.data;

          if (that.EditOrAdd == "update") {
            if (that.SelectedRow.attributeID) {
              var RowsAttrs = that.SelectedRow.attributeID.split(",");

              if (RowsAttrs.length > 0) {
                $.each(RowsAttrs, function (index, value) {
                  var name = RuleBaseManage.AttrsSelectOptions.find(function (a) {
                    return a.id == value;
                  }).name;
                  RuleBaseManage.DataFormModel.AttributeID.push(name);
                });
              }
            }
          }
        } else {
          that.AttrsSelectOptions = [];
        }
      });
    },
    TypeHasChanged: function TypeHasChanged() {
      if (this.DataFormModel.RuleType == "update") {
        this.AttrsSelectOptions = [];
        this.DataFormModel.AttributeID = [];
        this.InitAttrSelectOptions();
      }

      if (this.DataFormModel.RuleType == "add") {
        if (this.EditOrAdd == "update") {
          this.SelectedRow.attributeID = "";
        }

        this.AttrsSelectOptions = [];
        this.DataFormModel.AttributeID = [];
      }
    },
    TypeTransfer: function TypeTransfer(row, column, cellValue, index) {
      if (cellValue == "update") {
        return "变更监控";
      } else {
        return "添加";
      }
    },
    //------------------------------------ 数据验证------------------------------------------
    SubmitDataValidationFrom: function SubmitDataValidationFrom() {
      var _this11 = this;

      if (RuleBaseManage.EditOrAdd == 'Add') {
        RuleBaseManage.$refs.DataValidationForm.validate(function (valid) {
          if (valid) {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/InsertDataValidation', _this11.DataValidationForm, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.$nextTick(function () {
                  RuleBaseManage.$message.success(res.message);
                  RuleBaseManage.DatadialogVisible = false;
                  RuleBaseManage.GetDataValidationTableList();
                  RuleBaseManage.$refs.DataValidationForm.resetFields(); //重置表单
                });
              } else {
                RuleBaseManage.$message.error(res.message);
                RuleBaseManage.DatadialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      } else {
        RuleBaseManage.$refs.DataValidationForm.validate(function (valid) {
          if (valid) {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/UpdateDataValidation', _this11.DataValidationForm, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.$nextTick(function () {
                  RuleBaseManage.$message.success(res.message);
                  RuleBaseManage.DatadialogVisible = false;
                  RuleBaseManage.GetDataValidationTableList();
                  RuleBaseManage.$refs.DataValidationForm.resetFields(); //重置表单
                });
              } else {
                RuleBaseManage.$message.error(res.message);
                RuleBaseManage.DatadialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      }
    },
    //数据验证提交表单
    DataCancelAddOrUpdate: function DataCancelAddOrUpdate() {
      RuleBaseManage.DatadialogVisible = false;
      RuleBaseManage.$refs.DataValidationForm.resetFields();
    },
    GetDataValidationTableList: function GetDataValidationTableList() {
      this.DataValidationTabLoading = true;
      $.post('/MasterData_Quality_Manage/SearchDataValidation', {
        Where: this.DataValidationForm.EntityID,
        Page: this.pagenum,
        limit: this.pagesize
      }, function (res) {
        RuleBaseManage.DataValidationTableData = res.data;
        RuleBaseManage.total = res.total;
        RuleBaseManage.DataValidationTabLoading = false;
      }, 'json');
    },
    OpenDataValidationTableDialog: function OpenDataValidationTableDialog() {
      RuleBaseManage.DatadialogVisible = true;
      RuleBaseManage.DatadialogTitle = '添加逻辑规则';
      RuleBaseManage.EditOrAdd = "Add";
    },
    //  添加打开面板
    OpenDataValidationEditDialog: function OpenDataValidationEditDialog(row) {
      RuleBaseManage.DatadialogVisible = true;
      RuleBaseManage.$nextTick(function () {
        RuleBaseManage.Title = '修改逻辑规则';
        RuleBaseManage.EditOrAdd = "Edit";
        RuleBaseManage.DataValidationForm.Id = row.id;
        RuleBaseManage.DataValidationForm.Name = row.name;
        RuleBaseManage.DataValidationForm.Description = row.description;
        RuleBaseManage.DataValidationForm.FunctionName = row.functionName;
      });
    },
    // 编辑打开页面
    DatahandleClose: function DatahandleClose() {},
    GetDataModelSelectData: function GetDataModelSelectData() {
      var _this12 = this;

      $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, function (res) {
        if (res.success) {
          _this12.DataModelSelectOptions = res.data;

          if (res.data != null) {
            _this12.DataModelName = res.data[0].name;
            _this12.DataValidationForm.ModelID = res.data[0].id;

            _this12.GetDataEntitySelectData(res.data[0].id);
          } else {
            _this12.DataModelName = "";

            _this12.GetDataEntitySelectData();
          }
        }
      });
    },
    //  模型下拉数据
    GetDataEntitySelectData: function GetDataEntitySelectData(ModelID) {
      var _this13 = this;

      $.post('/MasterData_Quality_Manage/GetEntitySelectRule', {
        ModelID: ModelID
      }, function (res) {
        if (res.success) {
          if (res.data.length > 0) {
            _this13.DataEntitySelectOptions = res.data; //this.DataEntityName = res.data[0].name;

            _this13.DataEntityName = "";
            _this13.DataValidationForm.EntityID = res.data[0].id;
          } else {
            _this13.DataEntityName = "";
            _this13.DataValidationForm.EntityID = null;
          }

          _this13.GetDataValidationTableList();
        }
      });
    },
    // 实体下拉数据
    DelDataValidation: function DelDataValidation(row) {
      RuleBaseManage.$confirm("是否删除", "数据验证删除", {
        cancelButtonClass: "btn-custom-cancel",
        cancelButtonText: "取消",
        confirmButtonText: "确定",
        type: 'warning',
        callback: function callback(action, instance) {
          if (action === 'confirm') {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/DeleteDataValidation', {
              DataID: row.id
            }, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.GetDataValidationTableList();
                RuleBaseManage.$message.success(res.message);
              } else {
                RuleBaseManage.$message.error(res.message);
              }
            }, 'json');
          }
        }
      });
    },
    // 删除数据
    //------------------------------------ 字段验证
    SubmitFieldValidationFrom: function SubmitFieldValidationFrom() {
      var _this14 = this;

      if (RuleBaseManage.EditOrAdd == 'Add') {
        RuleBaseManage.$refs.FieldValidationForm.validate(function (valid) {
          if (valid) {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/InsertFieldValidation', _this14.FieldValidationForm, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.$nextTick(function () {
                  RuleBaseManage.$message.success(res.message);
                  RuleBaseManage.FieldialogVisible = false;
                  RuleBaseManage.GetFielValidationTableList();
                  RuleBaseManage.$refs.FieldValidationForm.resetFields(); //重置表单
                });
              } else {
                RuleBaseManage.$message.error(res.message);
                RuleBaseManage.FielddialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      } else {
        RuleBaseManage.$refs.FieldValidationForm.validate(function (valid) {
          if (valid) {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/UpdateFieldValidation', _this14.FieldValidationForm, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.$nextTick(function () {
                  RuleBaseManage.$message.success(res.message);
                  RuleBaseManage.FieldialogVisible = false;
                  RuleBaseManage.GetFielValidationTableList();
                  RuleBaseManage.$refs.FieldValidationForm.resetFields(); //重置表单
                });
              } else {
                RuleBaseManage.$message.error(res.message);
                RuleBaseManage.FielddialogVisible = false;
              }
            }, 'json');
          } else {
            return false;
          }
        });
      }
    },
    //  字段验证提交表单
    FieldCancelAddOrUpdate: function FieldCancelAddOrUpdate() {
      RuleBaseManage.FieldialogVisible = false;
      RuleBaseManage.$refs.FieldValidationForm.resetFields();
    },
    GetFielValidationTableList: function GetFielValidationTableList() {
      this.FieldValidationTabLoading = true;
      $.post('/MasterData_Quality_Manage/SearchFieldValidation', {
        Where: this.FieldValidationForm.EntityID,
        Page: this.pagenum,
        limit: this.pagesize
      }, function (res) {
        RuleBaseManage.FieldValidationTableData = res.data;
        RuleBaseManage.total = res.total;
        RuleBaseManage.FieldValidationTabLoading = false;
      }, 'json');
    },
    //获取字段验证表单数据
    OpenFieldValidationTableDialog: function OpenFieldValidationTableDialog() {
      RuleBaseManage.FieldialogVisible = true;
      RuleBaseManage.FielddialogTitle = '添加逻辑规则';
      RuleBaseManage.EditOrAdd = "Add";
    },
    //  添加打开面板
    OpenFieldValidationEditDialog: function OpenFieldValidationEditDialog(row) {
      RuleBaseManage.FieldialogVisible = true;
      RuleBaseManage.$nextTick(function () {
        RuleBaseManage.Title = '修改逻辑规则';
        RuleBaseManage.EditOrAdd = "Edit";
        RuleBaseManage.FieldValidationForm.Id = row.id;
        RuleBaseManage.FieldValidationForm.Name = row.name;
        RuleBaseManage.FieldValidationForm.Description = row.description;
      });
    },
    // 编辑打开页面
    FieldhandleClose: function FieldhandleClose() {},
    GetFieldModelSelectData: function GetFieldModelSelectData() {
      var _this15 = this;

      $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, function (res) {
        if (res.success) {
          _this15.FieldModelSelectOptions = res.data;

          if (res.data != null) {
            _this15.FieldModelName = res.data[0].name;
            _this15.FieldValidationForm.ModelID = res.data[0].id;

            _this15.GetFieldEntitySelectData(res.data[0].id);
          } else {
            _this15.FieldModelName = "";

            _this15.GetFieldEntitySelectData(ModelID);
          }
        }
      });
    },
    //  模型下拉数据
    GetFieldEntitySelectData: function GetFieldEntitySelectData(ModelID) {
      var _this16 = this;

      $.post('/MasterData_Quality_Manage/GetEntitySelectRule', {
        ModelID: ModelID
      }, function (res) {
        if (res.success) {
          if (res.data.length > 0) {
            _this16.FieldEntitySelectOptions = res.data;
            _this16.FieldEntityName = res.data[0].name;
            _this16.FieldValidationForm.EntityID = res.data[0].id;
          } else {
            _this16.FieldEntityName = "";
            _this16.FieldValidationForm.EntityID = null;
          }

          _this16.GetFielValidationTableList();
        }
      });
    },
    // 实体下拉数据
    DelFieldValidation: function DelFieldValidation(row) {
      RuleBaseManage.$confirm("是否删除", "数据验证删除", {
        cancelButtonClass: "btn-custom-cancel",
        cancelButtonText: "取消",
        confirmButtonText: "确定",
        type: 'warning',
        callback: function callback(action, instance) {
          if (action === 'confirm') {
            var loading = RuleBaseManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/DeleteFieldValidation', {
              DataID: row.id
            }, function (res) {
              loading.close();

              if (res.success == true) {
                RuleBaseManage.GetFielValidationTableList();
                RuleBaseManage.$message.success(res.message);
              } else {
                RuleBaseManage.$message.error(res.message);
              }
            }, 'json');
          }
        }
      });
    } // 删除数据

  },
  watch: {
    RoleSearchName: function RoleSearchName(val, oldval) {
      RuleBaseManage.pagenum = 1;
      this.GetRoleTableList();
    }
  }
});