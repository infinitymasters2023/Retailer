using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Patner_Retailer_ADO.PhonePay
{
    public partial class PhonePeTest : System.Web.UI.Page
    {
        public class MetaInfo
        {
            public string udf1 { get; set; }
            public string udf2 { get; set; }
            public string udf3 { get; set; }
            public string udf4 { get; set; }
            public string udf5 { get; set; }
        }

        public class PaymentDetail
        {
            public string paymentMode { get; set; }
            public string transactionId { get; set; }
            public long timestamp { get; set; }
            public int amount { get; set; }
            public string state { get; set; }
        }

        public class PhonePeStatusResponse
        {
            public string orderId { get; set; }
            public string state { get; set; }
            public int amount { get; set; }
            public long expireAt { get; set; }
            public MetaInfo metaInfo { get; set; }
            public List<PaymentDetail> paymentDetails { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Force TLS 1.2 for .NET 4.0
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string accessToken = GetPhonePeAccessToken();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    string result = InitiatePhonePePayment(accessToken, txtAmount.Text.Trim());
                    // lblResult.Text = result;

                    var serializer = new JavaScriptSerializer();
                    var responseObj = serializer.Deserialize<Dictionary<string, object>>(result);

                    // responseObj should contain redirectUrl key, but sometimes it is nested. 
                    // PhonePe returns something like { "success": true, "redirectUrl": "...", ...}
                    // Check if redirectUrl exists directly:

                    if (responseObj.ContainsKey("redirectUrl"))
                    {
                        string redirectUrl = responseObj["redirectUrl"] as string;

                        if (!string.IsNullOrEmpty(redirectUrl))
                        {
                            // Open redirect URL in client browser by redirecting the ASP.NET page:
                            Response.Redirect(redirectUrl);
                            return;
                        }
                    }

                    //  CheckPaymentStatus();
                }
                else
                {
                    lblResult.Text = "Failed to retrieve access token.";
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error: " + ex.Message;
            }
        }



        private string GetPhonePeAccessToken()
        {
            //  string url = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";
            string url = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";
            //   string postData = "client_id=TEST-M223N21JH5HYM_25071&client_version=1&client_secret=MmJkMWUzOWUtMDAzOS00YmRhLWIzYTgtN2U1MTVmYTljODhh&grant_type=client_credentials";
            string postData = "client_id=SU2508112228179045954860&client_version=1&client_secret=6549829c-f010-4e57-8b06-1c3ee2652b7a&grant_type=client_credentials";

            byte[] data = Encoding.UTF8.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                dynamic result = serializer.Deserialize<object>(json);
                if (result.ContainsKey("access_token"))
                {
                    return result["access_token"].ToString();
                }
                else
                {
                    return "access_token not found";
                }
            }

            //   return null;
        }

        private string InitiatePhonePePayment(string accessToken, string amount)
        {
            //  string url = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/pay";

            string url = "https://api.phonepe.com/apis/pg/checkout/v2/pay";

            string OrderID = "txn" + DateTime.Now.Ticks;
            Session["OrderID"] = OrderID;

            HttpContext.Current.Cache.Insert("OrderID", OrderID, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);

            var payload = new
            {
                merchantOrderId = OrderID,
                amount = Convert.ToInt32(amount) * 100,
                expireAfter = 1200,
                metaInfo = new
                {
                    udf1 = "test1",
                    udf2 = "param2",
                    udf3 = "test3",
                    udf4 = "value4",
                    udf5 = "ref5"
                },
                paymentFlow = new
                {
                    type = "PG_CHECKOUT",
                    message = "Payment test",
                    merchantUrls = new
                    {
                        redirectUrl = "https://retailer.infyshield.com/PhonePay/PaymentCallback.aspx"
                    }
                }
            };

            string jsonPayload = new JavaScriptSerializer().Serialize(payload);
            byte[] data = Encoding.UTF8.GetBytes(jsonPayload);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "O-Bearer " + accessToken);
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {

                return reader.ReadToEnd();
            }
        }
    }

}
