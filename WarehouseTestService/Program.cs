using WarehouseTestService.Data;
using WarehouseTestService.Helpers;
using WarehouseTestService.Presentation;
using WarehouseTestService.Repositories;

using var context = new WarehouseContext();

//var generator = new DBDataGenerator(context);

//await generator.Generate();

var repository = new Repository(context);

await repository.ReadContextAsync();

var list1 = repository.GroupAndSortByDateThenSortByWeight();
var list2 = repository.TakeThreeOldestSortByVolume();

var printer = new ConsolePrinter();

printer.PrintPalletsGroupedByDate(list1);
printer.PrintTopThreeNewest(list2);

Console.ReadKey();
