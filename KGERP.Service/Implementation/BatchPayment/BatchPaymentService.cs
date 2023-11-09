using KGERP.Data.Models;
using KGERP.Service.Interface;
using KGERP.Service.ServiceModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using KGERP.Utility;
using KGERP.Service.Implementation.Accounting;
using KGERP.Service.Implementation.Configuration;
using KGERP.Service.Implementation.Procurement;

namespace KGERP.Service.Implementation.BatchPayment
{
    public class BatchPaymentService : IBatchPaymentService
    {
        private readonly ERPEntities context;
        private readonly AccountingService _accountingService;
        private readonly CollectionService _collectionService;
        private readonly ProcurementService _procurementService;
        public BatchPaymentService(ERPEntities context, AccountingService accountingService, CollectionService collectionService, ProcurementService procurementService)
        {
            this.context = context;
            _accountingService = accountingService;
            _collectionService = collectionService;
            _procurementService = procurementService;
        }

        #region Customer Batch Collection
        public async Task<int> CustomerBatchPaymentMasterAdd(BatchPaymentMasterModel model)
        {
            int result = -1;
            var bPaymentCount = context.BatchPaymentMasters.Count(c => c.CompanyId == model.CompanyId);
            #region Payment No


            if (bPaymentCount == 0)
            {
                bPaymentCount = 1;
            }
            else
            {
                bPaymentCount++;
            }



            string bPaymentNo = "BP-" + bPaymentCount.ToString().PadLeft(4, '0');

            #endregion
            BatchPaymentMaster batchPaymentMaster = new BatchPaymentMaster
            {
                BatchPaymentNo = bPaymentNo,
                TransactionDate = model.TransactionDate,
                BankCharge = model.BankCharge,
                PaymentToHeadGLId = model.Accounting_BankOrCashId,
                BankChargeHeadGLId = model.Accounting_BankOrCashParantId ?? 50613604,
                IsSubmitted = false,
                IsActive = true,

                CompanyId = (int)model.CompanyId,
                CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                CreatedDate = DateTime.Now,
            };

            context.BatchPaymentMasters.Add(batchPaymentMaster);
            if (await context.SaveChangesAsync() > 0)
            {
                result = batchPaymentMaster.BatchPaymentMasterId;
            }
            return result;
        }

        public async Task<int> CustomerBatchPaymentDetailAdd(BatchPaymentMasterModel model)
        {
            int result = -1;
            Vendor vendor = await context.Vendors.FindAsync(model.batchPaymentDetailModel.VendorId);
            if (vendor == null) throw new Exception("Sorry! Customer not found!");

            BatchPaymentDetail batchPaymentDetail = new BatchPaymentDetail
            {
                MoneyReceiptDate = model.batchPaymentDetailModel.MoneyReceiptDate,
                MoneyReceiptNo = model.batchPaymentDetailModel.MoneyReceiptNo,
                BatchPaymentMasterId = model.BatchPaymentMasterId,
                VendorId = vendor.VendorId,
                VendorTypeId = vendor.VendorTypeId,
                ReferenceNo = model.batchPaymentDetailModel.ReferenceNo,
                InAmount = model.batchPaymentDetailModel.InAmount,
                OutAmount = model.batchPaymentDetailModel.OutAmount,
                IsActive = true,

                CompanyId = (int)model.CompanyId,
                CreatedBy = System.Web.HttpContext.Current.Session["EmployeeName"].ToString(),
                CreatedDate = DateTime.Now,
            };

            context.BatchPaymentDetails.Add(batchPaymentDetail);

            if (await context.SaveChangesAsync() > 0)
            {
                result = batchPaymentDetail.BatchPaymentMasterId;
            }

            return result;
        }

