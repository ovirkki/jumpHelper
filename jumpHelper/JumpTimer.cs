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
        private const int INIT_TIME_MS = 35000;
        private const int INTERVAL = 1000;
        public JumpTimer(TextView inputField, Context context) : base(INIT_TIME_MS, INTERVAL)
        {
            this.inputField = inputField;
            this.context = context;
            initTime();
        }
        public override void OnTick(long millisUntilFinished)
        {
            inputField.Text = formatRemainingTime((int)millisUntilFinished / 1000);
        }

        public override void OnFinish()
        {
            inputField.Text = "0";
            Android.Net.Uri notificationUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            Ringtone tone = RingtoneManager.GetRingtone(context, notificationUri);
            tone.Play();
        }
        public string formatRemainingTime(int remainingTimeSecs)
        {
            return "0:" + remainingTimeSecs;
        }
        public void initTime()
        {
            this.inputField.Text = formatRemainingTime((int)INIT_TIME_MS / 1000);
        }
    }
}