using FinalExam.DAL;
using Microsoft.EntityFrameworkCore;

namespace FinalExam.Service
{
    public class LayoutService
    {
        
        private readonly AppDbContext _db;
        public LayoutService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<Dictionary<string, string>> GetSettingsAsync()
        {
            Dictionary<string, string> setting = await _db.Settings.ToDictionaryAsync(p => p.Key, p => p.Value);
            return setting;
        }
        
    }
}
