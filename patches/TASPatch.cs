using System;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

// Token: 0x0200019E RID: 414
public partial class Controller : MonoBehaviour
{
	// Token: 0x06000870 RID: 2160 RVA: 0x00068F08 File Offset: 0x00067108
	private void Update()
	{
		if (!this.playerIndexSet || !Controller.prevState.IsConnected)
		{
			for (int i = 0; i < 4; i++)
			{
				PlayerIndex playerIndex = (PlayerIndex)i;
				if (GamePad.GetState(playerIndex).IsConnected)
				{
					Debug.Log(string.Format("GamePad found {0}", playerIndex));
					Controller.playerIndex = playerIndex;
					this.playerIndexSet = true;
				}
			}
		}
		Controller.prevState = Controller.state;
		Controller.state = GamePad.GetState(Controller.playerIndex);
		Vector2 vector = new Vector2(Controller.ThumbStickL.x, Controller.ThumbStickL.y);
		Vector2 vector2 = new Vector2(Controller.ThumbStickR.x, Controller.ThumbStickR.y);
		float magnitude = vector.magnitude;
		double num = Math.Atan2((double)vector.y, (double)vector.x) * 57.29577951308232;
		float magnitude2 = vector2.magnitude;
		double num2 = Math.Atan2((double)vector2.y, (double)vector2.x) * 57.29577951308232;
		Controller.Keys keys = Controller.Keys.None;
		Controller.Keys keys2 = Controller.Keys.None;
		if (magnitude > 0.9f)
		{
			if (num >= -45.0 && num < 45.0)
			{
				keys = Controller.Keys.LRight;
			}
			else if (num >= -135.0 && num < -45.0)
			{
				keys = Controller.Keys.LDown;
			}
			else if (num >= 45.0 && num < 135.0)
			{
				keys = Controller.Keys.LUp;
			}
			else
			{
				keys = Controller.Keys.LLeft;
			}
		}
		if (magnitude2 > 0.9f)
		{
			if (num2 >= -45.0 && num2 < 45.0)
			{
				keys2 = Controller.Keys.RRight;
			}
			else if (num2 >= -135.0 && num2 < -45.0)
			{
				keys2 = Controller.Keys.RDown;
			}
			else if (num2 >= 45.0 && num2 < 135.0)
			{
				keys2 = Controller.Keys.RUp;
			}
			else
			{
				keys2 = Controller.Keys.RLeft;
			}
		}
		if (magnitude > 0f || magnitude2 > 0f)
		{
			MainScr.UpdateInputType(InputType.Gamepad);
		}
		for (int j = 0; j < Controller.values.Length; j++)
		{
			Controller.Keys keys3 = (Controller.Keys)Controller.values.GetValue(j);
			int num3 = (int)keys3;

			xNyuTAS TASController = GameObject.FindObjectOfType<xNyuTAS>();
			if (TASController != null){
				int f_num3 = num3;
				if(num3 == -1) f_num3 = 32;
				if (TASController.TASkeyStateFixedPressed[f_num3])
				{
					if (Controller.keyState[num3] < 0)
					{
						Controller.keyState[num3] = 0;
					}
					Dictionary<int, int> dictionary;
					int key;
					(dictionary = Controller.keyState)[key = num3] = dictionary[key] + 1;
					MainScr.UpdateInputType(InputType.Keyboard);
				}
				else if (keys3 != keys && keys3 != keys2)
				{
					if (Controller.keyState[num3] > 0)
					{
						Controller.keyState[num3] = 0;
					}
					else
					{
						Dictionary<int, int> dictionary;
						int key3;
						(dictionary = Controller.keyState)[key3 = num3] = dictionary[key3] - 1;
					}
				}
			}
			else if (Controller.keyboardMapping.ContainsKey(num3) && Input.GetKey(Controller.keyboardMapping[num3]))
			{
				if (Controller.keyState[num3] < 0)
				{
					Controller.keyState[num3] = 0;
				}
				Dictionary<int, int> dictionary;
				int key;
				(dictionary = Controller.keyState)[key = num3] = dictionary[key] + 1;
				MainScr.UpdateInputType(InputType.Keyboard);
			}
			else if (Controller.gamepadMapping.ContainsKey(num3) && Controller.XInputIsDown(Controller.gamepadMapping[num3]))
			{
				if (Controller.keyState[num3] < 0)
				{
					Controller.keyState[num3] = 0;
				}
				Dictionary<int, int> dictionary;
				int key2;
				(dictionary = Controller.keyState)[key2 = num3] = dictionary[key2] + 1;
				MainScr.UpdateInputType(InputType.Gamepad);
			}
			else if (keys3 != keys && keys3 != keys2)
			{
				if (Controller.keyState[num3] > 0)
				{
					Controller.keyState[num3] = 0;
				}
				else
				{
					Dictionary<int, int> dictionary;
					int key3;
					(dictionary = Controller.keyState)[key3 = num3] = dictionary[key3] - 1;
				}
			}
		}
		for (int k = 0; k < Controller.xvalues.Length; k++)
		{
			Controller.GamePadButtons gamePadButtons = (Controller.GamePadButtons)Controller.xvalues.GetValue(k);
			int num4 = (int)gamePadButtons;
			if (Controller.XInputIsDown(gamePadButtons))
			{
				if (Controller.inputState[num4] < 0)
				{
					Controller.inputState[num4] = 0;
				}
				Dictionary<int, int> dictionary;
				int key4;
				(dictionary = Controller.inputState)[key4 = num4] = dictionary[key4] + 1;
			}
			else if (Controller.inputState[num4] > 0)
			{
				Controller.inputState[num4] = 0;
			}
			else
			{
				Dictionary<int, int> dictionary;
				int key5;
				(dictionary = Controller.inputState)[key5 = num4] = dictionary[key5] - 1;
			}
		}
		if (keys != Controller.Keys.None)
		{
			if (Controller.keyState[(int)keys] < 0)
			{
				Controller.keyState[(int)keys] = 0;
			}
			Dictionary<int, int> dictionary;
			int key6;
			(dictionary = Controller.keyState)[key6 = (int)keys] = dictionary[key6] + 1;
		}
		else if (Controller.keyState[(int)keys] > 0)
		{
			Controller.keyState[(int)keys] = 0;
		}
		else
		{
			Dictionary<int, int> dictionary;
			int key7;
			(dictionary = Controller.keyState)[key7 = (int)keys] = dictionary[key7] - 1;
		}
		if (keys2 != Controller.Keys.None)
		{
			if (Controller.keyState[(int)keys2] < 0)
			{
				Controller.keyState[(int)keys2] = 0;
			}
			Dictionary<int, int> dictionary;
			int key8;
			(dictionary = Controller.keyState)[key8 = (int)keys2] = dictionary[key8] + 1;
		}
		else if (Controller.keyState[(int)keys2] > 0)
		{
			Controller.keyState[(int)keys2] = 0;
		}
		else
		{
			Dictionary<int, int> dictionary;
			int key9;
			(dictionary = Controller.keyState)[key9 = (int)keys2] = dictionary[key9] - 1;
		}
		if (Controller.rumbleDuration > 0f)
		{
			Controller.rumbleDuration = Mathf.MoveTowards(Controller.rumbleDuration, 0f, Time.deltaTime);
			if (Controller.rumbleState == 0)
			{
				GamePad.SetVibration(Controller.playerIndex, Controller.rumbleLeft, Controller.rumbleRight);
				Controller.rumbleState = 1;
			}
		}
		else if (Controller.rumbleState == 1 && Controller.rumbleDuration <= 0f)
		{
			Controller.resetRumble();
		}
	}
}









