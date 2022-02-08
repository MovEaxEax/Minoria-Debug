using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Linq;

public class xNyuTAS : MonoBehaviour
{
	[DllImport("user32.dll", EntryPoint = "MessageBox")]
	public static extern int ShowMessage(int hWnd, string text, string caption, uint type);

	public void Update()
	{
		try
		{
			if (ScriptIteration >= ScriptLines.Count)
			{
				ScriptEnd = true;
			}

			if (!ScriptEnd)
			{
				//TAS Routine

				//Clean Key Checkers
				for (int i = 0; i < AllInputsCheck.Length; i++) AllInputsCheck[i] = false;
				if (ScriptLines[ScriptIteration].Contains("repeat("))
				{
					RepeatFrames = int.Parse(ScriptLines[ScriptIteration].Replace("repeat(", "").Split(')')[0]);
				}

				if (WaitFrames <= 0)
				{
					if (ScriptLines[ScriptIteration].Contains("frame{"))
					{
						string[] frame = ScriptLines[ScriptIteration].Replace("frame{", "").Replace("}", "").Split(';');

						if (frame.Length > 0)
						{

							for (int i = 0; i < 33; i++)
							{
								TASkeyStateFixedPressed[i] = false;
							}
							foreach (string action in frame)
							{
								//Detect what should be pessed
								if (action == "up()")
								{
									TASkeyStateFixedPressed[0] = true;
                                }
								if (action == "down()")
								{
									TASkeyStateFixedPressed[1] = true;
								}
								if (action == "left()")
								{
									TASkeyStateFixedPressed[2] = true;
								}
								if (action == "right()")
								{
									TASkeyStateFixedPressed[3] = true;
								}
								if (action == "lup()")
								{
									TASkeyStateFixedPressed[4] = true;
								}
								if (action == "ldown()")
								{
									TASkeyStateFixedPressed[5] = true;
								}
								if (action == "lleft()")
								{
									TASkeyStateFixedPressed[6] = true;
								}
								if (action == "lright()")
								{
									TASkeyStateFixedPressed[7] = true;
								}
								if (action == "l3()")
								{
									TASkeyStateFixedPressed[8] = true;
								}
								if (action == "rup()")
								{
									TASkeyStateFixedPressed[9] = true;
								}
								if (action == "rdown()")
								{
									TASkeyStateFixedPressed[10] = true;
								}
								if (action == "rleft()")
								{
									TASkeyStateFixedPressed[11] = true;
								}
								if (action == "rright()")
								{
									TASkeyStateFixedPressed[12] = true;
								}
								if (action == "lockon()")
								{
									TASkeyStateFixedPressed[13] = true;
								}
								if (action == "confirm()")
								{
									TASkeyStateFixedPressed[14] = true;
								}
								if (action == "option()")
								{
									TASkeyStateFixedPressed[15] = true;
								}
								if (action == "cancel()")
								{
									TASkeyStateFixedPressed[16] = true;
								}
								if (action == "interact()")
								{
									TASkeyStateFixedPressed[17] = true;
								}
								if (action == "summon()")
								{
									TASkeyStateFixedPressed[18] = true;
								}
								if (action == "useitem()")
								{
									TASkeyStateFixedPressed[19] = true;
								}
								if (action == "useitem2()")
								{
									TASkeyStateFixedPressed[20] = true;
								}
								if (action == "switchiteml()")
								{
									TASkeyStateFixedPressed[21] = true;
								}
								if (action == "switchitemr()")
								{
									TASkeyStateFixedPressed[22] = true;
								}
								if (action == "jump()")
								{
									TASkeyStateFixedPressed[23] = true;
								}
								if (action == "meleeattack()")
								{
									TASkeyStateFixedPressed[24] = true;
								}
								if (action == "meleeattack2()")
								{
									TASkeyStateFixedPressed[25] = true;
								}
								if (action == "dodge()")
								{
									TASkeyStateFixedPressed[26] = true;
								}
								if (action == "map()")
								{
									TASkeyStateFixedPressed[27] = true;
								}
								if (action == "pause()")
								{
									TASkeyStateFixedPressed[28] = true;
								}
								if (action == "back()")
								{
									TASkeyStateFixedPressed[29] = true;
								}
								if (action == "cameramode()")
								{
									TASkeyStateFixedPressed[30] = true;
								}
								else if (action.Contains("game.") || action.Contains("player.") || action.Contains("debug.") || action.Contains("special.") || action.Contains("custom.") || action.Contains("camera."))
                                {
									DebugMenu.HotkeyToFunc(action);
								}
							}

							bool none_pressed = true;
							for(int i = 0; i < 31; i++)
                            {
                                if (TASkeyStateFixedPressed[i])
                                {
									none_pressed = false;
									break;
                                }
                            }
							TASkeyStateFixedPressed[32] = none_pressed;
						}
					}
					else
					{
						//Wait
						WaitFramesToSet = int.Parse(ScriptLines[ScriptIteration].Replace("wait{", "").Replace("}", "")) - 1;
						WaitExtraFramesResult += WaitFramesToSet;
					}
				}

				//Debug Message
				if (ScriptLines.Count > 10)
				{
					if (ScriptIteration > 0 && ScriptIteration % ((int)Math.Round((float)ScriptLines.Count / 10f)) == 0)
					{
						ScriptProgress++;
						WriteConsole("TAS Progress: " + (ScriptProgress * 10).ToString() + "%");
					}
				}

				//Increase Iterator
				if (WaitFrames > 0)
				{
					WaitFrames--;
				}
				else
				{
					ScriptIteration++;
					WaitFrames = WaitFramesToSet;
					WaitFramesToSet = 0;
				}

			}
			else
			{
				//Script End
				WriteConsole("\n--- TAS finished ---\n");
				//WriteConsole("Duration: " + (DateTime.Now - ScriptStartTime).ToString("hh:mm:ss.FFF"));
				WriteConsole("Frames: " + (ScriptIteration + 1 + WaitExtraFramesResult).ToString() + "\n");
				Destroy(this.gameObject);
			}
		}
		catch(Exception e)
        {
			WriteConsole("!!! ERROR ORCCURED !!!");
			WriteConsole(e.Message);
			WriteConsole(e.StackTrace);
			Destroy(this.gameObject);
        }

	}

