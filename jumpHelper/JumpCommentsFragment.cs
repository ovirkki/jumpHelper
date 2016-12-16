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
        private Context context;
        public JumpCommentsFragment(List<string> jump, Context context)
        {
            this.jump = jump;
            this.context = context;
        }

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.JumpNotes, container, false);
            view.FindViewById<TextView>(Resource.Id.jumpCommentsIntro).Text = string.Join(GetString(Resource.String.JumpSeparator), jump);
            ExpandableListView listOutput = view.FindViewById<ExpandableListView>(Resource.Id.jumpNotesListView);
            var adapter = new FormationsWithNotesAdapter(this.Activity, listOutput, FSNotesHandler.Notes, this.jump, true);
            listOutput.SetAdapter(adapter);
            for (int i = 0; i < adapter.GroupCount; i++)
                listOutput.ExpandGroup(i);
            TextView timeLeftField = view.FindViewById<TextView>(Resource.Id.jumpTimeLeft);
            
            Button jumpTimerOperButton = view.FindViewById<Button>(Resource.Id.jumpTimerOperButton);
            var timer = new JumpTimer(timeLeftField, context);
            timeLeftField.Text = timer.formatRemainingTime(FSNotesHandler.JUMPTIME_MS / 1000);
            bool isStarted = false;
            //muuta nappi taustaltaa v‰rikk‰‰skis!!!!
            jumpTimerOperButton.Click += delegate
            {
                if (isStarted)
                {
                    timer.Cancel();
                    jumpTimerOperButton.Text = "Start timer";
                    timer.initTime();
                }
                else
                {
                    timer.Start();
                    jumpTimerOperButton.Text = "Stop timer";
                }
                isStarted = !isStarted;
            };
            return view;
        }
    }
}