<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BuyInfyShieldPlans.aspx.cs" Inherits="Patner_Retailer_ADO.BuyInfyShieldPlans" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
<div class="container-fluid  dashboard-content">
    <div class="row">
        <!-- ============================================================== -->
        <!-- validation form -->
        <!-- ============================================================== -->
        <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
            <div class="card">
                <h5 class="card-header">Bootstrap Validation Form</h5>
                <div class="card-body">
                    <form class="needs-validation" novalidate>
                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Product Type</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Product Type" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Product Purchase Price</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Product Purchase Price" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Product Purchase Date</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Product Purchase Date" required>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Brand</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Brand" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Model</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Model" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Model No.</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Model No." required>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Serial No.</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Serial No." required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Product Installation Date</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Product Installation Date" required>
                            </div>
                        </div>
                        
                        <label>Manufacturer Warranty</label>
                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Years</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Years" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Months</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Months" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Days</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Days" required>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Customer Email ID</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Customer Email ID" required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <label for="validationCustom01">Customer Mobile No.</label>
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Customer Mobile No." required>
                            </div>

                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <button class="btn btn-primary" type="submit">Submit</button>
                            </div>
                        </div>
                        <div class="row"></div>
                        <h3 style="text-align:center;">
                            Choose the Best Plan for Your Product
                        </h3>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                <label for="validationCustom01">1 Year EW</label>
                                <input type="radio" name="ServicePlan" style="display: inline;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                <label for="validationCustom01">2 Year EW</label>
                                <input type="radio" name="ServicePlan" style="display: inline;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                <label for="validationCustom01">3 Year EW</label>
                                <input type="radio" name="ServicePlan" style="display: inline;" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xl-2 col-lg-2 col-md-2 col-sm-2 col-2 ">
                                </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <input type="text" class="form-control" id="validationCustom01" placeholder="Apply Your Promo Code" required>
                            </div>
                            <div class="col-xl-4 col-lg-4 col-md-4 col-sm-4 col-4 ">
                                <button class="btn btn-primary" type="submit">Apply</button>
                            </div>

                        </div>
                        <div class="form-row">
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mb-12" style="text-align: center;">
                                <label for="validationCustom03" style="width: 35%;">
                                    By paying just an additional ₹1600, you can upgrade to the
                                    2 Years Extended Warranty plan!
                                </label>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                <div class="form-group">
                                    <div class="form-check">
                                        <input class="form-check-input" type="checkbox" value="" id="invalidCheck" required>
                                        <label class="form-check-label" for="invalidCheck">
                                            By proceeding, you agree to the Terms and Conditions
                                        </label>                                      
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12" style="text-align:center;">
                                <button class="btn btn-primary" type="submit">Add to Cart</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <!-- ============================================================== -->
        <!-- end validation form -->
        <!-- ============================================================== -->
    </div>

</div>

</asp:Content>
