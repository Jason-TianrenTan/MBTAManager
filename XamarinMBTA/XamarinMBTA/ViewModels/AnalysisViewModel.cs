using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using XamarinMBTA.Globals;
using XamarinMBTA.Performance;

namespace XamarinMBTA.ViewModels
{
    public class AnalysisViewModel
    {
        public List<DailyAccuracyModel> AccuracyList { get; set; }
        public double MinValue { get; set; } = 100;
        public double MaxValue { get; set; } = -1;
        public AnalysisViewModel()
        {
            AccuracyList = Database.currentAccModel;
            foreach (DailyAccuracyModel item in AccuracyList)
            {
                MinValue = MinValue < item.MinErr ? MinValue : item.MinErr;
                MaxValue = MaxValue > item.MaxErr ? MaxValue : item.MaxErr;
                item.AverErr = (MinValue + MaxValue) / 2;
            }
            MinValue -= 0.4;
            MaxValue += 0.3;
        }
    }
}
