using System;
using System.Collections.Generic;

namespace StructBenchmarking {
    public enum TaskType {
        ArrayCreateClass,
        ArrayCreateStruct,
        MethodCallClass,
        MethodCallStruct,
    }

    public interface IExperiment {
        ITask CreateTask(TaskType type, int size);
        List<ExperimentResult> GetResults(TaskType type,
            IBenchmark benchmark, int repetitionCount);
    }

    public class Experiment : IExperiment {
        public ITask CreateTask(TaskType type, int size) {
            switch (type) {
                case TaskType.ArrayCreateClass:
                    return new ClassArrayCreationTask(size);
                case TaskType.ArrayCreateStruct:
                    return new StructArrayCreationTask(size);
                case TaskType.MethodCallClass:
                    return new MethodCallWithClassArgumentTask(size);
                case TaskType.MethodCallStruct:
                    return new MethodCallWithStructArgumentTask(size);
                default:
                    throw new ArgumentException();
            }
        }

        public List<ExperimentResult> GetResults(TaskType type,
            IBenchmark benchmark, int repetitionCount) {
            var results = new List<ExperimentResult>();
            foreach (var size in Constants.FieldCounts) {
                var task = CreateTask(type, size);
                var time = benchmark.MeasureDurationInMs(task, repetitionCount);
                results.Add(new ExperimentResult(size, time));
            }
            return results;
        }
    }

    public class Experiments {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount) {
            var experiment = new Experiment();
            return new ChartData {
                Title = "Create array",
                ClassPoints = experiment.GetResults(TaskType.ArrayCreateClass, benchmark, repetitionsCount),
                StructPoints = experiment.GetResults(TaskType.ArrayCreateStruct, benchmark, repetitionsCount),
            };
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount) {
            var experiment = new Experiment();
            return new ChartData {
                Title = "Call method with argument",
                ClassPoints = experiment.GetResults(TaskType.MethodCallClass, benchmark, repetitionsCount),
                StructPoints = experiment.GetResults(TaskType.MethodCallStruct, benchmark, repetitionsCount),
            };
        }
    }
}