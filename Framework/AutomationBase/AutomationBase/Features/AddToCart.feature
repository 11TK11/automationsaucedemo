Feature: Add To Cart
	this feature file has scenarios when user is trying to add items to the cart

@AddCart
Scenario: User is able to add items to the cart
	Given default user is logged
	When on 'Products' page, click on 'ADD TO CART' button for the following prodcuts
		| Product             |
		| Sauce Labs Backpack |
	Then on 'Products' page, button 'ADD TO CART' changed text to 'REMOVE' for the following products
		| Product             |
		| Sauce Labs Backpack |
	When on 'Products' page, click on 'Cart' icon
	Then on 'Cart' page, the following products are displayed
		| Product             |
		| Sauce Labs Backpack |
