using Data;
using Service.CSV;

namespace SongAPI.Common
{
    public class ServiceCaller
    {
        public ServiceCaller(SongContext context)
        {
            Context = context;
        }

        public SongContext Context { get; }
        public void ReadData()
        {
            CSVService cSVService = new CSVService(Context);
            var records = cSVService.ReadCSV("C:\\Users\\Ivan I\\Desktop\\ppp\\top50MusicFrom2010-2019.csv");
            cSVService.CSVDataFilling(records, false);
        }
    }
}
