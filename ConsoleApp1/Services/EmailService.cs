namespace ConsoleApp1
{
    public class EmailService : IEmailService
    {
        public async Task SendResultAsync(string triangleType, List<(int, int)> coordinates)
        {
            // Имитация отправки email
            await Task.Delay(100); // Имитация сетевой задержки
            
            string coordinatesStr = string.Join(", ", coordinates);
            string message = $"Результат анализа треугольника:\nТип: {triangleType}\nКоординаты: {coordinatesStr}";
            
            // В реальном приложении здесь был бы код для отправки email
            Console.WriteLine($"[EMAIL SERVICE] Отправка результата: {message}");
            
            // Логирование имитации отправки
            await File.AppendAllTextAsync("email_log.txt", 
                $"{DateTime.UtcNow}: Отправлен результат - Тип: {triangleType}, Координаты: {coordinatesStr}\n");
        }
    }
}
