﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mvc.JQuery.DataTables;
using WebGames.Models;

namespace WebGames.Libs
{
    public class UserDTModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Shop { get; set; }
        public string Controls { get; set; }
    }

    public class DataTableManager
    {
        public static DataTablesResult GetUsers(DataTablesParam dataTableParam)
        {
            try
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var exceptIds = Roles.GetUsersInRole("sysadmin").ToList();
                    //exceptIds.AddRange(Roles.GetUsersInRole("admin"));

                    var searchValue = dataTableParam.sSearch ?? "";
                    var q = db.Users.AsQueryable();
                    if (searchValue != "")
                    {
                        q = q.Where(u => u.FullName.Contains(searchValue) && !exceptIds.Contains(u.Id) );
                    }

                    var users = q.ToList().Select(row => new UserDTModel()
                    {
                        Id = row.Id ?? "",
                        Name = row.FullName ?? "",
                        Shop = row.Shop ?? "",
                        Controls = ""
                    }).AsQueryable();

                    var res = DataTablesResult.Create<UserDTModel>(users, dataTableParam );
                    return res;
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                throw exc;
            }
        }
    }
}