## [v1.2.1](https://github.com/Edvinas01/ggj-2025/compare/v1.1.0...v1.2.1) - 2025-01-29

### Fixed

- WebGL audio bank loading.

## [v1.2.0](https://github.com/Edvinas01/ggj-2025/compare/v1.1.0...v1.2.0) - 2025-01-29

### Changed

- Optimized shadow rendering.
- Optimized materials by using `Simple Lit` shader instead of `Lit`.
- Rendering path to use `Forward` instead of `Forward+` (WebGL doesn't like `Forward+`)
- Main menu scene will be loaded on game victory instead of restarting.

### Fixed

- WebGL builds.

## [v1.1.0](https://github.com/Edvinas01/ggj-2025/compare/v1.0.0...v1.1.0) - 2025-01-29

### Added

- More clear health tracking on mirror.
- Bubble highlight.
- Background to pause menu.
- Hand as default cursor.
- Minimal intro cinematic.
- Blood VFX.
- More spawn points.
- Wobble animation to controls help panel.

### Changed

- Reduced table-top size so the characters are more visible.
- Enabled vSync.
- Optimized the game by reducing shadow caster count and simplifying post FX.
- Game over menu button will load the menu scene instead of exiting the game.
- Test character data to make more sense.
- Victory scene will reload the game instead of loading the main menu.
- Reduced render scale, still looks good.

### Fixed

- Selection cursor offset.
- Made ray-casting more accurate.
- Pause menu UI text to use LT.
- Some typos and incorrect options in character texts.
- Cursor being visible in victory scene.
- Timing sync in victory animation.
- Shopper VO not finishing on punch.

### Removed

- Pause menu from victory screen.
- Choice bubble collision.

## [v1.0.0](https://github.com/Edvinas01/ggj-2025/compare/v0.0.1) - 2025-01-26

Initial release.
