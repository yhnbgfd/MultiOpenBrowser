namespace EShopHelper.Repositorys
{
    class WebEnvironmentRepo : BaseRepo<WebEnvironment>
    {
        public WebEnvironmentRepo(IUnitOfWork? uow) : base(uow, null, null)
        {
        }
    }
}
