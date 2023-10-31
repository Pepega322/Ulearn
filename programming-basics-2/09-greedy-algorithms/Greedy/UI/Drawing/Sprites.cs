using System;
using System.IO;
using Avalonia;
using Avalonia.Media.Imaging;

namespace Greedy.UI.Drawing;

public static class Sprites
{
	private static readonly string spritesFolder =
        System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "UI", "Drawing", "images");

	private static Size spriteSize => Wall.Size;

	public static readonly Bitmap Wall = new(System.IO.Path.Combine(spritesFolder, "wall.png"));

	public static readonly Bitmap PlayerStanding = new(System.IO.Path.Combine(spritesFolder, "standing.png"));

	public static readonly Bitmap PlayerWon =
		new(System.IO.Path.Combine(spritesFolder, "has-won.png"));

	public static readonly Bitmap PlayerDead =
		new(System.IO.Path.Combine(spritesFolder, "is-dead.png"));

	public static readonly Bitmap PlayerRunningRight1 =
		new(System.IO.Path.Combine(spritesFolder, "running-right-1.png"));

	public static readonly Bitmap PlayerRunningRight2 =
		new(System.IO.Path.Combine(spritesFolder, "running-right-2.png"));

	public static readonly Bitmap PlayerRunningLeft1 =
		new(System.IO.Path.Combine(spritesFolder, "running-left-1.png"));

	public static readonly Bitmap PlayerRunningLeft2 =
		new(System.IO.Path.Combine(spritesFolder, "running-left-2.png"));

	public static readonly Bitmap PlayerClimbedDown1 =
		new(System.IO.Path.Combine(spritesFolder, "climbing-1.png"));

	public static readonly Bitmap PlayerClimbedDown2 =
		new(System.IO.Path.Combine(spritesFolder, "climbing-2.png"));

	public static readonly Bitmap ChestOpened = new(System.IO.Path.Combine(spritesFolder, "chest-opened.png"));
	public static readonly Bitmap ChestClosed = new(System.IO.Path.Combine(spritesFolder, "chest-closed.png"));
}