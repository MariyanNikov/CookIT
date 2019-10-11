# SoftUni @ ASP.NET Core Exam Project 
 [Project requirements - DOWNLOAD](https://dox.abv.bg/download?id=8eb0dd88ec)
 
 The project is using ASP.NET Core template - [NikolayIT - ASP.NET Templates](https://github.com/NikolayIT/ASP.NET-MVC-Template)
# Project - CookIt

## Type - Web Store
 
## Description
 
The idea of the project is a Web Store which
sells cooking recipes with the ingridients required for it.Guests have access
to some of the general functionality(Register,Login,Check the
listed recipes,perform a search).
Registered Users have access to actually order a recipe,
see the status of a current order.
Registered Users would also be able to write a review on a recipe.
Administrators can access all the functionality the Registered Users can.
Administrators can promote Registered Users making them either Administrator or Courier.
Administrators can also add Ingredients,Ingredient Types,Create Recipes (edit,delete,view details)
Couriers have a simple task to change the status of an order and once order is actually delivered
that they get email with instructions for the ordered recipes.
Courier picks up the order by changing the status of it on "Getting Groceries" and once is ready and change it to the next status "Delivering" which triggers an notification for the recipient
in which it says an interval of delivery time -> once order is delivered and acquired by the recipient they receive email with the instructions of the recipes which were on the order.
