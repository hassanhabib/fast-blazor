using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentAccordion
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public ExpandMode? ExpandMode { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}