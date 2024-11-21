namespace MultiOpenBrowser.Repositorys
{
    internal class WebEnvironmentRepo(IUnitOfWork? uow) : BaseRepo<WebEnvironment>(uow, null, null)
    {
        public static async Task LoadAsync()
        {
            WebEnvironmentRepo repo = new(null);
            GlobalData.WebEnvironmentList = await repo.Select
                .LeftJoin(a => a.WebBrowser != null && a.WebBrowserId == a.WebBrowser.Id)
                .OrderByDescending(a => a.Order)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<WebEnvironment?> GetAsync(int id)
        {
            var webEnvironment = await Select
                .LeftJoin(a => a.WebBrowserId == a.WebBrowser.Id)
                .LeftJoin(a => a.WebEnvironmentGroupId == a.WebEnvironmentGroup.Id)
                .Where(a => a.Id == id)
                .FirstAsync();
            return webEnvironment;
        }
    }
}
