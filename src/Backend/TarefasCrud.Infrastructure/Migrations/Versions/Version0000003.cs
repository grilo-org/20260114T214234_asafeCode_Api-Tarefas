using FluentMigrator;

namespace TarefasCrud.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TASK_TABLE, "Create table to save tasks")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        CreateTable("Tasks")
            .WithColumn("Title").AsString(150).NotNullable()
            .WithColumn("Description").AsString(150).Nullable()
            .WithColumn("WeeklyGoal").AsInt32().NotNullable()
            .WithColumn("Progress").AsInt32().NotNullable()
            .WithColumn("Category").AsString().NotNullable()
            .WithColumn("StartDate").AsDate().NotNullable()
            .WithColumn("WeekOfMonth").AsInt32().NotNullable()
            .WithColumn("IsCompleted").AsBoolean().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Tasks_User_Id", "Users", "Id");
    }
}