using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


public class Item : MonoBehaviour
{
	public Sprite item;
	public Sprite brokenItem;

	public int attantionVal;
	public int angerVal;

	public bool isBroken = false;



	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{ UI.Instance.ShowInteractionButton(false); }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			UI.Instance.ShowInteractionButton(true);
		}

	}

	public void BreakItem()
	{
		if (!isBroken)
		{
			GetComponent<SpriteRenderer>().sprite = brokenItem;
			Destroy(GetComponent<BoxCollider2D>());
			isBroken = true;
		}
	}
}
