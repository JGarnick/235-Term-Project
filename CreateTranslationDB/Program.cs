
using SQLite;


namespace CreateTranslationDB
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbPath = @"../../../PigLatinTranslator/Assets/pigTranslatorDB.db3";
            var db = new SQLiteConnection(dbPath);

            db.CreateTable<TranslationDataObject>();

            db.Insert(new TranslationDataObject { DateTranslated = "test Date", Translation = "test Translation", Word = "test Word" });


            // Create a Stocks table
            if (db.CreateTable<TranslationDataObject>() == 0)
            {
                // A table already exists, delete any data it contains
                db.DeleteAll<TranslationDataObject>();
            }
        }
    }
}
