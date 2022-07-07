using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using KahootFr.Server.Storage;
using KahootFr.Shared;
using Microsoft.AspNetCore.Mvc;

namespace KahootFr.Server.Controllers
{
    [Route("excels")]
    [ApiController]
    public class ExcelsController : ControllerBase
    {
        private readonly IRepository<ExcelFile> _excRepos;

        public ExcelsController(IRepository<ExcelFile> excRepos)
        {
            _excRepos = excRepos;
        }

        [HttpGet]
        public IEnumerable<ExcelFile> Get()
        {
            return _excRepos.GetAll();
                
        }

        private string DecideType(SharedStringTablePart stringTable, Cell cell)
        {
            if (cell.DataType == null)
                return cell.CellValue.InnerText;

            else if (cell.DataType == CellValues.SharedString)
                return stringTable.SharedStringTable.ElementAt(int.Parse(cell.CellValue.InnerText)).InnerText;

            else if (cell.DataType == CellValues.Boolean)
            {
                switch (int.Parse(cell.CellValue.InnerText))
                {
                    case 0:
                        return "False";
                    case 1:
                        return "True";
                }
            }
            return null;
        }
        [HttpGet("{id?}")]
        public IEnumerable<IEnumerable<string>> GetContent(Guid id)
        {

            var entity = _excRepos.GetAll().Single(item => item.Id == id);
            List<string> Questions = new List<string>();
            List<string> Answers = new List<string>();
            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Open(entity.FullFileName, false))
            {
                WorkbookPart workbookPart = spreadsheet.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault();

                SharedStringTablePart sharedStringTable = workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

       
                foreach (Row row in sheetData)
                {
                    foreach (Cell cell in row)
                    {

                        if(cell.CellReference.ToString().Contains("A") && Questions.Count == Answers.Count)
                        { 
                            Questions.Add(DecideType(sharedStringTable, cell));     
                        }
                        else if (cell.CellReference.ToString().Contains("A") && Questions.Count > Answers.Count)
                        {
                            Answers.Add("Empty");
                            Questions.Add(DecideType(sharedStringTable, cell));
                        }
                        else if(cell.CellReference.ToString().Contains("B") && Questions.Count > Answers.Count)
                        {
                            Answers.Add(DecideType(sharedStringTable, cell));
                        }
                        else if(cell.CellReference.ToString().Contains("B") && Questions.Count == Answers.Count)
                        {
                            
                            Questions.Add("Empty");
                            Answers.Add(DecideType(sharedStringTable, cell));

                        }
                    }
                }
            }
            
            return (new List<List<string>>() { Questions, Answers});
        


            //var entity = _excRepos.GetAll().Single(iterator => iterator.Id == id);
            //return entity.Content;
        }
    }
}
