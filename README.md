Html Optimized View Efficient Response (HOVER), the code on Hover.Us

KIS DRY: Keep It Simple (drop the redundant Stupid S) and Don't Repeat Yourself. Absolutely no 3rd party addin required.

Everything on a website should be fast, but especially the homepage. Start with good HTML, mobile first. Imagine the homepage speed on a CDN! Use flexbox and vw units so it shows well on bigger screens too. I resize my web browser and don't like when what I am reading disappears. Don't let js or css loading block the homepage, inline Above The Fold css and avoid waiting for window.onload to change something static that should've loaded differently. I am not against jQuery, bootstrap, or whatever framework. If it helps you KIS DRY, great! However, Vanilla or Plain Old JavaScript POJS is faster. Your homepage needs user specific content? You can deliver static HTML fast, then use javascript and microservices to update it as needed. I am most familiar with ASP.NET so I used ASP.NET Core (Visual Studio 2017 is free) for services, but you can use whatever server side tech you want. Just like the code the browser gets, I strive to KIS DRY the project itself.

Simple meaning as simple as possible. It does not mean boring. DRY unless some repeating is simpler than not repeating. Abstraction has lots of advantages, but not always. CSS variables, for instance, can help KIS DRY, and be updated in a media query. However, if the variable is not changable on the client, it should not be on the client. It is a constant and should be removed/replaced with its constant value, either preprocessed or by you.

The root site is static and only subdomain(s) run .NET code so no app.UseStaticFiles()! Let your web sever do that. The data store is MySQL and the web user can only run stored procedures, no direct table access. 

Feel free to use the code, copy the look and feel of Hover Us, but not the content. The content on https://hover.us is protected by copyright, patent and trademark laws, and other intellectual property rights and laws.

We will see how HOVER code scales when hopefully hover.us is the place to buy the hoverboard that floats on air.
