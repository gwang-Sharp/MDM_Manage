#pragma checksum "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\CommonToolView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ab05cec42d1ccdbab9fd7ed8520ef485ab57dd7f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_System_CommonToolView), @"mvc.1.0.view", @"/Views/System/CommonToolView.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ab05cec42d1ccdbab9fd7ed8520ef485ab57dd7f", @"/Views/System/CommonToolView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd5b3bba8f8af06ec4afc66ff69e7e53058283ba", @"/Views/_ViewImports.cshtml")]
    public class Views_System_CommonToolView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("include", "Development", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("exclude", "Development", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\CommonToolView.cshtml"
  
    ViewData["Title"] = "实体公用视图";
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("<script>\r\n        window.Entity =\'");
#nullable restore
#line 7 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\CommonToolView.cshtml"
                   Write(ViewBag.entity);

#line default
#line hidden
#nullable disable
            WriteLiteral("\'==null ? null :\'");
#nullable restore
#line 7 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\CommonToolView.cshtml"
                                                   Write(ViewBag.entity);

#line default
#line hidden
#nullable disable
            WriteLiteral("\';\r\n</script>\r\n<style>\r\n    .cell {\r\n        padding: 0px !important\r\n    }\r\n</style>\r\n<div id=\"CommonTool\" v-cloak>\r\n");
            WriteLiteral(@"    <el-form :inline=""true"" class=""demo-form-inline"" style=""margin: 8px 0px; display: flex; justify-content: space-between;"">
        <div>
            <el-form-item label=""模型:"" style=""margin-bottom: 0px;"">
                <el-select style=""margin-left: 2%;"" v-model=""ModelID"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged"">
                    <el-option v-for=""(item,index) in ModelSelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label=""实体:"" style=""margin-bottom: 0px;"">
                <el-select style=""margin-left: 2%;"" v-model=""EntityID"" placeholder=""请选择实体"" ");
            WriteLiteral(@"@change=""EntityHasChanged"">
                    <el-option v-for=""(item,index) in EntitySelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                    </el-option>
                </el-select>
            </el-form-item>
        </div>
        <div>
            <el-button size=""small"" circle type=""primary"" icon=""el-icon-view"" ");
            WriteLiteral("@click=\"DataFilter\" title=\"数据筛选\"></el-button>\r\n            <el-button size=\"small\" circle type=\"primary\" icon=\"el-icon-upload\" ");
            WriteLiteral("@click=\"UploadExcel\" title=\"批量导入\"></el-button>\r\n            <el-button size=\"small\" circle type=\"primary\" icon=\"el-icon-download\" ");
            WriteLiteral("@click=\"DownExcelTemp\" title=\"模板下载\"></el-button>\r\n        </div>\r\n    </el-form>\r\n");
            WriteLiteral(@"    <el-upload class=""upload-demo""
               action=""/system/UploadExcel""
               :on-success=""UploadSuccess""
               :before-upload=""BeforeUpload""
               :show-file-list=""false""
               :data=""UploadExtra""
               accept="".xlsx,.xls""
               style=""display:none""
               id=""UploadBtn"">
        <el-button size=""small"" type=""primary"">点击上传</el-button>
    </el-upload>
");
            WriteLiteral(@"    <el-row style=""margin-top:5px;"" v-show=""ShowTable"">
        <el-table :header-cell-style=""headerClass""
                  :data=""TableData""
                  border
                  style=""width: 100%; overflow-y: auto;""
                  :height=""TableHeight""
                  v-loading.fullscreen.lock=""EntityColsTabLoading""
                  element-loading-text=""加载中......""
                  element-loading-background=""rgba(255, 255, 255, 0.75)"">
");
            WriteLiteral(@"            <el-table-column :key=""index"" v-for=""(item,index) in TableCols""
                             :label=""item.Label""
                             align=""center"">
                <template slot-scope=""scope"">
                    <el-input :readonly=""true"" v-model=""scope.row[item.field]""></el-input>
                </template>
            </el-table-column>
            <el-table-column label=""操作"" align=""center"" width=""100"" :fixed=""ColumnFixed"">
                <template slot-scope=""scope"">
                    <el-button size=""small"" type=""primary"" icon=""el-icon-edit"" circle class=""button_button"" plain ");
            WriteLiteral("@click=\"handleEdit(scope.$index,scope.row,$event)\" title=\"编辑\"></el-button>\r\n");
            WriteLiteral("                </template>\r\n            </el-table-column>\r\n        </el-table>\r\n    </el-row>\r\n");
            WriteLiteral("    <el-row class=\"table_footer_btn\" v-show=\"ShowTable\">\r\n        <el-pagination background\r\n                       ");
            WriteLiteral("@size-change=\"handleSizeChange\"\r\n                       ");
            WriteLiteral(@"@current-change=""handleCurrentChange""
                       :current-page=""pagination.currentpage""
                       :page-sizes=""pagination.pagesizes""
                       :page-size=""pagination.pagesize""
                       layout=""total, sizes, prev, pager, next, jumper""
                       :total=""pagination.allNum""
                       style=""position:fixed; margin-top: 15px;"">
        </el-pagination>
    </el-row>


");
            WriteLiteral("    <el-drawer ");
            WriteLiteral(@"@closed=""DrawerhandleClose""
               :visible.sync=""DrawerDialog""
               direction=""rtl""
               custom-class=""demo-drawer""
               ref=""drawer""
               size=""50%"">
        <div class=""demo-drawer__content"">
            <div style=""padding-left:10px;padding-bottom:10px;"">
                <el-button size=""small"" icon=""el-icon-circle-plus-outline"" ");
            WriteLiteral("@click=\"AddFilter\" title=\"添加筛选\"></el-button>\r\n                <el-button size=\"small\" icon=\"el-icon-refresh-right\" ");
            WriteLiteral("@click=\"RefreshFilter\" title=\"重置筛选\"></el-button>\r\n                <el-button size=\"small\" icon=\"el-icon-check\" ");
            WriteLiteral(@"@click=""SearchFilter"" title=""提交""></el-button>
            </div>
            <el-table :data=""FilterData""
                      border
                      size=""small""
                      style=""width:100%"">
                <el-table-column v-for=""(item,index) in DrawerCols""
                                 :prop=""item.field""
                                 :key=""index""
                                 show-overflow-tooltip
                                 align=""center""
                                 :label=""item.label"">
                    <template slot-scope=""scope"">
                        <el-select v-model=""scope.row[scope.column.property]"" v-if=""scope.column.property=='AttrName'""
                                   placeholder=""请选择"">
                            <el-option v-for=""(item,index) in AttrsSelectOptions""
                                       :key=""index""
                                       :label=""item""
                                       :value=""item"">
      ");
            WriteLiteral(@"                      </el-option>
                        </el-select>
                        <el-select v-model=""scope.row[scope.column.property]"" v-if=""scope.column.property=='Operator'""
                                   placeholder=""请选择"">
                            <el-option v-for=""(item,index) in Operator""
                                       :key=""index""
                                       :label=""item.filter""
                                       :value=""item.value"">
                            </el-option>
                        </el-select>
                        <el-input v-model=""scope.row[scope.column.property]"" v-if=""scope.column.property=='Content'"" placeholder=""请输入条件""></el-input>
                    </template>
                </el-table-column>
                <el-table-column align=""center""
                                 label=""操作"">
                    <template slot-scope=""scope"">
                        <el-button size=""mini"" icon=""el-icon-delete"" circle type=""");
            WriteLiteral("danger\" ");
            WriteLiteral("@click=\"DrawerhandleDelete(scope.row)\" title=\"删除\"></el-button>\r\n                    </template>\r\n                </el-table-column>\r\n            </el-table>\r\n        </div>\r\n    </el-drawer>\r\n\r\n\r\n");
            WriteLiteral("</div>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ab05cec42d1ccdbab9fd7ed8520ef485ab57dd7f12554", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ab05cec42d1ccdbab9fd7ed8520ef485ab57dd7f12824", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 9219, "~/js/dev/CommonToolView.js?v=", 9219, 29, true);
#nullable restore
#line 183 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\CommonToolView.cshtml"
AddHtmlAttributeValue("", 9248, DateTime.Now.Millisecond, 9248, 25, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper.Include = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ab05cec42d1ccdbab9fd7ed8520ef485ab57dd7f15343", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ab05cec42d1ccdbab9fd7ed8520ef485ab57dd7f15613", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 9356, "~/js/dist/CommonToolView.js?v=", 9356, 30, true);
#nullable restore
#line 186 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\CommonToolView.cshtml"
AddHtmlAttributeValue("", 9386, DateTime.Now.Millisecond, 9386, 25, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper.Exclude = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n\r\n");
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
