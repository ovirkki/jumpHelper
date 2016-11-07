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
using Java.Lang;

namespace jumpHelper
{
    public class JumpListViewAdapter //: BaseExpandableListAdapter
    {
        Dictionary<string, List<string>> commentMapping;
        Dictionary<string, List<string>> jumpComments;
        Activity context;
        List<List<string>> jumps;
        public JumpListViewAdapter(Activity context, List<List<string>> jumps, Dictionary<string, List<string>> commentMapping)
        {
            this.commentMapping = commentMapping;
            this.context = context;
            this.jumps = jumps;
            //generateJumpComments();
        }
        /*
        private void generateJumpComments()
        {
            this.jumpComments = new Dictionary<string, List<string>>();
            this.jumps.ForEach(delegate (List<string> jump)
            {
                List<string> jumpComments = new List<string>();
                string jumpSequence = "";
                jump.ForEach(delegate (string f)
                {
                    jumpSequence += f + " ";
                });
                jump.ForEach(delegate (string formation)
                {
                    List<string> comments = this.commentMapping[formation.ToUpper()];
                    jumpComments.Concat(comments);
                });
                this.jumpComments.Add(jumpSequence, jumpComments);
            });
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return jumps[groupPosition].ToArray();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override int GroupCount
        {
            get { return jumps.Count; }
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            string jumpSequenceString = jumps[groupPosition].ToString();
            View view = convertView;
            if (view == null)
            {
                //LayoutInflater infalInflater = (LayoutInflater)this.context.GetSystemService(Context.LayoutInflaterService);
                view = this.context.LayoutInflater.Inflate(Resource.Layout.JumpListRowMain, null);
            }
            view.FindViewById<TextView>(Resource.Id.jumpSequence).Text = jumpSequenceString;
            return view;
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return commentMapping[getKey(groupPosition)][childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            int count = 0;
            this.jumps[groupPosition].ForEach(delegate (string formation)
            {
                List<string> jumpComments = this.commentMapping[formation];
                count = count + jumpComments.Count;
            });
            //List<string> childList = commentMapping[getKey(groupPosition)];
            return this.jumpComments[getKey(groupPosition)].Count;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {

            string jumpSequenceString = jumpComments[getKey(groupPosition)][childPosition];

            View view = convertView;
            if (view == null)
            {
                //LayoutInflater infalInflater = (LayoutInflater)this.context.GetSystemService(Context.LayoutInflaterService);
                view = this.context.LayoutInflater.Inflate(Resource.Layout.JumpListRowComments, null);
            }
            view.FindViewById<TextView>(Resource.Id.commentField).Text = jumpSequenceString;
            return view;
        }

        public override bool HasStableIds
        {
            get { return false; }
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
        private string getKey(int groupPosition)
        {
            return this.commentMapping.Keys.ToArray()[groupPosition];
        }
        */
        /*
        public override Object GetItem(int position)
        {
            return null;
        }

        public override int Count
        {
            get { return jumps.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = this.context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = jumps[position];
            return view;
        }

        */
    }
}

