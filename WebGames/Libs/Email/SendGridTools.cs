using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using WebGames.Libs;

public static class SendGridTools
{
    private static string SendGridUserName = SettingsManager.GetConfig().security.api.Sendgrid_User_Name;
    private static string SendGridPassword = SettingsManager.GetConfig().security.api.Sendgrid_Password;

    /// <summary>
    /// Remove an email from a distribution list
    /// </summary>
    /// <param name="EmaiLAddress">User's Email Address</param>
    /// <param name="DistributionList">Name of Distribution List</param>
    /// <returns></returns>
    public static string DeleteEmailFromList(string EmaiLAddress, string DistributionList)
    {
        string EncodedData = HttpContext.Current.Server.UrlEncode(EmaiLAddress);
        string URL = "http://sendgrid.com/api/newsletter/lists/email/delete.xml?list=" + DistributionList + "&email=" + EncodedData + "&api_user=" + SendGridUserName + "&api_key=" + SendGridPassword;
        return PerformHTTPGet(URL);
    }

    /// <summary>
    /// Add an email from a distribution list
    /// </summary>
    /// <param name="EmaiLAddress">User's Email Address</param>
    /// <param name="DistributionList">Name of Distribution List</param>
    /// <returns>Results log</returns>
    public static string AddEmailToList(string EmailAddress, string Name, string DistributionList)
    {
        string EncodedData = "{\"email\":\"" + EmailAddress + "\",\"name\":\"" + Name + "\"}";
        EncodedData = HttpContext.Current.Server.UrlEncode(EncodedData);
        string URL = "http://sendgrid.com/api/newsletter/lists/email/add.xml?list=" + DistributionList + "&data=" + EncodedData + "&api_user=" + SendGridUserName + "&api_key=" + SendGridPassword;
        return PerformHTTPGet(URL);
    }

