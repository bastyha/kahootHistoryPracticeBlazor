using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using KahootFr.Server.Storage;
using KahootFr.Shared;
using Microsoft.Extensions.DependencyInjection;



namespace KahootFr.Server
{
    public static class SampleData
    {
        public static void AddExcelRepository(this IServiceCollection services)
        {

            string path = Path.Combine(Directory.GetCurrentDirectory(), "ExcelFiles");
            string[] files = Directory.GetFiles(path);

            var excRepo = new MemoryRepository<ExcelFile>();
            foreach(var i in files)
            {
                excRepo.Add(new ExcelFile { FullFileName = i, ShortFileName=i.Replace(path+"\\",null) });   
            }

            services.AddSingleton<IRepository<ExcelFile>>(excRepo);

        }
    }
}
