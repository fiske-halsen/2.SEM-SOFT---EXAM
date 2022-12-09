Feature: Manage menus/menuitems in the system
 

Scenario: menu item gets created successfully
	When restaurant owner creates menu item
	Then the menu item is created successfully

Scenario: menu item gets deleted successfully
	When restaurant owner deletes menu item	
	Then the menu item is deleted successfully

Scenario: menu item gets updated successfully
	When restaurant owner updates menu item
	Then the menu item is updated successfully

