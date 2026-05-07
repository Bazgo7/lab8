namespace ConsoleApp1
{
    public interface IDatabaseService
    {
        Task<TriangleRecord?> GetTriangleRecordAsync(float side1, float side2, float side3);
        Task AddTriangleRecordAsync(TriangleRecord record);
        Task DeleteTriangleRecordAsync(float side1, float side2, float side3);
    }
}