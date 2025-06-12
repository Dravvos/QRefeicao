namespace QRefeicao.BLL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Guid> GetUserId(string email);
    }
}
