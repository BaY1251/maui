﻿using Xunit;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.Maui.Platform;
using System.Threading.Tasks;
using Microsoft.UI.Composition;

namespace Microsoft.Maui.DeviceTests
{
	public partial class BorderTests : ControlsHandlerTestBase
	{
		[Theory(DisplayName = "Inner CornerRadius Initializes Correctly")]
		[InlineData(0)]
		[InlineData(12)]
		[InlineData(24)]
		public async Task InnerCornerRadiusInitializesCorrectly(int cornerRadius)
		{
			SetupBuilder();

			var expected = Colors.Red;

			var border = new Border()
			{
				Content = new Label { Text = "Background", TextColor = Colors.White },
				StrokeShape = new RoundRectangle { CornerRadius = cornerRadius },
				Background = new SolidPaint(expected),
				StrokeThickness = 0,
				HeightRequest = 100,
				WidthRequest = 300
			};
			
			await AttachAndRun(border, (handler) =>
			{
				var contentPanel = GetNativeBorder(handler as BorderHandler);
				var content = contentPanel.Content;
				var visual = ElementCompositionPreview.GetElementVisual(content);

				var clip = visual.Clip as CompositionGeometricClip;
				Assert.NotNull(clip);

				var geometry = clip.Geometry as CompositionPathGeometry;
				var path = geometry.Path;
				Assert.NotNull(path);

				Assert.True(contentPanel.IsInnerPath);
			});

			await AssertColorAtPoint(border, expected, typeof(BorderHandler), cornerRadius, cornerRadius);
		}

		ContentPanel GetNativeBorder(BorderHandler borderHandler) =>
			borderHandler.PlatformView;

	}
}