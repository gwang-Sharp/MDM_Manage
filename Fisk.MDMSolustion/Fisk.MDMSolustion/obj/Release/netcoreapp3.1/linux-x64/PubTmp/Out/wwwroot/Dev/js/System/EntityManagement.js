var EntityVue = new Vue({
	el: '#EntityVue',
	data: {
		EntityTabLoading: false, // 加载loading
		Modelvalue: "", //模型
		ModelOptions: [], //模型数据
		searchName: "",  //查询条件
		SQLCommonFiled: [{    // 表格列
			Value: 'name', Label: '名称'
		}, {
				Value: 'remark', Label: '说明'
		}, {
				Value: 'stageTable', Label: '临时表'

        },
        {
            Value: 'autoCreateCode', Label: '是否自动创建代码'

        }
            , {
            Value: 'creater', Label: '创建人'
        }, {
            Value: 'createTime', Label: '创建时间'

        },
        {
            Value: 'updater', Label: '修改者'
        }, {
            Value: 'updateTime', Label: '修改时间'
        }
        ],
        EntitytableData: [],  //表格数据
        dialogVisible: false,   // 弹出面板
        dialogTitle: "添加实体",
        EntityRuleForm: {      // 面板
            Id: '',
            Name: '',
            Remark: "",
            StageTable: "",
            StartTime: "",
            isCreate: false,
            Model: '',
            ModelId: 0,
            BeganIn:1,

        },
		total: 1,//总条数
		pagenum: 1, // 当前页
		pagesize: 10, // 每页条数
		pageMaxsize: '', //当前页可以存放最大条数
		AddPanelStarts:1,    //添加面板的状态 1  添加  2  编辑
		EntityFromrules: {   //添加或编辑面板的表单验证

            Name: [{     //名字
                required: true,
                message: '请输入名称',
                trigger: 'blur'
            },
            {
                required: true,
                message: '请输入名称',
                trigger: 'change'
            }
            ],
            Remark: [{    //说明
                required: true,
                message: '请输入说明',
                trigger: 'change'
            }],
            Model: [{     //模型
                required: true,
                message: '请选择模型',
                trigger: 'change'
            }],
            StageTable: [{ //临时表

                required: true,
                message: '请输入临时表',
                trigger: 'change'
            }],
            StartTime: [{  // 开始于

                required: true,
                message: '请选择开始时间',
                trigger: 'change'
            }],

        },
        disabled: false
	},
	mounted: function () {
		this.$nextTick(() => {
			this.pagesize = parseInt((LayOutStaticWindowsHeight - 390) / 40);
			this.tableHeight = parseInt(LayOutStaticWindowsHeight - 150);
			this.pageMaxsize = this.pagesize;
			this.SearchDataTable();
			this.InitModelSelectOptions();
		});
		
	},
	methods: {
		// 获取表格数据
		SearchDataTable: function ()
		{
			var EntityRuleParams = {
                Page: this.pagenum,
				limit: this.pagesize,
				Where: this.searchName,
				ModelId: this.EntityRuleForm.ModelId
			};
			this.RoleTabLoading = true;
			$.post('/System/SearchEntity', EntityRuleParams, function (res) {
				EntityVue.EntitytableData = res.data;
                EntityVue.total = res.total;
				EntityVue.RoleTabLoading = false;
			}, 'json')
		},
		//打开添加面板
		AddEntity: function () {
            this.AddPanelStarts = 1;
            this.disabled = false;
			this.dialogVisible = true;
		},
		// 获取模型实体数据
		InitModelSelectOptions: function () {
			let that = this;
			$.post("/system/Attributes_ModelsGet", RES => {
				//console.log(RES);
				if (RES.success) {
					that.ModelOptions = RES.data;
					console.log(that.ModelOptions);
				}
				else {
					that.ModelOptions = [];
				}
			})
		},
		//模型下拉框选中触发事件
		selectModel: function (ModelId)
		{
			if (ModelId == null || ModelId == '') {
				
				EntityVue.$refs.EntityruleForms.resetFields();
			}
			this.EntityRuleForm.Model = this.Modelvalue;
			this.EntityRuleForm.ModelId = ModelId;
			this.SearchDataTable();
		},
		//面板模型下拉框事件
		DialogChange: function (val)
		{
			this.EntityRuleForm.ModelId = val;
		},
		//添加实体或者编辑提交
        AddsubmitForm: function (EntityRuleForm) {
			EntityVue.$refs.EntityruleForms.validate((valid) =>
			{
				if (valid) {
					var EntityRuleParams; 
					var url;
					switch (this.AddPanelStarts) {
						case 1:
							console.log(EntityVue.EntityRuleForm.ModelId);
							url = '/System/InserEntity';   //添加
							EntityRuleParams =
							{
								Name: EntityVue.EntityRuleForm.Name,
								Remark: EntityVue.EntityRuleForm.Remark,
								StageTable: EntityVue.EntityRuleForm.StageTable,
								AutoCreateCode: EntityVue.EntityRuleForm.isCreate == true ? 0 : 1,
                                ModelId: EntityVue.EntityRuleForm.ModelId,
                                BeganIn: EntityVue.EntityRuleForm.BeganIn,
							};
							break;
						case 2:
							url = '/System/UpdateEntity';  //修改
							EntityRuleParams =
							{
								Id: EntityVue.EntityRuleForm.Id,
								Name: EntityVue.EntityRuleForm.Name,
								Remark: EntityVue.EntityRuleForm.Remark,
								StageTable: EntityVue.EntityRuleForm.StageTable,
								AutoCreateCode: EntityVue.EntityRuleForm.isCreate == true ? 0 : 1,
                                ModelId: EntityVue.EntityRuleForm.ModelId,
                                BeganIn: EntityVue.EntityRuleForm.BeganIn,
							};
							break;
						default:
							break;
                    }
                    $.post(url, EntityRuleParams, function (res) {
                        if (res.success == true) {
                            EntityVue.$refs.EntityruleForms.resetFields();
                            EntityVue.$message.success(res.message);
                            EntityVue.EntityRuleForm.ModelId = 0;
                            EntityVue.pagenum= 1;
                            EntityVue.SearchDataTable();
                            EntityVue.EntityRuleForm.isCreate = false;

                        } else {
                            EntityVue.$message.error(res.message);

                        }
                        EntityVue.dialogVisible = false;
                    }, 'json')
                }
                else {
                    return false;
                }
            })

        },
        //面板取消
        resetForm: function () {
            this.dialogVisible = false;
            this.$refs.EntityruleForms.resetFields();
            this.EntityRuleForm.isCreate = false;
        },
        //  表头样式
        headerClass: function () {
            return 'text-align: center;background:#eef1f6;'
        },
        //  数据删除
        handleDelete: function (row) {
            this.$confirm('此操作将永久删除该模型，是否继续？', '提示', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning',
                callback: (action, instance) => {
                    if (action === 'confirm') {
                        var EntityRuleParams =
                        {
                            Id: row.id,
                        };
                        //console.log(row.id);
                        $.post('/System/DeleteEntity', EntityRuleParams, res => {
                            if (res.success) {
                                this.$message.success(res.message);
                                EntityVue.SearchDataTable();
                            } else
                                this.$message.error(res.message);
                        });
                    }
                }
            });
        },
        // 数据编辑
        handleEdit: function (row) {
            this.dialogVisible = true;
            this.disabled = true;
            this.AddPanelStarts = 2;
            this.$nextTick(() => {
                this.dialogTitle = "编辑实体";
                this.EntityRuleForm.Id = row.id;
                this.EntityRuleForm.Name = row.name;
                this.EntityRuleForm.Remark = row.remark;
                this.EntityRuleForm.StageTable = row.stageTable;
                this.EntityRuleForm.StartTime = row.createTime;
                this.EntityRuleForm.BeganIn = row.beganIn;
                this.EntityRuleForm.Model = row.modelId;
                if (row.autoCreateCode == "是") {
                    this.EntityRuleForm.isCreate = true;

                } else {
                    this.EntityRuleForm.isCreate = false;
                }
            });
        },
        //  面板关闭
        handleClose: function () {
            this.$refs.EntityruleForms.resetFields();
            this.dialogVisible = false;
            this.EntityRuleForm.isCreate = false;
        },
        //   分页
        CurrentChange: function (page) {
            this.pagenum = page;
            this.SearchDataTable();
        },
        // 只要每页条数变化了, 触发
        handleSizeChange: function (val) {
            // 更新每页条数
            EntityVue.pagesize = val;
            // 只要修改了每页条数, 数据所在的页码发生了变化了, 回到第一页
            EntityVue.pagenum = 1;
            // 重新渲染
            EntityVue.SearchDataTable();
        },
        // 只要当前页变化时, 触发函数
        handleCurrentChange: function (val) {
            // 更新当前页
            EntityVue.pagenum = val;
            // 重新渲染
            EntityVue.SearchDataTable();
        }
    },
    watch: {
        // 模糊查询监听
        searchName(val) {
            this.pagenum = 1;
            this.SearchDataTable();
        }
    }
});