        public async Task<int> CustomerBatchPaymentDetailEdit(BatchPaymentMasterModel model)
        {
            int result = -1;
            Vendor vendor = await context.Vendors.FindAsync(model.batchPaymentDetailModel.VendorId);
            if (vendor == null) throw new Exception("Sorry! Customer not found!");

            BatchPaymentDetail batchPaymentDetail = await context.BatchPaymentDetails.FindAsync(model.batchPaymentDetailModel.BatchPaymentDetailId);
            if (batchPaymentDetail == null) throw new Exception("Sorry! Batch Collection Detail not found!");


            batchPaymentDetail.MoneyReceiptDate = model.batchPaymentDetailModel.MoneyReceiptDate;
            batchPaymentDetail.MoneyReceiptNo = model.batchPaymentDetailModel.MoneyReceiptNo;
            batchPaymentDetail.BatchPaymentMasterId = model.BatchPaymentMasterId;
            batchPaymentDetail.VendorId = model.batchPaymentDetailModel.VendorId;
            batchPaymentDetail.VendorTypeId = vendor.VendorTypeId;
            batchPaymentDetail.ReferenceNo = model.batchPaymentDetailModel.ReferenceNo;
            batchPaymentDetail.InAmount = model.batchPaymentDetailModel.InAmount;
            batchPaymentDetail.OutAmount = model.batchPaymentDetailModel.OutAmount;
            batchPaymentDetail.IsActive = true;

            batchPaymentDetail.CompanyId = (int)model.CompanyId;
            batchPaymentDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            batchPaymentDetail.ModifiedDate = DateTime.Now;

            if (await context.SaveChangesAsync() > 0)
            {
                result = batchPaymentDetail.BatchPaymentMasterId;
            }

            return result;
        }

        public async Task<BatchPaymentMasterModel> GetCustomerBatchPaymentDetail(int companyId, int batchPaymentMasterId)
        {
            BatchPaymentMasterModel batchPaymentMasterModel = new BatchPaymentMasterModel();
            if (batchPaymentMasterId > 0)
            {
                batchPaymentMasterModel = await Task.Run(() => (from t1 in context.BatchPaymentMasters
                                                                join t2 in context.Companies on t1.CompanyId equals t2.CompanyId into t2_Join
                                                                from t2 in t2_Join.DefaultIfEmpty()
                                                                join t3 in context.HeadGLs on t1.PaymentToHeadGLId equals t3.Id into t3_join
                                                                from t3 in t3_join.DefaultIfEmpty()
                                                                join t4 in context.HeadGLs on t1.BankChargeHeadGLId equals t4.Id into t4_join
                                                                from t4 in t4_join.DefaultIfEmpty()
                                                                where t1.IsActive
                                                                    && t1.BatchPaymentMasterId == batchPaymentMasterId
                                                                    && t1.CompanyId == companyId

                                                                select new BatchPaymentMasterModel
                                                                {
                                                                    PaymentToHeadGLName = t3 != null ? t3.AccCode + " - " + t3.AccName : "",
                                                                    BankChargeHeadGLName = t3 != null ? t4.AccCode + " - " + t4.AccName : "",
                                                                    TransactionDate = t1.TransactionDate,
                                                                    BankCharge = t1.BankCharge,
                                                                    Accounting_BankOrCashId = t1.PaymentToHeadGLId,
                                                                    Accounting_BankOrCashParantId = t1.BankChargeHeadGLId,
                                                                    BatchPaymentMasterId = batchPaymentMasterId,
                                                                    CompanyId = companyId,
                                                                    IsSubmitted = t1.IsSubmitted,
                                                                    CreatedBy = t1.CreatedBy,
                                                                    CreatedDate = (DateTime)t1.CreatedDate,
                                                                    ModifiedBy = t1.ModifiedBy,
                                                                    ModifiedDate = t1.ModifiedDate,
                                                                    IsActive = t1.IsActive

                                                                }).FirstOrDefault());

                batchPaymentMasterModel.DetailList = await Task.Run(() => (from t1 in context.BatchPaymentDetails
                                                                           join t2 in context.BatchPaymentMasters on t1.BatchPaymentMasterId equals t2.BatchPaymentMasterId into t2_Join
                                                                           from t2 in t2_Join.DefaultIfEmpty()
                                                                           join t3 in context.Vendors on t1.VendorId equals t3.VendorId into t3_Join
                                                                           from t3 in t3_Join.DefaultIfEmpty()
                                                                           join t4 in context.SubZones on t3.SubZoneId equals t4.SubZoneId into t4_Join
                                                                           from t4 in t4_Join.DefaultIfEmpty()

                                                                           where t1.IsActive
                                                                      && t1.BatchPaymentMasterId == batchPaymentMasterId
                                                                      && t2.IsActive

                                                                           select new BatchPaymentDetailModel()
                                                                           {
                                                                               MoneyReceiptDate = t1.MoneyReceiptDate,
                                                                               MoneyReceiptNo = t1.MoneyReceiptNo,
                                                                               BatchPaymentMasterId = batchPaymentMasterId,
                                                                               VendorId = t3.VendorId,
                                                                               VendorName = t3.Name,
                                                                               InAmount = t1.InAmount,
                                                                               OutAmount = t1.OutAmount,
                                                                               ReferenceNo = t1.ReferenceNo,
                                                                               SubZoneFk = t4.SubZoneId,
                                                                               BatchPaymentDetailId = t1.BatchPaymentDetailId,
                                                                               CompanyId = t2.CompanyId,
                                                                               CreatedBy = t1.CreatedBy,
                                                                               CreatedDate = t1.CreatedDate,
                                                                               ModifiedBy = t1.ModifiedBy,
                                                                               ModifiedDate = t1.ModifiedDate,
                                                                               IsActive = t1.IsActive,
                                                                           }).OrderByDescending(x => x.BatchPaymentDetailId).AsEnumerable());

            }

            batchPaymentMasterModel.DataList = await Task.Run(() => (from t1 in context.BatchPaymentMasters
                                                                     join t2 in context.Companies on t1.CompanyId equals t2.CompanyId into t2_Join
                                                                     from t2 in t2_Join.DefaultIfEmpty()
                                                                     where t1.IsActive
                                                                           && t1.CompanyId == companyId
                                                                     select new BatchPaymentMasterModel
                                                                     {
                                                                         TransactionDate = t1.TransactionDate,
                                                                         BankCharge = t1.BankCharge,
                                                                         Accounting_BankOrCashId = t1.PaymentToHeadGLId,
                                                                         Accounting_BankOrCashParantId = t1.BankChargeHeadGLId,
                                                                         BatchPaymentMasterId = batchPaymentMasterId,
                                                                         CompanyId = companyId,
                                                                         IsSubmitted = t1.IsSubmitted,
                                                                         CreatedBy = t1.CreatedBy,
                                                                         CreatedDate = (DateTime)t1.CreatedDate,
                                                                         ModifiedBy = t1.ModifiedBy,
                                                                         ModifiedDate = t1.ModifiedDate,
                                                                         IsActive = t1.IsActive
                                                                     }).OrderByDescending(x => x.BatchPaymentMasterId).AsEnumerable());



            return batchPaymentMasterModel;
        }

