using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Android.Support.V4.Content;
//using mainApp = Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace jumpHelper
{
    public class JumpListViewAdapter : BaseAdapter<List<string>>
    {
        List<List<string>> jumps;
        FragmentActivity context;
        public JumpListViewAdapter(FragmentActivity context, List<List<string>> jumps)
        {
            this.jumps = jumps;
            this.context = context;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override List<string> this[int position]
        {
            get { return jumps[position]; }
        }
        public override int Count
        {
            get { return jumps.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.JumpRow, null);
            view.FindViewById<TextView>(Resource.Id.jumpSequence).Text = stringifyJump(this.jumps[position]);
            showCommentButtonHandler(view, this.jumps[position]);
            return view;
        }

        private bool hasComments(List<string> jump)
        {
            Dictionary<string, List<string>> filteredNotes = FSNotesHandler.getNotesWithFilterList(jump);
            return filteredNotes.Any(kvp => kvp.Value.Count > 0);
        }

        private void showCommentButtonHandler(View view, List<string> jump)
        {
            ImageButton showCommentsButton = view.FindViewById<ImageButton>(Resource.Id.showCommentsForJump);

            if(!hasComments(jump))
            {
                showCommentsButton.Enabled = false;
                var disabledColor = new Color(ContextCompat.GetColor(context, Resource.Color.colorPrimarySemi));
                showCommentsButton.SetBackgroundColor(disabledColor);
            }
            else
            {
                showCommentsButton.Enabled = true;
                showCommentsButton.Click += ((sender, eventArgs) =>
                {
                    JumpCommentsFragment fragment = new JumpCommentsFragment(jump, this.context);
                    var ft = context.SupportFragmentManager.BeginTransaction();
                    ft.SetTransition(FragmentTransaction.TransitFragmentFade);
                    Fragment prev = context.SupportFragmentManager.FindFragmentByTag("comments");
                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }
                    ft.AddToBackStack(null);
                    fragment.Show(ft, "comments");
                });
            }

        }

        private string stringifyJump(List<string> jump)
        {
           return string.Join(this.context.GetString(Resource.String.JumpSeparator), jump);
        }
    }
}