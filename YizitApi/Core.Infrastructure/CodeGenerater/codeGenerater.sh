#通过运行命令来根据数据库生成对应实体
&"tools/cli.ps1"   -CoreProject "YizitApi.Core" -EntryProject "YizitApi.Web.Entry" -ConnectionName "TurboDB"  -Context "TubroDbContext" -DbContextLocators "TubroDbContextLocator"  -UseDatabaseNames
