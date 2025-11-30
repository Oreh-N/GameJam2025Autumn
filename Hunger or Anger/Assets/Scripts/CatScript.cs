using UnityEngine;

public class CatScript : MonoBehaviour
{

	private void OnCollisionStay2D(Collision2D collision)
	{
		Item i = collision.gameObject.GetComponent<Item>();
		if (i)
		{
			if (Input.GetKeyDown(KeyCode.E))
				Debug.Log("Pressed E");
			i.BreakItem();
		}
	}
}
