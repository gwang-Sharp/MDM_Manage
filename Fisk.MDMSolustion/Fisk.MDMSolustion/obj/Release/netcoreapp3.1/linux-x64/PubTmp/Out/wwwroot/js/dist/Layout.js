"use strict";

var LayOutStaticWindowsHeight = $(window).height(); //全局可视窗口高度

var IsHasPermissionsData = false; //权限是否有数据

var layVM = new Vue({
  el: "#layout",
  data: {
    isCollapse: false,
    activeIndex: '1',
    MenuAutoSetting: '',
    //菜单默认自动选中
    NavTreeList: [],
    ThemeSetting: '',
    //设置菜单样式
    LogoShow: true
  },
  created: function created() {},
  mounted: function mounted() {
    this.loadMenu();
    $(".el-card__body")[0].style.padding = "0px";
  },
  methods: {
    handleOpen: function handleOpen(key, keyPath) {//console.log(key, keyPath);
    },
    handleClose: function handleClose(key, keyPath) {//console.log(key, keyPath);
    },
    loadMenu: function loadMenu() {
      var _self = this;

      $.ajax({
        url: '/System/getMenuList',
        dataType: 'json',
        type: 'get',
        success: function success(data) {
          if (data != null) {
            //data = vm.$options.methods.fn(data, 1); //火狐浏览器不兼容这种写法
            data = layVM.fn(data, 1);
            _self.NavTreeList = data; //console.log(_self.NavTreeList);

            var htmlStr = ""; //for (var i = 0; i < data.length; i++) {
            //    htmlStr += "<li class=\"layui-nav-item\">";
            //    htmlStr += vm.$options.methods.getMenuChildren(data, data[i]);
            //    htmlStr += "</li>";
            //}
            //$("#LAY-system-side-menu").append(htmlStr);
            //layui.use('element', function () {//重新渲染页面
            //    var element = layui.element;
            //    element.init();
            //});
          } //vm.$options.methods.HightlightMenu();

        }
      });
    },
    //获取子节点
    fn: function fn(data, pid) {
      var result = [],
          temp;

      for (var i in data) {
        if (data[i].parentNavCode == pid) {
          result.push(data[i]);
          temp = layVM.$options.methods.fn(data, data[i].navCode);

          if (temp.length > 0) {
            data[i].children = temp;
          } else {
            data[i].children = "";
          }
        }
      }

      return result;
    },
    TabMenu: function TabMenu(UrlData, navid) {
      layVM.MenuAutoSetting = navid;

      if (IsHasPermissionsData) {
        //切换菜单的时候判断是否存在未提交的表单数据。
        this.$confirm("修改数据没有保存，是否离开", "提示", {
          cancelButtonClass: "btn-custom-cancel",
          cancelButtonText: "取消",
          confirmButtonText: "确定",
          type: 'warning',
          callback: function callback(action) {
            if (action === 'confirm') {
              IsHasPermissionsData = false; //$(".layui-nav-item a").addClass("navdisable");
              //$(".layui-nav-child dd").addClass("navchilddisable");

              $(".el-menu").addClass("navdisable");
              $("canvas").remove();
              $("#MainBody").empty();
              window.location.href = "#" + UrlData.substr(UrlData.indexOf('/') + 1);
              setTimeout(function () {
                $("#MainBody").load(UrlData, function () {
                  //$(".layui-nav-item a").removeClass("navdisable");
                  //$(".layui-nav-child dd").removeClass("navchilddisable");
                  $(".el-menu").removeClass("navdisable");
                });
              }, 1000);
            } else {
              vm.MenuAutoSetting = '';
              vm.MenuAutoSetting = "24bcb62a-227d-42e8-994a-c01310790088"; //修改菜单默认选中

              return;
            }
          }
        });
      } else {
        layVM.MenuAutoSetting = navid; //$(".layui-nav-item a").addClass("navdisable");
        //$(".layui-nav-child dd").addClass("navchilddisable");

        $(".el-menu").addClass("navdisable");
        $("canvas").remove();
        $("#MainBody").empty();
        window.location.href = "#" + UrlData.substr(UrlData.indexOf('/') + 1);
        setTimeout(function () {
          $("#MainBody").load(UrlData, function () {
            //$(".layui-nav-item a").removeClass("navdisable");
            //$(".layui-nav-child dd").removeClass("navchilddisable");
            $(".el-menu").removeClass("navdisable");
          });
        }, 1000);
      }
    },
    Collapse: function Collapse() {
      if (this.isCollapse) {
        this.isCollapse = false;
        this.LogoShow = true;
      } else {
        this.isCollapse = true;
        this.LogoShow = false;
      }
    }
  }
});