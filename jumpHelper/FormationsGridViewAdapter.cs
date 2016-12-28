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
    public class FormationsGridViewAdapter : BaseAdapter
    {
        private List<string> formationList;
        private Activity context;
        public FormationsGridViewAdapter(List<string> formationList, Activity context)
        {
            this.formationList = formationList;
            this.context = context;
        }

        public override int Count
        {
            get { return this.formationList.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return this.formationList[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.FormationGridCell, null);
            view.FindViewById<TextView>(Resource.Id.formationInGrid).Text = this.formationList[position];
            return view;
        }
    }
}