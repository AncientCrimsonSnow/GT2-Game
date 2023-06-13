using Unity.Mathematics;

namespace Features.TileSystem.Scripts
{
    public interface IGridPosition
    {
        public int2 WorldPosition { get; }
        public int2 ArrayPosition { get; }
    }
}