using UnityEngine;

namespace Features.Input
{
    [CreateAssetMenu(fileName = "SkeletonFocus", menuName = "Focus/Skeleton")]
    public class SkeletonFocus : Focus_SO<GameObject>
    {
        public Vector3Int originPosition;

        public void SetOriginPosition(Vector3Int position)
        {
            originPosition = position;
        }

        public Vector3Int GetOriginPosition()
        {
            return originPosition;
        }
    }
}
