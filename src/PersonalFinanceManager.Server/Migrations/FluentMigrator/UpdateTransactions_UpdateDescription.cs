using FluentMigrator;

namespace PersonalFinanceManager.Server.Migrations.FluentMigrator
{
    [Migration(20250302_02)]
    public class UpdateTransactions_UpdateDescription : Migration
    {
        public override void Up()
        {
            var sql1 = File.ReadAllText("Migrations/ScriptsSQL/UpdateTransactions_DropDescription.sql");
            Execute.Sql(sql1);
            var sql2 = File.ReadAllText("Migrations/ScriptsSQL/UpdateTransactions_AddDescriptionText.sql");
            Execute.Sql(sql2);
        }

        public override void Down()
        {
            var sql1 = File.ReadAllText("Migrations/ScriptsSQL/UpdateTransactions_DropDescription.sql");
            Execute.Sql(sql1);
            var sql2 = File.ReadAllText("Migrations/ScriptsSQL/UpdateTransactions_AddDescription.sql");
            Execute.Sql(sql2);
        }
    }
}
