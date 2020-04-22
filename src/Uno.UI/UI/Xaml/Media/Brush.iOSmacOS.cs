using System;
using System.Collections.Generic;
using System.Text;
using Uno.Extensions;
using CoreGraphics;
using Uno.Disposables;
using Windows.UI.Xaml.Media;

#if __IOS__
using UIKit;
using _Image = UIKit.UIImage;
#elif __MACOS__
using AppKit;
using _Image = AppKit.NSImage;
#endif

namespace Windows.UI.Xaml.Media
{
	// iOS partial for SolidColorBrush
	public partial class Brush
	{
		internal static IDisposable AssignAndObserveBrush(Brush b, Action<CGColor> colorSetter)
		{
			var disposables = new CompositeDisposable(2);

			if (b is SolidColorBrush colorBrush)
			{
				colorSetter(colorBrush.ColorWithOpacity);

				colorBrush.RegisterDisposablePropertyChangedCallback(
						SolidColorBrush.ColorProperty,
						(s, colorArg) => colorSetter((s as SolidColorBrush).ColorWithOpacity)
					)
					.DisposeWith(disposables);

				colorBrush.RegisterDisposablePropertyChangedCallback(
						OpacityProperty,
						(s, colorArg) => colorSetter((s as SolidColorBrush).ColorWithOpacity)
					)
					.DisposeWith(disposables);
			}

			if (b is GradientBrush gradientBrush)
			{
				colorSetter(gradientBrush.FallbackColorWithOpacity);

				gradientBrush.RegisterDisposablePropertyChangedCallback(
						GradientBrush.FallbackColorProperty,
						(s, colorArg) => colorSetter((s as GradientBrush).FallbackColorWithOpacity)
					)
					.DisposeWith(disposables);

				gradientBrush.RegisterDisposablePropertyChangedCallback(
						OpacityProperty,
						(s, colorArg) => colorSetter((s as GradientBrush).FallbackColorWithOpacity)
					)
					.DisposeWith(disposables);
			}
			else if (b is ImageBrush imageBrush)
			{
				void ImageChanged(UIImage _) => colorSetter(SolidColorBrushHelper.Transparent.Color);

				imageBrush.ImageChanged += ImageChanged;

				disposables.Add(() => imageBrush.ImageChanged -= ImageChanged);
			}
			else
			{
				colorSetter(SolidColorBrushHelper.Transparent.Color);
			}

			return disposables;
		}
	}
}
