namespace EShopHelper.Repositorys
{
    internal class UserInfoRepo(IUnitOfWork? uow) : BaseRepo<UserInfo>(uow, null, null)
    {
    }
}
