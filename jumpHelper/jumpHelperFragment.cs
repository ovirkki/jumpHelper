using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public class jumpHelperFragment : Fragment
    {
        string title;
        //protected FSNotesHandler dataHandler;
        public jumpHelperFragment(string title/*, FSNotesHandler dataHandler*/)
        {
            this.title = title;
            //this.dataHandler = dataHandler;
        }

        public string Title
        {
            get { return this.title; }
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        /*
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            return inflater.Inflate(layout, container, false);
        }
        */
    }
}