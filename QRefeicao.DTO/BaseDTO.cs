namespace QRefeicao.DTO
{
    public class BaseDTO
    {
        public Guid? Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? UsuarioInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string? UsuarioAlteracao { get; set; }
    }
}
