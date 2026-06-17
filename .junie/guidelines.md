# Project Guidelines

## General Principles
- Maintain consistency with the existing C# codebase in Flax Engine.
- Follow the established naming conventions (PascalCase for classes, methods, and public members).
- Use `Debug.Logger.Log` for logging instead of `Console.WriteLine`.

## Flax Engine Specifics
- Use `NetworkReplicator` for networking logic.
- Ensure `NetworkRpc` attributes are correctly applied to methods that require network synchronization.
- Prefer using `Actor.Transform` and `PrefabManager.SpawnPrefab` for spawning objects.
- Use `HasTag` for identifying actors by tag.

## Code Style
- Use file-scoped namespaces (e.g., `namespace Game.Game.Player;`).
- Follow the existing indentation (4 spaces).
- Use `var` for local variables when the type is obvious.

## Networking
- Always check if the current instance is the server or host before performing server-only logic:
  `if (!FlaxEngine.Networking.NetworkManager.IsServer && !FlaxEngine.Networking.NetworkManager.IsHost) return;`
- Use `NetworkReplicator.AddObject` in `OnStart` for objects that need replication.
