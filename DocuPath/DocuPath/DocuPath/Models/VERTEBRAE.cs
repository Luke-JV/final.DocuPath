using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models
{
    public static class VERTEBRAE
    {
        public static List<NOTIFICATION> GetUnhandledNeurons()
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
                List<NOTIFICATION> unhandledNeurons = new List<NOTIFICATION>();

                foreach (var neuron in db.NOTIFICATION)
                {
                    unhandledNeurons.Add(neuron);
                }

                return unhandledNeurons;
            }
        }

        //public static List<METRIC> FetchVisionMetrics()
        //{
                // TODO: Compile a list of metrics for use in VISION dashboard.
                // Unsure what to do to have METRIC show up as an option when defining method return type... did this too long ago.
        //}
    }
}