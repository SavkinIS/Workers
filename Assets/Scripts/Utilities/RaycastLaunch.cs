using UnityEngine;

public class RaycastLaunch
{
    public bool TryGetClickedObject<T>(Vector2 screenPosition, out T hitObject ) where T : MonoBehaviour
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.gameObject.TryGetComponent(out T component))
            {
                hitObject = component;
                return true;
            }
        }

        hitObject = null;
        return false;
    }
}