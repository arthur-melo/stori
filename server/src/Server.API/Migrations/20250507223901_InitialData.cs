using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.API.Migrations
{
  /// <inheritdoc />
  public partial class InitialData : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      // Github doesn't allow uploading files larger than 100 MB on the free plan.
      // So, instead of having a single export for the database, I have separated the tables into multiple *.tsql files.
      // This function seeds them when running on development.
      DirectoryInfo di = new DirectoryInfo(Path.Combine("Migrations", "Data", "Create"));
      FileInfo[] files = di.GetFiles("*.tsql");

      // Disable constraint checks initially, so that importing doesn't need to be in-order.
      var disableContraints = string.Concat(
        "EXEC sp_MSforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"",
        Environment.NewLine
      );
      migrationBuilder.Sql(disableContraints);

      // Run separated initial data tsql files.
      files.ToList().ForEach(f => migrationBuilder.Sql(File.ReadAllText(f.FullName)));

      // Re-enable constraint checks.
      var enableConstraints =
        "EXEC sp_MSforeachtable @command1=\"print '?'\", @command2=\"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"";
      migrationBuilder.Sql(enableConstraints);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      var dropFilePath = Path.Combine("Migrations", "Data", "Drop", "InitialDataDrop.tsql");
      migrationBuilder.Sql(File.ReadAllText(dropFilePath));
    }
  }
}
