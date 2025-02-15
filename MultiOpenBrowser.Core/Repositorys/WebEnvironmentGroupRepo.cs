﻿namespace MultiOpenBrowser.Core.Repositorys
{
    public class WebEnvironmentGroupRepo(IUnitOfWork? uow) : BaseRepo<WebEnvironmentGroup>(uow, null)
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
