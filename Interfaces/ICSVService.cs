using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repository.Interfaces
{
    public interface ICSVService
    {
        public List<CSVViewModel> ReadCSV(string path);
        public List<CSVViewModel> DownloadCSV(Stream file);
        public void CSVDataFilling(List<CSVViewModel> record, bool isDownloadCSV);
        public IActionResult WriteCSV(List<CSVViewModel> records);
        public List<CSVViewModel> ConvertDataToCSV();
    }
}
