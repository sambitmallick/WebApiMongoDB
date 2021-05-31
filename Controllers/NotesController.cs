using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotebookAppApi.Interfaces;
using NotebookAppApi.Model;
using NotebookAppApi.Infrastructure;
using System;
using System.Collections.Generic;

namespace NotebookAppApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private   AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        [NoCache]
        [HttpGet]
        public async Task<IEnumerable<DynamoNote>> Get()
        {
            //var request = new QueryRequest
            //{
            //    TableName = "Reply",
            //    KeyConditionExpression = "Id = :v_Id",
            //    //        ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
            //    //{":v_Id", new AttributeValue { S =  "Amazon DynamoDB#DynamoDB Thread 1" }}}

            //};
            DynamoDBContext context = new DynamoDBContext(client);
            //var items = await context.


            var conditions = new List<ScanCondition>();
            // you can add scan conditions, or leave empty
            var allDocs = await context.ScanAsync<DynamoNote>(conditions).GetRemainingAsync();
            return allDocs;
        }

        [NoCache]
        [HttpGet("{id}")]
        public async Task<DynamoNote> Get(int id)
        {
            DynamoDBContext context = new DynamoDBContext(client);
            if (id > 0)
            { 
                var item = await context.LoadAsync<DynamoNote>(id);
                return item;
            }
            throw new ArgumentException("Id should be greater than zero");
        }

        // POST api/notes
        [HttpPost]
        public async Task<string> PostAsync([FromBody] DynamoNote newNote)
        {
            if (newNote.Id <= 0) return "Bad Request: Id less than 0";
            try
            {
                DynamoDBContext context = new DynamoDBContext(client);
                await context.SaveAsync<DynamoNote>(newNote);
                return "Ok";
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return "Error";
            }
        }
    }
}
