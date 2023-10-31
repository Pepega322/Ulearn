using System;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking {
    public class Benchmark : IBenchmark {
        public double MeasureDurationInMs(ITask task, int repetitionCount) {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            task.Run();

            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < repetitionCount; i++)
                task.Run();
            watch.Stop();

            return (double)watch.ElapsedMilliseconds / repetitionCount;
        }
    }

    public class StringConstructor : ITask {
        public void Run() => new string('a', 10000);
    }

    public class StringBuilderConstructor : ITask {
        public void Run() {
            var builder = new StringBuilder();
            for (var i = 0; i < 10000; i++)
                builder.Append('a');
            builder.ToString();
        }
    }

    [TestFixture]
    public class RealBenchmarkUsageSample {
        [Test]
        public void StringConstructorFasterThanStringBuilder() {
            var benchmark = new Benchmark();
            var stringTask = new StringConstructor() ;
            var builderTask = new StringBuilderConstructor();
            var stringTime = benchmark.MeasureDurationInMs(stringTask, 10000);
            var builderTime = benchmark.MeasureDurationInMs(builderTask, 10000);
            Assert.Less(stringTime, builderTime);
        }
    }
}