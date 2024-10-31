using Android.App;
using Android.Content;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
    DataHost = "auth",
    DataScheme = "msalb3850369-7f7e-4ae0-a5fa-cf2ce9989889"
)]
    public class MsalActivity : BrowserTabActivity
    {
        public MsalActivity()
        {
                
        }
    }
}
