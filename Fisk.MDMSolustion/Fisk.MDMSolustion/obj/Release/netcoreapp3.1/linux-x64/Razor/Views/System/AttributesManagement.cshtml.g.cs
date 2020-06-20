#pragma checksum "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\AttributesManagement.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ef38a71255b2f285e8614c5858fa15ccf7701448"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_System_AttributesManagement), @"mvc.1.0.view", @"/Views/System/AttributesManagement.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ef38a71255b2f285e8614c5858fa15ccf7701448", @"/Views/System/AttributesManagement.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd5b3bba8f8af06ec4afc66ff69e7e53058283ba", @"/Views/_ViewImports.cshtml")]
    public class Views_System_AttributesManagement : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\AttributesManagement.cshtml"
  
    ViewData["Title"] = "属性管理";
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("<div id=\"AttributesVue\">\r\n");
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
                <el-select style=""margin-left: 2%;"" v-model=""AttributesRuleForm.EntityID"" placeholder=""请选择实体"" clearable ");
            WriteLiteral(@"@change=""EntityHasChanged"">
                    <el-option v-for=""(item,index) in EntitySelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                    </el-option>
                </el-select>
            </el-form-item>
        </div>
        <el-form-item style=""margin-bottom: 0px;"">
            <el-button size='small' circle style=""margin-top: 10%"" ");
            WriteLiteral("@click=\"AddAttributes\" type=\"primary\" title=\"新建\" icon=\"el-icon-plus\"></el-button>\r\n        </el-form-item>\r\n    </el-form>\r\n");
            WriteLiteral(@"    <el-row>
        <el-table :header-cell-style=""headerClass""
                  :data=""AttributestableData""
                  style=""width: 100%; overflow-y: auto;""
                  height=""calc(100vh - 178px)""
                  v-loading=""AttributesTabLoading""
                  element-loading-text=""加载中......""
                  element-loading-background=""rgba(255, 255, 255, 0.75)"">
            <el-table-column prop=""name"" label=""英文姓名"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""displayName"" label=""显示名称"" min-width=""100"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""remark"" label=""说明"" min-width=""120"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""type"" label=""属性类型"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""startTrace"" label=""更改跟踪"" align=""center"" show-overflo");
            WriteLiteral(@"w-tooltip>
            </el-table-column>
            <el-table-column prop=""creater"" label=""创建者"" min-width=""80"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""createTime"" label=""创建时间"" min-width=""120"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""updater"" label=""修改者"" min-width=""120"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""updateTime"" label=""修改时间"" min-width=""120"" align=""center"" show-overflow-tooltip>
            </el-table-column>
            <el-table-column label=""操作"" align=""center"" width=""100"">
                <template slot-scope=""scope"">
                    <el-button size=""small"" type=""primary"" icon=""el-icon-edit"" circle class=""button_button"" plain ");
            WriteLiteral("@click=\"handleEdit(scope.$index, scope.row)\" title=\"编辑\"></el-button>\r\n                    <el-button size=\"small\" plain type=\"danger\" circle title=\"删除\" icon=\"el-icon-delete\" ");
            WriteLiteral("@click=\"handleDelete(scope.$index, scope.row)\"></el-button>\r\n                </template>\r\n            </el-table-column>\r\n        </el-table>\r\n    </el-row>\r\n");
            WriteLiteral("    <el-row class=\"table_footer_btn\">\r\n        <el-pagination background\r\n                       ");
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
            WriteLiteral("    <el-dialog :title=\"dialogTitle\" :visible.sync=\"dialogVisible\" width=\"40%\" ");
            WriteLiteral(@"@closed=""handleClose"">
        <el-form :model=""AttributesRuleForm"" ref=""AttributesRuleForm"" class=""demo-ruleForm"" :rules=""AttributesFromrules"" label-width=""100px"">
            <el-form-item label=""名字"" required prop=""Name"">
                <el-input size=""mini"" :disabled=""disabled"" v-model=""AttributesRuleForm.Name"" placeholder=""名字"" maxlength=""50"" show-word-limit></el-input>
            </el-form-item>
            <el-form-item label=""显示名称"" required prop=""DisplayName"">
                <el-input size=""mini"" v-model=""AttributesRuleForm.DisplayName"" placeholder=""显示名称"" maxlength=""50"" show-word-limit></el-input>
            </el-form-item>
            <el-form-item label=""说明"" required prop=""Remark"">
                <el-input size=""mini"" type=""textarea"" v-model=""AttributesRuleForm.Remark"" placeholder=""说明"" maxlength=""200"" show-word-limit></el-input>
            </el-form-item>
            <el-form-item label=""属性类型:"" required prop=""Type"">
                <el-select size='mini' v-model=""AttributesRuleForm.T");
            WriteLiteral("ype\" placeholder=\"请选择属性类型\" ");
            WriteLiteral(@"@change=""TypeHasChanged"">
                    <el-option v-for=""(item,index) in AttributeTypes"" :key=""index"" :label=""item.label"" :value=""item.value"">
                    </el-option>
                </el-select>
            </el-form-item>
            <el-form-item label=""数据类型:"" v-if=""AttributesRuleForm.Type!='基于域'"" style=""overflow:hidden;"">
                <el-select size='mini' v-model=""AttributesRuleForm.DataType"" placeholder=""请选择数据类型"" style=""float:left;"">
                    <el-option v-for=""(item,index) in AttributeDataType"" :key=""index"" :label=""item.label"" :value=""item.value"">
                    </el-option>
                </el-select>
                <div v-if=""AttributesRuleForm.DataType=='文本'"" style=""float:left;margin-left:1%;height:60px;"">
                    <el-form-item required prop=""TypeLength"">
                        <el-input size=""mini"" v-model=""AttributesRuleForm.TypeLength"" placeholder=""长度最少50""></el-input>
                    </el-form-item>
                </div>
      ");
            WriteLiteral("      </el-form-item>\r\n            <div v-else>\r\n                <el-form-item label=\"模型:\">\r\n                    <el-select size=\'mini\' v-model=\"AttributesRuleForm.LinkModelID\" placeholder=\"请选择模型\" ");
            WriteLiteral(@"@change=""ModelHasChanged2"">
                        <el-option v-for=""(item,index) in ModelSelectOptions"" :key=""index"" :label=""item.name"" :value=""item.id"">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label=""实体:"">
                    <el-select size='mini' id=""linkEntity"" v-model=""AttributesRuleForm.LinkEntityID"" placeholder=""请选择实体"">
                        <el-option v-for=""(item,index) in EntitySelectOptions2"" :key=""index"" :label=""item.name"" :value=""item.id"">
                        </el-option>
                    </el-select>
                </el-form-item>
            </div>
            <el-form-item>
                <el-checkbox size=""mini"" label=""启动更改跟踪"" v-model=""AttributesRuleForm.StartTrace""></el-checkbox>
            </el-form-item>
            <div class=""dialog_footer_btn"">
                <el-button size=""small"" type=""primary"" ");
            WriteLiteral("@click=\"AddOrUpdate\" v-html=\"\'保存\'\"></el-button>\r\n                <el-button size=\"small\" ");
            WriteLiteral("@click=\"CancelAddOrUpdate\" v-html=\"\'取消\'\"></el-button>\r\n            </div>\r\n        </el-form>\r\n    </el-dialog>\r\n</div>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ef38a71255b2f285e8614c5858fa15ccf770144812084", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 8321, "~/Dev/js/System/AttributesManagement.js?V=", 8321, 42, true);
#nullable restore
#line 142 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\AttributesManagement.cshtml"
AddHtmlAttributeValue("", 8363, DateTime.Now.Millisecond, 8363, 25, false);

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