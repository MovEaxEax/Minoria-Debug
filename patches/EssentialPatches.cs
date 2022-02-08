using System;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x0200015D RID: 349
public partial class TitleScreen : MonoBehaviour
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x0005A0DC File Offset: 0x000582DC
	private void Start()
	{
		if (UnityEngine.Object.FindObjectOfType<xNyuDebug>() == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<xNyuDebug>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		if (MainScr.publicDemo)
		{
			this.menuOptions = 4;
		}
		this.introTimer = 0f;
		this.introSpriteIndex = 0;
		this.introImage.color = new Color(1f, 1f, 1f, 0f);
		this.introImage.sprite = this.introSprites[this.introSpriteIndex];
		this.introImage.GetComponent<RectTransform>().sizeDelta = new Vector2(this.introImage.sprite.textureRect.width, this.introImage.sprite.textureRect.height);
		MainScr.MainCameraObj.GetComponent<PostProcessingBehaviour>().profile = null;
		MainScr.PerspectiveCameraObj.GetComponent<PostProcessingBehaviour>().profile = null;
		this.menuText = new string[this.menuStrings.Length];
		this.language_xml = MainScr.language;
		this.Disclaimer1 = base.gameObject.transform.GetChild(2).GetComponent<Text>();
		this.PressStart = base.gameObject.transform.GetChild(3).GetComponent<Text>();
		this.Logo = base.gameObject.transform.GetChild(4).GetComponent<Image>();
		this.c = new Color[64];
		this.c[1] = this.Disclaimer1.color;
		this.c[3] = this.PressStart.color;
		this.c[4] = this.Logo.color;
		this.c[5] = this.Background1.color;
		this.c[6] = this.Background2.color;
		this.menuObjects = new GameObject[this.save_slots + 2];
		this.saveSlotData = new TitleScreen.SaveSlotData[this.save_slots];
		this.menuSelectStates = new MenuSelectState[this.menuObjects.Length];
		for (int i = 0; i < this.menuObjects.Length; i++)
		{
			if (i < this.save_slots)
			{
				this.menuObjects[i] = UnityEngine.Object.Instantiate<GameObject>(this.menuSaveFilePrefab, this.menuSelectRT.parent.transform, false);
				this.menuObjects[i].name = "MenuSaveFile" + (i + 1).ToString();
				Text componentInChildren = this.menuObjects[i].GetComponentInChildren<Text>();
				componentInChildren.text = string.Format("{0} {1}", this.menuText[0], (i + 1).ToString());
				this.UpdateSaveSlotText(i);
			}
			else
			{
				this.menuObjects[i] = UnityEngine.Object.Instantiate<GameObject>(this.menuObjectPrefab, this.menuSelectRT.parent.transform, false);
				Text componentInChildren = this.menuObjects[i].GetComponentInChildren<Text>();
				int num = i - this.save_slots;
				if (num != 0)
				{
					if (num == 1)
					{
						componentInChildren.text = this.menuText[2];
					}
				}
				else
				{
					componentInChildren.text = this.menuText[1];
				}
				this.menuObjects[i].name = "MenuObject" + componentInChildren.text;
			}
			this.menuSelectStates[i] = this.menuObjects[i].GetComponentInChildren<MenuSelectState>();
			this.menuObjects[i].GetComponentInChildren<Image>().sprite = this.menuLines[i % this.menuLines.Length];
			this.menuObjects[i].GetComponentInChildren<Image>().rectTransform.sizeDelta = new Vector3(this.menuObjects[i].GetComponentInChildren<Image>().rectTransform.sizeDelta.x, this.menuLines[i % this.menuLines.Length].rect.size.y, 1f);
			RectTransform component = this.menuObjects[i].GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, (float)(-64 * i));
		}
		this.origin = this.menuSelectRT.anchoredPosition;
		this.prevPos = this.origin;
		this.prevPos.x = this.prevPos.x + (float)((!this.hide) ? 0 : -64);
		this.nextPos = this.prevPos;
		this.menuCursor = this.menuSelectRT.GetComponent<MenuCursor>();
		this.UpdateStrings(true);
		this.UpdateSaveSlotData();
	}
}




