using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple_API_Database.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    symbol = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    date = table.Column<string>(nullable: true),
                    isEnabled = table.Column<bool>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    iexId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.symbol);
                });

            migrationBuilder.CreateTable(
                name: "HistoricalDatas",
                columns: table => new
                {
                    high = table.Column<double>(nullable: false),
                    date = table.Column<string>(nullable: true),
                    open = table.Column<double>(nullable: false),
                    low = table.Column<double>(nullable: false),
                    close = table.Column<double>(nullable: false),
                    volume = table.Column<int>(nullable: false),
                    unadjustedVolume = table.Column<int>(nullable: false),
                    change = table.Column<double>(nullable: false),
                    changePercent = table.Column<double>(nullable: false),
                    vwap = table.Column<double>(nullable: false),
                    label = table.Column<string>(nullable: true),
                    changeOverTime = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricalDatas", x => x.high);
                });

            migrationBuilder.CreateTable(
                name: "KeyStats",
                columns: table => new
                {
                    symbol = table.Column<string>(nullable: false),
                    KeystatId = table.Column<int>(nullable: false),
                    marketcap = table.Column<string>(nullable: true),
                    beta = table.Column<string>(nullable: true),
                    week52high = table.Column<decimal>(nullable: false),
                    week52low = table.Column<decimal>(nullable: false),
                    week52change = table.Column<string>(nullable: true),
                    shortInterest = table.Column<string>(nullable: true),
                    shortDate = table.Column<string>(nullable: true),
                    dividendRate = table.Column<decimal>(nullable: false),
                    dividendYield = table.Column<string>(nullable: true),
                    exDividendDate = table.Column<string>(nullable: true),
                    latestEPS = table.Column<string>(nullable: true),
                    latestEPSDate = table.Column<string>(nullable: true),
                    sharesOutstanding = table.Column<string>(nullable: true),
                    srcfloat = table.Column<string>(nullable: true),
                    returnOnEquity = table.Column<string>(nullable: true),
                    consensusEPS = table.Column<decimal>(nullable: false),
                    numberOfEstimates = table.Column<string>(nullable: true),
                    EBITDA = table.Column<string>(nullable: true),
                    revenue = table.Column<string>(nullable: true),
                    grossProfit = table.Column<string>(nullable: true),
                    cash = table.Column<string>(nullable: true),
                    debt = table.Column<string>(nullable: true),
                    ttmEPS = table.Column<decimal>(nullable: false),
                    revenuePerShare = table.Column<string>(nullable: true),
                    revenuePerEmployee = table.Column<string>(nullable: true),
                    peRatioHigh = table.Column<string>(nullable: true),
                    peRatioLow = table.Column<string>(nullable: true),
                    EPSSurpriseDollar = table.Column<string>(nullable: true),
                    EPSSurprisePercent = table.Column<string>(nullable: true),
                    returnOnAssets = table.Column<string>(nullable: true),
                    returnOnCapital = table.Column<string>(nullable: true),
                    profitMargin = table.Column<string>(nullable: true),
                    priceToSales = table.Column<string>(nullable: true),
                    priceToBook = table.Column<string>(nullable: true),
                    day200MovingAvg = table.Column<string>(nullable: true),
                    day50MovingAvg = table.Column<string>(nullable: true),
                    institutionPercent = table.Column<string>(nullable: true),
                    insiderPercent = table.Column<string>(nullable: true),
                    shortRatio = table.Column<string>(nullable: true),
                    year5ChangePercent = table.Column<string>(nullable: true),
                    year2ChangePercent = table.Column<string>(nullable: true),
                    year1ChangePercent = table.Column<string>(nullable: true),
                    ytdChangePercent = table.Column<string>(nullable: true),
                    month6ChangePercent = table.Column<string>(nullable: true),
                    month3ChangePercent = table.Column<string>(nullable: true),
                    month1ChangePercent = table.Column<string>(nullable: true),
                    day5ChangePercent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyStats", x => x.symbol);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    symbol = table.Column<string>(nullable: false),
                    companyName = table.Column<string>(nullable: true),
                    primaryExchange = table.Column<string>(nullable: true),
                    sector = table.Column<string>(nullable: true),
                    calculationPrice = table.Column<string>(nullable: true),
                    open = table.Column<double>(nullable: false),
                    openTime = table.Column<long>(nullable: false),
                    close = table.Column<double>(nullable: false),
                    closeTime = table.Column<long>(nullable: false),
                    high = table.Column<double>(nullable: false),
                    low = table.Column<double>(nullable: false),
                    latestPrice = table.Column<double>(nullable: false),
                    latestSource = table.Column<string>(nullable: true),
                    latestTime = table.Column<string>(nullable: true),
                    latestUpdate = table.Column<long>(nullable: false),
                    latestVolume = table.Column<int>(nullable: false),
                    iexRealtimePrice = table.Column<string>(nullable: true),
                    iexRealtimeSize = table.Column<string>(nullable: true),
                    iexLastUpdated = table.Column<string>(nullable: true),
                    delayedPrice = table.Column<double>(nullable: false),
                    delayedPriceTime = table.Column<long>(nullable: false),
                    previousClose = table.Column<double>(nullable: false),
                    change = table.Column<string>(nullable: true),
                    changePercent = table.Column<string>(nullable: true),
                    iexMarketPercent = table.Column<string>(nullable: true),
                    iexVolume = table.Column<string>(nullable: true),
                    avgTotalVolume = table.Column<int>(nullable: false),
                    iexBidPrice = table.Column<string>(nullable: true),
                    iexBidSize = table.Column<string>(nullable: true),
                    iexAskPrice = table.Column<string>(nullable: true),
                    iexAskSize = table.Column<string>(nullable: true),
                    marketCap = table.Column<long>(nullable: false),
                    peRatio = table.Column<double>(nullable: false),
                    week52High = table.Column<double>(nullable: false),
                    week52Low = table.Column<double>(nullable: false),
                    ytdChange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.symbol);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "HistoricalDatas");

            migrationBuilder.DropTable(
                name: "KeyStats");

            migrationBuilder.DropTable(
                name: "Quotes");
        }
    }
}
