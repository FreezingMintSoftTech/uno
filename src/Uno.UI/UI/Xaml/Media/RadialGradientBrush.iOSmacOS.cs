using System;
using Windows.Foundation;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Uno.Extensions;

namespace Windows.UI.Xaml.Media
{
	partial class RadialGradientBrush
	{
		internal override CALayer GetLayer(CGSize size)
		{
			var center = Center;
			var radiusX = RadiusX;
			var radiusY = RadiusY;

			nfloat radius;

			var transform = RelativeTransform;

			if (MappingMode == BrushMappingMode.RelativeToBoundingBox)
			{
				center = new Point(center.X * size.Width, Center.Y * size.Height);
				radius = (nfloat)(radiusX * size.Width + radiusY * size.Height) / 4.0f; // We take the avg
			}
			else
			{
				radius = (nfloat) (radiusX + radiusY) / 2.0f; // We take the avg
			}

			var colors = GradientStops.SelectToArray(gs => (CGColor)gs.Color);
			var locations = GradientStops.SelectToArray(gs => new nfloat(gs.Offset));


			var context = UIGraphics.GetCurrentContext();

			var gradient = new CGGradient(CGColorSpace.CreateDeviceRGB(), colors, locations);

			var startCenter = new CGPoint(center.X, center.Y);
			context.DrawRadialGradient(gradient, startCenter, 0, startCenter, radius, CGGradientDrawingOptions.DrawsAfterEndLocation);

			var layer = new CALayer();

			// ??????

			return layer;




		}
	}
}
