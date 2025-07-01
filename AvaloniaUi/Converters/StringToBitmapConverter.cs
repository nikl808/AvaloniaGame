using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Globalization;

namespace AvaloniaUi.Converters;

internal class StringToBitmapConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var imagePath = value as string;
        if (!string.IsNullOrEmpty(imagePath))
        {
            var uri = new Uri($"avares://{value as string}");
            return new Bitmap(AssetLoader.Open(uri));
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    
}
