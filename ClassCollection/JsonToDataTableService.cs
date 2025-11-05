using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Data;
using System.IO;
using System.Text;
namespace OP1_API.ClassCollection
{
    public class JsonToDataTableService
    {
        // Path to the JSON file
        private readonly string _jsonFilePath = "data.json";
        public List<GetFinYearWithConnection_model> GetFinYearWithConnection()
        {
            // Read JSON file
            string json = File.ReadAllText(_jsonFilePath);

            // Deserialize JSON into a List of Person objects
            List<GetFinYearWithConnection_model> people = JsonConvert.DeserializeObject<List<GetFinYearWithConnection_model>>(json);
            // Convert the List to DataTable
            return people;
        }

        //private DataTable ConvertToDataTable(List<GetDataTableFromJson_model> people)
        //{
        //    DataTable table = new DataTable();

        //    // Create columns based on the properties of the Person class
        //    table.Columns.Add("Name", typeof(string));
        //    table.Columns.Add("Age", typeof(int));

        //    // Fill DataTable with data
        //    foreach (var person in people)
        //    {
        //        table.Rows.Add(person.Id, person.Name, person.Age);
        //    }

        //    return table;
        //}


        // Decode a Base64URL string (replaces '-' with '+', '_' with '/')
        public string Base64UrlDecode(string base64Url)
        {
            // Make the string a valid base64 string (Base64URL uses '-' and '_' instead of '+' and '/')
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');

            // If the length is not a multiple of 4, pad with '='
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }

   

    public class GetFinYearWithConnection_model
    {
        public string? sno { get; set; }
        public string? FinYear { get; set; }
        public string? Server { get; set; }
        public string? Database { get; set; }
        public string? User_Id { get; set; }
        public string? Password { get; set; }
        public string? Default { get; set; }
    }
}
