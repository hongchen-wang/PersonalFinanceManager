using FluentMigrator;

namespace PersonalFinanceManager.Server.Migrations.FluentMigrator
{
    [Migration(20250304_02)]
    public class UpdateUsers_AddRefreshToken : Migration
    {
        public override void Up()
        {
            var sql = File.ReadAllText("Migrations/ScriptsSQL/UpdateUsers_AddRefreshToken.sql");
            Execute.Sql(sql);
        }

        public override void Down()
        {
        }
    }
}
