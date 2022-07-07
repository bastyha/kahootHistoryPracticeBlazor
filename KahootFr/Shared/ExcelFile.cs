using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahootFr.Shared
{
    public class ExcelFile
    {


        public ExcelFile()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FullFileName { get; set; }
        public string ShortFileName { get; set; }
    }
}
