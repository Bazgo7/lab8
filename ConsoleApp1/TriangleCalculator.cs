using System.Globalization;

namespace ConsoleApp1
{
    public static class TriangleCalculator
    {
        public static (string type, List<(int, int)> coordinates, string error) CalculateTriangle(string sA, string sB, string sC)
        {
            try
            {
                // Проверка на числа
                if (!float.TryParse(sA, NumberStyles.Any, CultureInfo.InvariantCulture, out float a) ||
                    !float.TryParse(sB, NumberStyles.Any, CultureInfo.InvariantCulture, out float b) ||
                    !float.TryParse(sC, NumberStyles.Any, CultureInfo.InvariantCulture, out float c) ||
                    a <= 0 || b <= 0 || c <= 0)
                {
                    return ("", new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) }, "Некорректные числа");
                }

                // Проверка на существование
                if (a + b <= c || a + c <= b || b + c <= a)
                {
                    return ("не треугольник", new List<(int, int)> { (-1, -1), (-1, -1), (-1, -1) }, "");
                }

                // Определение типа
                string type = "разносторонний";
                if (a == b && b == c) type = "равносторонний";
                else if (a == b || b == c || a == c) type = "равнобедренный";

                // Координаты (упрощенный расчет)
                double cosA = (b * b + c * c - a * a) / (2 * b * c);
                double sinA = Math.Sqrt(1 - cosA * cosA);
                double x3 = b * cosA, y3 = b * sinA;

                double maxDim = Math.Max(c, Math.Max(x3, y3));
                double scale = 100.0 / (maxDim == 0 ? 1 : maxDim);

                var coords = new List<(int, int)> {
                    (0, 0),
                    ((int)(c * scale), 0),
                    ((int)(x3 * scale), (int)(y3 * scale))
                };

                return (type, coords, "");
            }
            catch (Exception ex)
            {
                return ("", new List<(int, int)> { (-2, -2), (-2, -2), (-2, -2) }, $"Критическая ошибка: {ex.Message}");
            }
        }
    }
}