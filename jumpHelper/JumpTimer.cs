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
using Android.Media;

namespace jumpHelper
{
    public class JumpTimer : CountDownTimer
    {
        private TextView inputField;
        private Context context;
        public JumpTimer(int totalTime, int interval, TextView inputField, Context context) : base(totalTime, interval)
        {
            this.inputField = inputField;
            this.context = context;
        }
        public override void OnTick(long millisUntilFinished)
        {
            inputField.Text = (int)millisUntilFinished / 1000 + "";
        }

        public override void OnFinish()
        {
            inputField.Text = "0";
            Android.Net.Uri notificationUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            Ringtone tone = RingtoneManager.GetRingtone(context, notificationUri);
            tone.Play();
        }
    }
}