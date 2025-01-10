namespace MultiOpenBrowser.Core.Repositorys
{
    public class UserInfoRepo(IUnitOfWork? uow) : BaseRepo<UserInfo>(uow, null)
    {
    }
}
