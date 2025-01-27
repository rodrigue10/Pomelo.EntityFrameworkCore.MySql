﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Pomelo.EntityFrameworkCore.MySql.FunctionalTests.TestUtilities;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Tests.TestUtilities.Attributes;

namespace Pomelo.EntityFrameworkCore.MySql.FunctionalTests.Query
{
    public class OwnedEntityQueryMySqlTest : OwnedEntityQueryRelationalTestBase
    {
        protected override ITestStoreFactory TestStoreFactory => MySqlTestStoreFactory.Instance;

        [SupportedServerVersionCondition(nameof(ServerVersionSupport.WindowFunctions))]
        public override async Task Multiple_single_result_in_projection_containing_owned_types(bool async)
        {
            await base.Multiple_single_result_in_projection_containing_owned_types(async);

            AssertSql(
                @"SELECT `e`.`Id`, `t0`.`Id`, `t0`.`Entity20277Id`, `t0`.`Owned_IsDeleted`, `t0`.`Owned_Value`, `t0`.`Type`, `t0`.`c`, `t1`.`Id`, `t1`.`Entity20277Id`, `t1`.`Owned_IsDeleted`, `t1`.`Owned_Value`, `t1`.`Type`, `t1`.`c`
FROM `Entities` AS `e`
LEFT JOIN (
    SELECT `t`.`Id`, `t`.`Entity20277Id`, `t`.`Owned_IsDeleted`, `t`.`Owned_Value`, `t`.`Type`, `t`.`c`
    FROM (
        SELECT `c`.`Id`, `c`.`Entity20277Id`, `c`.`Owned_IsDeleted`, `c`.`Owned_Value`, `c`.`Type`, 1 AS `c`, ROW_NUMBER() OVER(PARTITION BY `c`.`Entity20277Id` ORDER BY `c`.`Entity20277Id`, `c`.`Id`) AS `row`
        FROM `Child20277` AS `c`
        WHERE `c`.`Type` = 1
    ) AS `t`
    WHERE `t`.`row` <= 1
) AS `t0` ON `e`.`Id` = `t0`.`Entity20277Id`
LEFT JOIN (
    SELECT `t2`.`Id`, `t2`.`Entity20277Id`, `t2`.`Owned_IsDeleted`, `t2`.`Owned_Value`, `t2`.`Type`, `t2`.`c`
    FROM (
        SELECT `c0`.`Id`, `c0`.`Entity20277Id`, `c0`.`Owned_IsDeleted`, `c0`.`Owned_Value`, `c0`.`Type`, 1 AS `c`, ROW_NUMBER() OVER(PARTITION BY `c0`.`Entity20277Id` ORDER BY `c0`.`Entity20277Id`, `c0`.`Id`) AS `row`
        FROM `Child20277` AS `c0`
        WHERE `c0`.`Type` = 2
    ) AS `t2`
    WHERE `t2`.`row` <= 1
) AS `t1` ON `e`.`Id` = `t1`.`Entity20277Id`");
        }

        public override async Task Owned_collection_basic_split_query(bool async)
        {
            await  base.Owned_collection_basic_split_query(async);
        }
    }
}
