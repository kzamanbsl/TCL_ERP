using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KGERP.Service.ServiceModel
{
    public class BoqDivisionModel : BaseVM
    {
        public long BoqDivisionId { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }

        public List<BoQDivision> BoQDivisions { get; set; }
    }
}