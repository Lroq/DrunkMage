using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class InputRebinder : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject controlsDisplayMenu;
    [SerializeField] private GameObject dimPanel; // Reference to the DimPanel
    [SerializeField] private RectTransform highlight; // Reference to the Highlight RectTransform

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    void Start()
    {
        // Initialize the rebinding UI for each action
        SetupRebindUI("MoveUp", "ActionItem\"MoveUp\"/MoveUpBind", "ActionItem\"MoveUp\"/RebindMoveUpButton");
        SetupRebindUI("MoveDown", "ActionItem\"MoveDown\"/MoveDownBind", "ActionItem\"MoveDown\"/RebindMoveDownButton");
        SetupRebindUI("MoveLeft", "ActionItem\"MoveLeft\"/MoveLeftBind", "ActionItem\"MoveLeft\"/RebindMoveLeftButton");
        SetupRebindUI("MoveRight", "ActionItem\"MoveRight\"/MoveRightBind", "ActionItem\"MoveRight\"/RebindMoveRightButton");
        SetupRebindUI("Fire", "ActionItem\"Shoot\"/ShootBind", "ActionItem\"Shoot\"/RebindShootButton");
        SetupRebindUI("Dash", "ActionItem\"Dodge\"/DodgeBind", "ActionItem\"Dodge\"/RebindDodgeButton");
    }

    private void SetupRebindUI(string actionName, string bindingPath, string buttonPath)
    {
        Transform bindingTextTransform = controlsDisplayMenu.transform.Find(bindingPath);
        Transform rebindButtonTransform = controlsDisplayMenu.transform.Find(buttonPath);

        if (bindingTextTransform == null || rebindButtonTransform == null)
        {
            Debug.LogWarning($"UI elements not found for action: {actionName}");
            return;
        }

        TMP_Text bindingText = bindingTextTransform.GetComponent<TMP_Text>();
        Button rebindButton = rebindButtonTransform.GetComponent<Button>();

        // Update the binding display text
        UpdateBindingDisplay(actionName, bindingText);

        // Attach the rebinding functionality to the button
        rebindButton.onClick.AddListener(() => StartRebinding(actionName, bindingText, rebindButton));
    }

    private void UpdateBindingDisplay(string actionName, TMP_Text bindingText)
    {
        InputAction action = playerInput.actions[actionName];
        if (action != null && action.bindings.Count > 0)
        {
            bindingText.text = action.bindings[0].ToDisplayString();
        }
        else
        {
            bindingText.text = "Unbound";
        }
    }

    public void StartRebinding(string actionName, TMP_Text bindingText, Button rebindButton)
    {
        var action = playerInput.actions.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}' not found.");
            return;
        }

        action.Disable();
        int bindingIndex = action.GetBindingIndexForControl(action.controls[0]);

        // Activate dim panel and highlight
        dimPanel.SetActive(true);
        PositionHighlight(bindingText);

        // Update button UI
        bindingText.text = "Press any key...";
        rebindButton.interactable = false;

        rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                bindingText.text = InputControlPath.ToHumanReadableString(
                    action.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
                rebindButton.interactable = true;
                action.Enable();
                rebindingOperation.Dispose();

                // Deactivate dim panel
                dimPanel.SetActive(false);
            })
            .OnCancel(operation =>
            {
                bindingText.text = "Binding canceled";
                rebindButton.interactable = true;
                action.Enable();
                rebindingOperation.Dispose();

                // Deactivate dim panel
                dimPanel.SetActive(false);
            })
            .Start();
    }

    private void PositionHighlight(TMP_Text bindingText)
    {
        // Get the RectTransform of the bindingText
        RectTransform bindingTextRect = bindingText.GetComponent<RectTransform>();

        if (bindingTextRect == null)
        {
            Debug.LogWarning("BindingText RectTransform not found.");
            return;
        }

        // Use world position for accurate placement
        Vector3 worldPos = bindingTextRect.position;
        highlight.position = worldPos;

        // Ensure the highlight is the right size, you may adjust padding as needed
        highlight.sizeDelta = bindingTextRect.sizeDelta + new Vector2(20, 10); // Add padding if needed
    }

    private void OnDisable()
    {
        rebindingOperation?.Dispose();
    }

    public void SaveRebinds()
    {
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("InputRebinds", rebinds);
        PlayerPrefs.Save();
    }

    public void LoadRebinds()
    {
        if (PlayerPrefs.HasKey("InputRebinds"))
        {
            string rebinds = PlayerPrefs.GetString("InputRebinds");
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }
    }
}
