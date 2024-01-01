using System;
using System.Collections.Generic;
using System.Linq;
using TerminalApi.Classes;
using TerminalApi.Interfaces;
using TMPro;
using UnityEngine;

namespace TerminalApi
{
	public static class TerminalApi
	{
		/// <summary>
		/// The plugin of terminalapi
		/// </summary>
		public static Plugin plugin;
		
		internal static List<IDelayedAction> QueuedDelayedActions = new();

		internal static List<CommandInfo> CommandInfos = new();
		
		/// <summary>
		/// The ingame terminal script.
		/// </summary>
		public static Terminal Terminal { get; internal set; }

		public static List<TerminalNode> SpecialNodes => Terminal.terminalNodes.specialNodes;

		/// <summary>
		/// The string of the currently displayed text
		/// </summary>
		public static string CurrentText => Terminal.currentText;

		/// <summary>
		/// The TMP screen object
		/// </summary>
		public static TMP_InputField ScreenText => Terminal.screenText;

		/// <summary>
		/// Checks if the player is ingame via checking if the terminal script exists.
		/// </summary>
		/// <returns>Whether the player is in game or not</returns>
		public static bool IsInGame()
		{
			try
			{
				return Terminal is not null;
			}
			catch (NullReferenceException)
            {
				return false;
			}
		}


		/// <summary>
		///  Automatically creates and adds <see cref="TerminalKeyword"/> to the terminal based on inputs given.
		/// </summary>
		/// <param name="commandWord">This is essentially the noun word. What needs to be entered to trigger the display text.</param>
		/// <param name="displayText">The text to display when the command word is sent.</param>
		/// <param name="verbWord">The word that comes before the command word. Will be set as default so you can still just enter the command word to trigger.</param>
		/// <param name="clearPreviousText">Whether or not to clear the terminal after entering command.</param>
		public static void AddCommand(string commandWord, string displayText, string verbWord = null, bool clearPreviousText = true)
		{ 
			commandWord = commandWord.ToLower();
			TerminalKeyword mainKeyword = CreateTerminalKeyword(commandWord);
			TerminalNode triggerNode = CreateTerminalNode(displayText, clearPreviousText);
			if(verbWord != null)
			{
				verbWord = verbWord.ToLower();
				TerminalKeyword verbKeyword = CreateTerminalKeyword(verbWord, true);
				verbKeyword = verbKeyword.AddCompatibleNoun(mainKeyword, triggerNode);
				mainKeyword.defaultVerb = verbKeyword;
				AddTerminalKeyword(verbKeyword);
				AddTerminalKeyword(mainKeyword);
			}
			else
			{
				mainKeyword.specialKeywordResult = triggerNode;
				AddTerminalKeyword(mainKeyword);
			}
		}

        /// <summary>
        ///  Automatically creates and adds <see cref="TerminalKeyword"/> to the terminal based on inputs given.
        /// </summary>
        /// <param name="commandWord">This is essentially the noun word. What needs to be entered to trigger the display text.</param>
        /// <param name="displayText">The text to display when the command word is sent.</param>
        /// <param name="verbWord">The word that comes before the command word. Will be set as default so you can still just enter the command word to trigger.</param>
        /// <param name="clearPreviousText">Whether or not to clear the terminal after entering command.</param>
        public static void AddCommand(string commandWord, CommandInfo commandInfo, string verbWord = null, bool clearPreviousText = true)
        {
            commandWord = commandWord.ToLower();
            TerminalKeyword mainKeyword = CreateTerminalKeyword(commandWord);
            TerminalNode triggerNode = CreateTerminalNode("", clearPreviousText);
			
			commandInfo.TriggerNode = triggerNode;

            if (verbWord != null)
            {
                verbWord = verbWord.ToLower();
                TerminalKeyword verbKeyword = CreateTerminalKeyword(verbWord, true);
                verbKeyword = verbKeyword.AddCompatibleNoun(mainKeyword, triggerNode);
                mainKeyword.defaultVerb = verbKeyword;
                AddTerminalKeyword(verbKeyword);
                AddTerminalKeyword(mainKeyword, commandInfo);
            }
            else
            {
                mainKeyword.specialKeywordResult = triggerNode;
                AddTerminalKeyword(mainKeyword, commandInfo);
            }
        }



