# TerminalApi
## Please report any issues [here](https://github.com/NotAtomicBomb/TerminalApi/issues)
## Have the [Glowstick](https://thunderstore.io/c/lethal-company/p/Asylud/Glowstick/) mod and are unable to leave the terminal? [Try This](https://github.com/NotAtomicBomb/TerminalApi/issues/19#issuecomment-1898774242)

## Overview

Terminal Api provides a nice a easy way to add and modify terminal keywords.
It allows you to create and add them whenever you want in your code as it 
will automatically add the keywords when it is safe to do so.

See [Test Plugin](https://github.com/NotAtomicBomb/TerminalApi/blob/main/TestPlugin/Plugin.cs) and [Time Command](https://github.com/NotAtomicBomb/TimeCommand/blob/main/TimeTerminalCommand/Plugin.cs/) mod for an example case of this api.

## Installation

Just drag and drop the BepInEx folder to your Lethal Company root folder(Where the Lethal Company.exe is).
Or just use the thuderstore mod loader, much easier.

## Table of Contents

1. [Adding Commands](#adding-commands)
2. [Callbacks](#callbacks)
3. [Command Info](#command-info)
2. [Events](#events)
3. [Terminal Input](#terminal-input)
1. [Creating Terminal Keywords](#creating-terminal-keywords)
2. [Creating Terminal Nodes](#creating-terminal-nodes)
2. [Adding Text To Nodes](#adding-text-to-nodes)
3. [Adding Keywords](#adding-keywords)
4. [Updating Keywords](#updating-keywords)
5. [Deleting Keyword](#deleting-keywords)
5. [Getting Keywords](#getting-keywords)
6. [Compatible Nouns](#compatible-nouns)
    - [Adding Compatible Nouns to Newly Created Keyword](#adding-compatible-noun-to-newly-created-keyword)
    - [Updating Existing Compatible Noun](#updating-existing-compatible-noun)

> [!WARNING]
> Make sure you are not adding duplicate keywords. I do plan on having a system to handle this in the future.

## Adding Commands

This api give you an easy way to add terminal commands via the `AddCommand` method.
This methods returns nothing.
There are two methods:
- `AddCommand(string commandWord, string displayText, string verbWord = null, bool clearPreviousText = true)`
- `AddCommand(string commandWord, CommandInfo commandInfo, string verbWord = null, bool clearPreviousText = true)`

Example:

```cs
    AddCommand("frank", "Frank is not here\n", "get", true)
```
Will display `Frank is not here` when `get frank` or `frank` is sent in the terminal

## Callbacks

In 1.5.0 a way of adding callback functions was added. 
The current way only works on commands/terminalkeywords that are being added.
The callback function must return a string, that string is what will be displayed on the terminal when the
command is run.

There are two ways this can be achieved.

```cs
    AddTerminalKeyword(nounKeyword, new CommandInfo() {
	TriggerNode = triggerNode,
	DisplayTextSupplier = () =>
	{
		Logger.LogWarning("Put code here, and it will run when trigger node is loaded");
		return "This text will display";
	},
	Category = "Other",
	Description = "This is just a test command."
});
```

Or

```cs
    AddCommand("pop", new CommandInfo()
{
	DisplayTextSupplier = () =>
	{
		Logger.LogWarning("Wowow, this ran.");
		return "popped\n\n";
	},
	Category = "Other"
});
```

## Command Info

Also in 1.5.0 a Command Class was added ot help with adding callbacks and info about the command to any of the existing categories.
Note: I do plan on handling categories that don't exist being automatically created, but that will have to be another time.
Any commands added with a category will append a line looking like this `>[Title]\n[Description]`

The CommandInfo class has 5 properties

- `Title` : The title of the command, can be null. If null, will try to use the terminalkeyword's word.
- `Category` : The category that you want to add the info. Only works for existing categories like `other`.
- `Description` : The description of your command.
- `DisplayTextSupplier` : The callback function, needs to return a string.
- `TriggerNode` : The node that will trigger the `DisplayTextSupplier`. Can be null. If null, will try to use the terminalkeyword.specialKeywordResult.



## Events

There are multiple subscribable events that this api provides.
They can be found in the `TerminalApi.Events` namespace.
Here is a list of them:

- `TerminalAwake` - Runs when the terminal is fully awake.
- `TerminalWaking` - Runs when the terminal is waking up.
- `TerminalStarted` - Runs when the terminal is fully started.
- `TerminalStarting` - Runs when the terminal starts.
- `TerminalBeginUsing` - Runs when the player begins using.
- `TerminalBeganUsing` - Runs after begins using.
- `TerminalExited` - Runs when the player exits the terminal.
- `TerminalParsedSentence` - Runs when the player sends a command.
- `TerminalTextChanged` -  Runs when the player types in the terminal.

For more information please check [Test Plugin](https://github.com/NotAtomicBomb/TerminalApi/blob/main/TestPlugin/Plugin.cs).

## Terminal Input

With TerminalApi, you can get and set the users current input.
To do that we have two methods. `GetTerminalInput` will of course get the terminal input.
`SetTerminalInput` will set the input.

Examples:
```cs
    string input = TerminalApi.GetTerminalInput();
    if(input == "hello")
    {
        TerminalApi.SetTerminalInput("world");
    }
```

## Creating Terminal Keywords

To create a terminal keyword, you can use the `CreateTerminalKeyword` method.
There are two overloaded methods available: 
- `CreateTerminalKeyword(string word, bool isVerb = false, TerminalNode triggeringNode = null)`
- `CreateTerminalKeyword(string word, string displayText, bool clearPreviousText = false, string terminalEvent = "")`

Example: 

```cs
    TerminalKeyword keyword = TerminalApi.CreateTerminalKeyword("die", "No don't");
```

## Creating Terminal Nodes

If you don't know what terminal nodes are, they essential hold information that the terminal uses when the keyword associated with the node is sent. For most cases, when a word is sent, it will display the nodes `displayText`. You can create a node by using the `CreateTerminalNode` method. 

> [!NOTE]
> You can pretty much ignore the terminalEvent parameter. Unless you know what you're doing.

There is one method available:
- `CreateTerminalNode(string displayText, bool clearPreviousText = false, string terminalEvent = "")`

Example: 

```cs
    TerminalNode node = TerminalApi.CreateTerminalNode("Hello world");
```

## Adding Text To Nodes

To add a line of text to an existing node, you can use `NodeAppendLine`. 
It uses a keyword's word to get the node and then adds the given text as a new line.
In its current state, it must be run while in a game like in the terminal awake function.

There is one method available:
- `NodeAppendLine(string word, string text)`

Example:

```cs
    // Adds 'Hello' as a new line to the help node
    NodeAppendLine("help", "\nHello");
```


## Adding Keywords

Once you have your keyword ready to add you can use the `AddTerminalKeyword` to add the keyword.

There are two methods available:
- `AddTerminalKeyword(TerminalKeyword terminalKeyword)`
- `AddTerminalKeyword(TerminalKeyword terminalKeyword, CommandInfo commandInfo = null)`

Example:

```cs
    TerminalApi.AddTerminalKeyword(keyword);
```

## Updating Keywords

Use `UpdateKeyword` to update any keyword that already exists.

There is one method available:
- `UpdateKeyword(TerminalKeyword terminalKeyword)`

Example: 
```cs
    TerminalApi.UpdateKeyword(keyword);
```

## Deleting Keywords

Use `DeleteKeyword` to delete an existing keyword.

There is one method available:
- `DeleteKeyword(string word)`

Example:
```cs
    TerminalApi.DeleteKeyword("buy")
```
^ Will delete the buy keyword, if it exists

## Getting Keywords

Use `GetKeyword` to get an already existing keyword.
This uses the keyword's word to find it.

There is one method available:
- `GetKeyword(string keyword)`

Example:
```cs
    TerminalKeyword gottenKeyword = TerminalApi.GetKeyword("die");
```

## Compatible Nouns

> [!IMPORTANT]
> If you use compatible nouns, make sure that verb keyword and noun keywords are added to the terminal!

Every keyword has a field for an array of `CompatibleNoun`, not all keywords use it though. Only keywords marked as verbs use it.
It allows for verb-noun combos like `check fish`. The verb being `check` and the noun being `fish`.

### Adding Compatible Noun To Newly Created Keyword

In TerminalApi there is an exstension method that allows you to quickly add a `CompatibleNoun` to your verb keyword.
There are three of these overloaded methods:
- `AddCompatibleNoun(this TerminalKeyword terminalKeyword, TerminalKeyword noun, TerminalNode result)`
- `AddCompatibleNoun(this TerminalKeyword terminalKeyword, string noun, TerminalNode result)`
- `AddCompatibleNoun(this TerminalKeyword terminalKeyword, string noun, string displayText)`
- `AddCompatibleNoun(this TerminalKeyword terminalKeyword, TerminalKeyword noun, string displayText, bool clearPreviousText = false)`

Here is an example:
```cs
    TerminalKeyword verbKeyword = TerminalApi.CreateTerminalKeyword("check", true);
    TerminalKeyword nounKeyword = TerminalApi.CreateTerminalKeyword("fish");
    TerminalNode triggerNode = TerminalApi.CreateTerminalNode("There are no fish\n", true);

    verbKeyword = verbKeyword.AddcompatibleNoun(nounKeyword, triggerNode);
    TerminalApi.AddTerminalKeyword(verbKeyword);
    TerminalApi.AddTerminalKeyword(nounKeyword);
```
In the example above, if you were to type `check fish` into the terminal you would get `There are no fish\n`.

### Adding Compatible Noun to Existing Keyword

Use `AddCompatibleNoun` method to add a `CompatibleNoun` to an existing keyword.
The Noun should also already exist.

There are four overloaded methods:
- `AddCompatibleNoun(TerminalKeyword verbKeyword, string noun, string displayText, bool clearPreviousText = false)`
- `AddCompatibleNoun(TerminalKeyword verbKeyword, string noun, TerminalNode triggerNode)`
- `AddCompatibleNoun(string verbWord, string noun, TerminalNode triggerNode)`
- `AddCompatibleNoun(string verbWord, string noun, string displayText, bool clearPreviousText = false)`

Example: 
```cs
    TerminalApi.AddCompatibleNoun("check", "die", "You are not dead.");
```
The above code would add the existing `die` noun to check and when `check die` is typed into the terminal `You are not dead.` would display.

### Updating Existing Compatible Noun

Use `UpdateKeywordCompatibleNoun` method to update an existing compatible noun on a verb keyword.
 
There is four method:
- `UpdateKeywordCompatibleNoun(TerminalKeyword verbKeyword, string noun, TerminalNode newTriggerNode)`
- `UpdateKeywordCompatibleNoun(string verbWord, string noun, TerminalNode newTriggerNode)`
- `UpdateKeywordCompatibleNoun(TerminalKeyword verbKeyword, string noun, string newDisplayText)`
- `UpdateKeywordCompatibleNoun(string verbWord, string noun, string newDisplayText)`

Example:
```cs
    TerminalApi.UpdateKeywordCompatibleNoun("check", "fish", "There are 100 fish.")
```

With the above code `check fish` you should get `There are 100 fish.` in the terminal.