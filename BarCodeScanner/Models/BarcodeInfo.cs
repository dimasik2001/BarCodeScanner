using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace BarCodeScanner.Models
{
    public class BarcodeInfo
    {
        public Guid Id { get; set; }

        public byte[] Raw { get; init; }

        public string Value { get; init; }

        public string Format { get; init; }

        public IReadOnlyDictionary<string, object> Metadata { get; init; }

        public PointF[] PointsOfInterest { get; init; }
    }
}
