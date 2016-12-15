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
using Android.Media;

namespace jumpHelper
{
    public class TimerDialogFragment : DialogFragment
    {
        private static Context context;
        public static TimerDialogFragment NewInstance(Context actContext)
        {
            context = actContext;
            TimerDialogFragment fragment = new TimerDialogFragment();
            fragment.Arguments = new Bundle();
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.TimerDialog, container, false);

            TextView timeLeftField = view.FindViewById<TextView>(Resource.Id.timeLeft);
            Button startButton = view.FindViewById<Button>(Resource.Id.TimerStartButton);
            Button cancelButton = view.FindViewById<Button>(Resource.Id.CancelButton);
            var timer = new JumpTimer(timeLeftField, context);
            bool isStarted = false;
            startButton.Click += delegate
            {
                if (isStarted)
                {
                    timer.Cancel();
                    startButton.Text = "Start";
                    timer.initTime();
                }
                else
                {
                    timer.Start();
                    startButton.Text = "Reset";
                }
                isStarted = !isStarted;
            };
            cancelButton.Click += delegate {
                timer.Cancel();
                Dismiss();
            };
            return view;
        }
    }
}