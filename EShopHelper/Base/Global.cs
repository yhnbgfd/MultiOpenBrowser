﻿using FreeSql.Aop;
using FreeSql.Internal;
using NLog;
using System.IO;

namespace EShopHelper.Base
{
    class Global
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static IFreeSql FSql { get; private set; }

        static Global()
        {
            var dbDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
            var connectionString = $@"Data Source={dbDir}\Hang.CQMS.Repository.db;Pooling=true;Max Pool Size=10";

            FSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, connectionString, typeof(FreeSql.Sqlite.SqliteProvider<>))
                .UseAutoSyncStructure(true)
                .UseNameConvert(NameConvertType.ToLower)
                .Build();
            FSql.Aop.SyncStructureAfter += FSqlAop_SyncStructureAfter;
            FSql.Aop.ConfigEntityProperty += FSqlAop_ConfigEntityProperty;
            FSql.Aop.CurdAfter += FSqlAop_CurdAfter;
        }

        /// <summary>
        /// FreeSql的语句执行后处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FSqlAop_CurdAfter(object? sender, CurdAfterEventArgs e)
        {
            if (e.ElapsedMilliseconds > 200)
            {
                _logger.Warn($"慢查询, {e.ElapsedMilliseconds}ms, SQL={e.Sql}");
            }
        }

        /// <summary>
        /// FreeSql的类型映射
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FSqlAop_ConfigEntityProperty(object? sender, ConfigEntityPropertyEventArgs e)
        {
            // DateTimeOffset? => DateTime?
            if (e.Property.PropertyType == typeof(DateTimeOffset?))
            {
                e.ModifyResult.MapType = typeof(DateTime?);
                e.ModifyResult.IsNullable = true;
            }
            // DateTimeOffset => DateTime
            else if (e.Property.PropertyType == typeof(DateTimeOffset))
            {
                e.ModifyResult.MapType = typeof(DateTime);
            }
            // enum => int
            else if (e.Property.PropertyType.IsEnum)
            {
                e.ModifyResult.MapType = typeof(int);
            }
        }

        /// <summary>
        /// FreeSql的同步数据结果后事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void FSqlAop_SyncStructureAfter(object? sender, SyncStructureAfterEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Sql))
            {
                _logger.Warn($"FreeSql.SyncStructure: {Environment.NewLine}{e.Sql.Trim()}");
            }
        }
    }
}