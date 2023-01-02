using System.Collections.Generic;
using System.Windows.Media;

namespace SodikmLauncher;

internal class BrickColour
{
	public static Dictionary<int, Brush> RGB = new Dictionary<int, Brush>
	{
		{
			119,
			new SolidColorBrush(Color.FromRgb(164, 189, 71))
		},
		{
			24,
			new SolidColorBrush(Color.FromRgb(245, 205, 48))
		},
		{
			106,
			new SolidColorBrush(Color.FromRgb(218, 133, 65))
		},
		{
			21,
			new SolidColorBrush(Color.FromRgb(196, 40, 28))
		},
		{
			104,
			new SolidColorBrush(Color.FromRgb(107, 50, 124))
		},
		{
			23,
			new SolidColorBrush(Color.FromRgb(13, 105, 172))
		},
		{
			107,
			new SolidColorBrush(Color.FromRgb(0, 143, 156))
		},
		{
			37,
			new SolidColorBrush(Color.FromRgb(75, 151, 75))
		},
		{
			1001,
			new SolidColorBrush(Color.FromRgb(248, 248, 248))
		},
		{
			1,
			new SolidColorBrush(Color.FromRgb(242, 243, 243))
		},
		{
			208,
			new SolidColorBrush(Color.FromRgb(229, 228, 223))
		},
		{
			1002,
			new SolidColorBrush(Color.FromRgb(205, 205, 205))
		},
		{
			194,
			new SolidColorBrush(Color.FromRgb(163, 162, 165))
		},
		{
			199,
			new SolidColorBrush(Color.FromRgb(99, 95, 98))
		},
		{
			26,
			new SolidColorBrush(Color.FromRgb(27, 42, 53))
		},
		{
			1003,
			new SolidColorBrush(Color.FromRgb(17, 17, 17))
		},
		{
			1022,
			new SolidColorBrush(Color.FromRgb(127, 142, 100))
		},
		{
			105,
			new SolidColorBrush(Color.FromRgb(226, 155, 64))
		},
		{
			125,
			new SolidColorBrush(Color.FromRgb(234, 184, 146))
		},
		{
			153,
			new SolidColorBrush(Color.FromRgb(149, 121, 119))
		},
		{
			1023,
			new SolidColorBrush(Color.FromRgb(140, 91, 159))
		},
		{
			135,
			new SolidColorBrush(Color.FromRgb(116, 134, 157))
		},
		{
			102,
			new SolidColorBrush(Color.FromRgb(110, 153, 202))
		},
		{
			151,
			new SolidColorBrush(Color.FromRgb(120, 144, 130))
		},
		{
			5,
			new SolidColorBrush(Color.FromRgb(215, 197, 154))
		},
		{
			226,
			new SolidColorBrush(Color.FromRgb(253, 234, 141))
		},
		{
			1005,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 175, 0))
		},
		{
			101,
			new SolidColorBrush(Color.FromRgb(218, 134, 122))
		},
		{
			9,
			new SolidColorBrush(Color.FromRgb(232, 186, 200))
		},
		{
			11,
			new SolidColorBrush(Color.FromRgb(128, 187, 219))
		},
		{
			1018,
			new SolidColorBrush(Color.FromRgb(18, 238, 212))
		},
		{
			29,
			new SolidColorBrush(Color.FromRgb(161, 196, 140))
		},
		{
			1030,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 204, 153))
		},
		{
			1029,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, 204))
		},
		{
			1025,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 201, 201))
		},
		{
			1016,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 102, 204))
		},
		{
			1026,
			new SolidColorBrush(Color.FromRgb(177, 167, byte.MaxValue))
		},
		{
			1024,
			new SolidColorBrush(Color.FromRgb(175, 221, byte.MaxValue))
		},
		{
			1027,
			new SolidColorBrush(Color.FromRgb(159, 243, 233))
		},
		{
			1028,
			new SolidColorBrush(Color.FromRgb(204, byte.MaxValue, 204))
		},
		{
			1008,
			new SolidColorBrush(Color.FromRgb(193, 190, 66))
		},
		{
			1009,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, 0))
		},
		{
			1017,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 175, 0))
		},
		{
			1004,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 0, 0))
		},
		{
			1032,
			new SolidColorBrush(Color.FromRgb(byte.MaxValue, 0, 191))
		},
		{
			1010,
			new SolidColorBrush(Color.FromRgb(0, 0, byte.MaxValue))
		},
		{
			1019,
			new SolidColorBrush(Color.FromRgb(0, byte.MaxValue, byte.MaxValue))
		},
		{
			1020,
			new SolidColorBrush(Color.FromRgb(0, byte.MaxValue, 0))
		},
		{
			217,
			new SolidColorBrush(Color.FromRgb(124, 92, 70))
		},
		{
			18,
			new SolidColorBrush(Color.FromRgb(204, 142, 105))
		},
		{
			38,
			new SolidColorBrush(Color.FromRgb(160, 95, 53))
		},
		{
			1031,
			new SolidColorBrush(Color.FromRgb(98, 37, 209))
		},
		{
			1006,
			new SolidColorBrush(Color.FromRgb(180, 128, byte.MaxValue))
		},
		{
			1013,
			new SolidColorBrush(Color.FromRgb(4, 175, 236))
		},
		{
			45,
			new SolidColorBrush(Color.FromRgb(180, 210, 228))
		},
		{
			1021,
			new SolidColorBrush(Color.FromRgb(58, 125, 21))
		},
		{
			192,
			new SolidColorBrush(Color.FromRgb(105, 64, 40))
		},
		{
			1014,
			new SolidColorBrush(Color.FromRgb(170, 85, 0))
		},
		{
			1007,
			new SolidColorBrush(Color.FromRgb(163, 75, 75))
		},
		{
			1015,
			new SolidColorBrush(Color.FromRgb(170, 0, 170))
		},
		{
			1012,
			new SolidColorBrush(Color.FromRgb(33, 84, 185))
		},
		{
			1011,
			new SolidColorBrush(Color.FromRgb(0, 32, 96))
		},
		{
			28,
			new SolidColorBrush(Color.FromRgb(40, 127, 71))
		},
		{
			141,
			new SolidColorBrush(Color.FromRgb(39, 70, 45))
		}
	};
}
