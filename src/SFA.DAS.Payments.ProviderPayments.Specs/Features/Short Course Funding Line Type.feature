Feature: Short Course Funding Line Type

As a Training Provider
I would like Apprenticeship Unit payments and clawbacks to be reported against the correct funding line type
So that they are included in Summarisation

Background:
	Given the collection period has opened recently

Scenario: Levy funded Apprenticeship Unit payment is reported with the Levy funding line type
	Given the learning is undertaking an Apprenticeship Unit short course
	And the learning is funded under the "GSO Short Courses (Apprenticeship Units) Levy" funding line
	And the employer is Levy funded
	And the provider has delivered the first milestone of the short course
	When the funding source payment is received for the milestone
	Then the payment is recorded with funding line type "GSO Short Courses (Apprenticeship Units) Levy"
	And the funding line type is not an unknown value
	And the payment keeps the short course details

Scenario: Levy funded Apprenticeship Unit clawback is reported with the Levy funding line type
	Given the learning is undertaking an Apprenticeship Unit short course
	And the learning is funded under the "GSO Short Courses (Apprenticeship Units) Levy" funding line
	And the employer is Levy funded
	And the provider has delivered the first milestone of the short course
	When the funding source clawback is received for the milestone
	Then the payment is recorded with funding line type "GSO Short Courses (Apprenticeship Units) Levy"
	And the funding line type is not an unknown value
	And the payment keeps the short course details

Scenario: Non-Levy funded Apprenticeship Unit payment is reported with the Non-Levy funding line type
	Given the learning is undertaking an Apprenticeship Unit short course
	And the learning is funded under the "GSO Short Courses (Apprenticeship Units) Non-Levy" funding line
	And the employer is Non-Levy funded
	And the provider has delivered the first milestone of the short course
	When the funding source payment is received for the milestone
	Then the payment is recorded with funding line type "GSO Short Courses (Apprenticeship Units) Non-Levy"
	And the funding line type is not an unknown value
	And the payment keeps the short course details

Scenario: Non-Levy funded Apprenticeship Unit clawback is reported with the Non-Levy funding line type
	Given the learning is undertaking an Apprenticeship Unit short course
	And the learning is funded under the "GSO Short Courses (Apprenticeship Units) Non-Levy" funding line
	And the employer is Non-Levy funded
	And the provider has delivered the first milestone of the short course
	When the funding source clawback is received for the milestone
	Then the payment is recorded with funding line type "GSO Short Courses (Apprenticeship Units) Non-Levy"
	And the funding line type is not an unknown value
	And the payment keeps the short course details

Scenario: Non-Levy funded Apprenticeship Unit clawback without a course type is reported with the Non-Levy funding line type
	Given the learning is undertaking an Apprenticeship Unit short course
	And the learning is funded under the "GSO Short Courses (Apprenticeship Units) Non-Levy" funding line
	And the employer is Non-Levy funded
	And the funding source event does not carry the course type
	And the provider has delivered the first milestone of the short course
	When the funding source clawback is received for the milestone
	Then the payment is recorded with funding line type "GSO Short Courses (Apprenticeship Units) Non-Levy"
	And the funding line type is not an unknown value
	And the payment keeps the short course details
