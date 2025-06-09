namespace WarehouseTestService.Data
{
    public class PalletModel
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public List<BoxModel> Boxes { get; set; } = new();
    }
}
