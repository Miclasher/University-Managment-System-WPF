using iText.Kernel.Pdf;
using System.IO;
using University.DataLayer.Models;

namespace University.Domain.Utilities
{
    public static class PdfExporter
    {
        public static void ExportPdfGroup(Stream fs, Group group)
        {
            var pdfGroup = group.ToPdfGroup();

            using var writer = new PdfWriter(fs);
            using var pdf = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdf);

            document.Add(new iText.Layout.Element.Paragraph($"{pdfGroup.CourseName} : {pdfGroup.Name}").SetFontSize(20).SetBold());

            document.Add(new iText.Layout.Element.Paragraph($"Tutor: {pdfGroup.TutorFullName}").SetFontSize(16));

            document.Add(new iText.Layout.Element.Paragraph("Student List:").SetFontSize(16).SetBold());

            int number = 1;

            foreach (var studentFullName in pdfGroup.FullNames)
            {
                document.Add(new iText.Layout.Element.Paragraph($"{number++}) {studentFullName}").SetFontSize(14));
            }

            document.Close();
        }
    }
}
