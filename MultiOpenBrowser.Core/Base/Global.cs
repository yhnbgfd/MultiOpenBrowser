﻿using Autofac;
using FreeSql.Aop;
using FreeSql.Internal;
using MultiOpenBrowser.Core.WebBrowsers;
using static MultiOpenBrowser.Core.Entitys.WebBrowser;
using IContainer = Autofac.IContainer;

namespace MultiOpenBrowser.Core.Base
{
    public static class Global
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string[] _autoCreateDirectorys = ["Data", "Assets", "Logs"];

        public static IFreeSql FSql { get; private set; }
        public static IContainer Container { get; private set; }

        static Global()
        {
            foreach (var dir in _autoCreateDirectorys)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), dir);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            var dbDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            var connectionString = $@"Data Source={dbDir}\dat.db;Pooling=true;Max Pool Size=10";
            FSql = new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, connectionString, typeof(FreeSql.Sqlite.SqliteProvider<>))
                .UseAutoSyncStructure(true)
                .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
                .Build();
            FSql.Aop.SyncStructureAfter += FSqlAop_SyncStructureAfter;
            FSql.Aop.ConfigEntityProperty += FSqlAop_ConfigEntityProperty;
            FSql.Aop.CurdAfter += FSqlAop_CurdAfter;

            var builder = new ContainerBuilder();
            builder.RegisterType<Chrome>().Keyed<WebBrowserBase>(TypeEnum.Chrome);
            builder.RegisterType<MsEdge>().Keyed<WebBrowserBase>(TypeEnum.MsEdge);
            builder.RegisterType<Firefox>().Keyed<WebBrowserBase>(TypeEnum.Firefox);
            builder.RegisterType<Browser360>().Keyed<WebBrowserBase>(TypeEnum.Browser360);
            builder.RegisterType<CustomizeBrowser>().Keyed<WebBrowserBase>(TypeEnum.Other);
            Container = builder.Build();
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
