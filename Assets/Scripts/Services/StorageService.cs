using UnityEngine;

public class StorageService : MonoBehaviour
{
    [SerializeField] private Storage _storageStart;
    [SerializeField] private WorkerSpawner workerSpawner;
    [SerializeField] private StorageSpawner _storageSpawner;
    [SerializeField] private int _workerPrice = 3;
    [SerializeField] private int _storagePrice = 5;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private DragObject _dragObject;

    private readonly RaycastLaunch _raycastLaunch = new RaycastLaunch();
    private readonly ResourceService _resourceService = new ResourceService();
    
    private Storage _selectedStorage;

    private void OnEnable()
    {
        _playerInput.MouseClicked += MouseClicked;
    }

    private void Awake()
    {
        _storageStart.Initialize(_workerPrice, _storagePrice, BuildNewStorage, _resourceService);
        _dragObject.Initialize();
    }

    private void OnDisable()
    {
        _playerInput.MouseClicked -= MouseClicked;
    }

    private void MouseClicked(Vector2 screenPosition)
    {
        if (_selectedStorage == null)
        {
            if (_raycastLaunch.TryGetClickedObject<Storage>(screenPosition, out Storage storage))
            {
                if (storage != null)
                {
                    if (storage.CanBuildNext == false)
                        return;
                        
                    _selectedStorage = storage;
                    _selectedStorage.Flag.Activate();
                    _dragObject.SetTarget(_selectedStorage.Flag.transform);
                }
            }
        }
        else
        {
            if (_raycastLaunch.TryGetClickedObject<Storage>(screenPosition, out Storage storage))
            {
                if (storage == _selectedStorage)
                {
                    _selectedStorage.Flag.ResetPosition();
                    _dragObject.ResetTarget();
                    _selectedStorage =  null;
                }
                return;
            }
            
            if (_selectedStorage.Flag.IsActive)
            {
                _selectedStorage.Flag.Deactivate();
                _selectedStorage.EnableNewStorageConstruction();
                _dragObject.ResetTarget();
                _selectedStorage =  null;
            }
        }
    }
    
    private void BuildNewStorage(Vector3 newPosition, Worker worker)
    {
        Storage newStorage = _storageSpawner.Spawn(newPosition);
        newStorage.Initialize(_workerPrice, _storagePrice, BuildNewStorage, _resourceService, worker);
    }
}