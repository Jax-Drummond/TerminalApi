using System;

namespace TerminalApi
{
	public static class TerminalExtenstionMethods
	{
		/// <summary>
		/// Adds a compatible noun to a verb keyword.
		/// </summary>
		/// <param name="terminalKeyword">The keyword that is adding compatible nouns</param>
		/// <param name="noun">The noun you want to add</param>
		/// <param name="result">The node that will run when verb is ran with the noun</param>
		/// <returns>The edited <see cref="TerminalKeyword"/></returns>
		public static TerminalKeyword AddCompatibleNoun(this TerminalKeyword terminalKeyword, TerminalKeyword noun, TerminalNode result)
		{
			if (terminalKeyword.isVerb)
			{
				CompatibleNoun compatibleNoun = new CompatibleNoun() { noun = noun, result = result };
				if(terminalKeyword.compatibleNouns == null)
				{
					terminalKeyword.compatibleNouns = new CompatibleNoun[1] { compatibleNoun };
				}
				else
				{
					terminalKeyword.compatibleNouns = terminalKeyword.compatibleNouns.Add( compatibleNoun );
				}
				return terminalKeyword;
			}
			return null;
		}

		/// <summary>
		/// Adds a compatible noun to a verb keyword.
		/// </summary>
		/// <param name="terminalKeyword">The keyword that is adding compatible nouns</param>
		/// <param name="noun">The noun word you want to add</param>
		/// <param name="result">The node that will run when verb is ran with the noun</param>
		/// <returns>The edited <see cref="TerminalKeyword"/></returns>
		public static TerminalKeyword AddCompatibleNoun(this TerminalKeyword terminalKeyword, string noun, TerminalNode result)
		{
			if (terminalKeyword.isVerb)
			{
				CompatibleNoun compatibleNoun = new CompatibleNoun() { noun = TerminalApi.CreateTerminalKeyword(noun), result = result };
				if (terminalKeyword.compatibleNouns == null)
				{
					terminalKeyword.compatibleNouns = new CompatibleNoun[1] { compatibleNoun };
				}
				else
				{
					terminalKeyword.compatibleNouns = terminalKeyword.compatibleNouns.Add(compatibleNoun);
				}
				return terminalKeyword;
			}
			return null;
		}

		/// <summary>
		/// Adds a compatible noun to a verb keyword.
		/// </summary>
		/// <param name="terminalKeyword">The keyword that is adding compatible nouns</param>
		/// <param name="noun">The noun word you want to add</param>
		/// <param name="result">The text that will display when verb is ran with the noun</param>
		/// <returns>The edited <see cref="TerminalKeyword"/></returns>
		public static TerminalKeyword AddCompatibleNoun(this TerminalKeyword terminalKeyword, string noun, string displayText)
		{
			if (terminalKeyword.isVerb)
			{
				CompatibleNoun compatibleNoun = new CompatibleNoun() { noun = TerminalApi.CreateTerminalKeyword(noun), result = TerminalApi.CreateTerminalNode(displayText) };
				if (terminalKeyword.compatibleNouns == null)
				{
					terminalKeyword.compatibleNouns = new CompatibleNoun[1] { compatibleNoun };
				}
				else
				{
					terminalKeyword.compatibleNouns = terminalKeyword.compatibleNouns.Add(compatibleNoun);
				}
				return terminalKeyword;
			}
			return null;
		}

		/// <summary>
		/// Adds newItem to an array.
		/// </summary>
		/// <typeparam name="T">The type of the array</typeparam>
		/// <param name="array">The array to add to</param>
		/// <param name="newItem">The item to add to the array</param>
		/// <returns></returns>
		internal static T[] Add<T>(this T[] array, T newItem)
		{
			int newSize = array.Length + 1;
			Array.Resize(ref array, newSize);
			array[newSize - 1] = newItem;
			return array;
		}
	}
}
