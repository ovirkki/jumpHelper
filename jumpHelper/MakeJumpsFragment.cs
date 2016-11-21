using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public class MakeJumpsFragment : jumpHelperFragment
    {
        //private List<List<string>> jumps;

        public MakeJumpsFragment(string title) : base(title)
        {}
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.MakeJumps, container, false);
            initializeMakeJump(view);
            return view;
        }

        private async Task<string[]> convertJumpsToArrayAsync(List<List<string>> jumpList)
        {
            return await Task.Run(() =>
            {
                List<string> converted = new List<string>();
                jumpList.ForEach(delegate (List<string> formList)
                {
                    string jump = string.Join(GetString(Resource.String.JumpSeparator), formList);
                    converted.Add(jump);
                });
                return converted.ToArray();
            });
        }

        private void initializeMakeJump(View view)
        {
            Button makeJumpBtn = view.FindViewById<Button>(Resource.Id.makeJumpsBtn);
            RadioGroup radioGroup = view.FindViewById<RadioGroup>(Resource.Id.categories);
            ListView listOutput = view.FindViewById<ListView>(Resource.Id.jumpList);

            makeJumpBtn.Click += async (object sender, EventArgs e) =>
            {
                List<List<string>> jumps = await FSNotesHandler.getRandomizedJumps();
                string[] jumpArray = await convertJumpsToArrayAsync(jumps);
                var adapter = new JumpListViewAdapter(this.Activity, jumps);
                listOutput.Adapter = adapter;
            };

        }
    }
}