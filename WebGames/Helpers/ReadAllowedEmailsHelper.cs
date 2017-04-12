using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebGames.Models;

namespace WebGames.Helpers
{
    public class ReadAllowedEmailsHelper
    {
        public static void InitAllowedEmails()
        {
            return;

            //var file = "D:/Development/Repos/WebGames/WebGames/App_Data/contestants.txt";
            //List<Allowed_Email> emails = null;
            //using (StreamReader sr = new StreamReader(file))
            //{
            //    String line = (sr.ReadToEnd() ?? "").Trim();

            //    emails = line.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Select(e => new Allowed_Email() { Email = e.ToLower() }).ToList();
            //}

            //using (var db = ApplicationDbContext.Create())
            //{
            //    db.Alowed_Emails.AddRange(emails);
            //    db.SaveChanges();
            //}
        }
    }
}