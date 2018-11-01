﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.DAS.Payments.ProviderPayments.AcceptanceTests.Non_LevyLearner_BasicDay
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Provider resubmits ILR")]
    public partial class ProviderResubmitsILRFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ILR Resubmission.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Provider resubmits ILR", "\tAs a Provider \r\n\tI would like to be able to submit my ILR files multiple times i" +
                    "n the same period", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("ILR resubmission after original ILR submission payments have been stored")]
        public virtual void ILRResubmissionAfterOriginalILRSubmissionPaymentsHaveBeenStored()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ILR resubmission after original ILR submission payments have been stored", null, ((string[])(null)));
#line 5
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 6
 testRunner.Given("the current collection period is R02", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.And("the payments are for the current collection year", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Delivery Period",
                        "Transaction Type",
                        "Funding Source",
                        "Amount"});
            table1.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "Co-Invested Sfa",
                        "900"});
            table1.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "Co-Invested Employer",
                        "100"});
#line 8
 testRunner.And("the provider has submitted an ILR file with job id \"12345\" which has generated th" +
                    "e following payments:", ((string)(null)), table1, "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Delivery Period",
                        "Transaction Type",
                        "Funding Source",
                        "Amount"});
            table2.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "Co-Invested Sfa",
                        "900"});
            table2.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "Co-Invested Sfa",
                        "2700"});
            table2.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "Co-Invested Sfa",
                        "1800"});
            table2.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "Co-Invested Employer",
                        "100"});
            table2.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "Co-Invested Employer",
                        "300"});
            table2.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "Co-Invested Employer",
                        "1350"});
#line 12
 testRunner.When("the provider re-submits an ILR file with job id \"67890\" which triggers the follow" +
                    "ing funding source payments:", ((string)(null)), table2, "When ");
#line 20
 testRunner.Then("the provider payments service should remove all payments for job id \"12345\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Delivery Period",
                        "TransactionType",
                        "FundingSource",
                        "Amount"});
            table3.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "Co-Invested Sfa",
                        "900"});
            table3.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "Co-Invested Sfa",
                        "2700"});
            table3.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "Co-Invested Sfa",
                        "1800"});
            table3.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "Co-Invested Employer",
                        "100"});
            table3.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "Co-Invested Employer",
                        "300"});
            table3.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "Co-Invested Employer",
                        "1350"});
#line 21
 testRunner.And("the provider payments service will store the following payments:", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
