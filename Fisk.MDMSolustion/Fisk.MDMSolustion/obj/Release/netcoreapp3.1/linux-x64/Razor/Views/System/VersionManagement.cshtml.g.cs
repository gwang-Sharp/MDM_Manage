#pragma checksum "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\VersionManagement.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1500814c508a156f64a5adea26842d3948bd0a82"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_System_VersionManagement), @"mvc.1.0.view", @"/Views/System/VersionManagement.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1500814c508a156f64a5adea26842d3948bd0a82", @"/Views/System/VersionManagement.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd5b3bba8f8af06ec4afc66ff69e7e53058283ba", @"/Views/_ViewImports.cshtml")]
    public class Views_System_VersionManagement : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.EnvironmentTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\VersionManagement.cshtml"
  
    ViewData["Title"] = "版本快照管理";
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"Version\">\r\n    <el-tabs v-model=\"activeName\" ");
            WriteLiteral("@tab-click=\"handleClick\">\r\n");
            WriteLiteral("        <el-tab-pane label=\"快照管理\" name=\"first\">\r\n");
            WriteLiteral(@"            <el-form :inline=""true"" class=""demo-form-inline"" style=""margin: 8px 0px; display: flex; justify-content: space-between;"">
                <div>
                    <el-form-item label=""模型:"" style=""margin-bottom: 0px;"">
                        <el-select style=""margin-left: 2%;"" v-model=""ModelID"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged"">
                            <el-option v-for=""(item,index) in ModelSelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label=""实体:"" style=""margin-bottom: 0px;"">
                        <el-select style=""margin-left: 2%;"" v-model=""EntityID"" placeholder=""请选择实体"" clearable ");
            WriteLiteral(@"@change=""EntityHasChanged"">
                            <el-option v-for=""(item,index) in EntitySelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </div>
                <el-form-item style=""margin-bottom: 0px;"">
                    <el-button size='small' circle style=""margin-top: 10%"" ");
            WriteLiteral("@click=\"AddVersion\" type=\"primary\" title=\"新建\" icon=\"el-icon-plus\"></el-button>\r\n                </el-form-item>\r\n            </el-form>\r\n");
            WriteLiteral(@"            <el-row style=""margin-top:5px;"">
                <el-table :header-cell-style=""headerClass""
                          :data=""TableData""
                          border
                          style=""width: 100%; overflow-y: auto;""
                          :height=""TableHeight""
                          v-loading.fullscreen.lock=""ColsTabLoading""
                          element-loading-text=""加载中......""
                          element-loading-background=""rgba(255, 255, 255, 0.75)"">
                    <el-table-column :key=""index"" v-for=""(item,index) in VersionTableCols""
                                     :prop=""item.field""
                                     :label=""item.title""
                                     align=""center"">
                    </el-table-column>
                    <el-table-column label=""操作"" align=""center"" width=""100"">
                        <template slot-scope=""scope"">
                            <el-button size=""small"" plain type=""danger"" circle");
            WriteLiteral(" title=\"删除\" icon=\"el-icon-delete\" ");
            WriteLiteral("@click=\"handleDelete(scope.row)\"></el-button>\r\n                        </template>\r\n                    </el-table-column>\r\n                </el-table>\r\n            </el-row>\r\n");
            WriteLiteral("            <el-row class=\"table_footer_btn\">\r\n                <el-pagination background\r\n                               ");
            WriteLiteral("@size-change=\"handleSizeChange\"\r\n                               ");
            WriteLiteral(@"@current-change=""handleCurrentChange""
                               :current-page=""pagination.currentpage""
                               :page-sizes=""pagination.pagesizes""
                               :page-size=""pagination.pagesize""
                               layout=""total, sizes, prev, pager, next, jumper""
                               :total=""pagination.allNum""
                               style=""position:fixed;"">
                </el-pagination>
            </el-row>
        </el-tab-pane>
");
            WriteLiteral("        <el-tab-pane label=\"拉链管理\" name=\"second\">\r\n");
            WriteLiteral(@"            <el-form :inline=""true"" class=""demo-form-inline"" style=""margin: 8px 0px; display: flex; justify-content: space-between;"">
                <div>
                    <el-form-item label=""模型:"" style=""margin-bottom: 0px;"">
                        <el-select style=""margin-left: 2%;"" v-model=""ModelID2"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged2"">
                            <el-option v-for=""(item,index) in ModelSelectOptions2"" :key=""index"" :label=""item.name"" :value=""item.id"">
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label=""实体:"" style=""margin-bottom: 0px;"">
                        <el-select style=""margin-left: 2%;"" v-model=""EntityID2"" placeholder=""请选择实体"" clearable ");
            WriteLiteral(@"@change=""EntityHasChanged2"">
                            <el-option v-for=""(item,index) in EntitySelectOptions2"" :key=""index"" :label=""item.name"" :value=""item.id"">
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label=""属性:"" style=""margin-bottom: 0px;"">
                        <el-select style=""margin-left: 2%;"" v-model=""AttrID"" placeholder=""请选择属性"" clearable ");
            WriteLiteral(@"@change=""AttrHasChanged"">
                            <el-option v-for=""(item,index) in AttrsSelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </div>
            </el-form>
");
            WriteLiteral(@"            <el-row style=""margin-top:5px;"">
                <el-table :header-cell-style=""headerClass""
                          :data=""TableData2""
                          border
                          style=""width: 100%; overflow-y: auto;""
                          :height=""TableHeight""
                          v-loading=""ColsTabLoading2""
                          element-loading-text=""加载中......""
                          element-loading-background=""rgba(255, 255, 255, 0.75)"">
                    <el-table-column :key=""index"" v-for=""(item,index) in TableCols""
                                     :prop=""item.field""
                                     :label=""item.title""
                                     align=""center"">
                    </el-table-column>
                </el-table>
            </el-row>
");
            WriteLiteral("            <el-row class=\"table_footer_btn\">\r\n                <el-pagination background\r\n                               ");
            WriteLiteral("@size-change=\"handleSizeChange2\"\r\n                               ");
            WriteLiteral(@"@current-change=""handleCurrentChange2""
                               :current-page=""pagination2.currentpage""
                               :page-sizes=""pagination2.pagesizes""
                               :page-size=""pagination2.pagesize""
                               layout=""total, sizes, prev, pager, next, jumper""
                               :total=""pagination2.allNum""
                               style=""position:fixed;"">
                </el-pagination>
            </el-row>
        </el-tab-pane>
    </el-tabs>

");
            WriteLiteral("    <el-dialog :title=\"dialogTitle\" :close-on-click-modal=\"false\" :close-on-press-escape=\"false\" :visible.sync=\"dialogVisible\" width=\"40%\" ");
            WriteLiteral(@"@closed=""handleClose"">
        <el-form :model=""VsesionForm"" ref=""VsesionForm"" class=""demo-ruleForm"" :rules=""VersionFromrules"" label-width=""100px"">
            <el-form-item label=""名称"" required prop=""Name"">
                <el-input size=""mini"" v-model=""VsesionForm.Name"" placeholder=""名字"" maxlength=""50"" show-word-limit></el-input>
            </el-form-item>
            <el-form-item label=""说明"" required prop=""Remark"">
                <el-input size=""mini"" v-model=""VsesionForm.Remark"" placeholder=""说明"" maxlength=""200"" show-word-limit></el-input>
            </el-form-item>
            <el-form-item label=""涉及表名"" v-if=""tags.length>0"">
                <el-tag v-for=""tag in tags""
                        :key=""tag.name""
                        type=""info"">
                    {{tag.name}}
                </el-tag>
            </el-form-item>
            <div class=""dialog_footer_btn"">
                <el-button size=""small"" id=""submit"" type=""primary"" ");
            WriteLiteral("@click=\"addOrUpdate\" v-html=\"\'确定\'\"></el-button>\r\n                <el-button size=\"small\" ");
            WriteLiteral("@click=\"CancelAddOrUpdate\" v-html=\"\'取消\'\"></el-button>\r\n            </div>\r\n        </el-form>\r\n    </el-dialog>\r\n");
            WriteLiteral("    <el-dialog :visible.sync=\"dialogVisible2\" width=\"20%\" ");
            WriteLiteral(@"@closed=""handleClose2"" :close-on-click-modal=""false"" :close-on-press-escape=""false"" :show-close=""ShowClose"">
        <div class=""block"">
            <el-timeline>
                <el-timeline-item v-for=""(activity, index) in activities""
                                  :key=""index""
                                  :icon=""activity.icon""
                                  :type=""activity.type""
                                  :color=""activity.color""
                                  :size=""activity.size""
                                  :timestamp=""activity.timestamp"">
                    {{activity.content}}
                </el-timeline-item>
            </el-timeline>
        </div>
    </el-dialog>

</div>
");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1500814c508a156f64a5adea26842d3948bd0a8214202", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 9057, "~/lib/signalr/signalr.js?v=", 9057, 27, true);
#nullable restore
#line 165 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\VersionManagement.cshtml"
AddHtmlAttributeValue("", 9084, DateTime.Now.Millisecond, 9084, 25, false);

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
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1500814c508a156f64a5adea26842d3948bd0a8215739", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1500814c508a156f64a5adea26842d3948bd0a8216009", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 9176, "~/js/dev/VersionManagement.js?v=", 9176, 32, true);
#nullable restore
#line 167 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\VersionManagement.cshtml"
AddHtmlAttributeValue("", 9208, DateTime.Now.Millisecond, 9208, 25, false);

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
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1500814c508a156f64a5adea26842d3948bd0a8218534", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "1500814c508a156f64a5adea26842d3948bd0a8218804", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 9316, "~/js/dist/VersionManagement.js?v=", 9316, 33, true);
#nullable restore
#line 170 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\VersionManagement.cshtml"
AddHtmlAttributeValue("", 9349, DateTime.Now.Millisecond, 9349, 25, false);

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
            WriteLiteral("\r\n\r\n");
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