    /// <summary>
    /// Send an email to a distribution list
    /// </summary>
    /// <param name="FromName">The SendGrid Newsletter "From Name" You Wish to Send From</param>
    /// <param name="NewsletterTitle">Subject of the Message</param>
    /// <param name="NewsletterHTML">HTML Body of the Message</param>
    /// <param name="SendGridDistributionList">Name of Distribution List</param>
    /// <returns>Results log</returns>
    public static string SendNewsletterToList(string FromName, string NewsletterTitle, string NewsletterHTML, string SendGridDistributionList)
    {

        string EncodedNewsletterName = HttpContext.Current.Server.UrlEncode(NewsletterTitle + " - " + DateTime.Today.ToString("d")); //append date to deal with duplicate subject lines
        string EncodedNewsletterSubject = HttpContext.Current.Server.UrlEncode(NewsletterTitle);
        string EncodedNewletterHTML = HttpContext.Current.Server.UrlEncode(NewsletterHTML);

        string ResultsHTML = "";

        //create newsletter and publish to send grid
        HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://sendgrid.com/api/newsletter/add.xml");
        ASCIIEncoding encoding = new ASCIIEncoding();
        string postData = "identity=" + FromName;
        postData += "&name=" + EncodedNewsletterName;
        postData += "&subject=" + EncodedNewsletterSubject;
        postData += "&html=" + EncodedNewletterHTML;
        postData += "&api_user=" + SendGridUserName;
        postData += "&api_key=" + SendGridPassword;
        postData += "&data=";

        byte[] data = encoding.GetBytes(postData);

        httpWReq.Method = "POST";
        httpWReq.ContentType = "application/x-www-form-urlencoded";
        httpWReq.ContentLength = data.Length;

        using (Stream stream = httpWReq.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

        string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        ResultsHTML += "Creating Newsletter: " + responseString + "<br/>";

        //assign list to newsletter (i.e. this message goes to this list of recipients)
        string URL = "http://sendgrid.com/api/newsletter/recipients/add.xml?name=" + EncodedNewsletterName + "&list=" + SendGridDistributionList + "&api_user=" + SendGridUserName + "&api_key=" + SendGridPassword;
        string SendGridResponse = PerformHTTPGet(URL);
        ResultsHTML += "Assigning Newsletter to List: " + SendGridResponse + "<br/>";

        //schedule newsletter message to be send immediately. There are additional parameters that can be added if you want to schedule your newsletter in a future date
        URL = "http://sendgrid.com/api/newsletter/schedule/add.xml?name=" + EncodedNewsletterName + "&api_user=" + SendGridUserName + "&api_key=" + SendGridPassword;
        SendGridResponse = PerformHTTPGet(URL);
        ResultsHTML += "Scheduling Newsletter: " + SendGridResponse + "<br/>";

        return ResultsHTML;
    }

    /// <summary>
    /// Delete a distribution list
    /// </summary>
    /// <param name="DistributionList">Name of distribution list</param>
    /// <returns></returns>
    public static string DeleteDistributionList(string DistributionList)
    {
        string ResultsHTML = "";
        string URL = "http://sendgrid.com/api/newsletter/lists/delete.xml?list=" + DistributionList + "&api_user=" + SendGridUserName + "&api_key=" + SendGridPassword;
        string SendGridResponse = PerformHTTPGet(URL);
        ResultsHTML += "Deleting List: " + SendGridResponse + "<br/>";
        return ResultsHTML;
    }

    /// <summary>
    /// Create a distribution list
    /// </summary>
    /// <param name="DistributionList">Name of distribution list</param>
    /// <returns></returns>
    public static string CreateDistributionList(string DistributionList)
    {
        string ResultsHTML = "";
        string URL = "http://sendgrid.com/api/newsletter/lists/add.xml?list=" + DistributionList + "&api_user=" + SendGridUserName + "&api_key=" + SendGridPassword;
        string SendGridResponse = PerformHTTPGet(URL);
        ResultsHTML += "Deleting List: " + SendGridResponse + "<br/>";
        return ResultsHTML;
    }

    /// <summary>
    /// Used to add multiple email addresses to a list. 
    /// </summary>
    /// <param name="ListName">Name of distribution list</param>
    /// <param name="EmailAddresses">Email addresses to add (string array)</param>
    /// <returns></returns>
    public static string AddMultipleEmailstoList(string ListName, string[] EmailAddresses)
    {
        // This has been tested on an array with 50,000 recipients. It works well.

        string ResultsHTML = ""; string EncodedData = "";

        for (int x = 0; x < EmailAddresses.Length; x++)
        {
            string EmailAddress = EmailAddresses[x];

            if (IsValidEmail(EmailAddress))
            {
                EncodedData += "&data[]=" + HttpContext.Current.Server.UrlEncode(" {\"email\":\"" + EmailAddress + "\",\"name\":\"\"}");
            }

            if (x % 1000 == 0 || x == EmailAddresses.Length - 1) //break the requests up into blocks of 1,000 email addresses. 
            {

                try
                {
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create("http://sendgrid.com/api/newsletter/lists/email/add.json?list=");
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    string postData = "list=" + ListName;
                    postData += EncodedData;
                    postData += "&api_user=" + SendGridUserName;
                    postData += "&api_key=" + SendGridPassword;
                    byte[] data = encoding.GetBytes(postData);
                    httpWReq.Method = "POST";
                    httpWReq.ContentType = "application/x-www-form-urlencoded";
                    httpWReq.ContentLength = data.Length;
                    using (Stream stream = httpWReq.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                    string SendGridResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    ResultsHTML += "Adding Emails (at " + x.ToString() + "): " + SendGridResponse + "<br/>";
                }
                catch (Exception ex)
                {
                    ResultsHTML += "Error Adding Emails (at " + x.ToString() + "): " + ex.ToString() + "<br/>";
                }
                EncodedData = "";
            }

        }



        return ResultsHTML;
    }

    /// <summary>
    /// Perform an HTTP Get Request and return results
    /// </summary>
    private static string PerformHTTPGet(string Url)
    {
        try
        {
            // Open a connection
            HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(Url);

            // You can also specify additional header values like 
            // the user agent or the referer:
            WebRequestObject.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_5; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.231 Safari/534.10";
            WebRequestObject.Referer = "";

            // Request response:
            WebResponse Response = WebRequestObject.GetResponse();

            // Open data stream:
            Stream WebStream = Response.GetResponseStream();

            // Create reader object:
            StreamReader Reader = new StreamReader(WebStream, System.Text.Encoding.Default);

            // Read the entire stream content:
            string PageContent = Reader.ReadToEnd();

            // Cleanup
            Reader.Close();
            WebStream.Close();
            Response.Close();

            return PageContent;
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Determine if an email address is valid
    /// </summary>
    private static bool IsValidEmail(string emailaddress)
    {
        string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
        if (_Regex.IsMatch(emailaddress))
            return (true);
        else
            return (false);
    }

}