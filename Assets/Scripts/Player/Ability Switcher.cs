using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; 

public class AbilitySwitcher : MonoBehaviour
{
    [System.Serializable]
    public class AbilitySlot
    {
        public string abilityName; // Display name for clarity
        public MonoBehaviour abilityScript; // Reference to the actual ability script (e.g. PogoAbility)
        public KeyCode selectionKey; // Which key (W/A/S/D) activates this ability

        public Image uiIcon; // Optional icon image in the menu
        public TextMeshProUGUI uiLabel; // Optional TextMeshPro label for this ability
    }

    [Header("Ability Slots (Scene Specific)")]
    public List<AbilitySlot> availableAbilities = new List<AbilitySlot>();

    [Header("UI")]
    public GameObject abilityMenuUI; // Root UI object to show/hide when L is held
    public KeyCode menuHoldKey = KeyCode.L; // Key to hold for menu

    [Header("Player Scripts to Disable")]
    public MonoBehaviour[] scriptsToDisableOnMenu; // Movement, dash, etc.

    private PlayerMovement playerMovement; // Cache reference to PlayerMovement
    private bool menuWasActiveLastFrame = false;

    void Start()
    {
        // Cache PlayerMovement if available
        playerMovement = GetComponent<PlayerMovement>();

        // Turn off all ability scripts at the start
        ToggleAbilityScripts(false);

        // Hide the menu initially
        abilityMenuUI.SetActive(false);
    }

    void Update()
    {
        // Check if L is being held
        bool isHoldingMenu = Input.GetKey(menuHoldKey);

        // Show/hide the menu based on whether L is held
        abilityMenuUI.SetActive(isHoldingMenu);

        // Enable or disable player control scripts depending on menu state
        SetPlayerControlEnabled(!isHoldingMenu);

        // Only process ability switching if the menu is open
        if (isHoldingMenu)
        {
            foreach (var slot in availableAbilities)
            {
                if (Input.GetKeyDown(slot.selectionKey))
                {
                    SelectAbility(slot);
                }
            }
        }

        if (!isHoldingMenu && menuWasActiveLastFrame)
        {
            ResetUIHighlights();
        }

        menuWasActiveLastFrame = isHoldingMenu;
    }

    // Enables only the selected ability, disables all others.
    void SelectAbility(AbilitySlot selected)
    {
        ToggleAbilityScripts(false); // Disable all
        if (selected.abilityScript != null)
            selected.abilityScript.enabled = true;

        // Highlight selected icon/label
        foreach (var slot in availableAbilities)
        {
            if (slot.uiIcon != null)
                slot.uiIcon.color = (slot == selected) ? Color.yellow : Color.white;

            if (slot.uiLabel != null)
                slot.uiLabel.color = (slot == selected) ? Color.yellow : Color.white;
        }
    }

    // Enables or disables all ability scripts at once.
    void ToggleAbilityScripts(bool enable)
    {
        foreach (var slot in availableAbilities)
        {
            if (slot.abilityScript != null)
                slot.abilityScript.enabled = enable;
        }
    }

    // Enables or disables core player movement/dash scripts when menu is active.
    void SetPlayerControlEnabled(bool enabled)
    {
        if (playerMovement != null)
            playerMovement.enabled = enabled;

        foreach (var script in scriptsToDisableOnMenu)
        {
            if (script != null)
                script.enabled = enabled;
        }
    }

    // Resets UI icon and label colors after menu closes.
    void ResetUIHighlights()
    {
        foreach (var slot in availableAbilities)
        {
            if (slot.uiIcon != null)
                slot.uiIcon.color = Color.white;

            if (slot.uiLabel != null)
                slot.uiLabel.color = Color.white;
        }
    }
}
