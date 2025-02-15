﻿namespace MultiOpenBrowser.Core.Repositorys
{
    /// <summary>
    /// 标准仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepo<T> : BaseRepository<T> where T : class
    {
        /// <summary>
        /// 更新全部数据 UpdateDiy.Where("1=1")
        /// </summary>
        public IUpdate<T> UpdateAllDiy => UpdateDiy.Where("1=1");

        /// <summary>
        /// 标准仓储
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        /// <param name="filter"></param>
        /// <param name="asTable"></param>
        public BaseRepo(IUnitOfWork? uow, Func<string, string>? asTable) : base(Global.FSql)
        {
            if (asTable != null)
            {
                base.AsTable(new Func<Type, string, string>((a, b) => asTable(b)));
            }
            UnitOfWork = uow;
        }

        /// <summary>
        /// 删除全部数据 DeleteAsync(x => true)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(x => true, cancellationToken);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">分页参数</param>
        /// <param name="pageSize">分页参数</param>
        /// <param name="select">自定义ISelect条件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Pageable<T>> GetPageAsync(int page, int pageSize, ISelect<T> select, CancellationToken cancellationToken = default)
        {
            var data = await select
                .Count(out var total)
                .Page(page, pageSize)
                .ToListAsync(cancellationToken);

            Pageable<T> ret = new()
            {
                Page = page,
                PageCount = (int)Math.Ceiling(total / (double)pageSize),
                DataCount = (int)total,
                Data = data,
            };
            return ret;
        }
    }
}
