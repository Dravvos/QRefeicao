using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QRefeicao.Data.NoSQL.Models
{
    public class TRADUCAODTC_NOSQL
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.CSharpLegacy)]
        public Guid Id { get; set; }
        [BsonElement]
        public string? TextoOriginal { get; set; }
        [BsonElement]
        public string? IdiomaOriginal { get; set; }
        [BsonElement]
        public string? TextoTraduzido { get; set; }
        [BsonElement]
        public string? IdiomaTraduzido { get; set; }
        [BsonElement]
        public DateTime DataInclusao { get; set; }
        [BsonElement]
        public DateTime? DataAlteracao { get; set; }
    }
}
