Feature: Sort Products
	This feature is for sorting om products

@Sort
Scenario Outline: User is able to sort the products by name
	Given default user is logged
	When on 'Products' page, set order to '<OrderType>'
	Then on 'Products' page, current products are sorted by '<OrderType>'

	Examples: 
		| TestCaseId | OrderType     |
		| TC001      | Name (A to Z) |
		| TC002      | Name (Z to A) |