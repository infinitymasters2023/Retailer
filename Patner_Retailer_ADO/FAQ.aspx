<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="Patner_Retailer_ADO.FAQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // FAQ toggle
            document.querySelectorAll('.faq-question').forEach(q => {
                q.addEventListener('click', () => {
                    const parent = q.parentElement;
                    parent.classList.toggle('active');
                    q.querySelector('span').textContent = parent.classList.contains('active') ? '-' : '+';
                });
            });
        });
    </script>

    <style>
        .faq {
            padding: 0;
        }

        .faq-item {
            background: #f7f7f7;
            border-radius: 10px;
            margin-bottom: 15px;
            overflow: hidden;
        }

        .faq-question {
            padding: 15px 20px;
            cursor: pointer;
            font-weight: 600;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .faq-answer {
            max-height: 0;
            overflow: hidden;
            transition: max-height 0.4s ease, padding 0.3s ease;
            padding: 0 20px;
        }

        .faq-item.active .faq-answer {
            max-height: 100%;
            padding: 0px 20px 15px 20px;
        }

        .faq-answer ul {
            padding: 0px 35px;
            margin-bottom: 15px;
        }

            .faq-answer ul li {
                font-size: 13px;
                line-height: 20px;
                margin-bottom: 7px;
            }

        .faq-answer p {
            font-size: 13px;
            margin-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card p-4">
        <section class="faq">
            <div class="container-fluid">
                <div class="section-title">
                    <h2>Frequently Asked Questions</h2>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q1. How does InfyShield Partner Program work?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: Become a member of InfyShield partner program by clicking Login/Signup button above. Sell eligible plans to customers and earn instant commission on each sale. Leave customer support on us.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q2. How long does it take to become InfyShield Partner?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: Our partner onboarding process is easy and hassle free. Online form requires minimum and Basic information for registration. Post registration partner association approval is processed within 24 Hrs followed by legal agreement signup.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q3. How will commissions be settled?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: We work on instant commission settlement. You will collect full amount from customers; Retain your commission and make balance payment to us.</p>
                        <ul>
                            <li>Your additional benefits will settle Monthly/Bimonthly basis.</li>
                            <li>Salesperson commission, Additional benefits and special incentives will be settled Monthly/Bi-monthly</li>
                        </ul>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q4. How do I know which of products are eligible for protection plan sales?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: Our “Sell InfyShield Plans” tab on your login will indicate what products and plans are eligible for InfyShield Protection Plans.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q5. What should I consider before selling any InfyShield Service Plan?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: This is an important question. Here's what you should consider:</p>
                        <ul>
                            <li>Review whether customer actually need a service plan e.g. you should check whether customer product is covered under some other service plan or insurance.</li>
                            <li>Short term service plans are costlier. For example, a one-year extended service plan and renewing it for another year will be costlier than a 2-year Extended Warranty Service Plan. So, guide customers accordingly to purchase longer Extended Warranty Plans. </li>
                            <li>Be careful and guide all inclusions and exclusions to customers i.e. what is covered and what is not covered under selected InfyShield Service Plan.</li>
                            <li>Collect all required information, documents & product details and verify them physically before final sales.</li>
                        </ul>
                        <p>If you have any questions or concerns, you must share them with us. You should sell the Service Plans only after assuring yourself on sales.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q6. What if customer Change his mind post payment, or We sold un-eligible plan or Policy rejected by InfyShield.<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: Whatsoever reason if plan gets cancelled (Plan offer rejected by InfyShield / Customer made a cancellation post sales/Wrong sales by salesperson); Partners will have to refund full amount to customer and share acknowledgement slip with Infinity. We will reimburse the amount to partner which was paid to Infinity.</p>
                        <ul>
                            <li>Customers will have option to cancel his protection plan policy within 7 days of purchase. </li>
                        </ul>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q7. Do customer need to register InfyShield Service Plan.<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: Yes, registration is mandatory for us to know customer product description, customer contact details, location, warranty start & end dates etc. This ensures that we will be able to plan and equip ourselves to deliver him excellent service as and when customers need it.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q8. How can customer register his InfyShield Service Plan?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: <strong>“InfyVault”</strong> a self-help customer portal link will be shared with customers post plan sales to upload required documentation.</p>
                        <ul>
                            <li>Customer can also register his other product information to develop a respiratory where he can register service support by connecting relevant brand support number or Infinity customer care for service support. </li>
                        </ul>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q9. How can customer register claim?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans. Customers can register claim through “InfyVault” portal, or he can call to InfyShield Customer care for help. Partner can also register claim on behalf of customer by using InfyShield partner portal.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q10. When and how can customer avail service under InfyShield Service Plans?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: The whole objective of InfyShield Service Plans is to provide uninterrupted services to customer product in case it develops a defect or is accidentally damaged.</p>
                        <p>Services under InfyShield Extended Warranty Service Plan commence from the very next day when product manufacturer's warranty expires.</p>
                        <p>
                            Services under Accidental Damage Protection Plan commence from the date of purchase of product unless customer product is already covered for Accidental Protection by the manufacturer. In such a case the Accidental Damage Protection Plan shall also start from the very next day the manufacturer’s coverage expires.
                        </p>
                        <p>How to avail service -</p>
                        <ul>
                            <li>It is simple but before customer panic and call us, we recommend customers to check that the malfunctioning of your product is not due to external sources like network signals, incorrect settings, internet connectivity, improper power supply etc. This will help you resolve the problem quickly.</li>
                            <li>If your equipment still malfunctions, customer can contact our Service Desk as per details mentioned in the Contact Us section of our website or on the InfyShield Plan which he has purchased.</li>
                            <li>If customer equipment is damaged due to accidental damage, he can contact our Service Desk. A small service order charge is required to be paid after logging the service request. The service order charge is higher of Rs 500 or 10% of the product purchase price. After receiving service request, we will respond quickly to solve the problem through our call centre.</li>
                            <li>After receiving service request, we will respond quickly to solve the problem through our call centre. If the problem is not solved, we will depute our technician to visit customer (in case of On-site Service Plan) or customer will have to take his product along with a copy of proof of this plan to the nearest authorized service center (in case of Carry-in Service Plan).</li>
                        </ul>
                        <p>Please Note: Please guide customer to not to carry their product directly to any service centre whether or not authorized by us without prior approval of our Service Desk. This is to ensure he get services to the best of our efforts and updated contacts list.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q11. Customer have bought a product from outside India. Can we sell InfyShield Service Plans?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: We regret to inform you that InfyShield Service Plans can be purchased for products purchased in India only and those which have manufacturer's warranty coverage available in India. However, please collect the description of his product including make, model, serial number, product condition, customer contact details etc. in our Contact Form. We will contact once we have a solution for customer.</p>
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question">Q12. When does my first 3 months minimum 25% margin offer starts?<span style="font-size: 18px;">+</span></div>
                    <div class="faq-answer">
                        <p>Ans: Minimum 25% margin offer starts from date of InfyShield Partner association and profile approved for sales. It will be valid for 90 days and plan sold withing 90th days will be considered within this offer.</p>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
