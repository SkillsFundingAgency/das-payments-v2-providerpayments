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
namespace SFA.DAS.Payments.RequiredPayments.AcceptanceTests.Tests.ACT2_TransTypes_1To3_Basic_Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("R13 - Continuing after planned end date, No OnProgram payment")]
    public partial class R13_ContinuingAfterPlannedEndDateNoOnProgramPaymentFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Late_Continuing_No_Completion_Or_OnProg_Payment.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "R13 - Continuing after planned end date, No OnProgram payment", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
#line 3
#line 4
 testRunner.Given("the current processing period is 13", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.And("a learner with LearnRefNumber learnref1 and Uln 10000 undertaking training with t" +
                    "raining provider 10000", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table21 = new TechTalk.SpecFlow.Table(new string[] {
                        "AimSeqNumber",
                        "ProgrammeType",
                        "FrameworkCode",
                        "PathwayCode",
                        "StandardCode",
                        "FundingLineType",
                        "LearnAimRef",
                        "LearningStartDate",
                        "LearningPlannedEndDate",
                        "LearningActualEndDate",
                        "CompletionStatus"});
            table21.AddRow(new string[] {
                        "1",
                        "2",
                        "403",
                        "1",
                        "",
                        "16-18 Apprenticeship (From May 2017) Non-Levy Contract (non-procured)",
                        "ZPROG001",
                        "06/08/2017",
                        "20/08/2018",
                        "",
                        "continuing"});
#line 8
 testRunner.And("the following course information:", ((string)(null)), table21, "And ");
#line hidden
            TechTalk.SpecFlow.Table table22 = new TechTalk.SpecFlow.Table(new string[] {
                        "PriceEpisodeIdentifier",
                        "EpisodeStartDate",
                        "EpisodeEffectiveTNPStartDate",
                        "TotalNegotiatedPrice",
                        "Learning_1",
                        "SfaContributionPercentage"});
            table22.AddRow(new string[] {
                        "p1",
                        "06/08/2017",
                        "06/08/2017",
                        "9000",
                        "600",
                        "0.90000"});
#line 12
 testRunner.And("the following contract type 2 on programme earnings for periods 1-12 are provided" +
                    " in the latest ILR for the academic year 1718:", ((string)(null)), table22, "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Contract Type 2 On programme payments")]
        [NUnit.Framework.CategoryAttribute("Non-DAS")]
        [NUnit.Framework.CategoryAttribute("FinishingLate")]
        [NUnit.Framework.CategoryAttribute("NoPayment")]
        [NUnit.Framework.TestCaseAttribute("Learning_1", "600", null)]
        public virtual void ContractType2OnProgrammePayments(string transaction_Type, string amount, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "Non-DAS",
                    "FinishingLate",
                    "NoPayment"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Contract Type 2 On programme payments", null, @__tags);
#line 20
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 3
this.FeatureBackground();
#line hidden
            TechTalk.SpecFlow.Table table23 = new TechTalk.SpecFlow.Table(new string[] {
                        "LearnRefNumber",
                        "Ukprn",
                        "PriceEpisodeIdentifier",
                        "Period",
                        "ULN",
                        "TransactionType",
                        "Amount",
                        "SfaContributionPercentage"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "1",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "2",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "3",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "4",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "5",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "6",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "7",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "8",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "9",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "10",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "11",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table23.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "12",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
#line 22
 testRunner.And("the following historical contract type 2 on programme payments exist:", ((string)(null)), table23, "And ");
#line 37
 testRunner.When("a TOBY is received", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table24 = new TechTalk.SpecFlow.Table(new string[] {
                        "LearnRefNumber",
                        "Ukprn",
                        "PriceEpisodeIdentifier",
                        "Period",
                        "ULN",
                        "TransactionType",
                        "Amount",
                        "SfaContributionPercentage"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "1",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "2",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "3",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "4",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "5",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "6",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "7",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "8",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "9",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "10",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "11",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
            table24.AddRow(new string[] {
                        "learnref1",
                        "10000",
                        "p1",
                        "12",
                        "10000",
                        string.Format("{0}", transaction_Type),
                        string.Format("{0}", amount),
                        "0.90000"});
#line 39
 testRunner.Then("the payments due component will generate the following contract type 2 payable ea" +
                    "rnings:", ((string)(null)), table24, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
