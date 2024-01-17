using System.Text;

namespace AutocodeDB.Models
{
    public class SelectResult
    {
        public string[] Schema { get; set; }
        public string[] Types { get; set; }
        public string[][] Data { get; set; }
        public string ErrorMessage { get; set; }
        
        public SelectResult() 
        {
            this.Schema= Array.Empty<string>();
            this.Types = Array.Empty<string>();
            this.Data = Array.Empty<string[]>();
            this.ErrorMessage=String.Empty;
        }
       
        public SelectResult(int length)
        {
            this.Schema = new string[length];
            this.Types = new string[length];
            this.Data = Array.Empty<string[]>();
            this.ErrorMessage = String.Empty;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var schema = string.Join(",", Schema).Trim();
            var types = string.Join(",", Types).Trim();
            var data = new StringBuilder();
            foreach (var row in Data)
            {
                var dataRow = string.Join(",", row).Trim();
                data.Append(dataRow + Environment.NewLine);
            }
            sb.Append(schema+Environment.NewLine);
            sb.Append(types + Environment.NewLine);
            sb.Append(data.ToString().Trim() + Environment.NewLine);
            return sb.ToString().Trim();
        }
    }
}
