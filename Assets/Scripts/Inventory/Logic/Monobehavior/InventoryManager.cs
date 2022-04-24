using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    //TODO:最后添加模版保存
    [Header("Inventory Data")]    
    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionTemplate;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentTemplate;
    public InventoryData_SO equipmentData;

    [Header("ContainerS")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;

    [Header("UI Panel")]
    public GameObject bagPanel;
    public GameObject statesPanel;
    bool isOpen = false;

    [Header("States Text")]
    public Text healthText;
    public Text attackText;

    [Header("Tooltip")]
    public ItemTooltip tooltip;

    protected override void Awake()
    {
        base.Awake();
        if(inventoryTemplate != null)
            inventoryData = Instantiate(inventoryTemplate);
        if(actionTemplate != null)
            actionData = Instantiate(actionTemplate);
        if(equipmentTemplate != null)
            equipmentData = Instantiate(equipmentTemplate);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statesPanel.SetActive(isOpen);
        }

        UpdateStatesText(GameManager.Instance.playerStates.MaxHealth, GameManager.Instance.playerStates.attackData.minDamage, GameManager.Instance.playerStates.attackData.maxDamage);
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);
    }

    public void LoadData()
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);
    }

    public void UpdateStatesText(int health, int min, int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + " - " + max;
    }

    void Start() 
    {
        LoadData();
        inventoryUI.RefreshUI(); 
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();   
    }

    #region 检查拖拽物体是否在每一个Slot范围内
    public bool CheckInInventoryUI(Vector3 position)
    {
        for(int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = inventoryUI.slotHolders[i].transform as RectTransform; //类型转换

            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }

    public bool CheckInActionUI(Vector3 position)
    {
        for(int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform; //类型转换

            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }

    public bool CheckInEquipmentUI(Vector3 position)
    {
        for(int i = 0; i < equipmentUI.slotHolders.Length; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform; //类型转换

            if(RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }

        }
        return false;
    }

    #endregion
}
