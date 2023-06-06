using Features.TileSystem;
using Features.TileSystem.TileSystem;
using Unity.Mathematics;
using Zenject;

namespace ZenjectInstaller
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        
        public override void InstallBindings()
        {
            Container.Bind<ITileManager>().To<TileManager>().FromMethod(CreateTileManager).AsSingle();
        }

        private TileManager CreateTileManager(InjectContext arg)
        {
            return new TileManager(new int2(20, 20), new int2(-10,-10));
        }
    }
}