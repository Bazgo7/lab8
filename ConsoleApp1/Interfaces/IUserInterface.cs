namespace ConsoleApp1
{
    public interface IUserInterface
    {
        Task<(string side1, string side2, string side3)> GetTriangleSidesAsync();
        void DisplayResult(string triangleType, List<(int, int)> coordinates, string error);
    }
}