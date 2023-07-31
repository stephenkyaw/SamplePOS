using Application.DTOs.Point;

namespace Application.Interfaces
{
    public interface IPointService
    {
        Task<bool> AddPoint (PointDto pointDto);
    }
}
