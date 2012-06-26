using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiBridge.Web.MVC.ControlPanel.MockClasses;

namespace ApiBridge.Web.MVC.ControlPanel.Models
{
    public class LeadCaptureModel
    {
        public LeadCaptureModel()
        {
            InitFormTypeList();
        }

        public string Height { get; set; }
        public string Width { get; set; }

        public List<SelectListItem> FormTypeList { get; set; }
        public string SelectedFormType { get; set; }

        public string WidgetSource { get; set; }

        void InitFormTypeList()
        {
            this.FormTypeList = new List<SelectListItem>();
            string[] enumTypes = Enum.GetNames(typeof(FormTypeEnumeration.FormType));
            int[] enumVals = (int[])Enum.GetValues(typeof(FormTypeEnumeration.FormType));
            List<SelectListItem> formTypeVals = new List<SelectListItem>();

            for (int i = 0; i < enumTypes.Length; i++)
            {
                FormTypeList.Add(new SelectListItem { Text = enumTypes[i], Value = enumVals[i].ToString() });
            }
            
            FormTypeList[0].Selected = true;
        }
    }
}