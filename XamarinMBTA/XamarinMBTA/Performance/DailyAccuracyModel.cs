using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinMBTA.Performance
{
    public class DailyAccuracyModel
    {
        private string _date;
        public string Date { get
            {
                return this._date;
            }
            set
            {
                _date = value;
            }
        }
        public double MaxErr { get; set; }
        public double MinErr { get; set; }
        public double Accuracy1 { get; set; }
        public double Accuracy2 { get; set; }
        public double AverErr { get; set; }
    }
}
