﻿using KGERP.Data.Models;
using KGERP.Service.Implementation.Configuration;
using KGERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KGERP.Service.ServiceModel
{
    public class ChequeRegisterModel : BaseVM
    {
        public long ChequeRegisterId { get; set; }
        public int RegisterFor { get; set; }
        public int RequisitionId { get; set; }
        public string RequisitionNo { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ChequeDate { get; set; }
        public int ChequeNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime ClearingDate { get; set; }
        public string PayTo { get; set; }
        public bool IsSigned { get; set; }
        public SelectList RegisterForList { get; set; } = new SelectList(Enum.GetValues(typeof(EnumChequeRegisterFor)).Cast<EnumChequeRegisterFor>().Select(e => new SelectListItem { Text = e.ToString(), Value = ((int)e).ToString() }), "Value", "Text");
        public SelectList RequisitionList { get; set; } = new SelectList(new List<object>());
        public SelectList ProjectList { get; set; } = new SelectList(new List<object>());
        public SelectList SupplierList { get; set; } = new SelectList(new List<object>());
        public IEnumerable<ChequeRegisterModel> chequeRegisterList { get; set; } = new List<ChequeRegisterModel>();
    }
}
