namespace MultiOpenBrowser.Core.Repositorys
{
    public class CacheRepo(IUnitOfWork? uow) : BaseRepo<Cache>(uow, null, null)
    {
    }
}
