using System;
using System.Collections.Generic;
using PayPal.Api;

namespace cfunding.Services
{
    public class PayPalPaymentService
    {
        public int projectId { get; set;}
        public string pname { get; set;}
        public string donationAmount {get; set;}

        public PayPal.Api.APIContext apiContext ;
        public PayPal.Api.Payment payment;
        public static Payment ExecutePayment(string paymentId, string payerId)
        {
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            var apiContext = PayPalConfiguration.GetAPIContext();

            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            var payment = new Payment() { id = paymentId };

            // Execute the payment.
            var executedPayment = payment.Execute(apiContext, paymentExecution);
            return executedPayment;
        }

        public Payment CreatePayment(string baseUrl, string intent)
        {
            var apiContext = PayPalConfiguration.GetAPIContext();
            
            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = baseUrl + "PaymentCancelled",
                return_url = baseUrl + "PaymentSuccessfull"
            };

           
            var transactionList = new List<Transaction>();
            Double donationDouble = Double.Parse(donationAmount);
            donationDouble = Math.Round(donationDouble, 2, MidpointRounding.AwayFromZero);
            string strDonation = donationDouble.ToString();

            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Payment for project "+pname,
                invoice_number = Convert.ToString((new Random()).Next(100000)), //Generate an Invoice No
                amount = new Amount()
                {
                    currency = "USD",
                    total = strDonation, // Total must be equal to sum of tax, shipping and subtotal.
                    details = new Details(){
                        tax = "0",
                        shipping = "0",
                        subtotal = strDonation
                    }
                }
                ,
                item_list = new ItemList()
                {
                    items = new List<Item>()
                    {
                        new Item()
                        {
                            name = pname + " payment",
                            currency = "USD",
                            price = strDonation,
                            quantity = "1",
                            sku = "sku"
                        }
                    }
                }
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = new Payer() { payment_method = "paypal" },
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }


    }
}