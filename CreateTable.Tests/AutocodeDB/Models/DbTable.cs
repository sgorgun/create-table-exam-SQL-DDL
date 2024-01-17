using System.Collections.Generic;

namespace AutocodeDB.Models
{
    public class DbTable
    {
        public string TableName { get; set; }
        public Dictionary<string, string> ColumnList { get; }
        public List<DbTableForeignKey> ForeignKeys { get; }
        public int SequenceNumber { get; set; }
        
        public DbTable()
        {
            this.TableName = String.Empty;
            this.ColumnList = new Dictionary<string, string>(10);
            this.ForeignKeys = new List<DbTableForeignKey>();
            this.SequenceNumber = 0;
        }

        public override string ToString()
        {
            return $"{TableName}({SequenceNumber}):"+string.Join(',',ColumnList)+";"+string.Join(',',ForeignKeys);
        }
    }
}