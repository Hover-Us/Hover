
namespace Hover.Models
{
    public class Mail
    {
        /* Used for emails and "View as a Web Page". 
         https://github.com/TedGoas/Cerberus/ has nice templates, I started with the beautiful Fluid Template.
         The Welcome template is used for signups 
         Get the layout you want with content in a html file. 
         Then set up your placeHolders, minify it, check with /message/ then copy here. 
         Remember to get { and } like "body {" through string.format() you need "body {{", and minified CSS might have "}}"->"}}}}"

            12 August 2018: Could spend more time but YAGNI

            
       Other possibility: Google email tracking  https://ga-dev-tools.appspot.com/campaign-url-builder/   
        */
        static public class Templates
        {

            static public class Welcome
            {
                public const string name = "0";
                public const string userHash = "1";
                public const string placeHolderName = "new Friend";

                // htmlcompressor.com/compressor/
                public const string html = @"<!DOCTYPE html>
<html lang=en xmlns=http://www.w3.org/1999/xhtml xmlns:v=urn:schemas-microsoft-com:vml xmlns:o=urn:schemas-microsoft-com:office:office>
<head>
<meta charset=utf-8>
<meta name=viewport content=""width=device-width"">
<meta http-equiv=X-UA-Compatible content=""IE=edge"">
<meta name=x-apple-disable-message-reformatting>
<title>Hover Us</title>
<!--[if mso]>
<style>*{{font-family:sans-serif!important}}</style>
<![endif]-->
<style>html,body{{margin:0 auto!important;padding:0!important;height:100%!important;width:100%!important}}*{{-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%}}div[style*=""margin: 16px 0""]{{margin:0!important}}table,td{{mso-table-lspace:0!important;mso-table-rspace:0!important}}table{{border-spacing:0!important;border-collapse:collapse!important;table-layout:fixed!important;margin:0 auto!important}}table table table{{table-layout:auto}}img{{-ms-interpolation-mode:bicubic}}a{{text-decoration:none}}*[x-apple-data-detectors],.unstyle-auto-detected-links *,.aBn{{border-bottom:0!important;cursor:default!important;color:inherit!important;text-decoration:none!important;font-size:inherit!important;font-family:inherit!important;font-weight:inherit!important;line-height:inherit!important}}.a6S{{display:none!important;opacity:.01!important}}img.g-img+div{{display:none!important}}@media only screen and (min-device-width:320px) and (max-device-width:374px){{.email-container{{min-width:320px!important}}}}@media only screen and (min-device-width:375px) and (max-device-width:413px){{.email-container{{min-width:375px!important}}}}@media only screen and (min-device-width:414px){{.email-container{{min-width:414px!important}}}}</style>
<!--[if mso]>
<style type=text/css>ul,ol{{margin:0!important}}li{{margin-left:30px!important}}li.list-item-first{{margin-top:0!important}}li.list-item-last{{margin-bottom:10px!important}}</style>
<![endif]-->
<style>.button-td,.button-a{{transition:all 50ms ease-in;color:#fff}}.button-td-primary:hover,.button-a-primary:hover{{background:#30f!important;border-color:#d00!important;color:#f00}}@media screen and (max-width:600px){{.email-container p{{font-size:17px!important}}}}li{{padding-left:1em;text-indent:-.7em;margin:0 0 10px 30px}}li::before{{content:""▢ "";color:#d00;transform:scaleX(.6)}}.footer a{{color:#999;border-radius:3px;border:1px solid #bbb;padding:.3em .6em;transition:all 50ms ease-in}}.footer a:hover{{border:1px outset #f00;color:#f00;background:#30f}}</style>
<!--[if gte mso 9]>
<xml>
<o:OfficeDocumentSettings>
<o:AllowPNG/>
<o:PixelsPerInch>96</o:PixelsPerInch>
</o:OfficeDocumentSettings>
</xml>
<![endif]-->
</head>
<body width=100% style=margin:0;padding:0!important;mso-line-height-rule:exactly;background-color:#206>
<center style=width:100%;background-color:#206>
<!--[if mso | IE]>
<table role=presentation border=0 cellpadding=0 cellspacing=0 width=100% style=background-color:#206>
<tr><td>
<![endif]-->
<div style=""max-width:600px;margin:0 auto"" class=email-container>
<!--[if mso]>
<table align=center role=presentation cellspacing=0 cellpadding=0 border=0 width=600>
<tr><td>
<![endif]-->
<table align=center role=presentation cellspacing=0 cellpadding=0 border=0 width=100% style=""margin:0 auto"">
<tr><td style=""padding:10px 0""> </td></tr>
<tr><td style=background-color:#fff>
<a href=https://hover.us><img src=https://hover.us/imt/HUlogocaGcP-goIYmG4DeQe9XzT3E_3x4CTrA7hLUT8pD_Z20=w.png srcset=""https://hover.us/imt/HUlogo{1}w.svg 1x"" alt=Hover.Us border=0 style=width:100%;max-width:600px;height:auto;background:#eee;font-family:sans-serif;font-size:5em;color:#222;margin:auto class=g-img></a>
</td></tr>
<tr><td style=background-color:#fff>
<table role=presentation cellspacing=0 cellpadding=0 border=0 width=100%>
<tr><td style=padding:20px;font-family:sans-serif;font-size:15px;line-height:20px;color:#555>
<h1 style=""margin:0 0 10px 0;font-family:sans-serif;font-size:25px;line-height:30px;color:#333;font-weight:normal"">Welcome {0}, to Hover Us: The Hoverboard that floats on Air!</h1>
<p style=margin:0>We are small but we are ready to do big things, Fun Things! The KickStarter will have boards ready to ride, material kits for Do-It-Yourself types, and all backers (from $5) will get a small STEM focused PDF that grade schoolers and older can find interesting: How to design your own floating hoverboard. Learn about pressure, force, materials... We are not responsible if your leaf blower gets destroyed. Learning <i><b>can</b></i> be fun :)</p>
</td></tr>
<tr><td style=""padding:0 20px"">
<table align=center role=presentation cellspacing=0 cellpadding=0 border=0 style=margin:auto>
<tr><td class=""button-td button-td-primary"" style=border-radius:4px;background:#222>
<a class=""button-a button-a-primary"" href=https://www.kickstarter.com/profile/hoverus style=""background:#222;border:1px solid #000;font-family:sans-serif;font-size:15px;line-height:15px;text-decoration:none;padding:13px 17px;display:block;border-radius:4px;color:#fff"">Hover Us KickStarter</a>
</td></tr>
</table>
</td></tr>
<tr><td style=""padding:2em 2em 0 2em;font-family:sans-serif;font-size:15px;line-height:20px;color:#555"">
<h2 style=""margin:0 0 10px 0;font-family:sans-serif;font-size:18px;line-height:22px;color:#333;font-weight:bold"">The next message we send will be the KickStarter launch announcement.</h2>
<ul style=padding:0;margin:0;list-style:none>
<li class=list-item-first>Complete boards like the demo.</li>
<li>Hover Us detailed plans.</li>
<li>Material kits, with the good stuff.</li>
<li style=""margin:0 0 0 30px"" class=list-item-last>Stretch Goals include an extra powerful, built in blower.</li></ul>
<p>More to come so please stay tuned.</p>
</td></tr></table>
</td></tr>
<tr><td aria-hidden=true height=30 style=font-size:0;line-height:0>&nbsp;</td></tr>
<tr><td class=footer style=padding:20px;font-family:sans-serif;font-size:14px;line-height:17px;text-align:center;color:#555;background-color:#fff>
<a href=https://hover.us/message/welcome?u={1}&n={2} style=font-weight:bold>View as a Web Page</a>
<br><br>
<div style=font-size:16px>Hover Us</div><span class=unstyle-auto-detected-links>Huntington Beach, California</span>
<br><br>
<a href=https://hover.us/prefer?u={1}>Unsubscribe</a> &nbsp; &nbsp; &nbsp;
<a href=https://hover.us/docs/privacy>Privacy</a></td></tr>
<tr><td style=""padding:5px 0""> </td></tr>
</table>
<!--[if mso]>
</td></tr></table>
<![endif]-->
</div>
<!--[if mso | IE]>
</td></tr></table>
<![endif]-->
</center></body>
</html>";

                public const string text = @"Welcome {0}, to Hover Us: The Hoverboard that floats on Air!

We are small but we are ready to do big things, Fun Things! The KickStarter will have boards ready to ride, material kits for Do-It-Yourself types, and all backers (from $5) will get a small STEM focused PDF that grade schoolers and older can find interesting: How to design your own floating hoverboard. Learn about pressure, force, materials... We are not responsible if your leaf blower gets destroyed. Learning can be fun :)

Hover Us on KickStarter: https://www.kickstarter.com/profile/hoverus

The next message we send will be the KickStarter launch announcement.
    * Complete boards like the demo.
    * Hover Us detailed plans.
    * Material kits, with the good stuff.
    * Stretch Goals include an extra powerful, built in blower.

More to come so please stay tuned.

View as a Web Page: https://hover.us/message/welcome?u={1}&n={2}

Hover Us, Huntington Beach, California
";
            }

        }

    }
}
