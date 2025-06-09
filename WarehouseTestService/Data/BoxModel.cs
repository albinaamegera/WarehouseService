using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTestService.Data
{
    public class BoxModel
    {
        public int Id { get; set; }
        public int PalletId { get; set; }
        public PalletModel? Pallet { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
        public DateTime ProductionDate { get; set; }
    }
}
