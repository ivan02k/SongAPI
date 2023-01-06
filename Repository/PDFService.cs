using Data;
using Data.Entities;
using Data.Entities.IdentityClass;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Service
{
    public class PDFService: IPDFService
    {
        public readonly SongContext _songContext;
        public PDFService(SongContext songContext)
        {
            _songContext = songContext;
        }
        public async Task GetPDF()
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();

            //Add a page.
            PdfPage page = doc.Pages.Add();

            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();

            //Add values to the list.
            List<CSVViewModel> data = new List<CSVViewModel>();

            List<Artist> artists = _songContext.Artists.ToList();
            foreach (var artist in artists)
            {
                List<Song> songs = _songContext.Songs.Where(s => s.ArtistId == artist.Id).ToList();
                foreach(Song song in songs)
                {
                    SongSpecification? specification = _songContext.Specifications.Where(s => s.SongId == song.Id).FirstOrDefault();
                    if (specification == null)
                    {
                        specification.Beats = 0;
                        specification.Energy = 0;
                        specification.Danceability = 0;
                        specification.Valence = 0;
                        specification.Length = 0;
                        specification.Acousticness = 0;
                    }
                    CSVViewModel model = new CSVViewModel()
                    {
                        Title = song.Title,
                        Artist = artist.Name,
                        Genre = song.Genre,
                        Year = song.Year,
                        BeatsPerMinute =  specification.Beats,
                        Energy =  specification.Energy,
                        Danceability = specification.Danceability,
                        Valence = specification.Valence,
                        Length = specification.Length,
                        Acousticness = specification.Acousticness,
                    };
                    data.Add(model);
                }
            }

            //Add list to IEnumerable.
            IEnumerable<object> dataTable = data;

            //Assign data source.
            pdfGrid.DataSource = dataTable;

            //Apply built-in table style
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

            //Draw the grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));

            //Creating the stream object.
            MemoryStream stream = new MemoryStream();

            //Save the document as a stream.
            doc.Save(stream);

            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;

            //Close the document.
            doc.Close(true);

            //Defining the ContentType for pdf file.
            string contentType = "application/pdf";

            //Define the file name.
            string fileName = "Output.pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name.

            //File(stream, contentType, fileName);
        }

    }
}
