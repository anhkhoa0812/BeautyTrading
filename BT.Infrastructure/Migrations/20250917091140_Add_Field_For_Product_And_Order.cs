using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Field_For_Product_And_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AirPumpNegativePressure",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ControlMode",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IrFrequencyConversionInfraredLight",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IrInverterInfraredOutputPower",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedOutputPower",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedWavelength",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MachineNetWeight",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MachinePower",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MachineSize",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutputFrequency",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutputPower",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageSize",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PackageWeight",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RotaryRfHandleTorqueMachineSetWeight",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Voltage",
                table: "Product",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Order",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "Order",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AirPumpNegativePressure",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ControlMode",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IrFrequencyConversionInfraredLight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IrInverterInfraredOutputPower",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LedOutputPower",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "LedWavelength",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MachineNetWeight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MachinePower",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MachineSize",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OutputFrequency",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OutputPower",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PackageSize",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PackageWeight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "RotaryRfHandleTorqueMachineSetWeight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Voltage",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "Order");
        }
    }
}