        /// <summary>
        /// Creates a <see cref="TerminalKeyword"/>
        /// </summary>
        /// <param name="word">The terminal command word</param>
        /// <param name="isVerb">Whether the command word is a verb</param>
        /// <param name="triggeringNode">The <see cref="TerminalNode"/> that runs when command word is sent.</param>
        /// <returns>The newly created <see cref="TerminalKeyword"/></returns>
        public static TerminalKeyword CreateTerminalKeyword(string word, bool isVerb = false, TerminalNode triggeringNode = null) 
		{
			TerminalKeyword newKeyword = ScriptableObject.CreateInstance<TerminalKeyword>();
			newKeyword.word = word.ToLower();
			newKeyword.isVerb = isVerb;
			newKeyword.specialKeywordResult = triggeringNode;
			return newKeyword;
		}

		/// <summary>
		/// Creates a <see cref="TerminalKeyword"/>
		/// </summary>
		/// <param name="word">The terminal command word</param>
		/// <param name="displayText">The text to display when command word is sent.</param>
		/// <param name="clearPreviousText">Whether to clear the terminal when command is sent.</param>
		/// <param name="terminalEvent">The terminal you want to trigger. Just keep empty unless you know what you're doing.</param>
		/// <returns>The newly created <see cref="TerminalKeyword"/></returns>
		public static TerminalKeyword CreateTerminalKeyword(string word, string displayText, bool clearPreviousText = false, string terminalEvent = "")
		{
			TerminalKeyword newKeyword = ScriptableObject.CreateInstance<TerminalKeyword>();
			newKeyword.word = word.ToLower();
			newKeyword.isVerb = false;
			newKeyword.specialKeywordResult = CreateTerminalNode(displayText, clearPreviousText, terminalEvent);
			return newKeyword;
		}

		/// <summary>
		/// Creates a <see cref="TerminalNode"/>
		/// </summary>
		/// <param name="displayText">The text to display on command sent.</param>
		/// <param name="clearPreviousText">Whether to clear the terminal.</param>
		/// <param name="terminalEvent">The terminal you want to trigger. Just keep empty unless you know what you're doing.</param>
		/// <returns></returns>
		public static TerminalNode CreateTerminalNode(string displayText, bool clearPreviousText = false, string terminalEvent = "")
		{
			TerminalNode newNode = ScriptableObject.CreateInstance<TerminalNode>();
			newNode.displayText = displayText;
			newNode.clearPreviousText = clearPreviousText;
			newNode.terminalEvent = terminalEvent;
			return newNode;
		}

		/// <summary>
		/// Addes the keyword to the terminal's keywords
		/// </summary>
		/// <param name="terminalKeyword">The keyword to add</param>
		public static void AddTerminalKeyword(TerminalKeyword terminalKeyword, CommandInfo commandInfo = null)
		{
			if(IsInGame())
			{
				if (GetKeyword(terminalKeyword.word) is null)
				{
                    // Setup callback
                    if (commandInfo?.DisplayTextSupplier is not null)
					{
						if(commandInfo?.TriggerNode is null)
						{
							commandInfo.TriggerNode = terminalKeyword.specialKeywordResult;
						}
						CommandInfos.Add(commandInfo);
					}

					// Setup command help info/description
					if (commandInfo is not null)
					{
						// Set object name
						terminalKeyword.name = commandInfo.Title ?? terminalKeyword.word.Substring(0, 1).ToUpper() + terminalKeyword.word.Substring(1);
						string newEntry = $"\n>{terminalKeyword.name.ToUpper()}\n{commandInfo.Description ?? ""}\n\n";
						NodeAppendLine(commandInfo.Category, newEntry);
					}

                    Terminal.terminalNodes.allKeywords = Terminal.terminalNodes.allKeywords.Add(terminalKeyword);
					plugin.Log?.LogMessage($"Added {terminalKeyword.word} keyword to terminal keywords.");
				}
				else
				{
					plugin.Log?.LogWarning($"Failed to add {terminalKeyword.word} keyword. Already exists.");
				}
			}
			else
			{
				plugin.Log?.LogMessage($"Not in game, waiting to be in game to add {terminalKeyword.word} keyword.");
				Action<TerminalKeyword, CommandInfo> newAction = AddTerminalKeyword;
				DelayedAddTerminalKeyword delayedAction = new() { Action = newAction, Keyword = terminalKeyword, CommandInfo = commandInfo };
				QueuedDelayedActions.Add(delayedAction);
			}
		}

