<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Patner_Retailer_ADO.Index" %>

<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Retailer</title>
  <link href="https://fonts.googleapis.com/css2?family=Heebo:wght@100..900&family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet">
  <style>
    * { margin: 0; padding: 0; box-sizing: border-box; }
    body { font-family: "Poppins", sans-serif; font-size: 14px; line-height: 1.6; color: #333; }
    img { max-width: 100%; height: auto; display: block; }
    .container { max-width: 1200px; margin: 0 auto; padding: 0 20px; }

    /* Header */
    header {background: white;padding: 15px 0;box-shadow: 0 2px 10px rgba(0,0,0,0.1);position: fixed; width: 100%; top: 0; z-index: 1000;}
    .header-content { display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 15px; }
    .logo img { max-height: 42px; }
    .auth-btn {background: #22c55e; color: white;padding: 7px 15px;border-radius: 25px;font-weight: 400;border: none; text-decoration: none;transition: all .3s ease;font-size: .85rem;}
    .auth-btn svg {vertical-align: middle;}
    .auth-btn:hover { background: #16a34a; transform: translateY(-1px); }

    /* Hero */
    .hero {background: linear-gradient(135deg, #4338ca 0%, #6366f1 100%);color: white; padding: 40px 0 40px; margin-top: 70px;}
    .hero-content {display: grid; grid-template-columns: 1fr 1fr; gap: 40px; align-items: center;}
    .hero-text h1 {font-size: clamp(2rem, 5vw, 3em);font-weight: 700;margin-bottom: 20px;line-height: 1.2;}
    .hero-text p { font-size: 1.1rem; margin-bottom: 25px; }
    .hero-features { list-style: none; }
    .hero-features li { margin-bottom: 12px; font-size: 1rem; display: flex; align-items: center; }
    .hero-features li::before {content: "✓"; background: #22c55e; color: #fff; width: 22px; height: 22px;display: flex; align-items: center;justify-content: center; 
        border-radius: 50%; margin-right: 10px;
    }
    .hero-image img {animation: float 3s ease-in-out infinite;}

    @keyframes float {
        0%, 100% { transform: translateY(0); }
        50% { transform: translateY(-15px); }
    }

    /* Section titles */
    .section-title { text-align: center; margin-bottom: 40px; }
    .section-title h2 { font-size: clamp(1.5rem, 3vw, 2rem); font-weight: 600; margin-bottom: 10px; }
    .section-title p { color: #666; }

    /* How it works */
    .how-it-works {padding: 60px 0;background: #f8fafc;}
    .steps-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 30px; }
    .step-card {background: #fff; border-radius: 12px; text-align: center;padding: 25px; box-shadow: 0 6px 20px rgba(0,0,0,0.08);transition: all 0.3s ease; border-top: 4px solid #4338ca; }
    .step-card:hover { transform: translateY(-8px); }
    .step-icon { width: 80px; height: 80px; border-radius: 50%; margin: 0 auto 15px; background: #f1f5f9; display: flex; align-items: center; justify-content: center; }
    .step-icon img { max-width: 42px; }

    /* Omni Channel */
    .omni-channel { padding: 60px 0; background: #fff; }

    /* Benefits */
    .benefits { background: #f8fafc; padding: 60px 0; }
    .benefits-content { display: grid; grid-template-columns: 1fr 1fr; gap: 40px; align-items: center; }
    .benefits-list { list-style: none; }
    .benefits-list li {background: white; margin-bottom: 15px; padding: 12px;border-radius: 8px; font-weight: 500; display: flex; align-items: center;
      box-shadow: 0 4px 12px rgba(0,0,0,0.05);transition: all 0.3s ease;
    }
    .benefits-list li:hover {transform: translateX(10px);box-shadow: 0 8px 25px rgba(0,0,0,0.1);}
    .benefits-list li::before {content: "✓"; background: #22c55e; color: white; border-radius: 50%;width: 24px; height: 24px; display: flex; 
        align-items: center; justify-content: center;margin-right: 10px;
    }
    .benefits h2 {font-size: 32px;line-height: 36px;margin-bottom: 45px;}

    /* Stats */
    .stats { background: #F7F7F7; padding: 60px 0; }
    .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(160px, 1fr)); gap: 30px; }
    .stat-card { text-align: center; transition: all .3s; }
    .stat-card:hover { transform: translateY(-5px); }
    .stat-number { font-size: 1.8rem; font-weight: 700; color: #22c55e; }
    .stat-label { font-size: 0.9rem; }
    .stat-icon {background: #fff;width: 90px;height: 90px;border-radius: 50%;display: flex;justify-content: center;margin: 0px auto;margin-bottom: 20px;
        border: 1px solid #E7E7E7;
    }
    .stat-icon img {padding: 27px;}

    /* FAQ */
    .faq { padding: 60px 0; }
    .faq-item { background: #f7f7f7; border-radius: 10px; margin-bottom: 15px; overflow: hidden; }
    .faq-question { padding: 15px 20px; cursor: pointer; font-weight: 600; display: flex; justify-content: space-between; align-items: center; }
    .faq-answer { max-height: 0; overflow: hidden; transition: max-height 0.4s ease, padding 0.3s ease; }
      .faq-item.active .faq-answer {
          max-height: 100%;
          padding: 0px 20px 15px 20px;
      }

    /* Footer */
    footer { background: #1e293b; color: #94a3b8; padding: 40px 0 15px 0; }
    .footer-content { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 50px; margin-bottom: 20px; }
    .footer-section h3 { color: #22c55e; margin-bottom: 10px;font-weight: 600; }
    .footer-bottom { text-align: center; border-top: 1px solid #334155; padding-top: 15px; }
    .footer-section p,
    .footer-section ul {font-size: 0.85rem;line-height: 1.6;list-style: none;padding: 0;}
    .footer-section ul li {margin-bottom: 8px;}
    .footer-section a {text-decoration: none;color: #94a3b8;transition: color 0.3s;}
    .footer-section a:hover {color: #fff;}
      .faq-answer ul {
          padding: 0px 35px;
          margin-bottom: 15px;
      }
        .faq-answer ul li {
            font-size: 13px;
            line-height: 20px;
            margin-bottom: 7px;
        }
      .faq-answer p{
          font-size:13px;
          margin-bottom:10px;
      }
      /* Bottom */
      .footer-bottom {
          text-align: center;
          border-top: 1px solid #334155;
          margin-top: 20px;
          padding-top: 15px;
          font-size: 0.9rem;
      }
    .footer-section svg {font-size: 18px;vertical-align: middle;}

    /* Responsive */
    @media (max-width: 1024px) {
      .hero-content { grid-template-columns: 1fr; text-align: center; }
      .benefits-content { grid-template-columns: 1fr; text-align: center; }
      .hero-image { order: -1; }
    }
    @media (max-width: 768px) {
      .auth-btn {
          padding: 8px 11px;
          font-size: 0.75rem;
      }
      .how-it-works, .omni-channel, .benefits, .stats, .faq {
        padding: 40px 0;
      }
      .footer-content {
        grid-template-columns: 1fr; 
        gap: 25px;
        text-align: start;
      }
    }
    @media (max-width: 480px) {
      .hero { padding: 40px 0 60px; }
      .hero-text p { font-size: 1rem; }
      .benefits-list li { font-size: 0.9rem; }
    }
  </style>
</head>
<body>

  <!-- Header -->
  <header>
    <div class="container header-content">
      <div class="logo"><img src="assets/images/combinedlogo.png" alt="Infinity Logo"></div>
        <div>
            <asp:Label ID="lblWelcomeMessage" runat="server" Font-Size="13px"></asp:Label>
            <asp:Label ID="lblMessageName" runat="server" Font-Size="13px" ForeColor="#000082" style="font-weight:600"></asp:Label>
        </div>
      <a id="auth-link" href="Login.aspx" class="auth-btn">
        <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 448 512" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
          <path d="M313.6 304c-28.7 0-42.5 16-89.6 16-47.1 0-60.8-16-89.6-16C60.2 304 0 364.2 0 438.4V464c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48v-25.6c0-74.2-60.2-134.4-134.4-134.4zM400 464H48v-25.6c0-47.6 38.8-86.4 86.4-86.4 14.6 0 38.3 16 89.6 16 51.7 0 74.9-16 89.6-16 47.6 0 86.4 38.8 86.4 86.4V464zM224 288c79.5 0 144-64.5 144-144S303.5 0 224 0 80 64.5 80 144s64.5 144 144 144zm0-240c52.9 0 96 43.1 96 96s-43.1 96-96 96-96-43.1-96-96 43.1-96 96-96z"></path>
        </svg> 
        Log in / Sign Up</a>
    </div>
  </header>

  <main>

    <!-- Hero -->
    <section class="hero">
      <div class="container hero-content">
        <div class="hero-text">
          <h1>Earn More. Sell Smarter.</h1>
          <p>Join India's most trusted warranty partner network. Zero operations, full support.</p>
          <ul class="hero-features">
            <li>Instant Claim Handling</li>
            <li>Pickup & Drop Included</li>
            <li>High Margins & Fast Payouts</li>
          </ul>
        </div>
        <div class="hero-image">
          <img src="assets/images/hero-illustration.png" alt="Hero Illustration">
        </div>
      </div>
    </section>

    <!-- How It Works -->
    <section class="how-it-works">
      <div class="container">
        <div class="section-title">
          <h2>How it Works - "Just Sell & Earn"</h2>
          <p>You operate on water thin margins, operational costs are high and competition is fierce; We Help You WIN</p>
        </div>
        <div class="steps-grid">
          <div class="step-card">
            <div class="step-icon">
                <img src="assets/images/Shopping cart.png" alt="">
            </div>
            <h3>Easy for You to Sell</h3>
            <p>Very Simple, Step-by-Step</p>
          </div>
          <div class="step-card">
            <div class="step-icon">
                <img src="assets/images/Idea.png" alt="">
            </div>
            <h3>Easy to Understand</h3>
            <p>Clarity in understanding the terms for customers</p>
          </div>
          <div class="step-card">
            <div class="step-icon">
                <img src="assets/images/Supply chain.png" alt="">
            </div>
            <h3>Easy for Us to Manage</h3>
            <p>Streamlined & tech-drive automated service operations</p>
          </div>
          <div class="step-card">
            <div class="step-icon">
                <img src="assets/images/Rupee.png" alt="">
            </div>
            <h3>Recurring Income</h3>
            <p>Earn continuous revenue with Long term Loyal Customers</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Omni Channel -->
    <section class="omni-channel">
      <div class="container">
        <div class="section-title">
          <h2>Our Omni Channel Reach</h2>
          <p>Hassle-free Customer Care</p>
        </div>
        <img src="assets/images/omni-channel.png" alt="Omni Channel Diagram" style="width:100%; max-width:980px; margin:0 auto;">
      </div>
    </section>

    <!-- Benefits -->
    <section class="benefits">
      <div class="container benefits-content">
        <div><img src="assets/images/claim-process.png" alt="Claim Process"></div>
        <div>
          <h2>Benefits<br>Partner with Us</h2>
          <ul class="benefits-list">
            <li>Easy for Customer to Understand</li>
            <li>Effortless Sales Onboarding</li>
            <li>Healthy Commissions</li>
            <li>We Handle Ops, You Get Paid</li>
            <li>No Risk Involved</li>
          </ul>
        </div>
      </div>
    </section>

    <!-- Stats -->
    <section class="stats">
      <div class="container stats-grid">
        <div class="stat-card"><div class="stat-icon"><img src="assets/images/sales-partner.png" alt=""></div><span class="stat-number">1,000+</span><div class="stat-label">Sales Partner Network</div></div>
        <div class="stat-card"><div class="stat-icon"><img src="assets/images/service-network.png" alt=""></div><span class="stat-number">4,500+</span><div class="stat-label">Service Centers</div></div>
        <div class="stat-card"><div class="stat-icon"><img src="assets/images/claim-sattled.png" alt=""></div><span class="stat-number">3,00,000+</span><div class="stat-label">Satisfied Customers</div></div>
        <div class="stat-card"><div class="stat-icon"><img src="assets/images/pincode-covers.png" alt=""></div><span class="stat-number">19,000+</span><div class="stat-label">Pin Codes Covered</div></div>
        <div class="stat-card"><div class="stat-icon"><img src="assets/images/cities.png" alt=""></div><span class="stat-number">450+</span><div class="stat-label">Cities</div></div>
      </div>
    </section>

    <!-- FAQ -->
    <section class="faq">
        <div class="container">
            <div class="section-title">
                <h2>Frequently Asked Questions</h2>
            </div>
            <div class="faq-item">
                <div class="faq-question">Q1. How does InfyShield Partner Program work?<span style="font-size: 18px;">+</span></div>
                <div class="faq-answer"><p>Ans: Become a member of InfyShield partner program by clicking Login/Signup button above. Sell eligible plans to customers and earn instant commission on each sale. Leave customer support on us.</p></div>
            </div>
            <div class="faq-item">
                <div class="faq-question">Q2. How long does it take to become InfyShield Partner?<span style="font-size: 18px;">+</span></div>
                <div class="faq-answer"><p>Ans: Our partner onboarding process is easy and hassle free. Online form requires minimum and Basic information for registration. Post registration partner association approval is processed within 24 Hrs followed by legal agreement signup.</p></div>
            </div>
            <div class="faq-item">
                <div class="faq-question">Q3. How will commissions be settled?<span style="font-size: 18px;">+</span></div>
                <div class="faq-answer">
                    <p> Ans: We work on instant commission settlement. You will collect full amount from customers; Retain your commission and make balance payment to us.</p>
                    <ul>
                        <li>Your additional benefits will settle Monthly/Bimonthly basis.</li>
                        <li>Salesperson commission, Additional benefits and special incentives will be settled Monthly/Bi-monthly</li>
                    </ul>
                </div>
            </div>
            <div class="faq-item">
                <div class="faq-question">Q4. How do I know which of products are eligible for protection plan sales?<span style="font-size: 18px;">+</span></div>
                <div class="faq-answer"><p>Ans: Our “Sell InfyShield Plans” tab on your login will indicate what products and plans are eligible for InfyShield Protection Plans.</p> </div>
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
                    <p> If you have any questions or concerns, you must share them with us. You should sell the Service Plans only after assuring yourself on sales.</p>
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

  </main>

  <!-- Footer -->
    <footer>
        <div class="container">
            <div class="footer-content">
                <div class="footer-section">
                    <h3>About Us</h3>
                    <p>Headquartered in New Delhi, India, we are an Indian Angel Network (IAN) portfolio and CJEIT awarded company specializing in service optimization of warranty programs and enabling service providers to leverage technology products across industries.</p>
                </div>
                <div class="footer-section">
                    <h3>Useful Links</h3>
                    <ul>
                        <li><a target="_blank" href="refund.html">Refund Policy</a></li>
                        <li><a target="_blank" href="privacy-policy.html">Privacy Policy</a></li>
                        <li><a target="_blank" href="terms-and-conditions.html">Terms & Conditions</a></li>
                    </ul>
                </div>
                <div class="footer-section">
                    <h3>Contact With Us</h3>
                    <p style="margin-bottom: 10px;">
                        <strong>Head Office:</strong><br>
                        Infinity Assurance Solutions Pvt. Ltd.<br>
                        24, US Complex,<br>
                        Adjacent to Jasola Apollo Metro Station<br>
                        120, Mathura Road<br>
                        New Delhi 110 076 (India)
                    </p>
                    <p style="line-height: 30px;"><svg stroke="currentColor" fill="none" stroke-width="2" viewBox="0 0 24 24" stroke-linecap="round" stroke-linejoin="round" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path d="M5 4h4l2 5l-2.5 1.5a11 11 0 0 0 5 5l1.5 -2.5l5 2v4a2 2 0 0 1 -2 2a16 16 0 0 1 -15 -15a2 2 0 0 1 2 -2"></path><path d="M15 7a2 2 0 0 1 2 2"></path><path d="M15 3a6 6 0 0 1 6 6"></path></svg> +91 9911 24 4451<br>
                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg"><path fill="none" d="M0 0h24v24H0V0z"></path><path d="M22 6c0-1.1-.9-2-2-2H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6zm-2 0-8 4.99L4 6h16zm0 12H4V8l8 5 8-5v10z"></path></svg> contact@infinityassurance.com</p>
                </div>
            </div>
            <div class="footer-bottom">
                <p>&copy; 2025 Infinity Assurance Solutions. All rights reserved.</p>
            </div>
        </div>
    </footer>

  <script>
      // FAQ toggle
      document.querySelectorAll('.faq-question').forEach(q => {
          q.addEventListener('click', () => {
              const parent = q.parentElement;
              const answer = parent.querySelector('.faq-answer');

              parent.classList.toggle('active');

              if (parent.classList.contains('active')) {
                  // Expand
                  answer.style.maxHeight = answer.scrollHeight + "px";
                  q.querySelector('span').textContent = "-";
              } else {
                  // Collapse
                  answer.style.maxHeight = null;
                  q.querySelector('span').textContent = "+";
              }
          });
      });


      const url = window.location.href;

      const tokenMatch = url.match(/token=([a-zA-Z0-9-]+)/);

      if (tokenMatch) {
          const token = tokenMatch[1];

          // Find the login link
          const loginLink = document.getElementById("auth-link");

          // Append token as query param to href
          loginLink.href = `Login.aspx?qa=${token}`;
      }
  </script>

</body>
</html>
