using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class DatabaseService : IDatabaseService
    {
        private readonly Lab8DbContext _context;

        public DatabaseService(Lab8DbContext context)
        {
            _context = context;
        }

        public async Task<TriangleRecord?> GetTriangleRecordAsync(float side1, float side2, float side3)
        {
            return await _context.TriangleRecords
                .FirstOrDefaultAsync(tr => tr.Side1 == side1 && tr.Side2 == side2 && tr.Side3 == side3);
        }

        public async Task AddTriangleRecordAsync(TriangleRecord record)
        {
            // Генерируем уникальный ID на основе сторон
            record.Id = GenerateId(record.Side1, record.Side2, record.Side3);
            
            _context.TriangleRecords.Add(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTriangleRecordAsync(float side1, float side2, float side3)
        {
            var record = await GetTriangleRecordAsync(side1, side2, side3);
            if (record != null)
            {
                _context.TriangleRecords.Remove(record);
                await _context.SaveChangesAsync();
            }
        }

        private int GenerateId(float side1, float side2, float side3)
        {
            // Создаем уникальный ID на основе сторон
            int hash = 17;
            hash = hash * 31 + side1.GetHashCode();
            hash = hash * 31 + side2.GetHashCode();
            hash = hash * 31 + side3.GetHashCode();
            return Math.Abs(hash);
        }
    }
}