	public void Init()
    {
        try {
			WriteConsole("TAS.Execute() called");

			for(int i = 0; i < 33; i++)
            {
				TASkeyStateFixedPressed[i] = false;
			}

			//Get Script Path from DebugMenu
			DebugMenu = FindObjectOfType<xNyuDebug>();
			ScriptPath = DebugMenu.ScriptPath;

			//Read Lines from Script
			if (!File.Exists(ScriptPath))
			{
				WriteConsole("--- ERROR ---");
				WriteConsole("File " + @ScriptPath + " was not found!\n");
				Destroy(this.gameObject);
			}
			else
			{
				ScriptLines = File.ReadAllLines(ScriptPath).ToList<string>();

				List<string> ScriptLinesProcess = new List<string>();
				for (int i = 0; i < ScriptLines.Count; i++)
				{
					List<string> lines = new List<string>();
					string line = ScriptLines[i].ToLower().Replace(" ", "");
					if (line.Length > 4)
					{
						if (line.Contains("#"))
						{
							lines = line.Split('#').ToList<string>();
						}
						else
						{
							lines.Add(line);
						}
					}
					foreach(string s in lines)
					{
						if (s.Contains("repeat"))
						{
							int its = int.Parse(s.Replace("repeat(", "").Split(')')[0]);
							string payload = s.Substring(0, s.Length - 1).Replace("repeat(" + its.ToString() + "){", "");
							for (int k = 0; k < its; k++)
							{
								ScriptLinesProcess.Add(payload);
							}
						}
						else
						{
							ScriptLinesProcess.Add(s);
						}
					}
				}

				if (ScriptLines.Count > 0) ScriptLines.Clear();
				ScriptLines = ScriptLinesProcess;

				//Start Message
				WriteConsole("\n--- TAS START: " + ScriptPath.Split('\\')[ScriptPath.Split('\\').Length - 1] + " ---\n");
				WriteConsole(ScriptLines[0].Contains("wait") ? "Wait " + ScriptLines[0].Replace("wait{", "").Replace("}", "") + " frames before start..." : "");

				//Start Settings
				bool ScriptEnd = false;
				int ScriptIteration = 0;
				ScriptStartTime = DateTime.Now;
				ScriptProgress = 0;
				WaitFrames = 0;
				WaitFramesToSet = 0;
				WaitExtraFramesResult = 0;
				RepeatFrames = 0;

				AllInputsCheck = new bool[23];
			}
		}
		catch (Exception e)
		{
			WriteConsole("!!! ERROR ORCCURED !!!");
			WriteConsole(e.Message);
			WriteConsole(e.StackTrace);
			Destroy(this.gameObject);
		}

	}

	public void WriteConsole(object text)
	{
		try { Console.WriteLine(text); } catch { }
	}

	//Setings File

	//Settings TAS
	public string ScriptPath = "";
	public List<string> ScriptLines = new List<string>();
	public bool ScriptEnd = false;
	public bool[] AllInputsCheck;
	public int ScriptIteration = 0;
	public int ScriptProgress;
	public int RepeatFrames;
	public int WaitFrames;
	public int WaitFramesToSet;
	public int WaitExtraFramesResult;
	public DateTime ScriptStartTime;

	public bool[] TASkeyStateFixedPressed = new bool[33];

	public xNyuDebug DebugMenu;
}


