# Griffin.PowerMate (.NET Framework 4.8 Port)

This repository is an independent community effort to keep the Griffin Technology PowerMate USB / Bluetooth knob usable on modern Windows systems. It is a reconstructed and organized port of the original (now discontinued) Windows software, retargeted to .NET Framework 4.8.

## Origin / Background
Griffin Technology discontinued the PowerMate product and its official software. The last released Windows version (referenced internally as 2.0.1) is no longer actively maintained. This project was produced by decompiling that final public release, refactoring, and restructuring the code into a multi‑project solution for clarity and maintainability.

Wikipedia reference: https://en.wikipedia.org/wiki/Griffin_PowerMate

See NOTICE.md for important legal context, attribution, and limitations.

## Goals
- Preserve basic and advanced functionality (actions, volume, scrolling, application bindings, etc.)
- Make the code buildable with current tooling (.NET Framework 4.8)
- Separate concerns into modular action libraries
- Provide a foundation for fixes and enhancements

## Non‑Goals
- Exact 1:1 replication of all original private implementation details
- Official support or endorsement by Griffin (none is implied)

## Legal / Licensing Status
- Portions of this repository are reconstructed from the discontinued proprietary binary; those portions are NOT offered under an open source license.
- Newly authored / clean‑room rewritten code MAY be explicitly labeled and licensed (e.g., MIT) on a per‑file basis. Absent such a header, assume no license grant.
- PowerMate is a trademark of its respective owner. No affiliation or endorsement is claimed.

If/when a full clean rewrite replaces all decompiled code, a repository‑wide open source license may be applied.

## Repository Structure (High Level)
- PowerMateLib: Core device interaction abstractions
- PowerMate: Runtime / service components
- EditorUI / StartupUI / UpdaterUI: UI layers split by responsibility
- *Actions projects*: Modular action providers (Volume, Scroll, Mouse, SendKeys, Power, Open, iTunes, etc.)

## Building
1. Open the solution in Visual Studio (target: .NET Framework 4.8)
2. Build All
3. Run the Griffin.PowerMate.PowerMate project

## Contributing
Issues and pull requests are welcome for:
- Bug fixes
- Modern Windows compatibility improvements
- Code cleanup / decoupling / tests
- Clean‑room rewrites of decompiled components
- New action modules (keep them modular)

Please keep changes focused and documented. Mark clearly which files are original new work and add an explicit header if you are licensing them (example):
```
// Copyright (c) 2025 <Your Name>
// Licensed under MIT (applies to this file only)
```

## Roadmap (Aspirational)
- Optional migration path to .NET (Core) / cross‑platform abstractions (where feasible)
- Improved plugin discovery & sandboxing
- Unit tests for action dispatch and device handling
- Progressive replacement of decompiled code with clean implementations

## Disclaimer
Provided "as is" with no warranty. Use at your own risk. Review NOTICE.md before redistributing or reusing any portion.

---
Community preservation of a discontinued peripheral.