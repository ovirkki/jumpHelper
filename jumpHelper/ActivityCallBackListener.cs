using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public interface ActivityCallBackListener
    {
        void startDialogFragment(DialogFragment dialogFragment);
    }
}