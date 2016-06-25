using UnityEngine;
using System.Collections;

public class SkillIconSkin : MonoBehaviour 
{
	public UISprite originSprite;
	public UISprite progressSprite;
	public UILabel remainLabel;

	[HideInInspector]
	public int coolDownCount;

	public int remainCount;
	public bool isCD;

	private BoxCollider boxCollider;

	public void OnInit(string spriteName, int remainCount, int coolDownCount)
	{
		originSprite.spriteName = spriteName;
		progressSprite.spriteName = spriteName;
		this.coolDownCount = coolDownCount;
		boxCollider = this.gameObject.GetComponent<BoxCollider>();
		this.remainCount = remainCount;
		Render();
	}

	public void OnTurnStart()
	{
		remainCount--;
		Render();
	}

	public void OnSkillClick()
	{
		remainCount = coolDownCount;
		Render();
	}

	private void Render()
	{
		isCD = remainCount > 0;

		progressSprite.fillAmount = remainCount / (float)coolDownCount;
		remainLabel.gameObject.SetActive(isCD);
		remainLabel.text = remainCount.ToString();
		
		boxCollider.enabled = !isCD;
	}
}
