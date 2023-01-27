using Data;
using Data.Entities;
using Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Service
{
    public class PDFService: IPDFService
    {
        public readonly SongContext _songContext;
        public PDFService(SongContext songContext)
        {
            _songContext = songContext;
        }

        public ActionResult ArtistStatistics()
        {
            List<PDFArtist> data = new List<PDFArtist>();

            List<Artist> artists = _songContext.Artists.ToList();
            foreach (Artist artist in artists)
            {
                int numberOfSongs = _songContext.Songs.Where(s => s.ArtistId == artist.Id).ToList().Count();
                PDFArtist record = new PDFArtist { Artist = artist.Name, NumberOfSong = numberOfSongs };
                data.Add(record);
            }
            data.Sort((a, b) => a.NumberOfSong - b.NumberOfSong);
            data.Reverse();

            MemoryStream stream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            // Open the document to enable you to write to the document  
            document.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner   
            Paragraph heading = new Paragraph("List of artists and how many songs they have!");
            heading.Alignment = Element.ALIGN_CENTER;
            heading.Font.Size = 30;
            document.Add(heading);
            document.Add(new Paragraph(" "));
            PdfPTable table = new PdfPTable(2);
            foreach(PDFArtist record in data)
            {
                table.AddCell(record.Artist);
                table.AddCell($"{record.NumberOfSong}");
            }
            document.Add(table);
            // Close the document  
            document.Close();
            // Close the writer instance  
            writer.Close();

            FileStreamResult fsr = new FileStreamResult(new MemoryStream(stream.ToArray()), "application/pdf");
            fsr.FileDownloadName = "ArtistStatistics.pdf";
            return fsr;
        }

        public ActionResult SongStatistics()
        {
            List<PDFSong> data = new List<PDFSong>();
            
            List<Song> songs = _songContext.Songs.ToList();
            foreach (Song song in songs)
            {
                SongSpecification? songSpecification = _songContext.Specifications.Where(s => s.SongId == song.Id).FirstOrDefault();
                if (songSpecification != null)
                {
                    PDFSong record = new PDFSong() { SongName = song.Title, Genre = song.Genre, Length = songSpecification.Length };
                    data.Add(record);
                }
            }
            
            data.Sort((a, b) => a.Length - b.Length);
            data.Reverse();

            PDFSong[] topFive = data.GetRange(0, 5).ToArray();
            SortedDictionary<string, List<PDFSong>> dataTable = new SortedDictionary<string, List<PDFSong>>();
            foreach (PDFSong record in data)
            {
                if(record != null)
                {
                    string genre = record.Genre??"Other";
                    if (!dataTable.ContainsKey(genre))
                    {
                        dataTable.Add(genre, new List<PDFSong>());
                    }
                    dataTable[genre].Add(record);
                }
            }

            MemoryStream stream = new MemoryStream();
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            // Open the document to enable you to write to the document  
            document.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner   
            Paragraph heading = new Paragraph("Top 5 longest songs");
            heading.Alignment = Element.ALIGN_CENTER;
            heading.Font.Size = 30;
            document.Add(heading);
            document.Add(new Paragraph(" "));
            PdfPTable table = new PdfPTable(3);
            for (int i = 0; i < 5; i++)
            {
                table.AddCell(topFive[i].SongName);
                table.AddCell(topFive[i].Genre);
                table.AddCell($"{topFive[i].Length}");
            }
            document.Add(table);

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            foreach (string genre in dataTable.Keys)
            {
                PdfPTable table1 = new PdfPTable(2);
                Paragraph top = new Paragraph (genre);
                top.Alignment = Element.ALIGN_CENTER;
                top.Font.Size = 20;
                document.Add (top);
                document.Add(new Paragraph(" "));
                foreach (PDFSong record in dataTable[genre])
                {
                    table1.AddCell(record.SongName);
                    table1.AddCell($"{record.Length}");
                }
                document.Add(table1);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph(" "));
            }
            // Close the document  
            document.Close();
            // Close the writer instance  
            writer.Close();

            FileStreamResult fsr = new FileStreamResult(new MemoryStream(stream.ToArray()), "application/pdf");
            fsr.FileDownloadName = "SongStatistics.pdf";
            return fsr;
        }
    }
}
