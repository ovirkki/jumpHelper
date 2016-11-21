using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public class FormationSorter : IComparer<string>
    {
        public int Compare(string a, string b)
        {
            if (!isBlock(a) && !isBlock(b))
            {
                return a.CompareTo(b);
            }
            else if (isBlock(a) && isBlock(b))
            {
                return Math.Max(Int32.Parse(a), Int32.Parse(b));
            }
            else if (isBlock(a) && !isBlock(b))
            {
                return 1;
            }
            else if (!isBlock(a) && isBlock(b))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private bool isBlock(string formation)
        {
            int dummyInt;
            return Int32.TryParse(formation, out dummyInt);
        }
    }
}