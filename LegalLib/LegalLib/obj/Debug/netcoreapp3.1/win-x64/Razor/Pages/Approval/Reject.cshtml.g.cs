#pragma checksum "C:\Users\ANDREYAPRADINATA\source\repos\LegalLib\LegalLib\Pages\Approval\Reject.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ee10161ebb731d585e23645b29c7ccb6274ef06f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(LegalLib.Pages.Approval.Pages_Approval_Reject), @"mvc.1.0.razor-page", @"/Pages/Approval/Reject.cshtml")]
namespace LegalLib.Pages.Approval
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
#line 1 "C:\Users\ANDREYAPRADINATA\source\repos\LegalLib\LegalLib\Pages\_ViewImports.cshtml"
using LegalLib;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemMetadataAttribute("RouteTemplate", "{id:int}")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ee10161ebb731d585e23645b29c7ccb6274ef06f", @"/Pages/Approval/Reject.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"903326c937d1a6b242ecf50e65ef25de2ffd65c7", @"/Pages/_ViewImports.cshtml")]
    public class Pages_Approval_Reject : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\ANDREYAPRADINATA\source\repos\LegalLib\LegalLib\Pages\Approval\Reject.cshtml"
  
    ViewData["Title"] = "Reject";
    ViewData["URL"] = Model.Configuration["Setting:ExternalURL"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Reject</h1>\r\n");
#nullable restore
#line 9 "C:\Users\ANDREYAPRADINATA\source\repos\LegalLib\LegalLib\Pages\Approval\Reject.cshtml"
Write(ViewData["URL"]);

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<LegalLib.RejectModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<LegalLib.RejectModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<LegalLib.RejectModel>)PageContext?.ViewData;
        public LegalLib.RejectModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
