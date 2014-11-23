Upgrading your Windows Phone SQL CE Apps
=============

Back in WP7/WP8, they supported SQL CE 3.5 with the use of LinqToSql on top. A very powerful too. Universal/WinRT apps are here, and we know they're going to get significantly better with Windows 10. With Universal apps, the life of SQL CE (Compact Edition) databases are near the end, that is, if you'd like to continue making updates & using the latest developer tools. Now is the time to upgrade from these platforms, before it's too late. 

If you're upgrading an existing SQL CE app to a Universal app, you have two choices: 1) retain user data or 2) wipe it away and start over. Obviously we'd like to retain the users data so they can have a smooth transition to the updated data store. 

### ASP.NET 4.0/4.5

As of ASP.NET 4.0, you've been able to target SQL CE databases in your ASP.NET web applications. With the power of Azure and Web Api, that's exactly what we're going to do. 

#### Project

Let's start by creating a new ASP.NET MVC 4 Web Application with __.NET Framework of 4.5.3__. An alternative scenario is adding this project into your existing Windows Phone solution.

[Read more](http://bit.ly/11OnrBR) on my blog.