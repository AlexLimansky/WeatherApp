using System;

namespace FirstAspNetCoreApp.Services
{
    public class MyDateDisplayer : IDateDisplayer
    {
        private static DateTime started;
        public void SetStart() => started = DateTime.UtcNow;
        public string DisplayCurrent() => DateTime.UtcNow.ToString();
        public string DisplayStart() => started.ToString();
    }
}
