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
    [NUnit.Framework.DescriptionAttribute("One Non-Levy Learner Finishes Early PV2-195")]
    public partial class OneNon_LevyLearnerFinishesEarlyPV2_195Feature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "One Non-Levy Learner Finishes Early PV2-195.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "One Non-Levy Learner Finishes Early PV2-195", "Provider earnings and payments where learner completes earlier than planned", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void FeatureBackground()
        {
#line 4
#line 5
 testRunner.Given("a learner is undertaking a training with a training provider", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.And("the SFA contribution percentage is 90%", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("A non-DAS learner, learner finishes early")]
        [NUnit.Framework.CategoryAttribute("NonDas_BasicDay")]
        public virtual void ANon_DASLearnerLearnerFinishesEarly()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A non-DAS learner, learner finishes early", null, new string[] {
                        "NonDas_BasicDay"});
#line 9
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 4
this.FeatureBackground();
#line 10
 testRunner.Given("the current collection period is R02", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 11
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
                        "CoInvestedSfa",
                        "900"});
            table1.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "CoInvestedSfa",
                        "2700"});
            table1.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "CoInvestedSfa",
                        "1800"});
            table1.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "CoInvestedEmployer",
                        "100"});
            table1.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "CoInvestedEmployer",
                        "300"});
            table1.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "CoInvestedEmployer",
                        "1350"});
            table1.AddRow(new string[] {
                        "1",
                        "First16To18EmployerIncentive (TT4)",
                        "SfaFullyFunded",
                        "500"});
#line 12
 testRunner.And("the funding source service generates the following contract type 2 payments:", ((string)(null)), table1, "And ");
#line 21
 testRunner.When("the funding source payments event are received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Delivery Period",
                        "TransactionType",
                        "FundingSource",
                        "Amount"});
            table2.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "CoInvestedSfa",
                        "900"});
            table2.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "CoInvestedSfa",
                        "2700"});
            table2.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "CoInvestedSfa",
                        "1800"});
            table2.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "CoInvestedEmployer",
                        "100"});
            table2.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "CoInvestedEmployer",
                        "300"});
            table2.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "CoInvestedEmployer",
                        "1350"});
            table2.AddRow(new string[] {
                        "1",
                        "First16To18EmployerIncentive (TT4)",
                        "SfaFullyFunded",
                        "500"});
#line 22
 testRunner.Then("the provider payments service will store the following payments:", ((string)(null)), table2, "Then ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Delivery Period",
                        "TransactionType",
                        "FundingSource",
                        "Amount"});
            table3.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "CoInvestedSfa",
                        "900"});
            table3.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "CoInvestedSfa",
                        "2700"});
            table3.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "CoInvestedSfa",
                        "1800"});
            table3.AddRow(new string[] {
                        "1",
                        "Learning (TT1)",
                        "CoInvestedEmployer",
                        "100"});
            table3.AddRow(new string[] {
                        "1",
                        "Completion (TT2)",
                        "CoInvestedEmployer",
                        "300"});
            table3.AddRow(new string[] {
                        "1",
                        "Balancing (TT3)",
                        "CoInvestedEmployer",
                        "1350"});
            table3.AddRow(new string[] {
                        "1",
                        "First16To18EmployerIncentive (TT4)",
                        "SfaFullyFunded",
                        "500"});
#line 31
 testRunner.And("at month end the provider payments service will publish the following payments", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
