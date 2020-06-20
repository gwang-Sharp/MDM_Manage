/**
 * @author sushengbuyu
 * @date 2018/09/11 10:49
 * @version 1.0
 * @desc 自定义layui 选项卡 实现重复添加直接切换tab
 */
layui.define(['element', 'jquery'], function (exports) { //提示：模块也可以依赖其它模块，如：layui.define('layer', callback);
	var element = layui.element;
	var $ = layui.jquery;
	//tab dom选择器
	var domSelect;
	//所有tab的id集合
	var idArray = [];
	//所有可关闭tab的id集合
	var closeArray = [];

	var tLeft = 0; //标签组 left
	var oLeft; //标签组偏移量
	var width; //标签组可见宽度

	//初始化默认配置
	var option = {
		index: {
			id: "index",
			close: false, //是否可关闭
			type: 'iframe', //默认为iframe['text','html','iframe']
			title: '', //标题
			content: '', //内容 type为text、html时必填
			url: '', //url type为iframe时必填
			icon: 'layui-icon-home' //图标 目前仅支持layui图标
		}
	}

	//Tab默认配置
	var tabOpt = {
		id: "",
		close: true, //是否可关闭 默认为true
		type: 'iframe', //默认为iframe 可选['text','html','iframe']
		title: '', //标题
		content: '', //内容 type为text、html时必填
		url: '', //url type为iframe时必填
		icon: '' //图标 目前仅支持layui图标
	}

	Array.prototype.remove = function (val) {
		var index = this.indexOf(val);
		if (index > -1) {
			this.splice(index, 1);
		}
	};
	/**
	 * 引入tab样式文件
	 */
	function loadSzTabStyle() {
		//获取layui base 路径
		var base = layui.cache.base;
		$("<link>")
			.attr({
				rel: "stylesheet",
				type: "text/css",
				href: base + "/zTab/zTab.css"
			})
			.appendTo("head");
	}

	/**
	 * 初始化Tab布局
	 */
	function initTabs() {
		//引入tab样式文件
		loadSzTabStyle();
		$(domSelect).addClass("layadmin-pagetabs");
		$(domSelect).append(
				"<div class=\"layui-icon layadmin-tabs-control layui-icon-prev\" layadmin-event=\"leftPage\"></div>")
			.append(
				"<div class=\"layui-icon layadmin-tabs-control layui-icon-next\" layadmin-event=\"rightPage\"></div>")
			.append(
				$('<div class="layui-icon layadmin-tabs-control layui-icon-down">' +
					'<ul id="lay-sz-tab-option" class="layui-nav layadmin-tabs-select">' +
					'<li class="layui-nav-item">' +
					'<a href="javascript:;"></a>' +
					'<dl class="layui-nav-child layui-anim-fadein">' +
					'<dd layadmin-event="closeThisTabs"><a href="javascript:;">关闭当前标签页</a></dd>' +
					'<dd layadmin-event="closeOtherTabs"><a href="javascript:;">关闭其它标签页</a></dd>' +
					'<dd layadmin-event="closeAllTabs"><a href="javascript:;">关闭全部标签页</a></dd>' +
					'</dl></li></ul>' +
					'</div>'))
			.append(
				"<div class=\"layui-tab\" lay-unauto><ul id=\"lay-sz-tab-title\" class=\"layui-tab-title\"></ul></div>");

		//监听左侧导航的点击事件
		$("a[lay-href]").on('click', function () {
			//console.log("menu");
			var href = $(this).attr("lay-href");
			var tab = $.extend(true, {}, tabOpt, {
				id: href,
				url: href,
				title: $(this).text()
			});
			addTab_(tab);
		});
		//监听关闭按钮的点击事件
		$("#lay-sz-tab-title").on('click', 'i.layui-tab-close', function () {
			var li = $(this).parent();
			deleteTab_(li.attr("lay-id"));
			//返回false阻止冒泡
			return false;
		});
		//监听Tab的点击事件
		$("#lay-sz-tab-title").on('click', 'li', function () {
			var li = $(this);
			changeTab_(li.attr("lay-id"));
		});

		//监听上一页的点击事件
		$(".layadmin-pagetabs").on('click', 'div.layui-icon-prev[layadmin-event="leftPage"]', function () {
			prevPage();
		});

		//监听上一页的点击事件
		$(".layadmin-pagetabs").on('click', 'div.layui-icon-next[layadmin-event="rightPage"]', function () {
			nextPage();
		});

		//关闭选项添加hover监听
		//不知道为什么伪类:hover不起作用，同样的样式就是不行，只能自己来了
		$("#lay-sz-tab-option").hover(function () {
			$(this).find("dl").slideDown();
		}, function () {
			$(this).find("dl").slideUp();
		});

		$("#lay-sz-tab-option").on('click', 'dd', function () {
			var event = $(this).attr("layadmin-event");
			if (event === "closeThisTabs") {
				closeThisTab_();
			} else if (event === "closeOtherTabs") {
				closeOtherTabs_();
			} else if (event === "closeAllTabs") {
				closeAllTabs_();
			}
		});

		// 		$("#lay-sz-tab-title").on('mouseover', 'li', function(){
		// 			$(this).css("background-color","red");
		// 		});
	}

	/**
	 * 上一页
	 */
	function prevPage() {
		// //console.log("上一页");
		reflashWidth();
		tLeft = tLeft + width > 0 ? 0 : tLeft + width;
		correct();
	}

	/**
	 * 下一页
	 */
	function nextPage() {
		// //console.log("下一页");
		reflashWidth();
		var realW = $("#lay-sz-tab-title")[0].scrollWidth;
		// //console.log("真实宽度：" + realW + ",显示宽度：" + width);
		if (realW > width) {
			tLeft = (tLeft - width) + realW > 0 ? tLeft - width : tLeft;
			tLeft = tLeft + realW < width ? width - width / 2 - realW : tLeft;
			correct();
		}
	}

	/**
	 * 重新获取Tab组宽度和偏移量
	 */
	function reflashWidth() {
		oLeft = $(".layadmin-pagetabs").offset().left; //标签组偏移量
		width = $("#lay-sz-tab-title").outerWidth(); //标签组可见宽度
	}

	var correctTimer;
	/**
	 * 微调
	 * 定位到可见左侧最近的Tab
	 */
	function correct() {
		$("#lay-sz-tab-title").css("left", tLeft);
		correctTimer = setTimeout(function () {
			var ww = 0;
			$("#lay-sz-tab-title li").each(function () {
				var li = $(this);
				//计算偏移量
				var f = li.offset().left - oLeft - 40;
				var w = li.outerWidth();
				ww += w;
				//找出最接近左侧的Tab
				if (f <= 0 && f >= -80) {
					ww -= w;
					tLeft = -ww;
					$("#lay-sz-tab-title").css("left", tLeft);
					return false;
				}
			});
		}, 300);
		// correctTimer.start();
	}

	/**
	 * 定位当前Tab
	 */
	function locateThisTab(id) {
		reflashWidth();
		var ww = 0;
		$("#lay-sz-tab-title li").each(function () {
			var li = $(this);
			var id_ = li.attr("lay-id");
			var w = li.outerWidth();
			ww += w;
			if (id == id_) {
				//left 的值
				ww -= w;
				if (Math.abs(ww) > width){
				ww -= width / 2;
				}
				//左侧偏移量
				var xwidth = li.offset().left - (oLeft + 40);
				//右边距
				var x = Math.abs(xwidth - width);
//				//console.log("width:" + width + ",xwidth:" + xwidth + ",x:" + Math.abs(xwidth - width));
//				//console.log("ww:" + ww);
				if (x < 160) {
					tLeft = -ww;
					correct();
				} else if (xwidth > width || xwidth < 0) { //小于0或者大于Tab组的宽度即为不可见
					tLeft = -ww;
					correct();
				}
				return false;
			}
		});
	}

	/**
	 * 添加Tab
	 */
	function addTab_(tab) {
		//如果Tab页存在则直接切换
		if (idArray.indexOf(tab.id) != -1) {
			changeTab_(tab.id);
			return;
		}
		// //console.log("新增Tab:" + url);
		var i = $('<li lay-id="' + tab.id + '" lay-attr="' + tab.url + '" class="layui-this"><span>' + tab.title +
			'</span></li>');
		if (tab.close === undefined || tab.close) {
			i.append('<i class="layui-icon layui-unselect layui-tab-close">&#x1006;</i>');
			closeArray.push(tab.id);
		}
		if (tab.icon !== undefined) {
			i.prepend('<i class="layui-icon ' + tab.icon + '"></i>&nbsp;');
		}
		$("#lay-sz-tab-title").append(i);
		idArray.push(tab.id);
		setContent(tab);
	}

	/**
	 * 填充Tab内容
	 */
	function setContent(tab) {
		var divHtml = '<div lay-id="' + tab.id + '" class="layadmin-tabsbody-item">';
		if (tab.type === "text") {
			//将内容转义后拼接
			divHtml += $('<div>').text(tab.content).html();
		} else if (tab.type === "html") {
			divHtml += tab.content;
		} else if (tab.type === "iframe") {
			divHtml += '<iframe src="' + tab.url + '" class="layadmin-iframe" frameborder="0"></iframe>';
		}
		divHtml += '</div>';
		$(domSelect + "-body").append(divHtml);
		changeTab_(tab.id);
	}

	/**
	 * 切换tab页
	 */
	function changeTab_(id) {
		// //console.log("切换Tab:" + id);
		//去除所有选中样式
		$(".layadmin-pagetabs .layui-tab-title li.layui-this").removeClass("layui-this");
		//给当前点击的Tab添加选中样式
		$(".layadmin-pagetabs .layui-tab-title li[lay-id='" + id + "']").addClass("layui-this");
		//切换iframe
		//隐藏当前显示的iframe
		$(".layadmin-tabsbody-item.layui-show").removeClass("layui-show");
		//显示当前点击的Tab对应的iframe
		$(".layadmin-tabsbody-item[lay-id='" + id + "']").addClass("layui-show");
		locateThisTab(id);
	}

	/**
	 * 删除Tab
	 */
	function deleteTab_(id, change) {
		// //console.log("删除Tab:" + id);
		nextOrprev(id, change);
		//移除数组中的id
		idArray.remove(id);
		closeArray.remove(id);
	}
	
	function nextOrprev(id, change){
		if(change === undefined){
			change = true;
		}
		//切换到下一个Tab
		var prevUrl = "";
		var thisEle = $(".layadmin-pagetabs .layui-tab-title li[lay-id='" + id + "']");
		if (thisEle.next().length === 0) { //如果后面有Tab就切换到下一个Tab，否则切换到上一个Tab
			prevUrl = thisEle.prev().attr("lay-id");
		} else {
			prevUrl = thisEle.next().attr("lay-id");
		}
		if(change){
			changeTab_(prevUrl);
		}
		//移除Tab
		$(".layadmin-pagetabs .layui-tab-title li[lay-id='" + id + "']").remove();
		//移除iframe
		$(".layadmin-tabsbody-item[lay-id='" + id + "']").remove();
	}

	/**
	 * 关闭指定Tab
	 * @param {Object} id
	 */
	function closeTab_(id){
		deleteTab_(id);
	}

	/**
	 * 关闭当前Tab
	 */
	function closeThisTab_() {
		var id = $(".layadmin-pagetabs .layui-tab-title li[class='layui-this']").attr("lay-id");
		deleteTab_(id);
	}

	/**
	 * 关闭其他Tab
	 */
	function closeOtherTabs_() {
		var cid = $(".layadmin-pagetabs .layui-tab-title li[class='layui-this']").attr("lay-id");
		// //console.log(closeArray);
		//使用一个临时数组，将close数组的值复制过来
		var tempArr = closeArray.slice();
		for (var i = 0; i < tempArr.length; i++) {
			var id = tempArr[i];
			if (id !== cid) {
				deleteTab_(id, false);
			}
		}
		$("#lay-sz-tab-title").css("left", 0);
	}

	/**
	 * 关闭所有Tab(close为true的)
	 */
	function closeAllTabs_() {
		var tempArr = closeArray.slice();
		for (var i = 0; i < tempArr.length; i++) {
			deleteTab_(tempArr[i], false);
		}
		changeTab_("index");
		$("#lay-sz-tab-title").css("left", 0);
	}

	var obj = {
		/**
		 * 初始化Tab
		 * @param select 选择器 #ID .样式
		 * @param opt 初始化参数
		 */
		init: function (select, opt) {
			domSelect = select;
			initTabs();
			opt = $.extend(true, {}, option, opt);
			this.addTab(opt.index);
		},
		/**
		 * 添加新Tab
		 */
		addTab: function (tab) {
			tab = $.extend({}, tabOpt, tab);
			if (tab.id === '') {
				console.error("tab.id is empty");
				return;
			}
			//将ID转为字符串，避免存入数组出现错误
			tab.id = '' + tab.id;
			addTab_(tab);
		},
		/**
		 * 关闭指定Tab
		 */
		close: function (id){
			closeTab_(id);
		},
		/**
		 * 关闭所有tab
		 */
		closeAll: function () {
			closeAllTabs_();
		},
		/**
		 * 关闭其他tab页
		 * @param cid
		 */
		closeOthers: function (id) {
			closeOtherTabs_(id);
		}
	};

	//输出tab接口
	exports('tab', obj);
});
