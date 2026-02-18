using Mario.Entities;

namespace Mario.Services
{
    public interface IMarioServices
    {
        public Task<MarioEntity?> GetMarioLevelStatusAsync(string move);
    }
}
