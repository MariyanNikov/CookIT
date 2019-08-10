# SoftUni @ ASP.NET Core Exam Project 

# LIVE DEMO - [cookityourself.azurewebsites.net](https://cookityourself.azurewebsites.net/)
 
# Project - CookIt

![](https://cookityourself.azurewebsites.net/img/logo.png)

## Type - Web Store
 
## Description
 
The idea of the project is to be a Web Store which
sells cooking recipes with the ingridients required for it...Guests will have access
to some of the general functionality(Register,Login,Check the
listed recipes,perform a search).
Registered Users would have access to actually order a recipe,
see the status of a current order.
Registered Users would also be able to write a review on a recipe.
Administrators can access all the functionality the Registered Users can.
Administrators can promote Registered Users making them either Administrator or Courier.
Administrators can also add Ingredients,Ingredient Types,Create Recipes (edit,delete,view details)
Couriers have a simple task to change the status of an order and once order is actually delivered
that they get email with instructions for the ordered recipes.(fundamentally the role of the
courier is to pick up groceries,pack them into a presentable package and actually deliver them to the recipient...This would be
represented in the functionality of the courier that would be able to change the status of the order once the courier get the
groceries,this should trigger an Email which specifies the actual delivery details to the recipient of the recipe)
The actual flow of the application should be Guest Registers -> Orders an Recipe -> Administrator Processes and aproves the order ->
Courier picks up the order by changing the status of it on "Getting Groceries" and once is ready and change it to the next status "Delivering" which should trigger an notification for the recipient
in which it says an interval of delivery time -> once order is delivered and acquired by the recipient they receive email with the instructions of the recipes which were on the order.