		/// <summary>
		/// Appends a line of text to a node
		/// </summary>
		/// <param name="word">The word of the node</param>
		/// <param name="text">The text to append</param>
		public static void NodeAppendLine(string word, string text)
		{
			if (IsInGame())
			{
				TerminalKeyword terminalKeyword = GetKeyword(word.ToLower());
				if(terminalKeyword != null)
				{
					terminalKeyword.specialKeywordResult.displayText = terminalKeyword.specialKeywordResult.displayText.Trim() + "\n" + text;
				}
				else
				{
                    plugin.Log?.LogWarning($"Failed to add text to {word}. Does not exist.");
                }
			}
		}

		/// <summary>
		/// Gets the <see cref="TerminalKeyword"/> via its word.
		/// </summary>
		/// <param name="keyword">The word of the keyword</param>
		/// <returns>A <see cref="TerminalKeyword"/> from allKeywords. Or null</returns>
		public static TerminalKeyword GetKeyword(string keyword)
		{
			if (IsInGame())
			{
				return Terminal.terminalNodes.allKeywords.FirstOrDefault(Kw => Kw.word == keyword);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Updates the given keyword in the terminal's keywords.
		/// </summary>
		/// <param name="keyword">The key word to update</param>
		public static void UpdateKeyword(TerminalKeyword keyword)
		{
			if (IsInGame())
			{
				for (int i = 0; i < Terminal.terminalNodes.allKeywords.Length; i++)
				{
					if (Terminal.terminalNodes.allKeywords[i].word == keyword.word)
					{
						Terminal.terminalNodes.allKeywords[i] = keyword;
						return;
					}
				}
				plugin.Log?.LogWarning($"Failed to update {keyword.word}. Was not found in keywords.");
			}
		}

		/// <summary>
		/// Deletes a keyword based on its word.
		/// </summary>
		/// <param name="word">The word of the keyword to delete.</param>
        public static void DeleteKeyword(string word)
		{
			if (IsInGame())
			{
                for (int i = 0; i < Terminal.terminalNodes.allKeywords.Length; i++)
                {
                    if (Terminal.terminalNodes.allKeywords[i].word == word.ToLower())
                    {
						int newSize = Terminal.terminalNodes.allKeywords.Length - 1;
                        for (int j = i + 1; j < Terminal.terminalNodes.allKeywords.Length; j++)
						{
							Terminal.terminalNodes.allKeywords[j - 1] = Terminal.terminalNodes.allKeywords[j];
                        }
						Array.Resize(ref Terminal.terminalNodes.allKeywords, newSize);
                        plugin.Log?.LogMessage($"{word} was deleted successfully.");
                        return;
                    }
                }
                plugin.Log?.LogWarning($"Failed to delete {word}. Was not found in keywords.");
            }
		}

        /// <summary>
        /// Updates a <see cref="CompatibleNoun"/>. Only works for verbs. 
        /// </summary>
        /// <param name="verbKeyword">The keyword to update the compatible noun for.</param>
        /// <param name="noun">The noun word of the compatible noun</param>
        /// <param name="newTriggerNode">The new node that will trigger when the noun is used with verb.</param>
        public static void UpdateKeywordCompatibleNoun(TerminalKeyword verbKeyword, string noun, TerminalNode newTriggerNode)
		{
			if (IsInGame())
			{
				if (!verbKeyword.isVerb)
				{
					return;
				}

				for(int i = 0; i < verbKeyword.compatibleNouns.Length; i++)
				{
					CompatibleNoun compatibleNoun = verbKeyword.compatibleNouns[i];
					if(compatibleNoun.noun.word == noun)
					{
						compatibleNoun.result = newTriggerNode;
						UpdateKeyword(verbKeyword);
						return;
					}
				}
				plugin.Log?.LogWarning($"WARNING: No noun found for {verbKeyword}");
			}
		}

		/// <summary>
		/// Updates a <see cref="CompatibleNoun"/>. Only works for verbs. 
		/// </summary>
		/// <param name="verbWord">The verb word to update the compatible noun for.</param>
		/// <param name="noun">The noun word of the compatible noun</param>
		/// <param name="newTriggerNode">The new node that will trigger when the noun is used with verb.</param>
		public static void UpdateKeywordCompatibleNoun(string verbWord, string noun, TerminalNode newTriggerNode)
		{
			if (IsInGame())
			{
				TerminalKeyword verbKeyword = GetKeyword(verbWord);
				if (!verbKeyword.isVerb || verbWord == null)
				{
					return;
				}

				for (int i = 0; i < verbKeyword.compatibleNouns.Length; i++)
				{
					CompatibleNoun compatibleNoun = verbKeyword.compatibleNouns[i];
					if (compatibleNoun.noun.word == noun)
					{
						compatibleNoun.result = newTriggerNode;
						UpdateKeyword(verbKeyword);
						return;
					}
				}
				plugin.Log?.LogWarning($"WARNING: No noun found for {verbKeyword}");
			}
		}

		/// <summary>
		/// Updates a <see cref="CompatibleNoun"/>. Only works for verbs. 
		/// </summary>
		/// <param name="verbKeyword">The keyword to update the compatible noun for.</param>
		/// <param name="noun">The noun word of the compatible noun</param>
		/// <param name="newTriggerNode">The new text that will display when the noun is used with verb.</param>
		public static void UpdateKeywordCompatibleNoun(TerminalKeyword verbKeyword, string noun, string newDisplayText)
		{
			if (IsInGame())
			{
				if (!verbKeyword.isVerb)
				{
					return;
				}

				for (int i = 0; i < verbKeyword.compatibleNouns.Length; i++)
				{
					CompatibleNoun compatibleNoun = verbKeyword.compatibleNouns[i];
					if (compatibleNoun.noun.word == noun)
					{
						compatibleNoun.result.displayText = newDisplayText;
						UpdateKeyword(verbKeyword);
						return;
					}
				}
				plugin.Log?.LogWarning($"WARNING: No noun found for {verbKeyword}");
			}
		}

		/// <summary>
		/// Updates a <see cref="CompatibleNoun"/>. Only works for verbs. 
		/// </summary>
		/// <param name="verbKeyword">The verb word to update the compatible noun for.</param>
		/// <param name="noun">The noun word of the compatible noun</param>
		/// <param name="newTriggerNode">The new text that will display when the noun is used with verb.</param>
		public static void UpdateKeywordCompatibleNoun(string verbWord, string noun, string newDisplayText)
		{
			if (IsInGame())
			{
				TerminalKeyword verbKeyword = GetKeyword(verbWord);
				if (!verbKeyword.isVerb)
				{
					return;
				}

				for (int i = 0; i < verbKeyword.compatibleNouns.Length; i++)
				{
					CompatibleNoun compatibleNoun = verbKeyword.compatibleNouns[i];
					if (compatibleNoun.noun.word == noun)
					{
						compatibleNoun.result.displayText = newDisplayText;
						UpdateKeyword(verbKeyword);
						return;
					}
				}
				plugin.Log?.LogWarning($"WARNING: No noun found for {verbKeyword}");
			}
		}

		/// <summary>
		/// Adds a compatible noun to an already exiting keyword.
		/// </summary>
		/// <param name="verbKeyword">The verb keyword that already exists in the terminal</param>
		/// <param name="noun">The noun word that you want to add</param>
		/// <param name="displayText">The text you want to display when the verb noun combo is used</param>
		public static void AddCompatibleNoun(TerminalKeyword verbKeyword, string noun, string displayText, bool clearPreviousText = false)
		{
			if (IsInGame())
			{
				TerminalKeyword nounKeyword = GetKeyword(noun);
				verbKeyword = verbKeyword.AddCompatibleNoun(nounKeyword, displayText, clearPreviousText);
				UpdateKeyword(verbKeyword);
			}
		}

		/// <summary>
		/// Adds a compatible noun to an already exiting keyword.
		/// </summary>
		/// <param name="verbKeyword">The verb keyword that already exists in the terminal</param>
		/// <param name="noun">The noun word that you want to add</param>
		/// <param name="triggerNode">The node you want to trigger when the verb noun combo is used</param>
		public static void AddCompatibleNoun(TerminalKeyword verbKeyword, string noun, TerminalNode triggerNode)
		{
			if (IsInGame())
			{
				TerminalKeyword nounKeyword = GetKeyword(noun);
				verbKeyword = verbKeyword.AddCompatibleNoun(nounKeyword, triggerNode);
				UpdateKeyword(verbKeyword);
			}
		}

		/// <summary>
		/// Adds a compatible noun to an already exiting keyword.
		/// </summary>
		/// <param name="verbWord">The verb word that already exists in the terminal</param>
		/// <param name="noun">The noun word that you want to add</param>
		/// <param name="triggerNode">The node you want to trigger when the verb noun combo is used</param>
		public static void AddCompatibleNoun(string verbWord, string noun, TerminalNode triggerNode)
		{
			if (IsInGame())
			{
				TerminalKeyword verbTerminalKeyword = GetKeyword(verbWord);
				TerminalKeyword nounKeyword = GetKeyword(noun);
				if (verbTerminalKeyword == null) 
				{ 
					plugin.Log?.LogWarning("The verb given does not exist."); 
					return; 
				}
				verbTerminalKeyword = verbTerminalKeyword.AddCompatibleNoun(nounKeyword, triggerNode);
				UpdateKeyword(verbTerminalKeyword);
			}
		}

		/// <summary>
		/// Adds a compatible noun to an already exiting keyword.
		/// </summary>
		/// <param name="verbWord">The verb word that already exists in the terminal</param>
		/// <param name="noun">The noun word that you want to add</param>
		/// <param name="displayText">The text you want to display when the verb noun combo is used</param>
		public static void AddCompatibleNoun(string verbWord, string noun, string displayText, bool clearPreviousText = false)
		{
			if (IsInGame())
			{
				TerminalKeyword verbTerminalKeyword = GetKeyword(verbWord);
				TerminalKeyword nounKeyword = GetKeyword(noun);
				if (verbTerminalKeyword == null) 
				{ 
					plugin.Log?.LogWarning("The verb given does not exist."); 
					return; 
				}
				verbTerminalKeyword = verbTerminalKeyword.AddCompatibleNoun(nounKeyword, displayText, clearPreviousText);
				UpdateKeyword(verbTerminalKeyword);
			}
		}

		/// <summary>
		/// Gets the users current input
		/// </summary>
		public static string GetTerminalInput()
		{
			if (IsInGame())
			{
				return CurrentText.Substring(CurrentText.Length - Terminal.textAdded);
            }
			return "";
		}

		/// <summary>
		/// Sets the users current input to given string
		/// </summary>
		/// <param name="terminalInput">The text to set as input</param>
		public static void SetTerminalInput(string terminalInput)
		{
			Terminal.TextChanged(CurrentText.Substring(0, CurrentText.Length - Terminal.textAdded) + terminalInput);
			ScreenText.text = CurrentText;
            Terminal.textAdded = terminalInput.Length;
		}

	}
}
