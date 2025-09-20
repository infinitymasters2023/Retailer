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
    public partial class PaymentCallback : System.Web.UI.Page
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

            lblMessage.Text = "Payment completed. Please verify transaction using the /status API.";
            Session["OrderID"] = HttpContext.Current.Cache["OrderID"].ToString();
            CheckPaymentStatus();
        }




        private void CheckPaymentStatus()
        {

            string url = "https://api.phonepe.com/apis/pg/checkout/v2/order/" + Session["OrderID"].ToString() + "/status";

            // string url = "https://api-preprod.phonepe.com/apis/pg-sandbox/checkout/v2/order/" + Session["OrderID"].ToString() + "/status";

            string authorizationToken = GetPhonePeAccessToken();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("Authorization", "O-Bearer " + authorizationToken);
            request.ContentType = "application/json";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    // lblResult.Text = result;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    PhonePeStatusResponse statusResponse = serializer.Deserialize<PhonePeStatusResponse>(result);

                    StringBuilder sb = new StringBuilder();

                    sb.Append("Order ID: " + statusResponse.orderId + "<br/>");
                    sb.Append("State: " + statusResponse.state + "<br/>");
                    sb.Append("Amount: " + statusResponse.amount + "<br/>");
                    sb.Append("Expire At: " + statusResponse.expireAt + "<br/><br/>");

                    // Meta Info
                    sb.Append("<b>Meta Info:</b><br/>");
                    sb.Append("UDF1: " + statusResponse.metaInfo.udf1 + "<br/>");
                    sb.Append("UDF2: " + statusResponse.metaInfo.udf2 + "<br/>");
                    sb.Append("UDF3: " + statusResponse.metaInfo.udf3 + "<br/>");
                    sb.Append("UDF4: " + statusResponse.metaInfo.udf4 + "<br/>");
                    sb.Append("UDF5: " + statusResponse.metaInfo.udf5 + "<br/><br/>");

                    // Payment Details
                    sb.Append("<b>Payment Details:</b><br/>");
                    foreach (var payment in statusResponse.paymentDetails)
                    {
                        sb.Append("Payment Mode: " + payment.paymentMode + "<br/>");
                        sb.Append("Transaction ID: " + payment.transactionId + "<br/>");
                        sb.Append("Timestamp: " + payment.timestamp + "<br/>");
                        sb.Append("Amount: " + payment.amount/100 + "<br/>");
                        sb.Append("State: " + payment.state + "<br/><br/>");
                    }

                    lblMessage.Text = sb.ToString();




                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        Response.Write("<pre>Error: " + Server.HtmlEncode(errorResponse) + "</pre>");
                    }
                }
                else
                {
                    Response.Write("<pre>Error: " + Server.HtmlEncode(ex.Message) + "</pre>");
                }
            }
        }
        private string GetPhonePeAccessToken()
        {
            //  string url = "https://api-preprod.pho   nepe.com/apis/pg-sandbox/v1/oauth/token";
            //   string postData = "client_id=TEST-M223N21JH5HYM_25071&client_version=1&client_secret=MmJkMWUzOWUtMDAzOS00YmRhLWIzYTgtN2U1MTVmYTljODhh&grant_type=client_credentials";

            string url = "https://api.phonepe.com/apis/identity-manager/v1/oauth/token";
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
    }
}