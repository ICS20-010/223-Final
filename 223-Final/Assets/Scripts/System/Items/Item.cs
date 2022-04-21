using UnityEngine;

public class Item : MonoBehaviour
{

  public ItemType itemType = null;

  public void init()
  {
    if (!itemType)
    {
      Debug.Log("itemType not set");
      Debug.Break();
    }

    this.gameObject.name = itemType.item_name;

    if (itemType.item_model != null)
    {
      GameObject instance_item = GameObject.Instantiate(itemType.item_model, this.transform.position + itemType.spawnOffset, Quaternion.identity);
      instance_item.transform.parent = this.gameObject.transform;
      instance_item.transform.localScale = itemType.scale;
      this.gameObject.tag = itemType.item_tag;
      if (itemType.rotates)
      {
        spinObject spin = this.gameObject.AddComponent<spinObject>();
        spin.setRotations(itemType.x, itemType.y, itemType.z);
      }
      if (itemType.pickup)
      {
        CapsuleCollider capCollider = this.gameObject.AddComponent<CapsuleCollider>();
        MeshRenderer renderer = instance_item.GetComponent<MeshRenderer>();
        capCollider.height = renderer.bounds.size.y;
        capCollider.radius = renderer.bounds.size.x / 2;
        capCollider.isTrigger = true;

        Light lightComp = this.gameObject.AddComponent<Light>();
        lightComp.type = LightType.Point;
        lightComp.color = Color.blue;
      }
      if(itemType.item_name == "Health")
      {
        HealthData hData = this.gameObject.AddComponent<HealthData>();
      }
      if (itemType.interactable)
      {
        // TODO: place comonents need for interaction here
        // nothing for now
      }

      this.gameObject.tag = itemType.item_tag;
    }
    else
    {
      Debug.Log("No model given, a model is needed to represent the item");
    }
  }

  public void setItemType(ItemType itemType)
  {
    this.itemType = itemType;
  }
}