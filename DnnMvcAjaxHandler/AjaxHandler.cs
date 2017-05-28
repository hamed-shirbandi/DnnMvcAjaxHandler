using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace DnnMvcAjaxHandler
{
    public static class AjaxHandler
    {

        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString ActionLink(string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, AjaxHandlerOption ajaxOption)
        {
            string @params = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));

            var a = new TagBuilder("a");

            a.SetInnerText(linkText);
            a.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            a.MergeAttribute("alt", linkText);
            a.AddCssClass("ajax-call");
            a.MergeAttribute("data-actionName", actionName);
            a.MergeAttribute("data-controllerName", controllerName);
            a.MergeAttribute("data-params", @params);
            a.MergeAttribute("data-item-id", ajaxOption.ClickedItemId.ToString());
            a.MergeAttribute("data-onSuccess", ajaxOption.OnSuccess);
            a.MergeAttribute("data-fade-color", ajaxOption.FadeColor);
            a.MergeAttribute("data-onConfirm", ajaxOption.OnConfirm);
            a.MergeAttribute("data-update-element-id", ajaxOption.UpdateElementId);
            a.MergeAttribute("data-loading-element-id", ajaxOption.LoadingElementId);
            return MvcHtmlString.Create(a.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString CheckedChange(bool selected, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, AjaxHandlerOption ajaxOption)
        {
            string @params = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));


            var wrpLable = new TagBuilder("lable");
            wrpLable.AddCssClass("well well-sm");


            var input = new TagBuilder("input");
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("value", "1");

            if (selected)
            {
                input.MergeAttribute("checked", "checked");
            }

            input.AddCssClass("ajax-call");
            input.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            input.MergeAttribute("data-actionName", actionName);
            input.MergeAttribute("data-controllerName", controllerName);
            input.MergeAttribute("data-params", @params);
            input.MergeAttribute("data-onSuccess", ajaxOption.OnSuccess);
            input.MergeAttribute("data-fade-color", ajaxOption.FadeColor);
            input.MergeAttribute("data-onConfirm", ajaxOption.OnConfirm);
            input.MergeAttribute("data-update-element-id", ajaxOption.UpdateElementId);
            input.MergeAttribute("data-loading-element-id", ajaxOption.LoadingElementId);


            wrpLable.InnerHtml = input.ToString(TagRenderMode.Normal);


            var lable = new TagBuilder("lable");
            lable.SetInnerText(linkText);

            wrpLable.InnerHtml += lable.ToString(TagRenderMode.Normal);



            return MvcHtmlString.Create(wrpLable.ToString(TagRenderMode.Normal));
        }



        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString BeginForm(string btnText, string actionName, string controllerName, object htmlAttributes, AjaxHandlerOption ajaxOption)
        {
            var button = new TagBuilder("button");
            button.SetInnerText(btnText);
            button.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            button.MergeAttribute("alt", btnText);
            button.AddCssClass("ajax-search");
            button.MergeAttribute("data-actionName", actionName);
            button.MergeAttribute("data-controllerName", controllerName);
            button.MergeAttribute("data-target-form-id", ajaxOption.TargetFormId);
            button.MergeAttribute("data-onSuccess", ajaxOption.OnSuccess);
            button.MergeAttribute("data-update-element-id", ajaxOption.UpdateElementId);
            button.MergeAttribute("data-loading-element-id", ajaxOption.LoadingElementId);
            return MvcHtmlString.Create(button.ToString(TagRenderMode.Normal));
        }




        /// <summary>
        /// 
        /// </summary>
        public static MvcHtmlString PagedListPager(string actionName, string controllerName, object routeValues, AjaxHandlerOption ajaxOption, PagerOptions pagerOptions)
        {

            #region Init

            bool hasNextPage = pagerOptions.currentPage < pagerOptions.PageCount;
            bool hasPreviousPage = pagerOptions.currentPage > 1;
            bool isFirstPage = pagerOptions.currentPage == 1;
            bool isLastPage = pagerOptions.currentPage == pagerOptions.PageCount;

            if (pagerOptions.DisplayMode == PagedListDisplayMode.Never || (pagerOptions.DisplayMode == PagedListDisplayMode.IfNeeded && pagerOptions.PageCount <= 1))
                return null;

            string @params = String.Join("&", routeValues.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(routeValues, null)));


            var nextBtn = new TagBuilder("a");
            var prevBtn = new TagBuilder("a");
            prevBtn.AddCssClass("btn btn-default ltr");
            nextBtn.AddCssClass("btn btn-default ltr");
            var wrapper = new TagBuilder("nav");
            wrapper.MergeAttribute("aria-label", "Page navigation");
            var ul = new TagBuilder("ul");

            wrapper.AddCssClass(pagerOptions.WrapperClasses);
            ul.AddCssClass("pagination scrollable");
            ul.AddCssClass(pagerOptions.UlElementClasses);


            #endregion

            #region previous



            if (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToPreviousPage == PagedListDisplayMode.IfNeeded && !isFirstPage))
            {
                prevBtn.AddCssClass("ajax-paging");

                var span = new TagBuilder("span");
                span.SetInnerText(pagerOptions.LinkToPreviousPageFormat);

                prevBtn.MergeAttribute("data-actionName", actionName);
                prevBtn.MergeAttribute("data-controllerName", controllerName);

                var page = pagerOptions.currentPage <= 1 ? 1 : pagerOptions.currentPage - 1;
                prevBtn.MergeAttribute("data-params", @params + "&page=" + (page));
                prevBtn.MergeAttribute("data-onSuccess", ajaxOption.OnSuccess);
                prevBtn.MergeAttribute("data-update-element-id", ajaxOption.UpdateElementId);
                prevBtn.MergeAttribute("data-loading-element-id", ajaxOption.LoadingElementId);

                prevBtn.InnerHtml = span.ToString(TagRenderMode.Normal);
            }



            #endregion

            #region numbers


            for (int i = 1; i <= pagerOptions.PageCount; i++)
            {
                var li = new TagBuilder("li");
                li.AddCssClass(pagerOptions.LiElementClasses);

                if (i == 1 && pagerOptions.currentPage > pagerOptions.PageCount)
                {
                    li.AddCssClass("active");
                }
                else if (i == pagerOptions.currentPage)
                {
                    li.AddCssClass("active");
                }

                var a = new TagBuilder("a");
                a.AddCssClass("ajax-paging");

                var span = new TagBuilder("span");
                span.SetInnerText(i.ToString());

                a.MergeAttribute("data-actionName", actionName);
                a.MergeAttribute("data-controllerName", controllerName);
                a.MergeAttribute("data-params", @params + "&page=" + i);
                a.MergeAttribute("data-onSuccess", ajaxOption.OnSuccess);
                a.MergeAttribute("data-update-element-id", ajaxOption.UpdateElementId);
                a.MergeAttribute("data-loading-element-id", ajaxOption.LoadingElementId);
                a.InnerHtml = span.ToString(TagRenderMode.Normal);
                li.InnerHtml = a.ToString(TagRenderMode.Normal);
                ul.InnerHtml = ul.InnerHtml + li.ToString(TagRenderMode.Normal);
            }




            #endregion

            #region Next



            if (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.Always || (pagerOptions.DisplayLinkToNextPage == PagedListDisplayMode.IfNeeded && !isLastPage))
            {
                nextBtn.AddCssClass("ajax-paging");
                var span = new TagBuilder("span");
                span.SetInnerText(pagerOptions.LinkToNextPageFormat);
                nextBtn.MergeAttribute("data-actionName", actionName);
                nextBtn.MergeAttribute("data-controllerName", controllerName);

                var page = pagerOptions.currentPage >= pagerOptions.PageCount ? pagerOptions.PageCount : pagerOptions.currentPage + 1;
                nextBtn.MergeAttribute("data-params", @params + "&page=" + (page));
                nextBtn.MergeAttribute("data-onSuccess", ajaxOption.OnSuccess);
                nextBtn.MergeAttribute("data-update-element-id", ajaxOption.UpdateElementId);
                nextBtn.MergeAttribute("data-loading-element-id", ajaxOption.LoadingElementId);

                nextBtn.InnerHtml = span.ToString(TagRenderMode.Normal);
            }


            #endregion

            #region Info area

            wrapper.InnerHtml = ul.ToString(TagRenderMode.Normal);

            if (pagerOptions.DisplayInfoArea == true)
            {
                var infoDiv = new TagBuilder("div");
                infoDiv.AddCssClass("well well-sm text-primary clearfix text-center");


                if (pagerOptions.DisplayPageCountAndCurrentLocation == true)
                {

                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-right");
                    infoSpan.SetInnerText(pagerOptions.CurrentLocationFormat + " " + pagerOptions.currentPage + " " + pagerOptions.PageCountFormat + " " + pagerOptions.PageCount);
                    infoDiv.InnerHtml = infoSpan.ToString(TagRenderMode.Normal);

                }
                if (pagerOptions.DisplayTotalItemCount == true)
                {
                    var infoSpan = new TagBuilder("span");
                    infoSpan.AddCssClass("pull-left");
                    infoSpan.SetInnerText(pagerOptions.TotalItemCountFormat + " " + pagerOptions.TotalItemCount);
                    infoDiv.InnerHtml = infoDiv.InnerHtml + infoSpan.ToString(TagRenderMode.Normal);
                }


                if (hasNextPage)
                {
                    infoDiv.InnerHtml += nextBtn.ToString(TagRenderMode.Normal);
                }

                if (hasPreviousPage)
                {
                    infoDiv.InnerHtml += prevBtn.ToString(TagRenderMode.Normal);
                }


                wrapper.InnerHtml = wrapper.InnerHtml + infoDiv.ToString(TagRenderMode.Normal);
            }






            #endregion

            return MvcHtmlString.Create(wrapper.ToString(TagRenderMode.Normal));
        }


    }


}
