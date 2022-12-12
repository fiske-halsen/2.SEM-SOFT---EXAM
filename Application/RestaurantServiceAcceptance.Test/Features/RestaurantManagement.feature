Feature: Manage menus/menuitems in the system

@test
Scenario: menu item gets created successfully
	When restaurant owner creates menu item
	Then the menu item is created successfully
@test2
Scenario: menu item gets deleted successfully
	When restaurant owner deletes menu item	
	Then the menu item is deleted successfully
@test3
Scenario: menu item gets updated successfully
	When restaurant owner updates menu item
	Then the menu item is updated successfully

