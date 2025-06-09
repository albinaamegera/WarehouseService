using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseTestService.Packaging;

namespace WarehouseTestService.Presentation
{
    public class ConsolePrinter
    {
        public void PrintPalletsGroupedByDate(IEnumerable<IGrouping<DateTime, Pallet>> groups)
        {
            Console.WriteLine("первое задание\n");

            foreach (var group in groups)
            {
                Console.WriteLine($"Дата : {group.Key:dd.MM.yyyy}");

                foreach (var item in group)
                {
                    Console.WriteLine($"\tвес : {item.GetWeight()}");
                }

                Console.WriteLine();
            }
        }
        public void PrintTopThreeNewest(IEnumerable<Pallet> pallets)
        {
            Console.WriteLine("второе задание\n");

            foreach (var pallet in pallets)
            {
                Console.WriteLine($"дата : {pallet.GetExpireDate():dd.MM.yyyy} объем : {pallet.GetVolume()}");
            }
        }
    }
}
