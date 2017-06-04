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
using CreateTranslationDB;

namespace PigLatinTranslator
{
    public class TranslationDataAdapter : BaseAdapter<TranslationDataObject>, ISectionIndexer
    {
        private List<TranslationDataObject> historyObjects = new List<TranslationDataObject>();
        Activity context;       // The activity we are running in

        public List<TranslationDataObject> HistoryObjects { get { return historyObjects; } }

        public TranslationDataAdapter(Activity c, List<TranslationDataObject> tdo) : base()
        {
            historyObjects = tdo;
            context = c;
            BuildSectionIndex();
        }

        public override int Count
        {
            get { return historyObjects.Count; }
        }

        public override TranslationDataObject this[int position]
        {
            get { return historyObjects[position]; }
        }

        string[] sections;
        Java.Lang.Object[] sectionsObjects;
        Dictionary<string, int> alphaIndex;

        public int GetPositionForSection(int sectionIndex)
        {
            return alphaIndex[sections[sectionIndex]];
        }

        public int GetSectionForPosition(int position)
        {
            return position;
        }

        public Java.Lang.Object[] GetSections()
        {
            return sectionsObjects;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);
            }

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = historyObjects[position].DateTranslated;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = historyObjects[position].Word;

            return view;
        }

        private void BuildSectionIndex()
        {
            alphaIndex = new Dictionary<string, int>();     // Map sequential numbers
            for (var i = 0; i < historyObjects.Count; i++)
            {
                string key = "";
                // Use the month as a key
                var month = historyObjects[i].DateTranslated.Split('/')[0];
                if (month == "1")
                    key = "Jan";
                else if (month == "2")
                    key = "Feb";
                else if (month == "3")
                    key = "Mar";
                else if (month == "4")
                    key = "Apr";
                else if (month == "5")
                    key = "May";
                else if (month == "6")
                    key = "Jun";
                else if (month == "7")
                    key = "Jul";
                else if (month == "8")
                    key = "Aug";
                else if (month == "9")
                    key = "Sep";
                else if (month == "10")
                    key = "Oct";
                else if (month == "11")
                    key = "Nov";
                else if (month == "12")
                    key = "Dec";


                if (!alphaIndex.ContainsKey(key))
                {
                    alphaIndex.Add(key, i);
                }
            }

            // Get the count of sections
            sections = new string[alphaIndex.Keys.Count];
            // Copy section names into the sections array
            alphaIndex.Keys.CopyTo(sections, 0);

            // Copy section names into a Java object array
            sectionsObjects = new Java.Lang.Object[sections.Length];
            for (var i = 0; i < sections.Length; i++)
            {
                sectionsObjects[i] = new Java.Lang.String(sections[i]);
            }
        }
    }
}