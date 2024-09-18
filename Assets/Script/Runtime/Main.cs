using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("K");
            Load();
        }
    }

    private void Load()
    {
        var h = Addressables.LoadAssetAsync<GameObject>("Assets/Artwork/Cube.prefab");
        h.Completed += (AsyncOperationHandle<GameObject> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject go = handle.Result;
                Instantiate(go);
                Debug.Log("Completed");
            }
            else
            {
                Debug.LogError("Failed to load asset");
            }
        };

        AsyncOperationHandle<IList<IResourceLocation>> locations = Addressables.LoadResourceLocationsAsync("Assets/Artwork/", typeof(Object));
        locations.Completed += (AsyncOperationHandle<IList<IResourceLocation>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (IResourceLocation location in handle.Result)
                {
                    Debug.Log(location.PrimaryKey);
                }
            }
            else
            {
                Debug.LogError("Failed to load locations");
            }
        };
    }
}