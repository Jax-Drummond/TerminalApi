using System;
using System.Collections.Generic;
using System.Linq;

namespace TerminalApi
{
	public static class TerminalApi 
	{
		/// <summary>
		/// The plugin of terminalapi
		/// </summary>
		public static Plugin plugin;
		
		public static List<DelayedAction> QueuedActions = new List<DelayedAction>();
		/// <summary>
		/// The ingame terminal script.
		/// </summary>
		public static Terminal Terminal { get; internal set; }

		/// <summary>
		/// Checks if the player is ingame via checking if the terminal script exists.
		/// </summary>
		/// <returns></returns>
		internal static bool _isInGame()
		{
			try
			{
				return Terminal is not null;
			}
			catch (NullReferenceException ex)
			{
				return false;
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
			return new TerminalKeyword() { word = word, isVerb = isVerb, specialKeywordResult = triggeringNode };
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
			return new TerminalKeyword() { word = word, isVerb = false, specialKeywordResult = CreateTerminalNode(displayText, clearPreviousText, terminalEvent) };
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
			return new TerminalNode() { displayText = displayText, clearPreviousText = clearPreviousText, terminalEvent = terminalEvent };
		}

		/// <summary>
		/// Addes the keyword to the terminal's keywords
		/// </summary>
		/// <param name="terminalKeyword">The keyword to add</param>
		public static void AddTerminalKeyword(TerminalKeyword terminalKeyword)
		{
			if(_isInGame())
			{
				Terminal.terminalNodes.allKeywords = Terminal.terminalNodes.allKeywords.Add(terminalKeyword);
				plugin.Log.LogMessage($"Added {terminalKeyword.word} keyword to terminal keywords.");
			}
			else
			{
				plugin.Log.LogMessage($"Not in game, waiting to be in game to add {terminalKeyword.word} keyword.");
				Action<TerminalKeyword> newAction = AddTerminalKeyword;
				DelayedAction delayedAction = new() { Action = newAction, Keyword = terminalKeyword };
				QueuedActions.Add(delayedAction);
			}
		}

		/// <summary>
		/// Gets the <see cref="TerminalKeyword"/> via its word.
		/// </summary>
		/// <param name="keyword">The word of the keyword</param>
		/// <returns>A <see cref="TerminalKeyword"/> from allKeywords. Or null</returns>
		public static TerminalKeyword GetKeyword(string keyword)
		{
			if (_isInGame())
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
			if (_isInGame())
			{
				for (int i = 0; i < Terminal.terminalNodes.allKeywords.Length; i++)
				{
					if (Terminal.terminalNodes.allKeywords[i].word == keyword.word)
					{
						Terminal.terminalNodes.allKeywords[i] = keyword;
						plugin.Log.LogMessage($"Updated {keyword.word}");
						return;
					}
				}
				plugin.Log.LogInfo($"Failed to update {keyword.word}. Was not found in keywords.");
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
			if (_isInGame())
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
					}
				}
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
			if (_isInGame())
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
					}
				}
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
			if (_isInGame())
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
					}
				}
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
			if (_isInGame())
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
					}
				}
			}
		}

		/// <summary>
		/// Adds a compatible noun to an already exiting keyword.
		/// </summary>
		/// <param name="verbKeyword">The verb keyword that already exists in the terminal</param>
		/// <param name="noun">The noun word that you want to add</param>
		/// <param name="displayText">The text you want to display when the verb noun combo is used</param>
		public static void AddCompatibleNoun(TerminalKeyword verbKeyword, string noun, string displayText)
		{
			if (_isInGame())
			{
				verbKeyword = verbKeyword.AddCompatibleNoun(noun, displayText);
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
			if (_isInGame())
			{
				verbKeyword = verbKeyword.AddCompatibleNoun(noun, triggerNode);
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
			if (_isInGame())
			{
				TerminalKeyword verbTerminalKeyword = GetKeyword(verbWord);
				if(verbTerminalKeyword == null) { plugin.Log.LogError("The verb given does not exist."); return; }
				verbTerminalKeyword = verbTerminalKeyword.AddCompatibleNoun(noun, triggerNode);
				UpdateKeyword(verbTerminalKeyword);
			}
		}

		/// <summary>
		/// Adds a compatible noun to an already exiting keyword.
		/// </summary>
		/// <param name="verbWord">The verb word that already exists in the terminal</param>
		/// <param name="noun">The noun word that you want to add</param>
		/// <param name="displayText">The text you want to display when the verb noun combo is used</param>
		public static void AddCompatibleNoun(string verbWord, string noun, string displayText)
		{
			if (_isInGame())
			{
				TerminalKeyword verbTerminalKeyword = GetKeyword(verbWord);
				if (verbTerminalKeyword == null) { plugin.Log.LogError("The verb given does not exist."); return; }
				verbTerminalKeyword = verbTerminalKeyword.AddCompatibleNoun(noun, displayText);
				UpdateKeyword(verbTerminalKeyword);
			}
		}

	}
}
