using System;
using System.IO;

namespace Greedy.Architecture;

public static class Folders
{
	public static readonly DirectoryInfo StatesForStudents = new(
        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "states-for-students"));

	public static readonly DirectoryInfo NotGreedyStatesForStudents =
		new(System.IO.Path.Combine(StatesForStudents.FullName, "not-greedy-states"));
}