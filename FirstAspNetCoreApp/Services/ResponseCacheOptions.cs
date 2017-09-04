namespace FirstAspNetCoreApp.Services
{
    public class ResponseCacheOptions
    {
        public ResponseCacheOptions() { }
        public string Vary { get; set; }
        public int Lifetime { get; set; }
        public string ClearPath { get; set; }
    }
}
