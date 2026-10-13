Feature: Store external earnings identifier

As a payments processing system
I want to capture and persist the ExternalEarningsId from incoming FundingSource events
So that external earnings can be consistently tracked and reconciled across systems

Scenario: Store ExternalEarningsId from a FundingSource event
    Given a FundingSource event contains an ExternalEarningsId
    When Provider Payments processes the event
    Then the Payments table stores the ExternalEarningsId

Scenario: Store a null ExternalEarningsId for an apprenticeship payment
    Given a FundingSource event contains a null ExternalEarningsId
    When Provider Payments processes the event
    Then the Payments table stores a null ExternalEarningsId