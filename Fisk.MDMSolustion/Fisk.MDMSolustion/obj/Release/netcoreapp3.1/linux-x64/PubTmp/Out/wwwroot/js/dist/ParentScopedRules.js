"use strict";

var _methods;

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

var MergeRuleManage = new Vue({
  el: '#MergeRuleManage',
  data: {
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
    WeightIndex: 0,
    GlobalWeight: 0,
    TabNumber: 'first',
    pageMaxsize: '',
    //当前页可以存放最大条数
    Title: '添加角色',
    //弹框标题
    num: 0,
    RulesOfTheData: [],
    ModelName: "",
    slider: 25,
    SliderFirstWidth: 0,
    SliderSoncendWidth: 0,
    SliderThreeWidth: 0,
    RuleID: 0,
    value: [1, 3, 4, 8, 9, 10],
    ModelSelectOptions: [],
    EntityName: "",
    IntNumber: 0,
    activeName: 'first',
    AddAndSubtractState: 0,
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
      RulesOfTheData: []
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
    SetTheThresholddialogVisible: false,
    ResultHandlingdialogVisible: false,
    WeightIsShow: false,
    AttributeValue: '',
    BeginBeginMergeDataloading: false
  },
  created: function created() {},
  mounted: function mounted() {
    var _this = this;

    this.GetModelSelectData();
    this.$nextTick(function () {
      _this.pagesize = parseInt((LayOutStaticWindowsHeight - 390) / 40);
      _this.tableHeight = parseInt(LayOutStaticWindowsHeight - 150);
      _this.pageMaxsize = _this.pagesize;

      _this.GetModelSelectData();

      _this.GetRuleBaseTableList();
    });
  },
  methods: (_methods = {
    GetModelSelectData: function GetModelSelectData() {
      var _this2 = this;

      $.post('/MasterData_Quality_Manage/GetModelSelectRule', {}, function (res) {
        if (res.success) {
          _this2.ModelSelectOptions = res.data;
          _this2.DataModelSelectOptions = res.data;

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
    Getonmousedown: function Getonmousedown(ev) {},
    SetTheThresholddialog: function SetTheThresholddialog(row) {
      this.SetTheThresholddialogVisible = true;
      this.RuleID = row.id;
      this.SliderFirstWidth = parseInt(row.selfMotion);
      this.slider = this.SliderFirstWidth;
    },
    GetEntitySelectData: function GetEntitySelectData(ModelID) {
      var _this3 = this;

      $.post('/MasterData_Quality_Manage/GetEntitySelectRule', {
        ModelID: ModelID
      }, function (res) {
        if (res.success) {
          if (res.data.length > 0) {
            _this3.EntitySelectOptions = res.data;
            _this3.EntityName = res.data[0].name;
            _this3.RuleBaseForm.EntityID = res.data[0].id;

            _this3.GetAttributeSelectData();
          } else {
            _this3.EntityName = "";
            _this3.RuleBaseForm.EntityID = 0;
            _this3.EntitySelectOptions = [];
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
        _this4.AttributeSelectOptions = [];

        if (res.success) {
          _this4.AttributeSelectOptions = res.data;
        }
      });
    },
    //进入页面渲染表格
    GetRuleBaseTableList: function GetRuleBaseTableList() {
      this.RuleBaseTabLoading = true;
      $.post('/MasterData_Quality_Manage/SearchMergingrules', {
        Where: this.RuleBaseForm.EntityID,
        Page: this.pagenum,
        limit: this.pagesize <= 1 ? 10 : this.pagesize
      }, function (res) {
        MergeRuleManage.RuleBaseTableData = res.data;
        MergeRuleManage.total = res.total;
        MergeRuleManage.RuleBaseTabLoading = false;
      }, 'json');
    },
    // 只要每页条数变化了, 触发
    handleSizeChange: function handleSizeChange(val) {
      // 更新每页条数
      MergeRuleManage.pagesize = val; // 只要修改了每页条数, 数据所在的页码发生了变化了, 回到第一页

      MergeRuleManage.pagenum = 1; // 重新渲染

      MergeRuleManage.GetRoleTableList();
    },
    // 只要当前页变化时, 触发函数
    handleCurrentChange: function handleCurrentChange(val) {
      // 更新当前页
      MergeRuleManage.pagenum = val; // 重新渲染

      MergeRuleManage.GetRoleTableList();
    },
    //点击添加用户对话框
    OpenRuleBaseDialog: function OpenRuleBaseDialog() {
      MergeRuleManage.AddRuleBasedialogVisible = true;
      MergeRuleManage.Title = '添加合并规则';
      MergeRuleManage.EditOrAdd = "Add";
      this.CustomTypeIsShow = false;
    },
    //点击edit 弹框 ,修改弹框名称,内容回显
    OpenRuleBaseEditDialog: function OpenRuleBaseEditDialog(index, row) {
      var _this5 = this;

      MergeRuleManage.AddRuleBasedialogVisible = true;
      MergeRuleManage.$nextTick(function () {
        MergeRuleManage.Title = '修改合并规则';
        MergeRuleManage.EditOrAdd = "Edit";
        MergeRuleManage.RuleBaseForm.ID = row.id;
        MergeRuleManage.RuleBaseForm.ModelID = row.modelID;
        MergeRuleManage.RuleBaseForm.EntityID = row.entityID;
        var IntNymber = 0;

        for (var i = 0; i < row.detailsItem.length.length; i++) {
          IntNymber += row.detailsItem[i].weight;
        }

        _this5.IntNumber = IntNymber;
        _this5.RulesOfTheData = [];

        for (var i = 0; i < row.detailsItem.length; i++) {
          var Rul = MergeRuleManage.RulesOfTheData.length + 1;
          var AttributeValue = row.detailsItem[i].isGroop == 0 ? "权重" : "分组";
          var WeightIsShow = row.detailsItem[i].isGroop == 0 ? true : false;
          var RulesItem = {
            Index: Rul,
            AttributeName: row.detailsItem[i].name,
            AttributeID: row.detailsItem[i].id,
            Weight: row.detailsItem[i].weight,
            AttributeValue: AttributeValue,
            WeightIsShow: WeightIsShow
          };

          _this5.RulesOfTheData.push(RulesItem);
        }
      });
    },
    BeginMergeData: function BeginMergeData(index, row) {
      this.BeginBeginMergeDataloading = true;
      $.post('/MasterData_Quality_Manage/MergeData', {
        entityID: row.entityID,
        mergingCode: row.mergingCode
      }, function (res) {
        MergeRuleManage.BeginBeginMergeDataloading = false;
      }, 'json');
    },
    //点击删除角色
    DelRuleBase: function DelRuleBase(row) {
      MergeRuleManage.$confirm("是否删除", "业务规则删除", {
        cancelButtonClass: "btn-custom-cancel",
        cancelButtonText: "取消",
        confirmButtonText: "确定",
        type: 'warning',
        callback: function callback(action, instance) {
          if (action === 'confirm') {
            var loading = MergeRuleManage.$loading({
              lock: true,
              text: '加载中……',
              background: "rgba(255, 255, 255, 0.75)"
            });
            $.post('/MasterData_Quality_Manage/DeleteMergingrules', {
              RuleID: row.id
            }, function (res) {
              loading.close();

              if (res.success == true) {
                MergeRuleManage.GetRuleBaseTableList();
                MergeRuleManage.$message.success(res.message);
              } else {
                MergeRuleManage.$message.error(res.message);
              }
            }, 'json');
          }
        }
      });
    },
    //关闭添加用户对话框或者修改用户对话框
    ClosedAddRuleBasedialog: function ClosedAddRuleBasedialog() {
      MergeRuleManage.$nextTick(function () {
        MergeRuleManage.AddRuleBasedialogVisible = false;
        MergeRuleManage.$refs.RuleBaseForm.resetFields();
        MergeRuleManage.RulesOfTheData = [];
      });
    },
    //关闭添加用户对话框或者修改用户对话框
    CloseRuleBasedialogVisible: function CloseRuleBasedialogVisible(RoleForm) {
      MergeRuleManage.$nextTick(function () {
        MergeRuleManage.AddRuleBasedialogVisible = false;
        MergeRuleManage.$refs.RuleBaseForm.resetFields(); //重置表单

        MergeRuleManage.RulesOfTheData = [];
      });
    },
    SubmitRuleBaseForm: function SubmitRuleBaseForm() {
      var _this6 = this;

      //提交角色表单
      debugger;
      var IntNymber = 0;
      var WeightState = 0; //

      var IsNull = 0;
      var WeightIsNull = 0;
      var AttributeValueIsNull = 0; //  数组集合中知否有属性为空的

      for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
        if (MergeRuleManage.RulesOfTheData[i].AttributeValue == "") {
          AttributeValueIsNull = 1;
        }

        if (MergeRuleManage.RulesOfTheData[i].AttributeValue == "权重") {
          WeightState = 1;
          IntNymber += MergeRuleManage.RulesOfTheData[i].Weight;

          if (MergeRuleManage.RulesOfTheData[i].Weight == 0) {
            WeightIsNull = 1;
          }
        }

        if (MergeRuleManage.RulesOfTheData[i].AttributeValue == "" || MergeRuleManage.RulesOfTheData[i].AttributeValue == null) {
          IsNull = 1;
        }
      }

      if (this.RulesOfTheData.length == 0) {
        MergeRuleManage.$message.error("不能提交空值");
        return;
      }

      if (IntNymber != 100 && WeightState == 1) {
        MergeRuleManage.$message.error("权重总值必须等于100%");
        return;
      }

      if (IsNull == 1) {
        MergeRuleManage.$message.error("属性不能为空");
        return;
      }

      if (WeightIsNull == 1) {
        MergeRuleManage.$message.error("权重值不能为空");
        return;
      }

      if (AttributeValueIsNull == 1) {
        MergeRuleManage.$message.error("字段区分不能为空");
        return;
      } else {
        var item = [];

        for (var i = 0; i < this.RulesOfTheData.length; i++) {
          item.push(this.RulesOfTheData[i]);
        }

        this.RuleBaseForm.RulesOfTheData = item;

        if (MergeRuleManage.EditOrAdd == 'Add') {
          MergeRuleManage.$refs.RuleBaseForm.validate(function (valid) {
            if (valid) {
              var loading = MergeRuleManage.$loading({
                lock: true,
                text: '加载中……',
                background: "rgba(255, 255, 255, 0.75)"
              });
              $.post('/MasterData_Quality_Manage/InsertMergingrules', _this6.RuleBaseForm, function (res) {
                loading.close();

                if (res.success == true) {
                  MergeRuleManage.$nextTick(function () {
                    MergeRuleManage.$message.success(res.message);
                    MergeRuleManage.AddRuleBasedialogVisible = false;
                    MergeRuleManage.GetRuleBaseTableList();
                    MergeRuleManage.$refs.RuleBaseForm.resetFields(); //重置表单

                    MergeRuleManage.RulesOfTheData = [];
                  });
                } else {
                  MergeRuleManage.$message.error(res.message);
                  MergeRuleManage.AddRuleBasedialogVisible = false;
                }
              }, 'json');
            } else {
              return false;
            }
          });
        } else {
          MergeRuleManage.$refs.RuleBaseForm.validate(function (valid) {
            if (valid) {
              var loading = MergeRuleManage.$loading({
                lock: true,
                text: '加载中……',
                background: "rgba(255, 255, 255, 0.75)"
              });
              $.post('/MasterData_Quality_Manage/EiteMergingrules', _this6.RuleBaseForm, function (res) {
                loading.close();

                if (res.success == true) {
                  MergeRuleManage.$nextTick(function () {
                    MergeRuleManage.$message.success(res.message);
                    MergeRuleManage.AddRuleBasedialogVisible = false;
                    MergeRuleManage.GetRuleBaseTableList();
                    MergeRuleManage.$refs.RuleBaseForm.resetFields(); //重置表单

                    MergeRuleManage.RulesOfTheData = [];
                  });
                } else {
                  MergeRuleManage.$message.error(res.message);
                  MergeRuleManage.AddRuleBasedialogVisible = false;
                }
              }, 'json');
            } else {
              return false;
            }
          });
        }
      }
    },
    //  表头样式
    headerClass: function headerClass() {
      return 'text-align: center;background:#eef1f6;';
    },
    AddRole: function AddRole() {
      MergeRuleManage.TitleType = 'Add';
      MergeRuleManage.Add_Edit_Role();
    },
    //绑定回车事件
    EnterSearch: function EnterSearch() {
      $('#RoleKeyStr').bind('keydown', function (event) {
        if (event.keyCode == "13") {
          MergeRuleManage.ReloadTable('Role');
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
          this.GetFielValidationTableList();
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
          this.RegularIs = true;
          break;

        case "邮箱":
          this.CustomTypeIsShow = false;
          this.RegularIs = true;
          break;

        case "自定义验证规则":
          this.CustomTypeIsShow = true;
          this.RegularIs = false;
          break;

        default:
          break;
      }
    },
    AddRuleBase: function AddRuleBase() //添加数组 数据
    {
      var Rul = MergeRuleManage.RulesOfTheData.length + 1;
      var IntNymber = 0;

      for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
        IntNymber += MergeRuleManage.RulesOfTheData[i].Weight;
      }

      var RulesItem = {
        Index: Rul,
        AttributeName: '',
        TemporaryWeight: 0,
        AttributeID: 0,
        Weight: 0,
        AttributeValue: '',
        WeightIsShow: false
      };
      this.RulesOfTheData.push(RulesItem);
    },
    DelRule: function DelRule(item) //删除数组数据
    {
      for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
        if (MergeRuleManage.RulesOfTheData[i].Index === item.Index) {
          MergeRuleManage.RulesOfTheData.splice(i, 1);
        }
      }
    }
  }, _defineProperty(_methods, "AttributeNameDialogChange", function AttributeNameDialogChange(val, index) {
    for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
      if (MergeRuleManage.RulesOfTheData[i].Index === index) {
        MergeRuleManage.RulesOfTheData[i].AttributeID = val;
      }
    }
  }), _defineProperty(_methods, "NumberhandleChange", function NumberhandleChange(currentValue, oldValue) {
    var IntNymber = 0;

    for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
      IntNymber += MergeRuleManage.RulesOfTheData[i].Weight;
    }

    this.IntNumber = IntNymber;
  }), _defineProperty(_methods, "ChangeThreshold", function ChangeThreshold() {
    var Parmas = {
      Id: this.RuleID,
      SelfMotion: this.SliderFirstWidth
    };
    var loading = MergeRuleManage.$loading({
      lock: true,
      text: '加载中……',
      background: "rgba(255, 255, 255, 0.75)"
    });
    $.post('/MasterData_Quality_Manage/ChangeThreshold', Parmas, function (res) {
      loading.close();

      if (res.success == true) {
        MergeRuleManage.$nextTick(function () {
          MergeRuleManage.$message.success(res.message);
          MergeRuleManage.SetTheThresholddialogVisible = false;
          MergeRuleManage.GetRuleBaseTableList();
          $("canvas").remove();
        });
      } else {
        MergeRuleManage.$message.error(res.message);
        MergeRuleManage.SetTheThresholddialogVisible = false;
      }
    }, 'json');
  }), _defineProperty(_methods, "SliderChange", function SliderChange(val) {
    MergeRuleManage.SliderFirstWidth = val;
  }), _defineProperty(_methods, "AttributeNameChange", function AttributeNameChange(val, index) {
    if (val == "权重") {
      for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
        if (MergeRuleManage.RulesOfTheData[i].Index === index) {
          MergeRuleManage.RulesOfTheData[i].WeightIsShow = true;
        }
      }
    } else {
      for (var i = 0; i < MergeRuleManage.RulesOfTheData.length; i++) {
        if (MergeRuleManage.RulesOfTheData[i].Index === index) {
          MergeRuleManage.RulesOfTheData[i].WeightIsShow = false;
        }
      }
    }
  }), _methods),
  watch: {
    RoleSearchName: function RoleSearchName(val, oldval) {
      MergeRuleManage.pagenum = 1;
      this.GetRoleTableList();
    }
  }
});