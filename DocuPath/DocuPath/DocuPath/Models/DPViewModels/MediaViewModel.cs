﻿using DocuPath.DataLayer;
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
}