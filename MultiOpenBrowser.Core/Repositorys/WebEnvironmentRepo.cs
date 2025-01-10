namespace MultiOpenBrowser.Core.Repositorys
{
    public class WebEnvironmentRepo(IUnitOfWork? uow) : BaseRepo<WebEnvironment>(uow, null)
    {
        public static async Task LoadAsync()
        {
            WebEnvironmentRepo repo = new(null);
            GlobalData.WebEnvironmentList = await repo.Select
                .LeftJoin(a => a.WebBrowser != null && a.WebBrowserId == a.WebBrowser.Id)
                .LeftJoin(a => a.WebEnvironmentGroup != null && a.WebEnvironmentGroupId == a.WebEnvironmentGroup.Id)
                .OrderByDescending(a => a.Order)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<WebEnvironment?> GetAsync(int id)
        {
            var webEnvironment = await Select
                .LeftJoin(a => a.WebBrowserId == a.WebBrowser.Id)
                .LeftJoin(a => a.WebEnvironmentGroup != null && a.WebEnvironmentGroupId == a.WebEnvironmentGroup.Id)
                .Where(a => a.Id == id)
                .FirstAsync();
            return webEnvironment;
        }
    }
}
