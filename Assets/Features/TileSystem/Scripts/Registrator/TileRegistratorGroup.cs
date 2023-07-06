using UnityEngine;

namespace Features.TileSystem.Scripts.Registrator
{
    public class TileRegistratorGroup : MonoBehaviour
    {
        private BaseTileRegistrator[] BaseTileRegistrators { get; set; }
    
        // Start is called before the first frame update
        private void Awake()
        {
            BaseTileRegistrators = GetComponentsInChildren<BaseTileRegistrator>();
            foreach (var baseTileRegistrator in BaseTileRegistrators)
            {
                if (baseTileRegistrator.HasRegistratorGroup)
                {
                    Debug.LogWarning($"There is already a RegistrationGroup registered on: {baseTileRegistrator}");
                    continue;
                }
            
                baseTileRegistrator.AssignToRegistratorGroup(this, () => gameObject.SetActive(false), () => Destroy(gameObject));
            }
        }

        private void OnDestroy()
        {
            Debug.Log(name);
            foreach (var baseTileRegistrator in BaseTileRegistrators)
            {
                baseTileRegistrator.RemoveFromRegistratorGroup();
            }
        }
    }
}
