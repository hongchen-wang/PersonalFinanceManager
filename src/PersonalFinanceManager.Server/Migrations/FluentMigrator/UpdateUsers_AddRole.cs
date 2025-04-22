using FluentMigrator;

namespace PersonalFinanceManager.Server.Migrations.FluentMigrator
{
    [Migration(20250304_01)]
    public class UpdateUsers_AddRole : Migration
    {
        public override void Up()
        {
            var sql = File.ReadAllText("Migrations/ScriptsSQL/UpdateUsers_AddRole.sql");
            Execute.Sql(sql);
        }

        public override void Down()
        {
        }
    }
}
