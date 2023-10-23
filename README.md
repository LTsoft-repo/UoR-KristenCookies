# UoR-KristenCookies

## Description
Create a business-layer project to address the following situation.

Kristen is an entrepreneurial college student.   
She is currently able to use an oven that is in her dorm room to bake, but it will only let her bake a dozen cookies at a time, and she wants to track information that will help her figure out when she should scale up to a larger baking space.  
Part of her gimmick is that she will let people order cookies that are baked to order, with a huge topping selection like at Cold Stone Creamery (lots of different mix–ins).  
Orders consist of a minimum of a dozen cookies with the same mix–in (or combination of many mix–ins).  
At this point, she cannot offer partial dozens of cookies. Kristen’s customers will order their cookies by sending an email to Kristen.  
They will come to her dorm room to pick up the cookies; she does not deliver or plan to offer delivery services.  
She’s interested in tracking the following things for her business planning:
- Who ordered
- Order history

Be thoughtful and capture information so that Kristen can plan and so that she can promote specials in the future, but also keep a balance between customer privacy and Kristen’s time in doing data entry.

Please include comments or help text in your code if you want to explain any decisions you make.

## Solution
I have developed the business layer using .NET 6. Within this layer, I've created entities for Cookies, Orders, and Customers using Entity Framework. These entities are then utilized by the Managers to handle the business logic.

In addition, I've set up an ASP.NET application that incorporates the aforementioned services and includes a controller to manage incoming requests.

The current views in the application are as follows:
- A list of Cookies, which also allows the addition of new types.
- A list of Orders, where new orders can be added.
- A list of Customers, which can be added through the Orders view if they don't exist beforehand.

I chose to implement the creation of Customers through the Orders view because I believe it's more intuitive to create a customer when you're in the process of creating an order for them. This approach streamlines the order creation process.

When adding an order, the only required field for the customer is the email. If the customer does not already exist, the system prompts the user to enter their name and then creates the customer along with the order.

## What was left behind
was unable to create the reports mentioned in point 4 of the challenge. The development of the business logic and the MVC components ended up taking longer than originally anticipated, which left me with insufficient time to work on the reports.
