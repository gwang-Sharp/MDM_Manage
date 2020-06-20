
//--------------------------------------------------------------------------
//q 2019.04.02 
//登录页面需用到的方法

var canGetCookie = 0;//是否支持存储Cookie 0 不支持 1 支持
var ajaxmockjax = 1;//是否启用虚拟Ajax的请求响 0 不启用  1 启用
//默认账号密码



var codeval = 0;
//code();


function Code() {
    if (canGetCookie == 1) {
        createCode("admincode");
        var admincode = getcookievalue("admincode");
        showCheck(admincode);
    } else {
        showCheck(createCode(""));
    }
}
function showCheck(a) {
    CodeVal = a;
    var c = document.getElementById("myCanvas");
    var ctx = c.getContext("2d");
    ctx.clearRect(0, 0, 1000, 1000);
    ctx.font = "80px 'Hiragino Sans GB'";
    ctx.fillStyle = "#E8DFE8";
    ctx.fillText(a, 0, 100);
}
$(document).keypress(function (e) {
    // 回车键事件
    if (e.which == 13) {
        $('input[type="button"]').click();
    }
});

$('input[name="pwd"]').focus(function () {
    $(this).attr('type', 'password');
});
$('input[type="text"]').focus(function () {
    $(this).prev().animate({ 'opacity': '1' }, 200);
});
$('input[type="text"],input[type="password"]').blur(function () {
    $(this).prev().animate({ 'opacity': '.5' }, 200);
});
$('input[name="login"],input[name="pwd"]').keyup(function () {
    var Len = $(this).val().length;
    if (!$(this).val() == '' && Len >= 5) {
        $(this).next().animate({
            'opacity': '1',
            'right': '30'
        }, 200);
    } else {
        $(this).next().animate({
            'opacity': '0',
            'right': '20'
        }, 200);
    }
});
var open = 0;
debugger
layui.use('layer', function () {

    //非空验证
    $('input[type="button"]').click(function () {
        console.log(111)
        var login = $('input[name="loginName"]').val();
        var pwd = $('input[name="pwd"]').val();
        var code = $('input[name="code"]').val();
        if (login == '') {
            ErroAlert('请输入您的账号');
        } else if (pwd == '') {
            debugger

            ErroAlert('请输入密码');
        }
        //else if (code == '' || code.length != 4) {
        //    ErroAlert('输入验证码');
        //}
        else {
            //认证中..
          
            $('.login').addClass('test'); //倾斜特效
            setTimeout(function () {
                $('.login').addClass('testtwo'); //平移特效
            }, 300);
            setTimeout(function () {
                $('.authent').show().animate({ right: -320 }, {
                    easing: 'easeOutQuint',
                    duration: 600,
                    queue: false
                });
                $('.authent').animate({ opacity: 1 }, {
                    duration: 200,
                    queue: false
                }).addClass('visible');
            }, 500);
         
            //登陆
            var JsonData = { login: login, pwd: pwd, code: code };
            //此处做为ajax内部判断
            layui.use('layer', function () {
                var layer = layui.layer;
                var token = $('input[name="__RequestVerificationToken"]').val();
                var userAccount = JsonData.login;
                var pwd = JsonData.pwd;
                $.ajax({
                    url: '/Login/doLogin',
                    type: 'POST',
                    data: {
                        userAccount: userAccount,
                        pwd: pwd,
                        __RequestVerificationToken: token
                    },
                    success: function (result) {

                        if (result != "failure") {
                            window.location.href = result;
                        } else {
                            close("用户名或密码错误，请重新输入！")
                        }
                    }, error: function () {
                        close("用户名或密码错误，请重新输入！")
                    }
                });
            });

          
        }
    })
})
var fullscreen = function () {
    elem = document.body;
    if (elem.webkitRequestFullScreen) {
        elem.webkitRequestFullScreen();
    } else if (elem.mozRequestFullScreen) {
        elem.mozRequestFullScreen();
    } else if (elem.requestFullScreen) {
        elem.requestFullscreen();
    } else {
        //浏览器不支持全屏API或已被禁用
    }
}
//关闭动画并提示
function close(msg) {
    setTimeout(function () {
        $('.authent').show().animate({ right: 90 }, {
            easing: 'easeOutQuint',
            duration: 600,
            queue: false
        });
        $('.authent').animate({ opacity: 0 }, {
            duration: 200,
            queue: false
        }).addClass('visible');
        $('.login').removeClass('testtwo'); //平移特效
    }, 1500);
    setTimeout(function () {
        $('.authent').hide();
        $('.login').removeClass('test');
        Code();
        var index = layer.alert(msg, { icon: 5, time: 3000, offset: 'auto', closeBtn: 0, title: '错误信息', btn: [], anim: 1, shade: 0 });
        layer.style(index, {
            color: '#777'
        });
    }, 2000);


}