using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Item: MonoBehaviour
{
	public Sprite item;
	public Sprite brokenItem;

	public int attantionVal;
	public int angerVal;



	private void OnTriggerEnter2D(Collider2D collision)
	{
		
	}
}
