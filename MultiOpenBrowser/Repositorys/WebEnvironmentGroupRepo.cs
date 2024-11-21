
namespace MultiOpenBrowser.Repositorys
{
    internal class WebEnvironmentGroupRepo(IUnitOfWork? uow) : BaseRepo<WebEnvironmentGroup>(uow, null, null)
    {
        public static async Task LoadAsync()
        {
            WebEnvironmentGroupRepo repo = new(null);
            GlobalData.WebEnvironmentGroupList = await repo.Select
                .OrderByDescending(a => a.Order)
                .OrderBy(a => a.Id)
                .ToListAsync();
        }
    }
}
