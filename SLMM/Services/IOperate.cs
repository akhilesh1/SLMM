using SLMM.Models;
using System.Threading.Tasks;

namespace SLMM.Services
{
    public interface IOperate
    {
        Task<Position> GetPosition();
        Task Reset(int length, int width);
        Task<Position> Turn(bool isLeftTurn);
        Task<Position> MoveForward();
    }
}
