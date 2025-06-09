using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseTestService.Data;
using WarehouseTestService.Helpers;
using WarehouseTestService.Packaging;

namespace WarehouseTestService.Repositories
{
    public class Repository
    {
        private WarehouseContext Context { get; }
        private List<Pallet> Pallets { get; } 
        public Repository(WarehouseContext context)
        {
            Context = context;
            Pallets = new();
        }
        /// <summary>
        /// создает коллекции паллет и коробок из базы данных
        /// </summary>
        /// <returns></returns>
        public async Task ReadContextAsync()
        {
            var pallets = await Context.Pallets.Include(p => p.Boxes).ToListAsync();

            foreach (var p in pallets)
            {
                CreatePallet(p);
            }
        }
        /// <summary>
        /// группирует паллеты по сроку годности, сортирует по сроку годности
        /// в каждой группе сортирует по весу
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<DateTime, Pallet>> GroupAndSortByDateThenSortByWeight()
        {
            var result = Pallets
                .OrderBy(p => p.GetExpireDate())
                .ThenBy(p => p.GetWeight())
                .GroupBy(p => p.GetExpireDate());

            return result;
        }
        /// <summary>
        /// выбирает три самых "свежих" паллеты, сортирует по объему
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Pallet> TakeThreeOldestSortByVolume()
        {
            var result = Pallets
                .OrderByDescending(p => p.GetExpireDate())
                .Take(3)
                .OrderBy(p => p.GetVolume());

            return result;
        }
        private void CreatePallet(PalletModel model)
        {
            if (TryGetPallet(model, out var pallet))
            {
                foreach (var b in model.Boxes ?? Enumerable.Empty<BoxModel>())
                {
                    CreateBox(ref pallet, b);
                }

                AddPalletToListIfValid(pallet);
            }
        }
        private void CreateBox(ref Pallet pallet, BoxModel model)
        {
            if (TryGetBox(model, out var box))
            {
                TryAddBoxToPallet(ref pallet, box);
            }
        }

        #region Try Methods
        private bool TryGetPallet(PalletModel model, out Pallet pallet)
        {
            try
            {
                pallet = new(model.Width, model.Height, model.Depth);
                return true;
            }
            catch
            {
                pallet = null;
                return false;
            }
            
        }
        private bool TryGetBox(BoxModel model, out Box box)
        {
            try
            {
                box = new(model.Width, model.Height, model.Depth, model.Weight, model.ProductionDate);
                return true;
            }
            catch
            {
                box = null;
                return false;
            }
        }
        private bool TryAddBoxToPallet(ref Pallet pallet, Box box)
        {
            try
            {
                pallet.AddBox(box);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void AddPalletToListIfValid(Pallet pallet)
        {
            try
            {
                pallet.GetWeight();
                pallet.GetVolume();
                pallet.GetExpireDate();
                Pallets.Add(pallet);
            }
            catch
            {
                return;
            }
        }
        #endregion
    }
}
