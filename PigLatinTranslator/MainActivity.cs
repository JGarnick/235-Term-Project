using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using SQLite;
using CreateTranslationDB;

namespace PigLatinTranslator
{
    [Activity(Label = "PigLatinTranslator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button historyButton;
        TextView translatedWords;
        Button translateBtn;
        EditText enteredWords;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            historyButton = FindViewById<Button>(Resource.Id.historyBTN);
            translatedWords = FindViewById<TextView>(Resource.Id.translatedWordTV);
            enteredWords = FindViewById<EditText>(Resource.Id.enterWordsET);
            translateBtn = FindViewById<Button>(Resource.Id.translateBTN);

            string dbPath = "";
            SQLiteConnection db = null;

            // Get the path to the database that was deployed in Assets
            dbPath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "pigTranslatorDB.db3");

            // It seems you can read a file in Assets, but not write to it
            // so we'll copy our file to a read/write location
            if (!File.Exists(dbPath))
            {
                using (Stream inStream = Assets.Open("pigTranslatorDB.db3"))
                using (Stream outStream = File.Create(dbPath))
                    inStream.CopyTo(outStream);
            }
                

            // Open the database
            db = new SQLiteConnection(dbPath);

            string translation;

            if(bundle != null)
            {
                enteredWords.Text = bundle.GetString("EnteredWords", "");
                translatedWords.Text = bundle.GetString("Translation", "");
            }

            historyButton.Click += delegate
            {
                var history = new Intent(this, typeof(HistoryActivity));
                StartActivity(history);
            };

            translateBtn.Click += delegate
            {
                if (enteredWords.Text != "")
                {
                    translation = ToPigLatin(enteredWords.Text);
                    translatedWords.Text = translation;
                    db.Insert(new TranslationDataObject { DateTranslated = DateTime.Now.ToShortDateString(), Translation = translation, Word = enteredWords.Text });
                }
                else
                    translatedWords.Text = "Please enter a word in the field above";
            };
        }

        public static string ToPigLatin(string sentence)
        {
            const string vowels = "AEIOUaeio";
            List<string> newWords = new List<string>();

            foreach (string word in sentence.Split(' '))
            {
                string firstLetter = word.Substring(0, 1);
                string restOfWord = word.Substring(1, word.Length - 1);
                int currentLetter = vowels.IndexOf(firstLetter);

                if (currentLetter == -1)
                {
                    newWords.Add(restOfWord + firstLetter + "ay");
                }
                else
                {
                    newWords.Add(word + "way");
                }
            }
            return string.Join(" ", newWords);
        }
        public override void OnSaveInstanceState(Bundle outState, PersistableBundle outPersistentState)
        {
            base.OnSaveInstanceState(outState, outPersistentState);

            outState.PutString("EnteredWords", enteredWords.Text);
            outState.PutString("Translation", translatedWords.Text);
        }

    }
}

