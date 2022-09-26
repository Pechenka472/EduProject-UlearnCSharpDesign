using System;
using System.Drawing;

namespace MyPhotoshop
{
	public class Photo
	{
		public int width;
		public int height;
		public Pixel[] data;
	}

	public class Pixel
	{
		public int X;
		public int Y;
		public Color Color;
	}
}

