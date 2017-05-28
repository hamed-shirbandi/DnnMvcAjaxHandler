﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DnnMvcAjaxHandlerExample.Service.Users.Dto;
using DnnMvcAjaxHandlerExample.Domain;
using DnnMvcAjaxHandlerExample.Enums;

namespace DnnMvcAjaxHandlerExample.Service.Users
{
    public class UserService : IUserService
    {

        private List<User> users;

        public UserService()
        {
            users = new List<User>();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Create(UserInput input)
        {
            users.Add(
                new User
                {
                    Id=input.Id,
                    Family=input.Family,
                    Name=input.Name,
                    AddDate=input.AddDate
                });
        }



        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UserOutput> Search(int page, int recordsPerPage, string term, SortBy sortBy, SortOrder sortOrder, out int pageSize, out int TotalItemCount)
        {
            var queryable = users.AsQueryable();


            #region بر اساس متن



            if (!string.IsNullOrEmpty(term))
            {
                queryable = queryable.Where(c => c.Family.Contains(term) || c.Name.Contains(term));

            }



            #endregion

            #region مرتب سازی



            switch (sortBy)
            {
                case SortBy.AddDate:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.AddDate) : queryable.OrderByDescending(u => u.AddDate);
                    break;
                case SortBy.DisplayName:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.Name).ThenBy(u => u.Family) : queryable.OrderByDescending(u => u.Name).ThenByDescending(u => u.Family);
                    break;
                default:
                    break;
            }




            #endregion

            #region دریافت تعداد کل صفحات

            TotalItemCount = queryable.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);

            page = page > pageSize || page < 1 ? 1 : page;

            #endregion

            #region دریافت تعداد رکوردهای مورد تیاز


            var skiped = (page - 1) * recordsPerPage;
            queryable = queryable.Skip(skiped).Take(recordsPerPage);


            #endregion



            return queryable.Select(u => new UserOutput
            {
                Id=u.Id,
                AddDate=u.AddDate.ToShortDateString(),
                 Name=u.Name,
                 Family=u.Family,

            }).ToList();
        }
    }
}