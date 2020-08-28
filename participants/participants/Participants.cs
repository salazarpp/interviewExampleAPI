using MongoDB.Bson;

namespace participants
{
    internal class Participants
    {
        public ObjectId Id { get; set; }
        public int confirmation { get; set; }

        public string name { get; set; }
    }
}