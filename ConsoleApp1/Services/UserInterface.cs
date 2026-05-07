namespace ConsoleApp1
{
    public class UserInterface : IUserInterface
    {
        public async Task<(string side1, string side2, string side3)> GetTriangleSidesAsync()
        {
            Console.WriteLine("Введите стороны A, B и C:");
            
            string side1 = await Task.Run(() => Console.ReadLine() ?? "");
            string side2 = await Task.Run(() => Console.ReadLine() ?? "");
            string side3 = await Task.Run(() => Console.ReadLine() ?? "");

            return (side1, side2, side3);
        }

        public void DisplayResult(string triangleType, List<(int, int)> coordinates, string error)
        {
            Console.WriteLine("\nРезультат:");
            
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine($"Ошибка: {error}");
            }
            else
            {
                Console.WriteLine($"Тип треугольника: {triangleType}");
                Console.WriteLine($"Координаты: {string.Join(", ", coordinates)}");
            }
        }
    }
}
