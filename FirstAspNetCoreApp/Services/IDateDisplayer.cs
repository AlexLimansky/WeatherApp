using System;

namespace FirstAspNetCoreApp.Services
{
    public interface IDateDisplayer
    {
        void SetStart();
        string DisplayCurrent();
        string DisplayStart();
    }
}
