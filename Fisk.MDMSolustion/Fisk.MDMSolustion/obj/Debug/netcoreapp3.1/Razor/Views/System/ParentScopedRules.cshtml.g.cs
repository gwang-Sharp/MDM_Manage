#pragma checksum "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\ParentScopedRules.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "21ef354b5b32364c1358f2c549bdd40aeed37c4e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_System_ParentScopedRules), @"mvc.1.0.view", @"/Views/System/ParentScopedRules.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"21ef354b5b32364c1358f2c549bdd40aeed37c4e", @"/Views/System/ParentScopedRules.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd5b3bba8f8af06ec4afc66ff69e7e53058283ba", @"/Views/_ViewImports.cshtml")]
    public class Views_System_ParentScopedRules : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/bootstrap/css/bootstrap.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/jquery/jquery-3.3.1.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("include", "Development", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("exclude", "Development", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/jquery/dist/core.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 2 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\ParentScopedRules.cshtml"
  
    ViewData["Title"] = "合并规则管理";
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<style>
    #stage :first-child {
        height: 18px;
        width: 530px;
        border-radius: 25px;
    }

    #First .el-slider__bar {
        background-color: #F56C6C;
        height: 16px;
    }

    #First .el-slider__button {
        background-color: #F56C6C;
        border: 2px solid #F56C6C;
        margin-top: 10px;
        width: 25px;
        height: 13px;
    }

    #First .el-slider__runway {
        height: 17px;
    }


    #Soncend .el-slider__bar {
        background-color: #E6A23C;
        height: 16px;
    }

    #Soncend .el-slider__button {
        background-color: #E6A23C;
        border: 2px solid #E6A23C;
        margin-top: 10px;
        width: 25px;
        height: 13px;
    }

    #Soncend .el-slider__runway {
        height: 17px;
    }

    #three .el-slider__bar {
        background-color: #67C23A;
        height: 16px;
    }

    #three .el-slider__runway {
        height: 17px;
    }

    #three .el-slider__button {
 ");
            WriteLiteral("       background-color: #67C23A;\r\n        border: 2px solid #67C23A;\r\n        margin-top: 10px;\r\n        width: 25px;\r\n        height: 13px;\r\n    }\r\n</style>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "21ef354b5b32364c1358f2c549bdd40aeed37c4e6876", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<div id=""MergeRuleManage""
     v-loading=""BeginBeginMergeDataloading""
     element-loading-text=""正在执行中，请稍等...."">

    <el-row style=""margin: 8px 0px;"">
        <el-col :span=""24"">

            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""ModelName"" placeholder=""请选择模型"" ");
            WriteLiteral(@"@change=""ModelHasChanged"">
                <el-option v-for=""item in ModelSelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                </el-option>
            </el-select>

            <el-select size=""small"" style=""margin-left: 2%;width:20%"" v-model=""EntityName"" placeholder=""请选择实体"" ");
            WriteLiteral("@change=\"EntityHasChanged\">\r\n                <el-option v-for=\"item in EntitySelectOptions\" :key=\"item.id\" :label=\"item.name\" :value=\"item.id\">\r\n                </el-option>\r\n            </el-select>\r\n            <el-button size=\"small\" ");
            WriteLiteral(@"@click=""OpenRuleBaseDialog"" type=""primary"" circle title=""新建"" icon=""el-icon-plus"" style=""margin-right:20px;float:right;margin-top:0.3%""></el-button>

        </el-col>
    </el-row>
    <el-row>
        <el-table :data=""RuleBaseTableData""
                  :height=""tableHeight""
                  :header-cell-style=""headerClass""
                  style=""width: 100%""
                  v-loading.fullscreen.lock=""RuleBaseTabLoading""
                  element-loading-text=""加载中......""
                  element-loading-background=""rgba(255, 255, 255, 0.75)""
                  ref=""RuleBaseTableData"">
            <el-table-column label=""序号""
                             align=""center""
                             width=""80"">
                <template scope=""scope"">
                    <span>{{scope.$index+(pagenum - 1) * pagesize + 1}} </span>
                </template>
            </el-table-column>
            <el-table-column prop=""modelName""
                             label=""模型""
            ");
            WriteLiteral(@"                 width=""100""
                             align=""center""
                             show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""entityName""
                             label=""实体""
                             align=""center""
                             width=""100""
                             show-overflow-tooltip>
            </el-table-column>
            <el-table-column prop=""cleaningRules""
                             label=""清洗规则""
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
   ");
            WriteLiteral(@"                          width=""200""
                             align=""center""
                             label=""创建时间"">
            </el-table-column>
            <el-table-column prop=""state""
                             width=""100""
                             label=""状态""
                             align=""center""
                             show-overflow-tooltip>
                <template scope=""scope"">
                    <el-button size=""small"" v-if=""scope.row.state=='执行'"" class=""button_button"" type=""primary"" plain title=""执行"">{{scope.row.state}}</el-button>
                    <el-button size=""small"" v-if=""scope.row.state=='空闲'"" class=""button_button"" type=""success"" plain title=""空闲"">{{scope.row.state}}</el-button>
                </template>
            </el-table-column>
            <el-table-column label=""操作"" width=""300"">
                <template scope=""scope"">
                    <el-button size=""small"" class=""button_button"" plain circle ");
            WriteLiteral("@click=\"BeginMergeData(scope.$index, scope.row)\" type=\"primary\" title=\"执行\" icon=\"el-icon-monitor\"></el-button>\r\n                    <el-button size=\"small\" class=\"button_button\" plain ");
            WriteLiteral("@click=\"OpenRuleBaseEditDialog(scope.$index, scope.row)\" circle type=\"primary\" title=\"编辑规则\" icon=\"el-icon-edit\"></el-button>\r\n                    <el-button size=\"small\" class=\"button_button\" plain circle type=\"primary\" ");
            WriteLiteral("@click=\"SetTheThresholddialog(scope.row)\" title=\"编辑阀值\" icon=\"el-icon-key\"></el-button>\r\n                    <el-button size=\"small\" class=\"button_button\" plain ");
            WriteLiteral("@click=\"ResultHandlingdialogVisible=true\" circle type=\"primary\" title=\"查看\" icon=\"el-icon-view\"></el-button>\r\n                    <el-button size=\"small\" plain type=\"danger\" title=\"删除\" circle icon=\"el-icon-delete\" ");
            WriteLiteral("@click=\"DelRuleBase(scope.row)\"></el-button>\r\n                </template>\r\n            </el-table-column>\r\n        </el-table>\r\n");
            WriteLiteral("        <el-pagination background\r\n                       style=\"text-align: center;\"\r\n                       ");
            WriteLiteral("@size-change=\"handleSizeChange\"\r\n                       ");
            WriteLiteral(@"@current-change=""handleCurrentChange""
                       :current-page=""pagenum""
                       :page-size=""pagesize""
                       :page-sizes=""[pageMaxsize,20,50]""
                       layout=""total, sizes, prev, pager, next, jumper""
                       :total=""total"">
        </el-pagination>
    </el-row>


");
            WriteLiteral(@"    <el-dialog :title=""Title""
               :visible.sync=""AddRuleBasedialogVisible""
               width=""30%""
               height=""80%""
               :append-to-body=""true""
               :close-on-click-modal=""false""
               :close-on-press-escape=""false""
               ");
            WriteLiteral(@"@close=""ClosedAddRuleBasedialog"">
        <el-form label-position=""left"" style=""height:350px"" :model=""RuleBaseForm"" ref=""RuleBaseForm"" :rules=""RuleBaseRules"" label-width=""58px"">
            <el-row style=""height:12%"">

                <el-button size=""mini"" ");
            WriteLiteral(@"@click=""AddRuleBase"" type=""primary"" circle title=""新建"" icon=""el-icon-plus"" style=""margin-right:20px;float:right;margin-top:0.3%""></el-button>
            </el-row>
            <div style=""height:250px;overflow:hidden;overflow-y:scroll"">
                <el-row v-for=""items in RulesOfTheData"" style=""margin-top:5px;"">
                    <el-col :offset=""1"" :span=""9"">
                        属性
                        <el-select maxlength=""50"" style=""width:70%"" show-word-limit ");
            WriteLiteral(@"@change=""AttributeNameDialogChange($event,items.Index)"" v-model=""items.AttributeName"" placeholder=""请选择模型"">
                            <el-option v-for=""item in AttributeSelectOptions"" :key=""item.id"" :label=""item.name"" :value=""item.id"">
                            </el-option>
                        </el-select>

                    </el-col>
                    <el-col :span=""12"">
                        <el-select style=""width:40%"" v-model=""items.AttributeValue"" ");
            WriteLiteral(@"@change=""AttributeNameChange($event,items.Index)"" placeholder=""请选择"">
                            <el-option value=""权重"">
                                权重
                            </el-option>
                            <el-option value=""分组"">
                                分组
                            </el-option>
                        </el-select>
                        <span v-if=""items.WeightIsShow""><el-input-number size=""medium"" style=""width:50%"" v-model=""items.Weight"" style=""width:70%"" ");
            WriteLiteral(@"@change=""NumberhandleChange"" controls-position=""right"" :min=""0"" :max=""100""></el-input-number>%</span>

                    </el-col>
                    <el-col :offset=""0.2"" :span=""2"">
                        <el-button style=""margin-top: 7px;"" size=""mini"" plain type=""danger"" title=""删除"" circle icon=""el-icon-delete"" ");
            WriteLiteral(@"@click=""DelRule(items)""></el-button>
                    </el-col>
                </el-row>

            </div>
            <div style=""margin-left:5%""> 注:权重值和必须为100%</div>
            <div class=""dialog_footer_btn"">
                <el-button size=""small"" type=""primary"" ");
            WriteLiteral("@click=\"SubmitRuleBaseForm(\'RuleBaseForm\')\">提交</el-button>\r\n                <el-button size=\"small\" ");
            WriteLiteral("@click=\"CloseRuleBasedialogVisible(\'RuleBaseForm\')\">取消</el-button>\r\n            </div>\r\n\r\n        </el-form>\r\n    </el-dialog>\r\n\r\n");
            WriteLiteral("    <el-dialog title=\"设置合并阀值\"\r\n               :visible.sync=\"SetTheThresholddialogVisible\"\r\n               width=\"30%\"\r\n               :append-to-body=\"true\"\r\n               :close-on-click-modal=\"false\"\r\n               ");
            WriteLiteral(@"@close=""ClosedAddRuleBasedialog"">

        <el-row style=""height:230px !important"">
            <el-row style=""margin-top:50px;"">
                <el-col :span=""8"">
                    <span style=""font-size:20px"">手动合并阀值</span>
                </el-col>
                <el-col :span=""8"">
                    <span style=""margin-left:20px;font-size:20px"">{{slider}}%</span>
                </el-col>
            </el-row>
            <el-row>
                <el-col :span=""24"" style=""margin-top:60px"">
                    <template>
                        <div class=""block"">
                            <el-slider ");
            WriteLiteral("@change=\"SliderChange\" v-model=\"slider\">\r\n\r\n                            </el-slider>\r\n                        </div>\r\n                    </template>\r\n\r\n                </el-col>\r\n            </el-row>\r\n");
            WriteLiteral("\r\n        </el-row>\r\n        <div class=\"dialog_footer_btn\">\r\n            <el-button size=\"small\" type=\"primary\" ");
            WriteLiteral("@click=\"ChangeThreshold\">提交</el-button>\r\n            <el-button size=\"small\" ");
            WriteLiteral("@click=\"SetTheThresholddialogVisible=false\">取消</el-button>\r\n        </div>\r\n\r\n    </el-dialog>\r\n");
            WriteLiteral("    <el-dialog title=\"处理结果\"\r\n               :visible.sync=\"ResultHandlingdialogVisible\"\r\n               width=\"30%\"\r\n               height=\"80%\"\r\n               :append-to-body=\"true\"\r\n               :close-on-click-modal=\"false\"\r\n               ");
            WriteLiteral(@"@close=""ClosedAddRuleBasedialog"">
        <el-row>
            <el-col :span=""18"" :offset=""4"" style=""background-color:#67C23A;height:100px;width:300px;border-radius:10px;margin-top:10px"">
                <el-row>
                    <el-col :span=""10"">
                        <span style=""color:#F2F2F2;font-size:18px;margin-left:10px"">已合并</span>
                    </el-col>
                </el-row>
                <el-row>
                    <span style=""color:#F2F2F2;margin-left:35%;font-size:35px"">1299</span>
                </el-row>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span=""15"" :offset=""4"" style=""background-color:#E6A23C;height:100px;width:300px;border-radius:10px;margin-top:10px"">
                <el-row>
                    <el-col :span=""10"">
                        <span style=""color:#F2F2F2;font-size:18px;margin-left:10px"">需手动</span>
                    </el-col>
                </el-row>
                <el-row>
                    <s");
            WriteLiteral(@"pan style=""color:#F2F2F2;margin-left:35%;font-size:35px"">157</span>
                </el-row>
            </el-col>
            <el-col :span=""3"" :offset=""1"" style=""margin-top:50px"">
                <el-button round icon=""el-icon-bottom""></el-button>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span=""15"" :offset=""4"" style=""background-color:#DCDFE6;height:100px;width:300px;border-radius:10px;margin-top:10px"">
                <el-row>
                    <el-col :span=""10"">
                        <span style=""color:#7F7F7F;font-size:18px;margin-left:10px"">未处理</span>
                    </el-col>
                </el-row>
                <el-row>
                    <span style=""color:#7F7F7F;margin-left:35%;font-size:35px"">166</span>
                </el-row>
            </el-col>
            <el-col :span=""3"" :offset=""1"" style=""margin-top:50px"">
                <el-button round icon=""el-icon-bottom""></el-button>
            </el-col>
        </el-row>
   ");
            WriteLiteral(" </el-dialog>\r\n");
            WriteLiteral("\r\n\r\n\r\n\r\n</div>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21ef354b5b32364c1358f2c549bdd40aeed37c4e21440", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21ef354b5b32364c1358f2c549bdd40aeed37c4e22480", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21ef354b5b32364c1358f2c549bdd40aeed37c4e22750", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 16495, "~/js/dev/ParentScopedRules.js?v=", 16495, 32, true);
#nullable restore
#line 368 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\ParentScopedRules.cshtml"
AddHtmlAttributeValue("", 16527, DateTime.Now.Millisecond, 16527, 25, false);

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
            __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper.Include = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("environment", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21ef354b5b32364c1358f2c549bdd40aeed37c4e25279", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21ef354b5b32364c1358f2c549bdd40aeed37c4e25549", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 16635, "~/js/dist/ParentScopedRules.js?v=", 16635, 33, true);
#nullable restore
#line 371 "F:\FISK\project\MDM\Fisk.MDMSolustion\Fisk.MDMSolustion\Views\System\ParentScopedRules.cshtml"
AddHtmlAttributeValue("", 16668, DateTime.Now.Millisecond, 16668, 25, false);

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
            __Microsoft_AspNetCore_Mvc_TagHelpers_EnvironmentTagHelper.Exclude = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "21ef354b5b32364c1358f2c549bdd40aeed37c4e28079", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
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