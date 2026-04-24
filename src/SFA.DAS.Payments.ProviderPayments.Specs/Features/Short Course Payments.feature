Feature: Short Course Payments

As a Training Provider 
I would like to receive payments for the delivery of short course training

Background: 
	Given the collection period has opened recently

Scenario: Payments for delivery of Milestone 1 of a short course for Levy paying employers
	Given the learning is undertaking an Apprenticeship Unit short course
	And the provider has delivered the first milestone of the short course
	When the employers Levy account is used to fund the milestone payment at period end 
	Then the levy funded Milestone payment should be recorded	
 
Scenario: Payments for delivery of completion of a short course for Levy paying employers
	Given the learning is undertaking an Apprenticeship Unit short course
	And the provider has recorded the completion of the short course
	When the employers Levy account is used to fund the completion payment at period end 
	Then the levy funded completion payment should be recorded
