namespace ShopAlp.Models
{
    public static class DataUser
    {
        public static string name { get; set; }
        public static string surname { get; set; }
        public static string phone { get; set; }
        public static bool onaccoutn { get; set; }
        public static int coutn { get; set; } = 0;

        public static List<List<object>> dataP;
        public static List<List<object>> productCart;
    }
}
