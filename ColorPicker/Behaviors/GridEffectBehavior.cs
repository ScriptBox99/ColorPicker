﻿using System.Windows;
using ColorPicker.Shaders;
using Microsoft.Xaml.Behaviors;

namespace ColorPicker.Behaviors
{
    public class GridEffectBehavior : Behavior<FrameworkElement>
    {
        private static double baseZoomImageSizeInPx = 50;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/dependency-property-security#:~:text=Dependency%20properties%20should%20generally%20be%20considered%20to%20be,make%20security%20guarantees%20about%20a%20dependency%20property%20value.")]
        public static DependencyProperty EffectProperty = DependencyProperty.Register("Effect", typeof(GridShaderEffect), typeof(GridEffectBehavior));
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/dependency-property-security#:~:text=Dependency%20properties%20should%20generally%20be%20considered%20to%20be,make%20security%20guarantees%20about%20a%20dependency%20property%20value.")]
        public static DependencyProperty ZoomFactorProperty = DependencyProperty.Register("ZoomFactor", typeof(double), typeof(GridEffectBehavior));

        public GridShaderEffect Effect
        {
            get { return (GridShaderEffect)GetValue(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }

        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        private void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var position = e.GetPosition(AssociatedObject);

            var relativeX = position.X / AssociatedObject.ActualWidth;
            var relativeY = position.Y / AssociatedObject.Height;
            Effect.MousePosition = new Point(relativeX, relativeY);
            if (ZoomFactor >= 4)
            {
                Effect.Radius = 0.04;
                Effect.SquareSize = ZoomFactor;
                Effect.TextureSize = baseZoomImageSizeInPx * ZoomFactor;
            }
            else
            {
                // don't show grid, too small pixels
                Effect.Radius = 0.0;
                Effect.SquareSize = 0;
                Effect.TextureSize = 0;
            }
        }
    }
}
