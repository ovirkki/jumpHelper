using System;
using Android.App;
//using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace jumpHelper
{
    [Activity(Label = "jumpHelper", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        //private FSNotesHandler fsDataHandler;

        /*public FSNotesHandler FSNotesHandler
        {
            get { return fsDataHandler; }
        }*/

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);

            var pager = FindViewById<ViewPager>(Resource.Id.pager);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.sliding_tabs);
            var adapter = new JumpHelperPagerAdapter(SupportFragmentManager/*, this.fsDataHandler*/);
            var toolbar = FindViewById<Toolbar>(Resource.Id.my_toolbar);

            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Jump helper";
            pager.Adapter = adapter;
            tabLayout.SetupWithViewPager(pager);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_toolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.category:
                    item.SetTitle("A");
                    return false;
                default:
                    return false;
            }
        }
    }
}