        public async Task<BatchPaymentDetailModel> GetCustomerBatchPaymentDetailById(int id)
        {
            var batchPaymentDetailModel = await Task.Run(() => (from t1 in context.BatchPaymentDetails
                                                                join t2 in context.BatchPaymentMasters on t1.BatchPaymentMasterId equals t2.BatchPaymentMasterId into t2_Join
                                                                from t2 in t2_Join.DefaultIfEmpty()
                                                                join t3 in context.Vendors on t1.VendorId equals t3.VendorId into t3_Join
                                                                from t3 in t3_Join.DefaultIfEmpty()
                                                                join t4 in context.SubZones on t3.SubZoneId equals t4.SubZoneId into t4_Join
                                                                from t4 in t4_Join.DefaultIfEmpty()

                                                                where t1.IsActive
                                                             && t2.IsActive
                                                             && t1.IsActive
                                                             && t3.IsActive
                                                             && t1.BatchPaymentDetailId == id

                                                                select new BatchPaymentDetailModel
                                                                {
                                                                    MoneyReceiptDate = t1.MoneyReceiptDate,
                                                                    MoneyReceiptNo = t1.MoneyReceiptNo,
                                                                    BatchPaymentMasterId = t2.BatchPaymentMasterId,
                                                                    VendorId = t3.VendorId,
                                                                    VendorName = t3.Name,
                                                                    SubZoneFk = t4.SubZoneId,
                                                                    InAmount = t1.InAmount,
                                                                    OutAmount = t1.OutAmount,
                                                                    BatchPaymentDetailId = t1.BatchPaymentDetailId,
                                                                    ReferenceNo = t1.ReferenceNo,
                                                                    CompanyId = t2.CompanyId,
                                                                    CreatedBy = t1.CreatedBy,
                                                                    CreatedDate = t1.CreatedDate,
                                                                    ModifiedBy = t1.ModifiedBy,
                                                                    ModifiedDate = t1.ModifiedDate,
                                                                    IsActive = t1.IsActive,
                                                                }).FirstOrDefault());

            return batchPaymentDetailModel;
        }

