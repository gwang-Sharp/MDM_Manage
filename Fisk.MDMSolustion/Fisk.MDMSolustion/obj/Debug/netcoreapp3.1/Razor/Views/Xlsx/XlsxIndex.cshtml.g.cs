#pragma checksum "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\Xlsx\XlsxIndex.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "20f5dcb8b640d401582a7e12bc66381a77f77be0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Xlsx_XlsxIndex), @"mvc.1.0.view", @"/Views/Xlsx/XlsxIndex.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\_ViewImports.cshtml"
using Fisk.MDMSolustion;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\_ViewImports.cshtml"
using Fisk.MDMSolustion.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"20f5dcb8b640d401582a7e12bc66381a77f77be0", @"/Views/Xlsx/XlsxIndex.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd5b3bba8f8af06ec4afc66ff69e7e53058283ba", @"/Views/_ViewImports.cshtml")]
    public class Views_Xlsx_XlsxIndex : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\Xlsx\XlsxIndex.cshtml"
  
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
            WriteLiteral(@"

    <div id=""testcontent"">
        <input type=""button"" onclick=muiltAdd() value=""多条数据新增"" style=""margin-bottom:20px;"" />
        <br />
        <input type=""button"" onclick=testsimilar() value=""测试相似度计算"" style=""margin-bottom:20px;"" />
        <br />
        <input type=""button"" onclick=testexception() value=""测试全局异常处理"" style=""margin-bottom:20px;"" />
        <p>测试Jenkins自动部署</p>
        <input type=""button"" onclick=mergedata() value=""合并数据"" style=""margin-bottom:20px;"" />
        <br />
        <input type=""button"" onclick=gettoken() value=""获取Token"" style=""margin-bottom:20px;"" />
        <p id=""token""></p>
        <br />
        <input type=""button"" onclick=test() value=""test Token"" style=""margin-bottom:20px;"" />
        <p id=""result""></p>
        <br />
        <input type=""text"" id=""entityid"" placeholder=""实体ID"" />
        <input type=""button"" onclick=outexcel() value=""excel下载"" style=""margin-bottom:20px;"" />

        <el-upload class=""upload-demo""
                   ref=""upload""
         ");
            WriteLiteral(@"          accept="".xlsx""
                   action=""/system/UploadExcel""
                   :data=uploadParam
                   :auto-upload=""false"">
            <el-button slot=""trigger"" size=""small"" type=""primary"">选取文件</el-button>
            <el-button style=""margin-left: 10px;"" size=""small"" type=""success"" ");
            WriteLiteral(@"@click=""submitUpload"">上传到服务器</el-button>
            <div slot=""tip"" class=""el-upload__tip"">只能上传xlsx/xls文件</div>
        </el-upload>
    </div>

<!-- import Vue before Element -->
<script src=""https://unpkg.com/vue/dist/vue.js""></script>
<!-- import JavaScript -->
<script src=""https://unpkg.com/element-ui/lib/index.js""></script>
<script>
    var testvue = new Vue({
        el: '#testcontent',
        data: {
            uploadParam: {
                entityname: 'channel'
            },
        },
        methods: {
            submitUpload() {
                this.$refs.upload.submit();
            },
        }
    });
    function outexcel() {
        var entityid = $(""#entityid"").val();
        window.open(""/xlsx/OutputExcel?entityid="" + entityid + """");
    }
    function gettoken() {
        $.ajax({
            type: ""get"",
            url: ""http://192.168.11.201:5004/OAuth/token?name=admin&pwd=123"",
            dataType: ""json"",
            success: function (data) {
  ");
            WriteLiteral(@"              debugger
                var token = data.authorization;
                $(""#token"").text(token);
            }, error: function (error) {
                console.log(error);
            }
        });
    }
    function test() {
        $.ajax({
            url: 'http://192.168.11.201:5004/mdmapi/test',
            headers: {
                'authorization': $(""#token"").text()
            },
            success: function (data) {
                $(""#result"").text(data)
            },
            error: function (err) {
            }
        });
    }
    function mergedata() {
        $.ajax({
            url: ""/system/MergeData"",
            success: function (data) {
                console.log(data);
            }, error: function (error) {
                console.log(error);
            }
        });
    }
    function testexception() {
        $.ajax({
            url: ""/system/testGlobalException"",
            success: function (data) {
                co");
            WriteLiteral(@"nsole.log(data);
            }, error: function (error) {
                console.log(error);
            }
        });
    }
    function testsimilar() {
        $.ajax({
            url: ""/system/testsimilar"",
            data: { ""entityID"": ""320"", ""mergingCode"":""26522187-4266-4009-bfc5-e77351db7fb6""},
            success: function (data) {
                console.log(data);
            }, error: function (error) {
                console.log(error);
            }
        });
    }
    function muiltAdd() {
        var jsonObj = { ""EntityName"": ""test"", ""Data"": [{ ""batch_id"": ""20200428"", ""name"": ""ZBQ"", ""code"": ""20"", ""BissnessRuleStatus"": ""0"", ""ValidateStatus"": ""2"" }, { ""batch_id"": ""20200428"", ""name"": ""ZBQ11"", ""code"": ""201"", ""BissnessRuleStatus"": ""0"", ""ValidateStatus"": ""2"" } ]};
        $.post('/system/DataSaveEntityMember', { body: JSON.stringify(jsonObj) }, res => {

        });
    }
</script>

");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
