using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Slot[] slots;

    private Vector3 _target;
    private Iteminfo carryingitem;

    private Dictionary<int, Slot> slotDictionary;
    // Start is called before the first frame update
    void Start()
    {
        slotDictionary = new Dictionary<int, Slot>();

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SendRayCast();
        }

        if(Input.GetMouseButton(0)) 
        {
            OnItemSelected();
        }

        if(Input.GetMouseButtonUp(0))
        {
            SendRayCast();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlaceRandomItem();
        }
    }
    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<Slot>();
            if(slot.state == Slot.SLOTSTATE.FULL && carryingitem == null)
            {
                string itemPath = "Prefabs/Item_Grabbed_" + slot.itemObject.id.ToString("000");
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));
                itemGo.transform.position = slot.transform.position;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingitem = itemGo.GetComponent<Iteminfo>();
               
                slot.ItemGrabbed();
            }
            else if (slot.state == Slot.SLOTSTATE.FULL && carryingitem != null)
            {
                slot.CreateItem(carryingitem.itemId);
                Destroy(carryingitem.gameObject);
            }
            else if (slot.state == Slot.SLOTSTATE.FULL && carryingitem != null)
            {
                if(slot.itemObject.id == carryingitem.itemId)
                {
                    OnItemMergedWithTarget(slot.id);
                }
                else
                {
                    OnItemCarryFail();
                }
            }
        }
        else
        {
            if(!carryingitem)
            {
                return;
            }
            OnItemCarryFail();
        }
    }
    void OnItemSelected()
    {
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = 0;

        var delta = 10 * Time.deltaTime;

        delta *= Vector3.Distance(transform.position, _target);
        carryingitem.transform.position = Vector3.MoveTowards(carryingitem.transform.position, _target, delta);
    }

    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);
        slot.CreateItem(carryingitem.itemId + 1);
        Destroy(carryingitem.gameObject);
    }

    void OnItemCarryFail()
    {
        var slot = GetSlotById(carryingitem.slotId);
        slot.CreateItem(carryingitem.itemId);
        Destroy(carryingitem.gameObject);
    }

    void PlaceRandomItem()
    {
        if(AllSlotsOccupied())
        {
            Debug.Log("슬롯이 다 차있음 => 생성 불가");
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length);
        var slot = GetSlotById(rand);
        while (slot.state == Slot.SLOTSTATE.FULL)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }

        slot.CreateItem(0);
    }

    bool AllSlotsOccupied()
    {
        foreach(var slot in slots)
        {
            if(slot.state == Slot.SLOTSTATE.EMPTY)
            {
                return false;
            }
        }
        return true;
    }
    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}