        public async Task<int> SubmitCustomerBatchPayment(int batchPaymentMasterId)
        {
            int result = -1;

            if (batchPaymentMasterId <= 0) throw new Exception("Sorry! No Batch Payment Id found for submit!");

            var batchPaymentObj = context.BatchPaymentMasters.Include(bp => bp.BatchPaymentDetails).FirstOrDefault(x =>
                x.BatchPaymentMasterId == batchPaymentMasterId && x.IsActive && !x.IsSubmitted);
            if (batchPaymentObj == null) throw new Exception("Sorry! No Batch Payment found for Submit!");

            var vendorIds = batchPaymentObj.BatchPaymentDetails.Where(b => b.IsActive).Select(c => c.VendorId).ToList();
            List<Vendor> vendorsObj = context.Vendors
               .Where(x => x.CompanyId == batchPaymentObj.CompanyId && vendorIds.Contains(x.VendorId)).ToList();
          
            var orders = context.OrderMasters.Where(o => vendorIds.Contains((int)o.CustomerId) && o.IsActive == true && o.Status == (int)POStatusEnum.Submitted)
                .Include(d => d.OrderDetails).ToList();
            var orderMasterIds = orders.Select(o => o.OrderMasterId).ToList();

            List<PaymentMaster> orderPayments = context.PaymentMasters
                                .Where(pm => orderMasterIds.Contains((long)pm.Payments.FirstOrDefault(x => x.PaymentMasterId == pm.PaymentMasterId).OrderMasterId) && pm.IsActive)
                                .Include(pm => pm.Payments).ToList();
            
            var paymentMastersCount = context.PaymentMasters.Where(x =>
                       x.CompanyId == batchPaymentObj.CompanyId && vendorIds.Contains(x.VendorId))
                   .GroupBy(p => p.VendorId).Select(g => new
                   {
                       VendorId = g.Key,
                       PmCount = g.Count()
                   }).ToList();


            List<PaymentMaster> addablePaymentMasters = new List<PaymentMaster>();
            List<VendorDeposit> addableVendorDeposits = new List<VendorDeposit>();

            foreach (var vendorId in vendorIds)
            {
                decimal vendorCollAmount = 0;
                var vendorObj = vendorsObj.FirstOrDefault(x => x.VendorId == vendorId);
                var vendorOrders = orders.Where(x => x.CustomerId == vendorId).ToList();
                vendorCollAmount = batchPaymentObj.BatchPaymentDetails.Where(x => x.VendorId == vendorId && x.IsActive == true)
                    .Sum(b => b.InAmount);
                if (vendorCollAmount <= 0) throw new Exception("Sorry! No Customer Collection Amount Found!");

                var vpCount = 0;
                var slCount = 0;

                foreach (var vOrder in vendorOrders)
                {
                    if (vendorCollAmount <= 0) break;

                    decimal orderAmount = 0;
                    decimal paidAmount = 0;
                    decimal dueAmount = 0;

                    orderAmount = vOrder.OrderDetails.Sum(x => (decimal)x.Amount - x.DiscountAmount) + (decimal)vOrder.CourierCharge;
                    if (orderAmount <= 0) continue;

                    paidAmount = orderPayments.Where(m => m.VendorId == vendorId).Select(m =>
                            m.Payments.Where(d => d.OrderMasterId == vOrder.OrderMasterId).ToList()
                                .Sum(s => s.InAmount))
                        .Sum();

                    dueAmount = orderAmount - paidAmount;
                    if (dueAmount <= 0) continue;

                    #region Payment No

                    if (slCount == 0)
                    {
                        vpCount = paymentMastersCount.Count(x => x.VendorId == vendorId);
                        if (vpCount == 0)
                        {
                            vpCount = 1;
                        }
                        else
                        {
                            vpCount++;
                        }
                    }
                    else
                    {
                        vpCount++;
                    }


                    string paymentNo = "C-" + vendorObj?.Code + "-" + vpCount.ToString().PadLeft(4, '0');

                    #endregion

                    var batchPaymentDetail =
                        batchPaymentObj.BatchPaymentDetails.FirstOrDefault(x => x.VendorId == vendorId && x.IsActive == true);

                      PaymentMaster paymentMaster = new PaymentMaster
                        {
                            PaymentNo = paymentNo,
                            TransactionDate = batchPaymentObj.TransactionDate,
                            VendorId = vendorId,
                            VendorTypeId = vendorObj.VendorTypeId,
                            BatchPaymentDetailId = batchPaymentDetail.BatchPaymentDetailId,
                            MoneyReceiptNo = batchPaymentDetail.MoneyReceiptNo,
                            MoneyReceiptDate = batchPaymentDetail.MoneyReceiptDate,
                            ReferenceNo = batchPaymentDetail.ReferenceNo,
                            PaymentToHeadGLId = batchPaymentObj.PaymentToHeadGLId,
                            BankChargeHeadGLId = batchPaymentObj.BankChargeHeadGLId ?? 50613604, //BankCharge HeadGL Id
                            BankCharge = slCount == 0 ? (batchPaymentObj.BankCharge / vendorIds.Count()) : 0,
                            IsActive = true,
                            IsFinalized = true,

                            CompanyId = batchPaymentObj.CompanyId,
                            CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                            CreatedDate = DateTime.Now,
                            //ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name,
                            //ModifiedDate = DateTime.Now,

                        };

                        Payment payment = new Payment()
                        {
                            PaymentMasterId = paymentMaster.PaymentMasterId,

                            OutAmount = batchPaymentDetail.OutAmount,
                            ProductType = "R",
                            ReferenceNo = batchPaymentDetail.ReferenceNo,
                            TransactionDate = batchPaymentObj.TransactionDate,
                            MoneyReceiptNo = batchPaymentDetail.MoneyReceiptNo,
                            VendorId = batchPaymentDetail.VendorId,
                            OrderMasterId = vOrder.OrderMasterId,
                            PaymentFromHeadGLId = vendorObj.HeadGLId,
                            IsActive = true,

                            CompanyId = batchPaymentDetail.CompanyId,
                            CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                            CreatedDate = DateTime.Now

                        };

                        if (dueAmount > 0 && vendorCollAmount > 0 && (dueAmount - vendorCollAmount) >= 0)
                        {
                            payment.InAmount = vendorCollAmount;
                            vendorCollAmount = 0;
                        }
                        else if (dueAmount > 0 && vendorCollAmount > 0 && (dueAmount - vendorCollAmount) < 0)
                        {
                            payment.InAmount = dueAmount;
                            vendorCollAmount -= dueAmount;
                        }

                        paymentMaster.Payments.Add(payment);
                        addablePaymentMasters.Add(paymentMaster);
                    

                    slCount++;
                }

                if (vendorCollAmount > 0)
                {
                    VendorDeposit vendorDeposit = new VendorDeposit
                    {
                        VendorDepositId = 0,
                        VendorId = vendorId,
                        VendorTypeId = (int)ProviderEnum.Customer,
                        DepositDate = batchPaymentObj.TransactionDate,
                        DepositAmount = vendorCollAmount,
                        Description = "Deposit from batch payment, Payment No is " +
                                      batchPaymentObj.BatchPaymentNo,
                        BankCharge = 0,
                        PaymentToHeadGlId = batchPaymentObj.PaymentToHeadGLId,
                        BankChargeHeadGlId = batchPaymentObj.BankChargeHeadGLId ?? 50613604,
                        VoucherId = null,
                        IsActive = true,
                        IsSubmit = false,
                        CreateDate = DateTime.Now,
                        CreatedBy = System.Web.HttpContext.Current.User.Identity.Name,
                        CompanyId = vendorObj.CompanyId
                    };

                    addableVendorDeposits.Add(vendorDeposit);
                }

            }

            //using (var scope = context.Database.BeginTransaction())
            //{
            var paymentMastersResult = context.PaymentMasters.AddRange(addablePaymentMasters);
            var depositsResult = context.VendorDeposits.AddRange(addableVendorDeposits);

            var res1 = context.SaveChanges();

            #region Accounting Section

            List<VMPayment> vmPayments = new List<VMPayment>();
            if (addablePaymentMasters.Count() > 0)
            {
                foreach (var vm in addablePaymentMasters)
                {
                    var vmPayment = await _collectionService.ProcurementOrderMastersGetByID(batchPaymentObj.CompanyId, vm.PaymentMasterId);
                    vmPayments.Add(vmPayment);
                }

                // foreach (var vmPayment in vmPayments)
                // {
                await _accountingService.BatchPaymentsPush(batchPaymentObj.CompanyId, vmPayments, (int)GCCLJournalEnum.CreditVoucher);
                //}
            }


            // CustomerDeposit
            if (depositsResult?.Count() > 0)
            {
                foreach (var vd in depositsResult)
                {
                    var vdResult = _procurementService.CustomerDepositSubmit(vd.VendorDepositId);
                }
            }

            #endregion

            batchPaymentObj.IsSubmitted = true;
            batchPaymentObj.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
            batchPaymentObj.ModifiedDate = DateTime.Now;
            var res = await context.SaveChangesAsync();

            //    scope.Commit();
            //}

            result = batchPaymentMasterId;
            return result;

        }

