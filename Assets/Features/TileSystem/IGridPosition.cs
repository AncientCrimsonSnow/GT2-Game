using Unity.Mathematics;

namespace Features.TileSystem
{
    public interface IGridPosition
    {
        public int2 WorldPosition { get; }
        public int2 ArrayPosition { get; }
    }
}