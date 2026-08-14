# Topic 4: Blender-to-Godot Workflow

## Theme

Create a repeatable asset pipeline from the engineering model to the real-time twin.

## Learning outcomes

- export assets using glTF/GLB;
- preserve scale, orientation, hierarchy, and materials;
- diagnose missing materials and incorrect origins;
- separate source assets from runtime scenes.

## Preparation

- Complete the Topic 3 model.
- Review Godot project and folder conventions.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:25 | Asset pipeline and coordinate-system differences |
| 00:25-01:00 | Guided GLB export/import and validation |
| 01:00-01:25 | Collision, pivots, materials, and inherited scenes |
| 01:25-02:25 | Import team assets and build a reusable equipment scene |
| 02:25-02:50 | Test re-export after a Blender revision |
| 02:50-03:00 | Visual and dimensional inspection |

## Laboratory

Import the complete equipment model into Godot. Add a scale reference, camera, neutral lighting, and basic collision only where interaction requires it. Correct problems at the source rather than applying unexplained offsets in multiple scenes.

## Deliverable

A Godot project that opens without missing assets and contains one reusable scene per major equipment type.

## Evidence and assessment

- One Blender meter corresponds to one Godot meter
- Orientation and origins remain correct
- Re-export does not destroy scene logic
- Project paths are portable across team computers

## Instructor notes

Have teams exchange repositories briefly. Import problems that appear only on another computer usually reveal absolute paths or uncommitted assets.
