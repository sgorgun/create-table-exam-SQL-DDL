namespace AutocodeDB.Models
{
    public class DbTableForeignKey
    {
        public string LocalColumn { get; }
        public string RefTable { get; }
        public string RefColumn { get; }

        public DbTableForeignKey(string localColumn, string refTable, string refColumn)
        {
            this.LocalColumn = localColumn;
            this.RefTable = refTable;
            this.RefColumn = refColumn;
        }

        public override string ToString()
        {
            return $"{LocalColumn}=>{RefTable}:{RefColumn}";
        }
    }
}