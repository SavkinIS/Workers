using UnityEngine;
using UnityEngine.UI;

public class InputViewInformer : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private TMPro.TextMeshProUGUI _leftButton;
    [SerializeField] private TMPro.TextMeshProUGUI _rightButton;
    [SerializeField] private TMPro.TextMeshProUGUI _upButton;
    [SerializeField] private TMPro.TextMeshProUGUI _downButton;

    private void Awake()
    {
        _leftButton.text = $"{_input.LeftButton}";
        _rightButton.text = $"{_input.RightButton}";
        _upButton.text = $"{_input.UpButton}";
        _downButton.text = $"{_input.Downutton}";
    }
}