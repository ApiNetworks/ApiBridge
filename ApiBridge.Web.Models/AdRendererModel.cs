using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiBridge.Web.MVC.ControlPanel.MockClasses;

namespace ApiBridge.Web.MVC.ControlPanel.Models
{
    public class AdRendererModel
    {
        public AdRendererModel()
        {
            InitAdSizeList();
            InitAdTypeList();
        }

        public string Height { get; set; }
        public string Width { get; set; }

        public List<SelectListItem> AdSizeList { get; set; }
        public string SelectedSize { get; set; }

        public List<SelectListItem> AdTypeList { get; set; }
        public string SelectedAdType { get; set; }

        public string WidgetSource { get; set; }

        void InitAdSizeList()
        {
            this.AdSizeList = new List<SelectListItem>();
            
            for (int i = 0; i < AdSizes.AdSizeArray.Length; i++)
            {
                AdSizeList.Add(new SelectListItem() { Text = AdSizes.AdSizeArray[i], Value = AdSizes.AdSizeArray[i] });
            }
            AdSizeList[0].Selected = true;
        }

        void InitAdTypeList()
        {
            AdTypeList = new List<SelectListItem>();

            AdTypeList.Add(new SelectListItem() { Text = AdTypes.ImageAd.ToString(), Value = ((int)AdTypes.ImageAd).ToString() });
            AdTypeList.Add(new SelectListItem() { Text = AdTypes.TextAd.ToString(), Value = ((int)AdTypes.TextAd).ToString() });

            AdTypeList[0].Selected = true;
        }
    }
}