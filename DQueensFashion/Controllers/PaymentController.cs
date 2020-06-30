using DQueensFashion.Core.Model;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public PaymentController(IOrderService orderService,IProductService productService, ICustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
        }

        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class
                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View(nameof(Failed));
                    }
                }

            }
            catch (Exception ex)
            {
                return View(nameof(Failed));
            }
            try
            {
                Session["cart"] = null;
                var paymentIdGuid = Request.Params["guid"];
                var _payment = PayPal.Api.Payment.Get(apiContext, Session[paymentIdGuid] as string);
                List<LineItem> lineItems = new List<LineItem>();
                lineItems = _payment.transactions.FirstOrDefault()
                    .item_list.items.Select(item => new LineItem()
                    {
                        Product = _productService.GetProductById(Int32.Parse(item.sku)),
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        IsDeleted = false,
                        Quantity = Int32.Parse(item.quantity),
                        UnitPrice = Int32.Parse(item.price),
                        TotalAmount = Decimal.Parse(item.price) * Int32.Parse(item.quantity),
                        Description= item.description,
                    }).ToList();

                DQueensFashion.Core.Model.Order order = new DQueensFashion.Core.Model.Order()
                {
                    CustomerId =customer.Id,
                    LineItems = lineItems,
                    TotalAmount = lineItems.Sum(l => l.TotalAmount),
                    TotalQuantity = lineItems.Sum(l => l.Quantity),
                    OrderStatus = OrderStatus.Processing,
                };

                _orderService.CreateOrder(order);

            }
            catch (Exception ex)
            {
                   //
            }
            //on successful payment, show success page to user.
            return RedirectToAction(nameof(Success));

        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            Random rand = new Random();
            ViewCartViewModel carts = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
            };

            //create itemlist and add item objects to it
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            itemList.items = carts.Carts
                 .Select(item => new Item()
                 {
                     name=item.Product.Name,
                     currency = "USD",
                     price = item.UnitPrice.ToString(),
                     quantity= item.Quantity.ToString(),
                     sku = item.Product.Id.ToString(),
                     description = item.Description,
                 }).ToList();

            //payer info
            var payerInfo = new PayerInfo()
            {

            };

            var payer = new Payer()
            {
                payment_method = "paypal",
            };
            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = carts.SubTotal.ToString(),
            };
            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = carts.SubTotal.ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = GenerateInvoiceNumber(),//Generate an Invoice No
                amount = amount,
                item_list = itemList,
                
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls,
                
            };
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
        
        private string GenerateInvoiceNumber()
        {
            return _orderService.GetOrderCount().ToString()
                    + "-" + DateTime.Now.ToString("yyyymmddhhmmssfff");
        }
        private string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
        }

        private Customer GetLoggedInCustomer()
        {
            var userId = GetLoggedInUserId();
            return _customerService.GedCustomerByUserId(userId);
        }

    }
}