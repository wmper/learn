using System;
using System.Collections.Generic;
using System.Text;

namespace Example.ES
{
    public class Order
    {
        public string orderId { get; set; }
        public string no { get; set; }
        public string sellerId { get; set; }
        public string buyerId { get; set; }
        public string wholesaleOrderId { get; set; }
        public string orderNo { get; set; }
        public int directStatus { get; set; }
        public int payMode { get; set; }
        public int deliveryWay { get; set; }
        public int orderStatus { get; set; }
        public int onlinePayStatus { get; set; }
        public DateTime createTime { get; set; }
        public int status { get; set; }
        public int orderSource { get; set; }
        public int auxiliaryType { get; set; }
        public int isBooking { get; set; }
        public int customerServiceStatus { get; set; }
        public int abnormalReceiving { get; set; }
        public int goodsReceiptWay { get; set; }
        public int receivingReceiptStatus { get; set; }
        public int uploadSaleInvoice { get; set; }
        public int isInsteadCustomers { get; set; }
        public Wholesaleorder wholesaleOrder { get; set; }
        public Sku[] skus { get; set; }
        public int pushAccountStatus { get; set; }
        public string sourceStorageId { get; set; }
    }

    public class Wholesaleorder
    {
        public DateTime orderTime { get; set; }
    }

    public class Sku
    {
        public string skuName { get; set; }
        public string gbCode { get; set; }
        public string skuCode { get; set; }
        public string skuShortCode { get; set; }
        public string providerId { get; set; }
        public string providerName { get; set; }
    }
}
