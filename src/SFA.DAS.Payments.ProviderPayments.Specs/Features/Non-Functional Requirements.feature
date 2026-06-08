Feature: Non-Functional Requirements

As a person responsible for the Operational aspects of the Payments System
I would like to ensure that payment processing is secure, performant, reliable, compliant, and scalable under all operating conditions. 

Background: 
	Given the collection period has opened recently

Scenario: Ensure transactional consistenty of short course payments processing
	Given the learner is undertaking a short course
	And the initial earnings have been processed by the payments system
	When there is a change to the earnings and a new set of earnings have been received 
	Then the payments generated for the previous earnings should be reversed
