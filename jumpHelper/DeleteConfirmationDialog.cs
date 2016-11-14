using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mainApp = Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace jumpHelper
{
    public class DeleteConfirmationDialog : DialogFragment
    {
        private const string FORMATION = "Formation";
        private const string COMMENT = "Comment";

        public static DeleteConfirmationDialog NewInstance(string formation, string comment)
        {
            DeleteConfirmationDialog fragment = new DeleteConfirmationDialog();
            Bundle args = new Bundle();
            args.PutString(FORMATION, formation);
            args.PutString(COMMENT, comment);
            fragment.Arguments = args;
            return fragment;
        }

        public override mainApp.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            mainApp.AlertDialog.Builder builder = new mainApp.AlertDialog.Builder(Activity);
            string formation = Arguments.GetString(FORMATION);
            string comment = Arguments.GetString(COMMENT);
            builder
                .SetTitle("Confirm delete")
                .SetMessage("Are you sure you want to remove a comment \"" + comment + "\" for formation " + formation)
                .SetPositiveButton("Delete", async (senderAlert, args) =>
                {
                    Dismiss();
                    await FSNotesHandler.removeComment(formation, comment);
                })
                .SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    AppEventHandler.emitInfoTextUpdate("Delete cancelled");
                });

            return builder.Create();
        }
    }
}