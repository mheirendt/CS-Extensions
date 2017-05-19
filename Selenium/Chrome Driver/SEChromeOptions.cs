using OpenQA.Selenium.Chrome;

namespace Tobin.EFD.Server.BusinessLogic.Websites
{
    class SEChromeOptions : ChromeOptions
    {
        #region constructors
        public SEChromeOptions() : base()
        {
            // The chrome driver instantiates a new user profile for each session. To auto
            // download PDF images, you must manually set the user preference to true

            // This was a PAIN. If this ever does not work, here is how I got the preference name:
            // 1. Navigate to : chrome://settings/content
            // 2. Scroll to the bottom "PDF Documents" section
            // 3. Right-Click and inspect element on the check box titled "Open PDF files in the default PDF viewer application"
            // 4. The preference name is the pref key for the input, in this case: pref="plugins.always_open_pdf_externally"
            this.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
        }
        #endregion
    }
}
