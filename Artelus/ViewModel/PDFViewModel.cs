using Helpers;
using System;

namespace Artelus.ViewModel
{
    public class PDFViewModel : BaseViewModel
    {
        public Action CloseAction { get; set; }
        private string pdfFile;
        public string PdfFile
        {
            get { return pdfFile; }
            set
            {
                if (value != pdfFile)
                {
                    pdfFile = value;
                    RaisePropertyChange("PdfFile");
                }
            }
        }
        public PDFViewModel(string pdf)
        {
            PdfFile = pdf;
        }
    }
}
