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
    public class JumpCommentsFragment : DialogFragment
    {
        private List<string> jump;
        public JumpCommentsFragment(List<string> jump)
        {
            this.jump = jump;
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.JumpNotes, container, false);
            view.FindViewById<TextView>(Resource.Id.jumpCommentsIntro).Text = "Comments for jump " + string.Join(",", jump);
            ExpandableListView listOutput = view.FindViewById<ExpandableListView>(Resource.Id.jumpNotesListView);
            var adapter = new FormationsWithNotesAdapter(this.Activity, listOutput, FSNotesHandler.Notes, this.jump, false);
            listOutput.SetAdapter(adapter);
            for (int i = 0; i < adapter.GroupCount; i++)
                listOutput.ExpandGroup(i);
            return view;
        }
    }
}