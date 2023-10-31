using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static linq_slideviews.ParsingTask;

namespace linq_slideviews;

public class ParsingTask {
    public static Func<string, bool> IsInt =
        s => int.TryParse(s, out int i);
    public static Func<string, bool> IsDate =
        s => DateOnly.TryParse(s, out DateOnly i);
    public static Func<string, bool> IsTime =
        s => TimeOnly.TryParse(s, out TimeOnly i);
    public static Func<string, bool> IsSlideType =
        s => Enum.TryParse(s, true, out SlideType type);
    public static Func<string, int> ToInt =
        s => int.Parse(s);
    public static Func<string, DateOnly> ToDate =
        s => DateOnly.Parse(s);
    public static Func<string, TimeOnly> ToTime =
        s => TimeOnly.Parse(s);
    public static Func<string, SlideType> ToSlideType =
        s => Enum.Parse<SlideType>(s, true);
    public static Func<string[], bool> IsSlideRecord =
        s => s.Length == 3 && IsInt(s[0]) && IsSlideType(s[1]);
    public static Func<string[], bool> IsVisitRecord =
        s => s.Length == 4 && IsInt(s[0]) && IsInt(s[1]) && IsDate(s[2]) && IsTime(s[3]);
    public static Func<DateOnly, TimeOnly, DateTime> GetDateTime =
        (d, t) => new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, t.Second);

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines) {
        return lines
            .Skip(1)
            .Select(line => line.ToLower().Split(';'))
            .Where(info => IsSlideRecord(info))
            .Select(info => new SlideRecord(ToInt(info[0]), ToSlideType(info[1]), info[2]))
            .ToDictionary(slide => slide.SlideId, slide => slide);
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides) {
        var visitInfo = lines.Skip(1)
           .Select(l => l.Split(';'));
        var wrongLine = visitInfo
            .Where(info => !IsVisitRecord(info) || !slides.ContainsKey(ToInt(info[1])))
            .Take(1);
        if (wrongLine.Any()) {
            var message = wrongLine
                .Select(info => string.Join(';', info))
            .First();
            throw new FormatException($"Wrong line [{message}]");
        }
        return visitInfo
            .Select(l => new VisitRecord(ToInt(l[0]), ToInt(l[1]),
                GetDateTime(ToDate(l[2]), ToTime(l[3])),
                slides[ToInt(l[1])].SlideType));
    }
}
