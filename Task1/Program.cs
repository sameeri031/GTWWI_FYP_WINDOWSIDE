namespace Task1
{
    internal static class Program
    {

        public static string connectionString = "Server=.;Database=GEO PHOTO TAGGING;User Id=sa;Password=123;TrustServerCertificate=True;";
        public static string imageBasePath = @"C:\Users\Dogesh\Desktop\PHOTO_SERVER"; // 👈 jahan images actually stored hain
        public static string BASE_URL = "http://10.17.52.99:8000";
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ALBUM());
        }
    }
}