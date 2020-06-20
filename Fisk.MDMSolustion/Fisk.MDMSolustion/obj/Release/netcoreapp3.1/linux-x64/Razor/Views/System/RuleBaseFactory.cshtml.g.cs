#pragma checksum "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\RuleBaseFactory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "737e6c28d4c6995da852160889d5a9c072f45e54"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_System_RuleBaseFactory), @"mvc.1.0.view", @"/Views/System/RuleBaseFactory.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"737e6c28d4c6995da852160889d5a9c072f45e54", @"/Views/System/RuleBaseFactory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd5b3bba8f8af06ec4afc66ff69e7e53058283ba", @"/Views/_ViewImports.cshtml")]
    public class Views_System_RuleBaseFactory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Dev/js/System/RuleBaseFactory.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\RuleBaseFactory.cshtml"
  
    ViewData["Title"] = "业务规则管理";
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <div id=\"RuleBaseManage\">\r\n\r\n        <template>\r\n            <el-tabs v-model=\"activeName\" ");
            WriteLiteral(@"@tab-click=""TabHandleClick"">
                <el-tab-pane label=""字段验证"" name=""first"">

                    <el-row style=""margin: 8px 0px;"">
                        <el-col :span=""24"">

                            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""ModelName"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged"">
                                <el-option v-for=""item in ModelSelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                                </el-option>
                            </el-select>

                            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""EntityName"" placeholder=""请选择实体"" ");
            WriteLiteral(@"@change=""EntityHasChanged"">
                                <el-option v-for=""item in EntitySelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                                </el-option>
                            </el-select>
                            <el-button size=""small"" type=""primary"" style=""margin-right:20px;float:right;margin-top:0.3%"" ");
            WriteLiteral("@click=\"ReleaseProc\">发布</el-button>\r\n                            <el-button size=\"small\" ");
            WriteLiteral(@"@click=""OpenRuleBaseDialog"" type=""primary"" circle title=""新建"" icon=""el-icon-plus"" style=""margin-right:20px;float:right;margin-top:0.3%""></el-button>

                        </el-col>
                    </el-row>
                    <el-row>
                        <el-table :data=""RuleBaseTableData""
                                  :height=""tableHeight""
                                  :header-cell-style=""headerClass""
                                  style=""width: 100%""
                                  v-loading=""RuleBaseTabLoading""
                                  element-loading-text=""加载中......""
                                  element-loading-background=""rgba(255, 255, 255, 0.75)""
                                  ref=""RuleBaseTableData"">
                            <el-table-column label=""序号""
                                             align=""center""
                                             width=""80"">
                                <template scope=""scope"">
                  ");
            WriteLiteral(@"                  <span>{{scope.$index+(pagenum - 1) * pagesize + 1}} </span>
                                </template>
                            </el-table-column>
                            <el-table-column prop=""ruleName""
                                             label=""规则名称""
                                             align=""center""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <el-table-column prop=""description""
                                             label=""规则描述""
                                             align=""center""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <el-table-column prop=""state""
                                             label=""状态""
                                             align=""center""
                                             show-overflow-tooltip>
     ");
            WriteLiteral(@"                       </el-table-column>
                            <el-table-column prop=""creater""
                                             show-overflow-tooltip
                                             width=""180""
                                             align=""center""
                                             label=""创建者"">
                            </el-table-column>
                            <el-table-column prop=""createTime""
                                             show-overflow-tooltip
                                             width=""200""
                                             align=""center""
                                             label=""创建时间"">
                            </el-table-column>
                            <el-table-column prop=""updater""
                                             show-overflow-tooltip
                                             width=""180""
                                             align=""center""
                   ");
            WriteLiteral(@"                          label=""更新者"">
                            </el-table-column>
                            <el-table-column prop=""updateTime""
                                             show-overflow-tooltip
                                             width=""200""
                                             align=""center""
                                             label=""更新时间"">
                            </el-table-column>
                            <el-table-column label=""操作"" width=""160"">
                                <template scope=""scope"">
                                    <el-button size=""small"" class=""button_button"" plain ");
            WriteLiteral("@click=\"OpenRuleBaseEditDialog(scope.$index, scope.row)\" circle type=\"primary\" title=\"编辑\" icon=\"el-icon-edit\"></el-button>\r\n                                    <el-button size=\"small\" plain type=\"danger\" title=\"删除\" circle icon=\"el-icon-delete\" ");
            WriteLiteral("@click=\"DelRuleBase(scope.row)\"></el-button>\r\n                                </template>\r\n                            </el-table-column>\r\n                        </el-table>\r\n");
            WriteLiteral("                        <el-pagination background\r\n                                       style=\"text-align: center;\"\r\n                                       ");
            WriteLiteral("@size-change=\"handleSizeChange\"\r\n                                       ");
            WriteLiteral(@"@current-change=""handleCurrentChange""
                                       :current-page=""pagenum""
                                       :page-size=""pagesize""
                                       :page-sizes=""[pageMaxsize,20,50]""
                                       layout=""total, sizes, prev, pager, next, jumper""
                                       :total=""total"">
                        </el-pagination>
                    </el-row>

                </el-tab-pane>
                <el-tab-pane label=""数据验证"" name=""second"">
                    <el-row style=""margin: 8px 0px;"">
                        <el-col :span=""24"">

                            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""DataModelName"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged"">
                                <el-option v-for=""item in DataModelSelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                                </el-option>
                            </el-select>

                            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""DataEntityName"" placeholder=""请选择实体"" ");
            WriteLiteral(@"@change=""EntityHasChanged"">
                                <el-option v-for=""item in DataEntitySelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                                </el-option>
                            </el-select>
                            <el-button size=""small"" ");
            WriteLiteral(@"@click=""OpenDataValidationTableDialog"" type=""primary"" circle title=""新建"" icon=""el-icon-plus"" style=""margin-right:20px;float:right;margin-top:0.3%""></el-button>

                        </el-col>
                    </el-row>
                    <el-row>
                        <el-table :data=""DataValidationTableData""
                                  :height=""tableHeight""
                                  :header-cell-style=""headerClass""
                                  style=""width: 100%""
                                  v-loading=""DataValidationTabLoading""
                                  element-loading-text=""加载中......""
                                  element-loading-background=""rgba(255, 255, 255, 0.75)""
                                  ref=""DataValidationTableData"">
                            <el-table-column label=""序号""
                                             align=""center""
                                             width=""80"">
                                <template scope");
            WriteLiteral(@"=""scope"">
                                    <span>{{scope.$index+(pagenum - 1) * pagesize + 1}} </span>
                                </template>
                            </el-table-column>
                            <el-table-column prop=""name""
                                             label=""名称""
                                             width=""100""
                                             align=""center""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <el-table-column prop=""description""
                                             label=""描述""
                                             align=""center""
                                             width=""100""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <el-table-column prop=""functionName""
                                             lab");
            WriteLiteral(@"el=""存储过程名称""
                                             align=""center""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <
                            <el-table-column prop=""creater""
                                             show-overflow-tooltip
                                             width=""180""
                                             align=""center""
                                             label=""创建者"">
                            </el-table-column>

                            <el-table-column prop=""createTime""
                                             show-overflow-tooltip
                                             width=""200""
                                             align=""center""
                                             label=""创建时间"">
                            </el-table-column>
                            <el-table-column label=""操作"" width=""300"">
                ");
            WriteLiteral("                <template scope=\"scope\">\r\n                                    <el-button size=\"small\" class=\"button_button\" plain ");
            WriteLiteral("@click=\"OpenDataValidationEditDialog( scope.row)\" circle type=\"primary\" title=\"编辑\" icon=\"el-icon-edit\"></el-button>\r\n                                    <el-button size=\"small\" plain type=\"danger\" title=\"删除\" circle icon=\"el-icon-delete\" ");
            WriteLiteral("@click=\"DelDataValidation(scope.row)\"></el-button>\r\n                                </template>\r\n                            </el-table-column>\r\n                        </el-table>\r\n");
            WriteLiteral("                        <el-pagination background\r\n                                       style=\"text-align: center;\"\r\n                                       ");
            WriteLiteral("@size-change=\"handleSizeChange\"\r\n                                       ");
            WriteLiteral(@"@current-change=""handleCurrentChange""
                                       :current-page=""pagenum""
                                       :page-size=""pagesize""
                                       :page-sizes=""[pageMaxsize,20,50]""
                                       layout=""total, sizes, prev, pager, next, jumper""
                                       :total=""total"">
                        </el-pagination>
                    </el-row>
                </el-tab-pane>
                <el-tab-pane label=""数据维护"" name=""Three"">
                    <el-row style=""margin: 8px 0px;"">
                        <el-col :span=""24"">

                            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""FieldModelName"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged"">
                                <el-option v-for=""item in FieldModelSelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                                </el-option>
                            </el-select>

                            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""FieldEntityName"" placeholder=""请选择实体"" ");
            WriteLiteral(@"@change=""EntityHasChanged"">
                                <el-option v-for=""item in FieldEntitySelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                                </el-option>
                            </el-select>
                            <el-button size=""small"" ");
            WriteLiteral(@"@click=""OpenFieldValidationTableDialog"" type=""primary"" circle title=""新建"" icon=""el-icon-plus"" style=""margin-right:20px;float:right;margin-top:0.3%""></el-button>

                        </el-col>
                    </el-row>
                    <el-row>
                        <el-table :data=""FieldValidationTableData""
                                  :height=""tableHeight""
                                  :header-cell-style=""headerClass""
                                  style=""width: 100%""
                                  v-loading=""FieldValidationTabLoading""
                                  element-loading-text=""加载中......""
                                  element-loading-background=""rgba(255, 255, 255, 0.75)""
                                  ref=""FieldValidation"">
                            <el-table-column label=""序号""
                                             align=""center""
                                             width=""80"">
                                <template scope=""sco");
            WriteLiteral(@"pe"">
                                    <span>{{scope.$index+(pagenum - 1) * pagesize + 1}} </span>
                                </template>
                            </el-table-column>
                            <el-table-column prop=""name""
                                             label=""名称""
                                             align=""center""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <el-table-column prop=""description""
                                             label=""描述""
                                             align=""center""
                                             show-overflow-tooltip>
                            </el-table-column>
                            <el-table-column prop=""creater""
                                             show-overflow-tooltip
                                             align=""center""
                                             ");
            WriteLiteral(@"label=""创建者"">
                            </el-table-column>

                            <el-table-column prop=""createTime""
                                             show-overflow-tooltip
                                             align=""center""
                                             label=""创建时间"">
                            </el-table-column>
                            <el-table-column label=""操作"">
                                <template scope=""scope"">
                                    <el-button size=""small"" class=""button_button"" plain ");
            WriteLiteral("@click=\"OpenFieldValidationEditDialog( scope.row)\" circle type=\"primary\" title=\"编辑\" icon=\"el-icon-edit\"></el-button>\r\n                                    <el-button size=\"small\" plain type=\"danger\" title=\"删除\" circle icon=\"el-icon-delete\" ");
            WriteLiteral("@click=\"DelFieldValidation(scope.row)\"></el-button>\r\n                                </template>\r\n                            </el-table-column>\r\n                        </el-table>\r\n");
            WriteLiteral("                        <el-pagination background\r\n                                       style=\"text-align: center;\"\r\n                                       ");
            WriteLiteral("@size-change=\"handleSizeChange\"\r\n                                       ");
            WriteLiteral(@"@current-change=""handleCurrentChange""
                                       :current-page=""pagenum""
                                       :page-size=""pagesize""
                                       :page-sizes=""[pageMaxsize,20,50]""
                                       layout=""total, sizes, prev, pager, next, jumper""
                                       :total=""total"">
                        </el-pagination>
                    </el-row>
                </el-tab-pane>

            </el-tabs>
        </template>



");
            WriteLiteral("        <el-dialog :title=\"Title\"\r\n                   :visible.sync=\"AddRuleBasedialogVisible\"\r\n                   width=\"30%\"\r\n                   :append-to-body=\"true\"\r\n                   :close-on-click-modal=\"false\"\r\n                   ");
            WriteLiteral(@"@close=""ClosedAddRuleBasedialog"">
            <el-form label-position=""left"" :model=""RuleBaseForm"" ref=""RuleBaseForm"" :rules=""RuleBaseRules"" label-width=""95px"">
                <el-form-item label=""名称"" prop=""RuleName"">
                    <el-input v-model=""RuleBaseForm.RuleName""
                              maxlength=""50""
                              placeholder=""请输入规则名称""
                              show-word-limit></el-input>
                </el-form-item>
");
            WriteLiteral(@"                <el-form-item label=""规则描述"" prop=""Description"">
                    <el-input v-model=""RuleBaseForm.Description""
                              maxlength=""150""
                              placeholder=""请输入规则名称""
                              show-word-limit></el-input>
                </el-form-item>
                <el-form-item label=""属性"" prop=""AttributeName"">
                    <el-select maxlength=""50"" show-word-limit ");
            WriteLiteral(@"@change=""AttributeNameDialogChange"" v-model=""RuleBaseForm.AttributeName"" placeholder=""请选择模型"">
                        <el-option v-for=""item in AttributeSelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label=""必填"" prop=""Required"" style=""margin-left:10px"">
                    <el-switch v-model=""RuleBaseForm.Required""></el-switch>
                </el-form-item>
                <el-form-item label=""验证规则"" prop=""Type"">
                    <el-radio-group ");
            WriteLiteral(@"@change=""TypeChange"" v-model=""RuleBaseForm.Type"">
                        <el-radio label=""手机号""></el-radio>
                        <el-radio label=""邮箱""></el-radio>
                        <el-radio label=""自定义验证规则""></el-radio>
                    </el-radio-group>
                </el-form-item>
                <el-form-item style=""margin-left:10px"" prop=""Expression"" v-if=""CustomTypeIsShow"" label=""自定义规则"">
                    <el-input v-model=""RuleBaseForm.Expression""
                              maxlength=""150""
                              placeholder=""请输入自定义验证规则""
                              show-word-limit></el-input>
                </el-form-item>
                <div class=""dialog_footer_btn"">
                    <el-button size=""small"" type=""primary"" ");
            WriteLiteral("@click=\"SubmitRuleBaseForm(\'RuleBaseForm\')\">提交</el-button>\r\n                    <el-button size=\"small\" ");
            WriteLiteral("@click=\"CloseRuleBasedialogVisible(\'RuleBaseForm\')\">取消</el-button>\r\n                </div>\r\n            </el-form>\r\n        </el-dialog>\r\n\r\n\r\n");
            WriteLiteral("\r\n        <el-dialog :title=\"FieldialogTitle\" :visible.sync=\"FieldialogVisible\" width=\"30%\" ");
            WriteLiteral(@"@closed=""FieldhandleClose"">
            <el-form :model=""FieldValidationForm"" label-position=""left"" ref=""FieldValidationForm"" class=""demo-ruleForm"" label-width=""120px"" :rules=""FieldValidationFormRules"">
                <el-form-item label=""名称"" required prop=""Name"">
                    <el-input size=""mini"" v-model=""FieldValidationForm.Name"" placeholder=""名称"" maxlength=""50"" show-word-limit></el-input>
                </el-form-item>
                <el-form-item label=""说明"" required prop=""Description"">
                    <el-input size=""mini"" v-model=""FieldValidationForm.Description"" placeholder=""说明"" maxlength=""50"" show-word-limit></el-input>
                </el-form-item>
                <div class=""dialog_footer_btn"">
                    <el-button size=""small"" type=""primary"" ");
            WriteLiteral("@click=\"SubmitFieldValidationFrom\" v-html=\"\'保存\'\"></el-button>\r\n                    <el-button size=\"small\" ");
            WriteLiteral("@click=\"FieldCancelAddOrUpdate\" v-html=\"\'取消\'\"></el-button>\r\n                </div>\r\n            </el-form>\r\n        </el-dialog>\r\n\r\n");
            WriteLiteral("\r\n\r\n\r\n\r\n");
            WriteLiteral("\r\n\r\n        <el-dialog :title=\"DatadialogTitle\" :visible.sync=\"DatadialogVisible\" width=\"30%\" ");
            WriteLiteral(@"@closed=""DatahandleClose"">
            <el-form :model=""DataValidationForm"" label-position=""left"" ref=""DataValidationForm"" class=""demo-ruleForm"" label-width=""120px"" :rules=""DataValidationFormRules"">
                <el-form-item label=""名称"" required prop=""Name"">
                    <el-input size=""mini"" v-model=""DataValidationForm.Name"" placeholder=""名称"" maxlength=""50"" show-word-limit></el-input>
                </el-form-item>
                <el-form-item label=""说明"" required prop=""Description"">
                    <el-input size=""mini"" v-model=""DataValidationForm.Description"" placeholder=""说明"" maxlength=""50"" show-word-limit></el-input>
                </el-form-item>
                <el-form-item label=""存储过程名称"" required prop=""FunctionName"">
                    <el-input size=""mini"" v-model=""DataValidationForm.FunctionName"" placeholder=""存储过程名称"" maxlength=""50"" show-word-limit></el-input>
                </el-form-item>
                <div class=""dialog_footer_btn"">
                    <el-button siz");
            WriteLiteral("e=\"small\" type=\"primary\" ");
            WriteLiteral("@click=\"SubmitDataValidationFrom\" v-html=\"\'保存\'\"></el-button>\r\n                    <el-button size=\"small\" ");
            WriteLiteral("@click=\"DataCancelAddOrUpdate\" v-html=\"\'取消\'\"></el-button>\r\n                </div>\r\n            </el-form>\r\n        </el-dialog>\r\n\r\n");
            WriteLiteral("\r\n    </div>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "737e6c28d4c6995da852160889d5a9c072f45e5427889", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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