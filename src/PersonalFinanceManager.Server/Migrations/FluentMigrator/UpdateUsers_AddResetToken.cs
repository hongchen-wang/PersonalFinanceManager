using FluentMigrator;

namespace PersonalFinanceManager.Server.Migrations.FluentMigrator
{
    [Migration(20250314_01)]
    public class UpdateUsers_AddResetToken : Migration
    {
        public override void Up()
        {
            var sql = File.ReadAllText("Migrations/ScriptsSQL/UpdateUsers_AddResetToken.sql");
            Execute.Sql(sql);
        }

        public override void Down()
        {
        }
    }
}
