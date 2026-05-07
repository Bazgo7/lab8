using System.Globalization;

namespace ConsoleApp1
{
    public class TriangleController
    {
        private readonly IDatabaseService _databaseService;
        private readonly IUserInterface _userInterface;
        private readonly IEmailService _emailService;

        public TriangleController(IDatabaseService databaseService, IUserInterface userInterface, IEmailService emailService)
        {
            _databaseService = databaseService;
            _userInterface = userInterface;
            _emailService = emailService;
        }

        public async Task ProcessTriangleAsync()
        {
            try
            {
                // 1. У пользователя запрашиваются входные данные
                var (side1, side2, side3) = await _userInterface.GetTriangleSidesAsync();

                // Проверка корректности ввода чисел
                if (!float.TryParse(side1, NumberStyles.Any, CultureInfo.InvariantCulture, out float s1) ||
                    !float.TryParse(side2, NumberStyles.Any, CultureInfo.InvariantCulture, out float s2) ||
                    !float.TryParse(side3, NumberStyles.Any, CultureInfo.InvariantCulture, out float s3))
                {
                    _userInterface.DisplayResult("", new List<(int, int)>(), "Некорректные числа");
                    return;
                }

                // 2. Осуществляется проверка: если для данных нет записи в БД => вычисляется результат и добавляется запись в БД
                var existingRecord = await _databaseService.GetTriangleRecordAsync(s1, s2, s3);
                
                string triangleType;
                List<(int, int)> coordinates;
                string error;

                if (existingRecord != null)
                {
                    // Результат берется из БД
                    triangleType = existingRecord.TriangleType;
                    error = existingRecord.ErrorMessage;
                    
                    // Восстанавливаем координаты из строки
                    coordinates = ParseCoordinates(existingRecord.Coordinates);
                }
                else
                {
                    // Вычисляется результат
                    var result = TriangleCalculator.CalculateTriangle(side1, side2, side3);
                    triangleType = result.type;
                    coordinates = result.coordinates;
                    error = result.error;

                    // Добавляется запись в БД
                    var newRecord = new TriangleRecord
                    {
                        Side1 = s1,
                        Side2 = s2,
                        Side3 = s3,
                        TriangleType = triangleType,
                        ErrorMessage = error,
                        Coordinates = string.Join(";", coordinates.Select(c => $"{c.Item1},{c.Item2}"))
                    };

                    await _databaseService.AddTriangleRecordAsync(newRecord);
                }

                // 3. Осуществляется отправка строки-результата сторонней зависимости
                if (!string.IsNullOrEmpty(triangleType))
                {
                    await _emailService.SendResultAsync(triangleType, coordinates);
                }

                // 4. Метод возвращает результат выполнения операции
                _userInterface.DisplayResult(triangleType, coordinates, error);
            }
            catch (Exception ex)
            {
                _userInterface.DisplayResult("", new List<(int, int)>(), $"Критическая ошибка: {ex.Message}");
            }
        }

        private List<(int, int)> ParseCoordinates(string coordinatesStr)
        {
            var coordinates = new List<(int, int)>();
            
            if (string.IsNullOrEmpty(coordinatesStr))
                return coordinates;

            var pairs = coordinatesStr.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                var parts = pair.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                {
                    coordinates.Add((x, y));
                }
            }

            return coordinates;
        }
    }
}
