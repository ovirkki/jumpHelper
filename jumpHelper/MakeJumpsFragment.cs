using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private List<List<string>> jumps;

        public MakeJumpsFragment(string title) : base(title)
        {}

        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

        }
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            View view = inflater.Inflate(Resource.Layout.MakeJumps, container, false);
            initializeMakeJump(view);
            return view;
        }

        private void initializeMakeJump(View view)
        {
            Button makeJumpBtn = view.FindViewById<Button>(Resource.Id.makeJumpsBtn);
            //TextView output = view.FindViewById<TextView>(Resource.Id.jumpOutput);
            RadioGroup radioGroup = view.FindViewById<RadioGroup>(Resource.Id.categories);
            ListView listOutput = view.FindViewById<ListView>(Resource.Id.jumpList);

            makeJumpBtn.Click += (object sender, EventArgs e) =>
            {
                //output.Text = "";
                string[] categoryMapping = { "R", "A", "AA", "AAA" };
                int checkedRadioButtonId = radioGroup.CheckedRadioButtonId;
                RadioButton checkedRadioButton = view.FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);
                int index = radioGroup.IndexOfChild(checkedRadioButton);
                MakeJumpsController makeJumps = new MakeJumpsController(categoryMapping[index]);
                this.jumps = makeJumps.getJumps();
                string[] jumpArray = makeJumps.getJumpsArray();
                SortedDictionary<string, List<string>> comments = FSNotesHandler.Notes;
                //var adapter = new JumpListViewAdapter(this.Activity, this.jumps, comments);
                var adapter = new ArrayAdapter<String>(this.Activity, Android.Resource.Layout.SimpleListItem1, jumpArray);
                listOutput.Adapter = adapter;
                /*jumps.ForEach(delegate (List<string> jump)
                {
                    jump.ForEach(delegate (string form)
                    {
                        output.Text += form + " ";
                    });
                    output.Text += "\n";
                });*/
            };
        }
    }
    /*
    public class JumpListViewAdapter : BaseAdapter
    {
        List<List<string>> jumps;
        Activity context;
        public JumpListViewAdapter(Activity context, List<List<string>> jumps)
        {
            this.jumps = jumps;
            this.context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

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
    }*/
}