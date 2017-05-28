using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnnMvcAjaxHandler
{
   public class AjaxHandlerOption
    {
        public string TargetFormId { get; set; }
        public int ClickedItemId { get; set; }
        public string LoadingElementId { get; set; }
        public string UpdateElementId { get; set; }
        public string OnSuccess { get; set; }
        public string OnConfirm { get; set; }
        public string FadeColor { get; set; }
    }
}
