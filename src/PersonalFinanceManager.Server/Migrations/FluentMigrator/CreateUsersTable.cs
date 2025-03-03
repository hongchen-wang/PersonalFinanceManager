using FluentMigrator;

namespace PersonalFinanceManager.Server.Migrations.FluentMigrator
{
    [Migration(20250303_01)]
    public class CreateUsersTable : Migration
    {
        public override void Up()
        {
            var sql = File.ReadAllText("Migrations/ScriptsSQL/CreateUsersTable.sql");
            Execute.Sql(sql);
        }

        public override void Down()
        {
        }
    }
}
