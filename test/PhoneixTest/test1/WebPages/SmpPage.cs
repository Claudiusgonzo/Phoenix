﻿
namespace Phoenix.Test.UI.Framework.WebPages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.PageObjects;
    using Phoenix.Test.UI.Framework;
    using Phoenix.Test.UI.Framework.Controls;
    using Phoenix.Test.UI.Framework.Logging;
    using Phoenix.Test.UI.Framework.WebPages;
    using Phoenix.Test.Data;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Interactions;
    using System.Diagnostics;
    using System.Threading;

    public class SmpPage : Page
    {
        public SmpPage(IWebDriver browser) : base(browser) { }

        [FindsBy(How = How.ClassName, Using = "wizard-button-cancel")]
        private HtmlButton btnCloseFirstTimeWizard { get; set; }

        [FindsBy(How = How.ClassName, Using = "fxs-drawertaskbar-newbutton")]
        protected HtmlButton btnNew { get; set; }

        [FindsBy(How = How.ClassName, Using = "fxs-drawertray-button fx-button")]
        protected HtmlButton completedActions { get; set; }

        [FindsBy(How = How.ClassName, Using = "fxs-drawer-drawermenu")]
        protected HtmlSection drawer { get; set; }

        //[FindsBy(How = How.ClassName, Using = "fx-grid-full")]
        //[FindsBy(How = How.Id, Using = "__fx-grid13")] // Not use, random grid Id !!!
        private HtmlTable tableAzureVMs { get; set; }

        [FindsBy(How = How.ClassName, Using = "fx-grid-full")]
        private HtmlTable tableAllItems { get; set; }

        ////[FindsBy(How = How.Id, Using = "fxshell-nav1-items")]
        protected HtmlScrollBarMenu_Tenant mainMenuTenantPortal { get; set; }

        protected HtmlScrollBarMenu_Admin mainMenuAdminPortal { get; set; }

        [FindsBy(How = How.Id, Using = "fxshell-nav1-items")]
        protected HtmlScrollBarMenu_Admin mainMenu { get; set; }

        [FindsBy(How = How.Name, Using = "Service Management")]
        protected HtmlDiv smp { get; set; }

        public bool IsFirstTimeLogin
        {
            get { return this.btnCloseFirstTimeWizard != null; }
        }

        public override HtmlControl VerifyPageElement
        {
            get { return smp; }
        }

        public void CreateVmFromNewButton(CreateVmData data)
        {
            Browser.WaitForAjax();

            Log.Information("---Click New button---");
            OpenDrawer();
            Log.Information("---Select Create VM---");
            this.drawer.SelectItem("AZURE VMS");
            this.drawer.SelectItem("CREATE AZURE VM");

            Log.Information("---Go through wizard to create VM---");
            var createVmWizard = new CreateVmWizard(this.Browser);
            createVmWizard.Step1(data, this); createVmWizard.GoNext();
            createVmWizard.Step2(data); createVmWizard.GoNext();
            createVmWizard.Step3(data); createVmWizard.Complete();
            Log.Information("---Create VM request send successfully---");
        }

        public void CreateVmFromMainMenu(CreateVmData data)
        {
            Browser.WaitForAjax();

            Log.Information("Find main menu ...");
            GetMainMenu_TenantPortal();
            Log.Information("---Select Create VM---");
            this.mainMenuTenantPortal.SelectAzureVms();

            //this.drawer.SelectItem("AZURE VMS");
            //this.drawer.SelectItem("CREATE AZURE VM");

            Log.Information("---Go through wizard to create VM---");
            var createVmWizard = new CreateVmWizard(this.Browser);
            createVmWizard.Step1(data, this); createVmWizard.GoNext();
            createVmWizard.Step2(data); createVmWizard.GoNext();
           // createVmWizard.Step3(data); 
            createVmWizard.Complete();
            Log.Information("---Create VM request send successfully---");
        }

        public void CreatePlanFromNewButton(CreatePlanData data)
        {
            Log.Information("---Click New button---");
            Browser.WaitForAjax();
            OpenDrawer();
            Log.Information("---Select Create Plan---");

            // IJavaScriptExecutor js = this.Browser as IJavaScriptExecutor;
            // js.ExecuteScript("arguments[-1].click()", this.Browser.FindElement(By.ClassName("fxs-menu-tablediv")));
            Browser.WaitForAjax();

            this.drawer.SelectItem("PLAN");

            Browser.WaitForAjax();
            this.drawer.SelectItem("CREATE PLAN");

            Log.Information("---Go through wizard to create plan---");
            var createPlanWizard = new CreatePlanWizard(this.Browser);
            createPlanWizard.Step1(data); createPlanWizard.GoNext();
            createPlanWizard.Step2(data); createPlanWizard.GoNext();
            createPlanWizard.Step3(data); createPlanWizard.Complete();
            Log.Information("---Create plan request send successfully---");
        }

        public void CreateAddonFromNewButton(CreateAddonData data)
        {
            Log.Information("---Click New button---");
            Browser.WaitForAjax();
            OpenDrawer();
            Log.Information("---Select Create Addon---");
            this.drawer.SelectItem("PLAN");
            this.drawer.SelectItem("CREATE ADD-ON");

            Log.Information("---Go through wizard to create Addon---");
            var createAddonWizard = new CreateAddonWizard(this.Browser);
            createAddonWizard.Step1(data); createAddonWizard.GoNext();
            createAddonWizard.Step2(data); createAddonWizard.Complete();
            Log.Information("---Create add-on request send successfully---");


        }

        public void OpenDrawer()
        {
            Log.Information("Click New button to open drawer.");
            // this.btnNew.Click();
            this.btnNew.ExcuteScriptOnElement(".click()");
        }

        public void MouseHoverByJavaScript(IWebElement targetElement)
        {

            string javaScript = "var evObj = document.createEvent('MouseEvents');" +
                                "evObj.initMouseEvent(\"mouseover\",true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);" +
                                "arguments[0].dispatchEvent(evObj);";
            IJavaScriptExecutor js = this.Browser as IJavaScriptExecutor;
            js.ExecuteScript(javaScript, targetElement);
        }


        public void OpenMenuPlans()
        {
            if (this.mainMenuAdminPortal == null)
                Log.Information("cannot find main menu Admin Portal");
            this.mainMenuAdminPortal.SelectPlans();
        }

        public void GetMainMenu_TenantPortal()
        {
            this.mainMenuTenantPortal = new HtmlScrollBarMenu_Tenant(this, By.Id("fxshell-nav1-items"));
        }

        public void GetMainMenu_AdminPortal()
        {
            this.mainMenuAdminPortal = new HtmlScrollBarMenu_Admin(this, By.Id("fxshell-nav1-items"));
        }

        public bool VerifyVmCreated(CreateVmData data)
        {
            Log.Information("Click Completed operation button...");
            Thread.Sleep(1000 * 15);
            var completedOp = new HtmlButton(this, By.ClassName("fxs-drawertray-button"));
            completedOp.Click();

            Log.Information("Check the progress box...");

            var progressBox = new HtmlDiv(this, By.ClassName("fxs-progressbox-header"));
            if (progressBox.Text == "Successfully submitted VM request.")
                return true;
            else
                return false;
            //Log.Information("Find main menu ...");
            //GetMainMenu_TenantPortal();
            //this.mainMenuTenantPortal.SelectAzureVms();

            //var status = GetBuildStatus(data);

            //Log.Information("Build Status: " + status);
            //if (status.ToLower().Contains("complete"))
            //{
            //    Log.Information("Create VM success!");
            //    return true;
            //}
            //else
            //{
            //    Log.Information("Create VM failed. Error Message: " + this.tableAzureVMs.RowValues[data.serverName][1]);
            //    return false;
            //}
        }
       // string buildStatus;

        //public string GetBuildStatus(CreateVmData data)
        //{

        //    for (int retryAttempt = 1; retryAttempt <= 5; retryAttempt++)
        //    {

        //        this.tableAzureVMs = new HtmlTable(this, By.ClassName("fx-grid-full"));
        //        var row = this.tableAzureVMs.Rows[data.serverName];
        //        var statusColumn = row.GetColumn("BUILD STATUS");
        //        row.GetColumn("BUILD STATUS").Click();
        //        buildStatus = statusColumn.Text;
        //        if (buildStatus == "Complete")
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            Thread.Sleep(1000 * 60 * 5); // Sleep 1 second before retrying
        //        }

        //    }
        //    return buildStatus;

        //}
        public void CloseFirstTimeWizard()
        {
            this.btnCloseFirstTimeWizard.Click();
        }
    }
}
