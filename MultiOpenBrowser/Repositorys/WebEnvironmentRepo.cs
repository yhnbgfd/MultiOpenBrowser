namespace MultiOpenBrowser.Repositorys
{
    internal class WebEnvironmentRepo(IUnitOfWork? uow) : BaseRepo<WebEnvironment>(uow, null, null)
    {
        public async Task<WebEnvironment?> GetAsync(int id)
        {
            var webEnvironment = await Select
                .LeftJoin(a => a.WebBrowserId == a.WebBrowser.Id)
                .Where(a => a.Id == id)
                .FirstAsync();
            return webEnvironment;
        }
    }
}
