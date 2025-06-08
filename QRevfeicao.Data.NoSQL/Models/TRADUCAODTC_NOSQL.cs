using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRevfeicao.Data.NoSQL.Models
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
