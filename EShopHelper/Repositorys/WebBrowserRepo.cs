namespace EShopHelper.Repositorys
{
    class WebBrowserRepo : BaseRepo<WebBrowser>
    {
        public WebBrowserRepo(IUnitOfWork? uow) : base(uow, null, null)
        {
        }
    }
}
