# Releases

## Version 1.0.0

- Initial Release

## Version 1.0.1

- Updated Github link

## Version 1.0.2

- Added Installation Instructions to README

## Version 1.0.3

- Added proper tags to mod

## Version 1.1.0

- Added a subscribable event for Terminal Awake
`TerminalApi.Patches.TerminalAwakePatch.TerminalAwake`
- Some file organizing 
- Added CHANGELOG

## Version 1.2.0

- Added more events
- Events are now located at `TerminalApi.Events`
- Added the `AddCommand` method to easily add terminal commands
- Update README

## Version 1.3.0

- Added `TerminalTextChanged` event

## Version 1.3.1

- Fixed bug with TextChanged event

## Version 1.3.2

- [Removed logging on keyword updates](https://github.com/NotAtomicBomb/TerminalApi/pull/5)

## Version 1.4.0

- Added DeleteKeyword
- Added GetTerminalInput
- Added SetTerminalInput

## Version 1.5.0

- Added CommandInfo class
- CommandInfo allows for adding callbacks functions
- Added AddCommand overload that accepts CommandInfo
- Added AddTerminalKeyword overload that also accepts CommandInfo
- Added a config option that allows users to disable TerminalApi logs
- Added NodeAppendLine, appends a line of text to a node via its keyword

