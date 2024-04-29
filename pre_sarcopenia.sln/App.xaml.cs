using System.Configuration;
using System.Data;
using System.Windows;
using Syncfusion.Licensing;

namespace pre_sarcopenia.sln
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCe0xyWmFZfVpgcl9HYFZUQWYuP1ZhSXxXdkFhXn9YcXRQQ2NeV0w=");
        }
    }
}