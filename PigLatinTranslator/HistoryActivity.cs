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
using CreateTranslationDB;
using SQLite;
using System.IO;

namespace PigLatinTranslator
{
    [Activity(Label = "History", ParentActivity = typeof(MainActivity))]
    public class HistoryActivity : ListActivity
    {
        List<TranslationDataObject> historyList;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.SetDisplayShowHomeEnabled(true);

            historyList = new List<TranslationDataObject>();
            string dbPath = "";
            SQLiteConnection db = null;

            // Get the path to the database that was deployed in Assets
            dbPath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "pigTranslatorDB.db3");

            db = new SQLiteConnection(dbPath);


            historyList = db.Table<TranslationDataObject>().ToList();
            // Instantiate our custom listView adapter
            ListAdapter = new TranslationDataAdapter(this, historyList);

            // This is all you need to do to enable fast scrolling
            ListView.FastScrollEnabled = true;
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            string word = historyList[position].Translation;
            Android.Widget.Toast.MakeText(this,
            word, Android.Widget.ToastLength.Short).Show();

        }
    }
}