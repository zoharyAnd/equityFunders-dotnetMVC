# Equity Funders - .NetMVC application

Equity Funders is an equity crowdfunding application, using Paypal as a payment gateway.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

First you will need to install **vscode** on your computer, then install the following extension:
* ASP.NET core VS Code Extension Pack
* ASP.NET Helper
* Razor +

Make sure that you have also installed the following programs globally on your computer:
* Microsoft .NET Framework 4.5 or +
* Microsoft .NET Core SDK 2.1 or +

For storing the database install PostgreSQL 's [PgAdmin](https://www.postgresql.org/download/) 
* create your database using the PgAdmin panel
* and replace the connectionString credentials by your newly created database 
```
in startup.cs

server=localhost;port=your_port;database=your_db_name;user Id=user_id;password=your_password;Integrated Security=true;Pooling=true;"
```
### Installing

To get a development env running execute:

```
dotnet build

dotnet run
```

## Database

Populate the database with seed data with relationships which includes
```
users 
category (a_category_name)
projects (a_name, a_description, an_amounttoraise, an_amountraised, a_creationdate, a_closingdate, a_1stimage, a_2ndimage, a_3rdimage, a_4rthimage, an_userid, a_categoryid, an_ordinaryShareDescription, a_preferencialShareDescription, an_nonVotingShareDescription, an_redeemableShareDescription, a_boolean_validated)
transactions (a_projectid, an_userid, an_amount, a_transactiondate, an_accountemail)
answers (a_question_id, an_userid, an_answerdate, an_answermessage, a_boolean_for_validated_answers)
questions
favorites (an_userid, a_projectid))
contactus (an_userid, a_sendername, a_senderemail, a_mailsubject, a_mailmessage)
```

## Deployment
To deploy your site, you will need to create a site within an **IIS Server** 

```
dotnet publish
```
After this command, make your IIS instance point to the root of your published project.

## Built With

* [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet/mvc) - To build the structure and skeleton
* [PostgreSQL](https://www.postgresql.org/) - To store the database
* [Sass](https://sass-lang.com/) - To organise and define the application's theme
* [Gulp](https://rometools.github.io/rome/) - To compile the interface theme

## Collaborators

This project was built as a collaboration between 
[Zohary Andrianome](https://zoharyandrianome-portfolio.netlify.com/)
and
[Riana Andrianome](https://rianaandrianomeportfolio.netlify.com/)
