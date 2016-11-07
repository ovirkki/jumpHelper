using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
//using Android.App;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace jumpHelper
{
    public class JumpHelperPagerAdapter : FragmentPagerAdapter
    {

        private List<Fragment> fragmentList = new List<Fragment>();

        public JumpHelperPagerAdapter(FragmentManager manager) : base(manager)
        {
            fragmentList.Add(new FSNotesFragment("FS notes"));
            fragmentList.Add(new MakeJumpsFragment("MakeJumps"));
        }

        public override int Count
        {
            get { return fragmentList.Count; }
        }

        public override Fragment GetItem(int position)
        {
            return fragmentList[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            List<string> stringList = new List<string>();
            fragmentList.ForEach(delegate(Fragment fragment)
            {
                jumpHelperFragment jumpHelperFrag = (jumpHelperFragment)fragment;
                string title = jumpHelperFrag.Title;
                stringList.Add(title);
            });
            return CharSequence.ArrayFromStringArray(stringList.ToArray())[position];
        }
    }
}