        public async Task<int> CustomerBatchPaymentDetailDeleteById(int batchPaymentDetailId)
        {
            int result = -1;
            BatchPaymentDetail batchPaymentDetail = context.BatchPaymentDetails.FirstOrDefault(x => x.BatchPaymentDetailId == batchPaymentDetailId);
            if (batchPaymentDetail != null)
            {
                batchPaymentDetail.IsActive = false;
                batchPaymentDetail.ModifiedBy = System.Web.HttpContext.Current.User.Identity.Name;
                batchPaymentDetail.ModifiedDate = DateTime.Now;
                if (await context.SaveChangesAsync() > 0)
                {
                    result = batchPaymentDetail.BatchPaymentMasterId;
                }
            }
            return result;
        }

        public async Task<BatchPaymentMasterModel> GetCustomerBatchPaymentList(int companyId, DateTime? fromDate, DateTime? toDate)
        {
            BatchPaymentMasterModel batchPaymentMasterModel = new BatchPaymentMasterModel();
            batchPaymentMasterModel.DataList = await Task.Run(() => (from t1 in context.BatchPaymentMasters
                                                                     join t2 in context.Companies on t1.CompanyId equals t2.CompanyId into t2_Join
                                                                     from t2 in t2_Join.DefaultIfEmpty()
                                                                     join t3 in context.HeadGLs on t1.PaymentToHeadGLId equals t3.Id into t3_join
                                                                     from t3 in t3_join.DefaultIfEmpty()
                                                                     join t4 in context.HeadGLs on t1.BankChargeHeadGLId equals t4.Id into t4_join
                                                                     from t4 in t4_join.DefaultIfEmpty()
                                                                     where t1.IsActive
                                                                         && t1.CompanyId == companyId
                                                                             && t1.TransactionDate >= fromDate
                                                                             && t1.TransactionDate <= toDate
                                                                     select new BatchPaymentMasterModel
                                                                     {
                                                                         BatchPaymentMasterId = t1.BatchPaymentMasterId,
                                                                         PaymentToHeadGLName = t3 != null ? t3.AccCode + " - " + t3.AccName : "",
                                                                         BankChargeHeadGLName = t3 != null ? t4.AccCode + " - " + t4.AccName : "",
                                                                         TransactionDate = t1.TransactionDate,
                                                                         BankCharge = t1.BankCharge,
                                                                         TotalAmount = context.BatchPaymentDetails.Where(c => c.BatchPaymentMasterId == t1.BatchPaymentMasterId).Sum(c => c.InAmount),
                                                                         Accounting_BankOrCashId = t1.PaymentToHeadGLId,
                                                                         Accounting_BankOrCashParantId = t1.BankChargeHeadGLId,
                                                                         IsSubmitted = t1.IsSubmitted,
                                                                         IsActive = t1.IsActive,

                                                                         CompanyId = companyId,
                                                                         CreatedBy = t1.CreatedBy,
                                                                         CreatedDate = (DateTime)t1.CreatedDate,
                                                                         ModifiedBy = t1.ModifiedBy,
                                                                         ModifiedDate = t1.ModifiedDate

                                                                     }).OrderByDescending(x => x.BatchPaymentMasterId).AsEnumerable());

            return batchPaymentMasterModel;
        }

        #endregion


        #region Supplier Batch Payment

        // work on need

        #endregion
    }
}
