using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask {
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType) {
        var usersTime = visits.GroupBy(r => r.UserId)
            .Select(group => (userId: group.Key, group.OrderBy(r => r.DateTime).Bigrams()))
            .SelectMany(info => info.Item2.Where(r => r.First.SlideType == slideType))
            .Select(info => info.Second.DateTime - info.First.DateTime)
            .Where(time => time.TotalMinutes >= 1 && time.TotalMinutes <= 120)
            .Select(time => time.TotalMinutes)
            .ToArray();
        return (usersTime.Length == 0) ? 0 : usersTime.Median();
    }
}