using MultiOpenBrowser.Entitys;

namespace MultiOpenBrowser.Repositorys
{
    internal class UserInfoRepo(IUnitOfWork? uow) : BaseRepo<UserInfo>(uow, null, null)
    {
    }
}
