# Gameplay implementation notes

This repository did not include Unity scenes, prefabs, or editor settings, so the following were added as reusable scripts:

- `MageWardAura` and `TankHealth` for radius-based healing.
- `TankAuraIndicatorUI` to show active healing state.
- `HudController` for team sigils, imp count (clamped 0-8), and boss HP text/slider.
- `NavAgentConfigurator` presets for enemy and imp NavMesh agents.
- `GreyboxNavMeshBaker` helper to bake a `NavMeshSurface` when AI Navigation is present.

## Manual editor steps still required

1. Attach scripts to the relevant GameObjects in your scene/prefabs.
2. Assign references in the inspector (TankHealth, UI images/text/slider, NavMeshSurface).
3. Open your greybox level and invoke **Bake Greybox NavMesh** on the baker component.
4. Run play mode with 1-4 gamepads and tune values (heal rate, radius, colors, movement speeds).
