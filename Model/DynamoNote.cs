using Amazon.DynamoDBv2.DataModel;

namespace NotebookAppApi.Model
{
    [DynamoDBTable("Note")]
    public class DynamoNote
    {
        [DynamoDBHashKey]
        public int Id { get; set; }

        public string Body { get; set; }
    }
}
