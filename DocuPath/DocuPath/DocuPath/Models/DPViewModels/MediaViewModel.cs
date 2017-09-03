using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.DPViewModels
{
    public class MediaViewModel
    {
        public MEDIA media = new MEDIA();
        public List<CONTENT_TAG> tags = new List<CONTENT_TAG>();        
    }

    public class AddMediaViewModel
    {
        
        public List<MEDIA> mediaList { get; set; }
        //public List<MEDIA_PURPOSE> purposeList = new List<MEDIA_PURPOSE>();
        //public List<FORENSIC_CASE> fcList = new List<FORENSIC_CASE>();
    }
    public class MediaPOSTViewModel
    {

        public List<MEDIA> mediaList = new List<MEDIA>();
    }
}