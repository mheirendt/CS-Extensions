﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    public class SEInput : SEBaseElement
    {
        #region constructors
        /// <summary>
        /// Instantiate an Input from an IWebElement with the tag name "input"
        /// </summary>
        public SEInput(IWebElement element) : base(element)
        {
            string tagName = element.TagName;
            if (null == tagName || !"input".Equals(tagName.ToLower()))
                throw new UnexpectedTagNameException("input", tagName);
        }
        #endregion
    }
}