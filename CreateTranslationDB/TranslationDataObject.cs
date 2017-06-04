using SQLite;

namespace CreateTranslationDB
{
    [Table("Translations")]
    public class TranslationDataObject
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DateTranslated { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }
    }
}