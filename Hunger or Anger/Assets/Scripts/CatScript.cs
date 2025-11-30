using UnityEngine;

public class CatScript : MonoBehaviour
{
	private Item currentItem;

	private void OnTriggerStay2D(Collider2D collision)
	{
		Item i = collision.GetComponent<Item>();
		if (i && !i.isBroken)
		{
			currentItem = i; 
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Item i = collision.GetComponent<Item>();
		if (i && i == currentItem)
		{
			currentItem = null;
		}
	}

	private void Update()
	{
		if (currentItem != null && Input.GetKeyDown(KeyCode.E))
		{
			currentItem.BreakItem();
		}
	}
}
