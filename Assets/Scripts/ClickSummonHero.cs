using JsonClass;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickSummonHero : MonoBehaviour
{
    [SerializeField] List<Slot> slots;
    [SerializeField] HeroData heroData;
    [SerializeField] string layerName;
    InputAction touchAction;
    InputAction pointAction;

    void Start()
    {
        touchAction = InputSystem.actions.FindAction("UI/Click");
        pointAction = InputSystem.actions.FindAction("UI/Point");
    }

    public void ResetSummonSpots()
    {
        foreach (var slot in slots)
        {
            slot.Init();
        }
    }

    public void SetHeroData(HeroData data)
    {
        heroData = data;

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].gameObject.SetActive(slots[i].SummonCheck(data));
        }
    }

    void Update()
    {
        if (touchAction.WasPressedThisFrame())
        {
            int layerMask = LayerMask.GetMask(layerName);
            Ray ray = Camera.main.ScreenPointToRay(pointAction.ReadValue<Vector2>());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layerMask))
            {
                Slot slot = hit.collider.GetComponent<Slot>();
                slot.OnClick(heroData);
                UIManager.Instance.PopPanel();
                gameObject.SetActive(false);
            }
        }
    }
}
