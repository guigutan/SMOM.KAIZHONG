using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

#pragma warning disable 1591

namespace SIE.CrossPlatform.Collect.PDFPrinter
{
    public class PdfException : Exception
    {

        public PdfException(string message)
            : base(message)
        {
        }

        public PdfException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PdfException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
