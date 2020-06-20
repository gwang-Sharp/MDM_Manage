
var AuthorityCenter = new Vue({
    el: '#AuthorityCenter',
    computed: {},
    data: {
        activeName: 'first',
        search_MenuAuthority: '',//搜索菜单权限
        MenuAuthorityData: [],   //菜单权限树数据
        UserLoading: false,
        search_AssignUsers: '',//搜索分配用户
        AssignUsersData: [],
        IsAllUser: false,
        defaultProps: {
            children: 'children',
            label: 'label'
        },
        UsersPagationChecked: 0,
        AssignUserHasConfigure: [], //已经配置的用户数据
        CurrentUserRole: '',//当前点击的角色
        AssignUsersPagation: {
            total: 0,
            page: 1,
            limit: 10,
            where: ''
        },
        UserRolePagation: {
            total: 0,
            page: 1,
            limit: 16,
            haschecked: 0,
            where: ''
        }, //角色表格分页数据
        ajaxSend: false,
        tableHeight: 300,
        search_userrole: '',
        UserLoading: false,
        Rolepane: 'Role',
        UserRoleData: [],  //角色表格数据
        UserRolePagation: {
            total: 0,
            page: 1,
            limit: 16,
            haschecked: 0,
            where: ''
        }, //角色表格分页数据
        screenHeight: "",
        MenuHasChecked: [],
    },
    created() {
        this.tableHeight = parseInt(LayOutStaticWindowsHeight - 250);
        this.TableRowNum = parseInt((LayOutStaticWindowsHeight - 350) / 40);
        this.AssignUsersPagation.limit = parseInt((LayOutStaticWindowsHeight - 420) / 40);
        this.UserRolePagation.limit = this.TableRowNum;
        this.screenHeight = this.TableRowNum * 40 + 41 + "px";
        this.GetAssignUsersData();
        this.GetUserRoleData();
        this.GetMenuAuthorityTree();
    },
    methods: {
        GetAssignUsersData: function () {
            let QueryParameters = {
                page: this.AssignUsersPagation.page,
                rows: this.AssignUsersPagation.limit,
                where: this.search_AssignUsers
            };
            this.UserLoading = true;
            $.ajax({
                data: QueryParameters,
                //async: false,
                dataType: 'json',
                type: 'post',
                url: '/system/UserGet',
                success: (res) => {
                    if (res.success) {
                        this.$nextTick(() => {
                            this.AssignUsersData = res.data;
                            this.AssignUsersPagation.total = res.total;
                            this.$refs.AssignUsers.clearSelection();
                            let tempHasChecked = [];
                            this.AssignUserHasConfigure.forEach(e => {
                                if (e.RoleID == this.CurrentUserRole) {
                                    tempHasChecked = e.UserHasCheckData;
                                }
                            });
                            this.AssignUsersData.forEach(e => {
                                if (tempHasChecked.indexOf(e.id) >= 0) {
                                    this.$refs.AssignUsers.toggleRowSelection(e, true);
                                }

                            });
                            this.UsersPagationChecked = tempHasChecked.length;
                            this.UserLoading = false;
                        });
                    } else {
                        this.UserLoading = false;
                    }
                }
            }); //根据CheckUser中的数据，选中某行



        }, //获取用户表格数据
        SearchAssignUsersData: function () {
            this.GetAssignUsersData();
        },   //回车键刷新表格
        SaveUsersData: function () {
            //if (this.CurrentUserRole === '') {
            //    return this.$message.warning("请选择角色");
            //}
            //if (this.AssignUserHasConfigure.filter(e => e.IsCheck != 0).length < 1) {
            //    return this.$message.success("提交成功，未做任何修改");
            //}

            let obj = {
                UserConfig: this.AssignUserHasConfigure.filter(e => e.IsCheck != 0)
            };
            
            this.SubmitLoading = true;
            $.post('/System/NewUserAssignmentRole', {
                data: JSON.stringify(obj)
            }, (res) => {
                if (res.success) {
                    //if (res.otherMsg.success == true) {
                    this.$message({
                        type: 'success',
                        message: res.message
                    });
                    //}
                    //else {
                    //this.$message({
                    //type: 'warning',
                    // message: $.i18n.prop("PermissionsWere")
                    //});
                    //}
                    this.IsAllUser = false;
                    this.ResetUsersPagination();
                    this.AssignUserHasConfigure = [];
                    this.search_AssignUsers = '';
                    this.AssignUsersTableSizeChange(this.CurrentUserRole);

                } else {
                    this.$message.error(res.message);
                }
                this.SubmitLoading = false;
                IsHasPermissionsData = false;
            });
        }, //提交分配的用户
        UserclickRow: function (row, column, event) {
            if (this.CurrentUserRole == "") {
                return;
            }

            this.$refs.AssignUsers.toggleRowSelection(row);

            let tempHasChecked = [];
            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
                }
            });
            if (tempHasChecked.indexOf(row.id) < 0) {
                tempHasChecked.push(row.id);
            }
            else if (tempHasChecked.indexOf(row.id) > -1) {
                tempHasChecked.splice(tempHasChecked.indexOf(row.id), 1);
            }
            this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户
            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    e.UserHasCheckData = tempHasChecked;
                    e.IsCheck = 1;
                }
            });
            IsHasPermissionsData = true;
        }, //用户表格行点击事件
        getRowKey: function (row) {
            return row.id;
        },
        selectRow: function (selection, row) {
            if (this.CurrentUserRole == "") {
                return;
            }
            let selected = selection.length && selection.indexOf(row) !== -1;   ////selected为 true就是选中，0或者false是取消选中
            let tempHasChecked = [];
            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
                }
            });
            if (selected === true) {  //勾选复选框
                if (tempHasChecked.indexOf(row.id) < 0) {
                    tempHasChecked.push(row.id);
                }
            } else { //取消勾选复选框
                if (tempHasChecked.indexOf(row.id) > -1) {
                    tempHasChecked.splice(tempHasChecked.indexOf(row.id), 1);
                }
            }
            this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户

            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    e.UserHasCheckData = tempHasChecked;
                    e.IsCheck = 1;
                }
            });
            IsHasPermissionsData = true;

        }, //用户行复选框点击事件
        selectAllRow: function (selection) {
            let selectedall = false;  ////selected为 true就是选中，false是取消选中
            selection.forEach(e => {
                selectedall = this.AssignUsersData.indexOf(e) !== -1;
            });
            let tempHasChecked = [];
            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
                }
            });
            if (selectedall) {
                selection.forEach(e => {
                    if (tempHasChecked.indexOf(e.id) < 0) {
                        tempHasChecked.push(e.id);
                    }
                });
            } else {
                this.AssignUsersData.forEach(e => {
                    if (tempHasChecked.indexOf(e.id) > -1) {
                        tempHasChecked.splice(tempHasChecked.indexOf(e.id), 1);
                    }
                });
            }
            this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户

            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    e.UserHasCheckData = tempHasChecked;
                    e.IsCheck = 1;
                }
            });
            IsHasPermissionsData = true;

        }, //表头全选点击事件
        openLoading() {
            this.Loading = true;
        },
        closeLoading() {
            //this.loading.close();
            this.Loading = false;
        },
        GetMenuAuthorityTree() {
            this.openLoading();
            $.post('/System/SearchMenu', {
                RoleNavCode: 'null'
            }, res => {
                this.MenuAuthorityData = res.data;
                this.closeLoading();
            }, 'json');
        }, //获取菜单层级树结构
        MenutreeNodeClick(node, check, child) {
            if (this.CurrentUserRole != "") {
                ////console.log("进入了点击事件")
                let checkMenu = [];
                checkMenu = this.$refs.MenuAuthority.getCheckedKeys().map(e => {
                    return {
                        id: e
                    };
                });

                this.MenuHasChecked.forEach(e => {
                    if (e.RoleID == this.CurrentUserRole) {
                        e.idList = checkMenu;
                        e.hasCheck = 1;
                    }
                });
                IsHasPermissionsData = true;
            }
            
        }, //选中状态改变时
        ResetUsersPagination() {
            this.AssignUsersPagation.page = 1;
            //this.AssignUsersPagation.limit = 10;
            this.AssignUsersPagation.where = '';
        }, //重置用户表格回到第一页
        AllUserChange: function () {
            let tempHasChecked = [];
            this.AssignUserHasConfigure.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    tempHasChecked = JSON.parse(JSON.stringify(e.UserHasCheckData));
                }
            });
            if (this.IsAllUser) {
                this.openLoading();
                $.post('/System/GetAllUser', { where: this.AssignUsersPagation.where }, res => {
                    if (res.success) {
                        tempHasChecked = res.data;
                        this.$nextTick(() =>
                        {
                            this.AssignUsersData.forEach(e => {
                                if (res.data.indexOf(e.id + '') >= 0) {
                                    this.$refs.AssignUsers.toggleRowSelection(e, true);
                                }
                            });
                        })
                      
                    }
                    else {
                        this.$message.error(res.message);
                    }
                    this.$refs.AssignUsers.clearSelection();
                    this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户
                    this.AssignUserHasConfigure.forEach(e => {
                        if (e.RoleID == this.CurrentUserRole) {
                            e.UserHasCheckData = res.data;
                            e.IsCheck = 1;
                        }

                    });
                    IsHasPermissionsData = true;
                    this.closeLoading();

                });
            } else {
                tempHasChecked = [];
                this.$refs.AssignUsers.clearSelection();
                this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户
                this.AssignUserHasConfigure.forEach(e => {
                    if (e.RoleID == this.CurrentUserRole) {
                        e.UserHasCheckData = res;
                        e.IsCheck = 1;
                    }

                });
                IsHasPermissionsData = true;
            }

            //setTimeout(() => {
            //    this.UsersPagationChecked = tempHasChecked.length;  //设置当前用户选择了多少用户
            //    this.AssignUserHasConfigure.forEach(e => {
            //        if (e.RoleID == this.CurrentUserRole) {
            //            e.UserHasCheckData = tempHasChecked;
            //            e.IsCheck = 1;
            //        }
            //    });
            //}, 0)

        }, //全选所有用户点击事件
        filterNode(value, data) {
            if (!value) return true;
            return data.label.toLowerCase().indexOf(value.toLowerCase()) !== -1;
        },
        AssignUsersTableSizeChange: function (RoleID) {

            this.UserLoading = true;

            let QueryParameters = {
                page: this.AssignUsersPagation.page,
                limit: this.AssignUsersPagation.limit,
                where: this.AssignUsersPagation.where,
                RoleID: RoleID
            };
            $.ajax({
                data: QueryParameters,
                //async: false,
                dataType: 'json',
                type: 'post',
                url: '/System/getRoleInUserPaginationList',
                success: (res) => {
                    if (res.status === 0) {
                        this.AssignUsersData = res.data;
                        this.AssignUsersPagation.total = res.total;

                        $.ajax({
                            data: {
                                RoleID: RoleID
                            },
                            //async: false,
                            dataType: 'json',
                            type: 'post',
                            url: '/Syste/RoleHaveUser',
                            success: (res) => {
                                if (res.success) {
                                    //把用户对应的所有角色放入CheckUser（已存在就不再放入）
                                    this.$nextTick(() => {
                                        this.$refs.AssignUsers.clearSelection();
                                        let isIn = false;
                                        let tempHasChecked = [];
                                        this.AssignUserHasConfigure.forEach(e => {
                                            if (e.RoleID == RoleID) {
                                                isIn = true;
                                                tempHasChecked = e.UserHasCheckData;
                                            }
                                        });
                                        if (isIn) {
                                            this.AssignUsersData.forEach(e => {
                                                if (tempHasChecked.indexOf(e.id) >= 0) {
                                                    this.$refs.AssignUsers.toggleRowSelection(e, true);
                                                }
                                            });
                                            this.UsersPagationChecked = tempHasChecked.length;
                                        } else {
                                            let temp = { RoleID: RoleID, UserHasCheckData: res.data, IsCheck: 0 };
                                            this.AssignUserHasConfigure.push(temp);
                                            this.UsersPagationChecked = res.data.length;
                                            this.AssignUsersData.forEach(e => {
                                                if (res.data.indexOf(e.id + '') >= 0) {
                                                    this.$refs.AssignUsers.toggleRowSelection(e, true);
                                                }
                                            });
                                        }

                                        this.UserLoading = false;
                                    });
                                } else {
                                    this.$message.error(res.msg);
                                    this.UserLoading = false;
                                }

                            }

                        }); //根据CheckUser中的数据，选中某行

                    }

                }
            }); //根据CheckUser中的数据，选中某行
        }, //角色切换时重新请求用户数据和已选用数据
        AssignUsersTablePageChange: function (val) {
            this.AssignUsersPagation.page = val;
            //this.AssignUsersTableSizeChange(this.CurrentUserRole);
            this.GetAssignUsersData();
        }, //用户表格页码切换事件
        UserecheckboxInit: function (row, index) {
            return this.CurrentUserRole == "" ? 0 : 1;
        },  //设置第一次进入页面时 ，角色未选择时 用户禁用
        RoleCurrentChange_row(val) {
            this.UserRolePagation.page = val;
            this.GetUserRoleData();
        },
        GetUserRoleData() {
            let QueryParameters = {
                Page: this.UserRolePagation.page,
                limit: this.UserRolePagation.limit,
                where: this.UserRolePagation.where
            };
          
            this.RoleLoading = true;
            $.post('/System/SearchRole', QueryParameters, (res) => {
                if (res.success) {
                    this.UserRoleData = res.data;
                    this.UserRolePagation.total = res.total;
                }
                this.RoleLoading = false;
            }, 'json');
        },
        UserRoleTableClick(currentRow, oldCurrentRow) {
            this.CurrentUserRole = currentRow.id;
            if (oldCurrentRow != null) {
                this.OldUserRole = oldCurrentRow.id;
            }
            //this.OldUserRole = oldCurrentRow.ID;
            switch (this.activeName) {
                case "first":
                    this.AssignUsersPagation.page = 1;
                    this.AssignUsersTableSizeChange(currentRow.id);  //设置角色已分配的用户
                    break;
                case "five":
                    this.GetMenuAuthorityTreeChecked(currentRow.id);    //设置角色已配置的菜单权限
                    break;
            }
            //this.GetGetSerchCubeData(); //获取已设置的Cube权限

            //this.GetTabularOptions();
        }, //点击角色表格

        GetMenuAuthorityTreeChecked(RoleID) {
            if (this.CurrentUserRole == "") {
                return;
            }
            this.openLoading();
            let isIn = false;
            let hasCheckData = [];

            this.MenuHasChecked.forEach(e => {
                if (e.RoleID == this.CurrentUserRole) {
                    isIn = true;
                    hasCheckData = e.idList.map(i => i.id);
                }
            });
            if (isIn) {
                let noChecked = false;
                this.MenuHasChecked.forEach(e => {
                    if (e.hasCheck == 1) {
                        noChecked = true;
                    }
                })

                if (!noChecked) {
                    this.$refs.MenuAuthority.setCheckedKeys(hasCheckData);
                    setTimeout(() => {
                        this.MenuHasChecked.forEach(e => {
                            if (e.RoleID == this.CurrentUserRole) {
                                e.hasCheck = 0;
                            }
                        });
                    }, 0);
                }
                else {
                    this.$refs.MenuAuthority.setCheckedKeys(hasCheckData);
                }
                this.closeLoading();
            }
            else {
                $.post("/System/GetRoleCheckedMenu", { RoleID: RoleID }, res => {
                    if (res.success) {
                        this.$nextTick(function () {
                            this.$refs.MenuAuthority.setCheckedKeys(res.data);
                            let oldCheck = res.data.map(e => { return { id: e } });
                            this.MenuHasChecked.push({ RoleID: RoleID, idList: oldCheck, hasCheck: 0 });
                            setTimeout(() => {
                                this.MenuHasChecked.forEach(e => {
                                    if (e.RoleID == this.CurrentUserRole) {
                                        e.hasCheck = 0;
                                    }
                                });
                            }, 0);
                        });
                        this.closeLoading();
                    } else {
                        this.$message.error(res.message);
                    }
                });
            }

        },  //设置角色选择的报表树
        AssignUsersTableSizeChange(RoleID) {

            this.UserLoading = true;

            let QueryParameters = {
                page: this.AssignUsersPagation.page,
                rows: this.AssignUsersPagation.limit,
                where: this.AssignUsersPagation.where,
                RoleID: RoleID
            };

            $.ajax({
                data: QueryParameters,
                //async: false,
                dataType: 'json',
                type: 'post',
                url: '/System/getRoleInUserPaginationList',
                success: (res) => {
                   
                    if (res.success) {
                        this.AssignUsersData = res.data;
                        this.AssignUsersPagation.total = res.total;
                        $.ajax({
                            data: {
                                RoleID: RoleID
                            },
                            //async: false,
                            dataType: 'json',
                            type: 'post',
                            url: '/System/RoleHaveUser',
                            success: (res) => {
                                if (res.success) {
                                    //把用户对应的所有角色放入CheckUser（已存在就不再放入）
                                    this.$nextTick(() => {
                                        this.$refs.AssignUsers.clearSelection();
                                        let isIn = false;
                                        let tempHasChecked = [];
                                        console.log(this.AssignUserHasConfigure);
                                        this.AssignUserHasConfigure.forEach(e => {
                                            if (e.RoleID == RoleID) {
                                                isIn = true;
                                                tempHasChecked = e.UserHasCheckData;
                                            }
                                        });
                                        if (isIn) {
                                            this.AssignUsersData.forEach(e => {
                                                if (tempHasChecked.indexOf(e.id + '') >= 0) {
                                                    this.$refs.AssignUsers.toggleRowSelection(e, true);
                                                }
                                            });
                                            this.UsersPagationChecked = tempHasChecked.length;
                                        } else {
                                            let temp = { RoleID: RoleID, UserHasCheckData: res.data, IsCheck: 0 };
                                            this.AssignUserHasConfigure.push(temp);
                                            this.UsersPagationChecked = res.data.length;
                                            this.AssignUsersData.forEach(e => {
                                                debugger
                                                if (res.data.indexOf(e.id+'') >= 0) {
                                                    this.$refs.AssignUsers.toggleRowSelection(e, true);
                                                }
                                            });
                                        }

                                        this.UserLoading = false;
                                    });
                                } else {
                                    this.$message.error(res.message);
                                    this.UserLoading = false;
                                }

                            }

                        }); //根据CheckUser中的数据，选中某行

                    }

                }
            }); //根据CheckUser中的数据，选中某行
        }, //角色切换时重新请求用户数据和已选用数据
        SaveMenuData() {
            if (this.CurrentUserRole === '') {
                return this.$message.warning($.i18n.prop('Pleaseselectarole'));
            }
            let ISChecked = false;
            //this.MenuHasChecked.forEach(e => {
            //    if (e.hasCheck == 1) {
            //        ISChecked = true;
            //    }
            //})
            //if (!ISChecked) {
            //    return this.$message.success($.i18n.prop('NoPermissionToModifyPleaseModifyBeforeSubmitting'));
            //}

            this.SubmitLoading = true;

            $.post('/System/NewEditRoleMenuByNavCode', {
                NavCodeObj: JSON.stringify(this.MenuHasChecked.filter(e => e.hasCheck != 0))
            }, (res) => {
                if (res.success) {
                    this.$message({
                        message: res.message,
                        type: 'success'
                    });
                    this.MenuHasChecked = [];
                    this.GetMenuAuthorityTreeChecked(this.CurrentUserRole);
                } else {
                    this.$message.error(res.message);
                }
                this.search_MenuAuthority = '';
                this.SubmitLoading = false;
                IsHasPermissionsData = false;
            });
        }, //提交菜单权限
  
    },
    watch: {
        search_AssignUsers(val, oldval) {
            this.search_AssignUsers = val;

            this.GetAssignUsersData();
        },
        search_userrole(val, oldval) {
            this.UserRolePagation.where= val;

            this.GetUserRoleData();
        },
        search_MenuAuthority: function (val, oldval) {

            this.$refs.MenuAuthority.filter(val);
        },
    }
});